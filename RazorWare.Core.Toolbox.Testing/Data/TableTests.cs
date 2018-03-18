using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RazorWare.Data;
using RazorWare.Data.Annotations;
using static RazorWare.Data.Repositories;
using Attribs = RazorWare.Data.Annotations.DataAttributes;

namespace RazorWare.Toolbox.Testing {
   [TestClass]
   public class TableTests {
      /*
       * Table, when instantiated with a schema, constructs 5 helpers:
       *    * Validator:   validates data prior to insertion into table
       *    * Inserter:    inserts data into table
       *    - Updater:     updates sepecific field(s)
       *    - Indexer:     indexes new records and updates existing indices
       *    - Reader:      reads data
       *    
       * Table will have 4 basic functions:
       *    - Create:      create new records (Write)
       *    - Read:        read records
       *    - Update:      update existing records
       *    - Delete:      delete existing records
       *    
       *    * indicates initial implementation of object/feature
       *    # indicates release feature complete
       *    
       *    CRUD operations will function respective of data constraints - e.g., deletion of
       *    a record will not be allowed if a field has a relationship with another table.
       *    
       *    Tables can be created to use Indexes, FKey, PKey, Unique, AutoIncr constraints just
       *    as expected from any standard SQL RDBMS.
       *    
       *    Table data is cached (TableCache : ICache) in memory until instructed to Commit(). 
       *    When committed, the table writes serialized data to file.
       * ***/
      const int ID = 1;
      const string PEOPLEDATA = "people.dat";

      private static object[][] data = new object[][] {
         new object[]{ 1, "Peter", "Gilbert", "Fraub", new DateTime(1968, 12, 2), DateTime.Now },
         new object[]{ 2, "Jack", null, "Martin", new DateTime(1972, 11, 21), DateTime.Now },
         new object[]{ 3, "Sally", null, "Williams", new DateTime(1984, 1, 16), DateTime.Now },
         new object[]{ 4, "Jane", "Margaret", "Fraub", new DateTime(1967, 10, 11), DateTime.Now },
         new object[]{ 5, "Molly", "Ann", "Smith", new DateTime(1964, 12, 23), DateTime.Now },
         new object[]{ 6, "Janet", "Harris", "Johnson", new DateTime(1978, 3, 18), DateTime.Now },
         new object[]{ 7, "Bart", "William", "Jones", new DateTime(1962, 8, 10), DateTime.Now },
         new object[]{ 8, "Brad", "Pittley", "Foster", new DateTime(1981, 12, 3), DateTime.Now },
         new object[]{ 9, "Brandon", "Wesley", "Norton", new DateTime(1998, 7, 4), DateTime.Now },
         new object[]{ 10, "Michael", "Harris", "Forrester", new DateTime(2002, 12, 2), DateTime.Now },
      };
      private static ISchema schema;

      private Table persons;

      internal static object[][] TestData => data;

      [TestInitialize]
      public void TestInitialize( ) {
         schema = SchemaTests.CreateSchema(ID);
      }

      [TestCleanup]
      public void TestCleanup( ) {
         persons.Clear();

         Assert.AreEqual(0, persons.RowCount);

         if (File.Exists(PEOPLEDATA)) {
            File.Delete(PEOPLEDATA);
         }
      }

      [TestMethod]
      public void InitializeTableDefaults( ) {
         persons = new Table(Schema.Initialize(ID));

         Assert.IsFalse(string.IsNullOrEmpty(persons.Name));
         Assert.IsFalse(persons.HasErrors);
      }

      [TestMethod]
      public void InsertTableDataWithPKey( ) {
         persons = new Table("People", schema, 1);

         var peter = data[0];

         Assert.IsTrue(persons.Insert(peter));
      }

      [TestMethod]
      public void InsertWithAutoIncrement( ) {
         var pkey = new DataValue(DataType.Int, Attribs.PKey | Attribs.AutoIncr);
         persons = new Table("People", schema, 1);

         var copy = new object[data.Length][];
         for (int i = 0; i < data.Length; i++) {
            // copy data array
            copy[i] = data[i];
            // replace element 0 with DataValue AutoIncr
            copy[i][0] = pkey;
         }

         foreach (var r in copy) {
            Assert.IsTrue(persons.Insert(r));
         }

         // all DataValue(s) replaced with sequential row IDs
         CollectionAssert.AreEqual(data, copy);
      }

      [TestMethod]
      public void ResetTableRowId( ) {
         persons = new Table("People", schema, 1);

         foreach (var d in data) {
            Assert.IsTrue(persons.Insert(d));
         }

         persons.Clear();
         persons.SetRowId(1);

         // reset data
         data[0][0] = 0;
         persons.Insert(data[0]);

         Assert.AreEqual(1, persons.RowCount);
         Assert.AreEqual(1, data[0][0]);
      }

      [TestMethod]
      public void UsingTableDelegate( ) {
         InitializeTable();

         var queryable = Using(persons);

         Assert.IsNotNull(queryable);
      }

      [TestMethod]
      public void SelectAllRecords( ) {
         InitializeTable();

         var queryable = Using(persons)
            .Select()
            .ToArray();

         Assert.AreEqual(6, queryable.First().ColCount);
         Assert.AreEqual(2, queryable.Count(r => (string)r[3] == "Fraub"));
         Assert.AreEqual(TestData.Length, queryable.Length);
      }

      [TestMethod]
      public void PersistTableToFile( ) {
         InitializeTable();
         persons.Save();
      }

      private void InitializeTable( ) {
         schema = SchemaTests.CreateSchema(ID)
            .Configure(f => f[3].Attributes |= Attribs.Index);

         persons = new Table("People", SchemaTests.CreateSchema(ID), 1);

         foreach (var d in data) {
            persons.Insert(d);
         }
      }
   }
}
