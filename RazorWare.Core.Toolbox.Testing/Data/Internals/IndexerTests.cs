using System;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RazorWare.Data;
using RazorWare.Data.Annotations;
using RazorWare.Testing._classes;
using static RazorWare.Data.Internals;
using Attribs = RazorWare.Data.Annotations.DataAttributes;

namespace RazorWare.Toolbox.Testing {
   [TestClass]
   public class IndexerTests {
      const int ID = 128;
      const string INDEXER = "Indexer";

      private static readonly Type internals = typeof(Internals);

      private static Type[] nested;
      private static ISchema schema;

      [ClassInitialize]
      public static void InitializeHarness(TestContext context) {
         nested = internals.GetNestedTypes(BindingFlags.NonPublic);

         Assert.IsTrue(nested.Any(t => t.Name == INDEXER));
      }

      [TestInitialize]
      public void InitializeTest( ) {
         schema = SchemaTests.CreateSchema(ID);
      }

      [TestMethod]
      public void InitializeIndexerObject( ) {
         // tests reflected initializer method
         var type = nested.First(t => t.Name == INDEXER);
         var ctor = type.GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, new[] { typeof(ISchema) }, null);

         Assert.IsNotNull(ctor, "no ctor info");

         var indexer = ctor.Invoke(new object[] { schema });

         Assert.IsNotNull(indexer, "no indexer object");
      }

      [TestMethod]
      public void IndexerDefaultTableIndex( ) {
         // default table index is created if the schema has no PKey configured.
         schema = InserterTests.CreatePKeylessSchema();
         var indexer = GetIndexer(schema);
         var index = indexer.PKey;

         Assert.AreEqual(-1, index.Ordinal);
         Assert.IsTrue(index.Attributes.HasAttribute(Attribs.PKey));
         Assert.AreEqual("RowKey", index.Name);
      }

      [TestMethod]
      public void IndexerWithSchemaPkey( ) {
         var indexer = GetIndexer(schema);
         var index = indexer.PKey;

         Assert.AreEqual(0, index.Ordinal);
         Assert.IsTrue(index.Attributes.HasAttribute(Attribs.PKey));
         Assert.AreEqual("Id", index.Name);
      }

      [TestMethod]
      public void IndexerWithSchemaIndex( ) {
         // test should validate that both the PKey and an Index key exist in the schema
         // Fields[3] is the 'Surname' property and we will add an index on the field
         schema.Configure(f => f[3].Attributes |= Attribs.Index);
         var indexer = GetIndexer(schema);
         var pkey = indexer.PKey;

         // test using ordinal number
         var srchKey = new DataValue(DataType.String, Attribs.Index).Value(3);

         Assert.IsTrue(indexer.TryGetIndex(srchKey, out IIndex idxSurname));
         Assert.AreEqual(3, idxSurname.Ordinal);

         // test using index name
         srchKey = srchKey.Value("Surname");

         Assert.IsTrue(indexer.TryGetIndex(srchKey, out idxSurname));
         Assert.AreEqual("Surname", idxSurname.Name);
      }

      [TestMethod]
      public void IndexerExtTryGetIndex( ) {
         schema.Configure(f => f[3].Attributes |= Attribs.Index);
         var indexer = GetIndexer(schema);

         Assert.IsTrue(indexer.TryGetIndex((Person m) => m.Surname, out IIndex idxSurname));
         Assert.AreEqual(3, idxSurname.Ordinal);
         Assert.AreEqual("Surname", idxSurname.Name);
      }
   }
}
