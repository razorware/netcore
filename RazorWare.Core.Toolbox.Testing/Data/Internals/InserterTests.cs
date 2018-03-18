using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RazorWare.Data;
using RazorWare.Data.Annotations;
using static RazorWare.Data.Internals;
using Attribs = RazorWare.Data.Annotations.DataAttributes;

namespace RazorWare.Toolbox.Testing {
   [TestClass]
   public class InserterTests {
      /* ASSUMPTION: All insert tests assume good data because all data will be validated by
       * the parent table's validator prior to calling the cache's append method.
       * ***/
      const int ID = 128;
      const string INSERTER = "Inserter";

      private static readonly Type internals = typeof(Internals);
      private static readonly Dictionary<ICache, int> rowIdMap = GetRowIdMap();

      private static Type[] nested;
      private static ISchema schema;
      private static ICache cache;
      private static IIndexer indexer;

      [ClassInitialize]
      public static void InitializeHarness(TestContext context) {
         nested = internals.GetNestedTypes(BindingFlags.NonPublic);

         Assert.IsTrue(nested.Any(t => t.Name == INSERTER));

         cache = GetTableCache(1);
      }

      [TestInitialize]
      public void InitializeTest( ) {
         // reset cache starting row id
         rowIdMap[cache] = 1;
         // rebuild unaltered schema
         schema = SchemaTests.CreateSchema(ID);
         // would be nice to be able to mock this (hint-hint)
         indexer = GetIndexer(schema);
      }

      [TestCleanup]
      public void CleanupTest( ) {
         cache.Clear();

         Assert.AreEqual(0, cache.RowCount);
      }

      [TestMethod]
      public void InitializeInserterObject( ) {
         // tests reflected initializer method
         var type = nested.First(t => t.Name == INSERTER);
         var ctor = type.GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance,
            null,
            new[] { typeof(ISchema), typeof(ICache) },
            null);

         Assert.IsNotNull(ctor, "no ctor info");

         var inserter = ctor.Invoke(new object[] { schema, cache });

         Assert.IsNotNull(inserter, "no inserter object");
      }

      [TestMethod]
      public void InserterFields( ) {
         var inserter = GetInserter(schema, cache);

         Assert.IsNotNull(inserter);
         Assert.IsInstanceOfType(inserter, typeof(IInserter));
         CollectionAssert.AreEqual(schema.Fields.ToList(), inserter.Fields.ToList());
      }

      [TestMethod]
      public void WriteDataToCache( ) {
         var inserter = GetInserter(schema, cache);

         var data = new object[] {
            1, "Peter", "Gilbert", "Fraub", new DateTime(1968, 12, 2), DateTime.Now
         };

         inserter.Insert(indexer, data);

         Assert.AreEqual(1, inserter.CurrentRowId);
      }

      [TestMethod]
      public void CalculateAutoIncrField( ) {
         var inserter = GetInserter(schema, cache);
         var pkey = new DataValue(schema.Fields[0].Type, schema.Fields[1].Attributes);

         var data = new object[] {
            pkey, "Peter", "Gilbert", "Fraub", new DateTime(1968, 12, 2), DateTime.Now
         };

         inserter.Insert(indexer, data);
         
         Assert.AreEqual(cache.RowCount, (int)data[0]);
      }

      [TestMethod]
      public void InsertWithDefaultValue( ) {
         var expDefault = true;
         // modify schema to have default
         Action<IField> defCfg = (f) => {
            f.Name = "Active";
            f.Attributes = Attribs.Default;
            f.Data = new DataValue(f.Type, f.Attributes).Value(expDefault);
         };
         schema.AddType(DataType.Boolean, defCfg);

         var inserter = GetInserter(schema, cache);
         var pkey = new DataValue(schema.Fields[0].Type, schema.Fields[1].Attributes);
         var defVal = new DataValue(schema.Fields[6].Type, schema.Fields[6].Attributes);

         var data = new object[] {
            pkey, "Peter", "Gilbert", "Fraub", new DateTime(1968, 12, 2), DateTime.Now, defVal
         };

         inserter.Insert(indexer, data);

         Assert.AreEqual(expDefault, (bool)data[6]);
      }

      [TestMethod]
      public void InsertWithoutPKeyDefined( ) {
         var schema = CreatePKeylessSchema();
         var tCache = GetTableCache(1);
         var inserter = GetInserter(schema, tCache);
         var indexer = GetIndexer(schema);

         // transform data to have appropriate number of fields (remove row id)
         var original = TableTests.TestData
            .Select(r => r.Skip(1).ToArray())
            .ToArray();
         var copy = original.ToArray();

         foreach(var c in copy) {
            inserter.Insert(indexer, c);
         }

         Assert.AreEqual(10, inserter.CurrentRowId);
         // nothing was mangled/mutilated/altered from original data set
         CollectionAssert.AreEqual(original, copy);
      }

      [TestMethod]
      public void InsertWithIndexedFieldDefined( ) {
         var schema = SchemaTests.CreateSchema(ID)
            .Configure(f => f[3].Attributes |= Attribs.Index);
         var tCache = GetTableCache(1);
         var inserter = GetInserter(schema, tCache);
         var indexer = GetIndexer(schema);

         //var expIdxList = TableTests.TestData
         //   .Select(r => new {
         //      row = (int)r[0],
         //      name = (string)r[3]
         //   })
         //   // index should collate a distinct list of key values & row IDs
         //   .GroupBy(key => key.name, index => index.row)
         //   .ToDictionary(k => k.Key, v => v.ToList());

         var rowId = 1;
         foreach(var d in TableTests.TestData) {
            inserter.Insert(indexer, d);

            Assert.AreEqual(rowId, inserter.CurrentRowId);

            ++rowId;
         }
      }

      private static Dictionary<ICache, int> GetRowIdMap( ) {
         // via reflection, retrieve Dictionary<ICache, int> tblRowIdMap from Internals
         var internals = typeof(Internals);
         var field = internals.GetField("tblRowIdMap", BindingFlags.NonPublic | BindingFlags.Static);

         return (Dictionary<ICache, int>)field.GetValue(Instance);
      }

      internal static ISchema CreatePKeylessSchema( ) {
         Action<IField> fNameCfg = (f) => {
            f.Name = "Forename";
            f.Attributes = Attribs.NotNull;
         };
         Action<IField> lNameCfg = (f) => {
            f.Name = "Surname";
            f.Attributes = Attribs.NotNull;
         };
         Action<IField> bDateCfg = (f) => {
            f.Name = "BirthDate";
            f.Attributes = Attribs.NotNull;
         };
         Action<IField> crDateCfg = (f) => {
            f.Name = "Created";
            f.Attributes = Attribs.NotNull;
         };
         
         return Schema.Initialize(ID)
            .AddType(DataType.Resize(DataType.String, 32), fNameCfg)
            .AddType(DataType.Resize(DataType.String, 32), (f) => f.Name = "MiddleName")
            .AddType(DataType.Resize(DataType.String, 32), lNameCfg)
            .AddType(DataType.TimeStamp, bDateCfg)
            .AddType(DataType.TimeStamp, crDateCfg);
      }
   }
}
