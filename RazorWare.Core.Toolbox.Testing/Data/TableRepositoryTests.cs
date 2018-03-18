using Microsoft.VisualStudio.TestTools.UnitTesting;
using RazorWare.Data;
using RazorWare.Testing._classes;
using static RazorWare.Data.Repositories;
using Attribs = RazorWare.Data.Annotations.DataAttributes;

namespace RazorWare.Toolbox.Testing {
   [TestClass]
   public class TableRepositoryTests {
      const int ID = 128;

      private ISchema schema;
      private object[][] data;
      private Table persons;

      [TestInitialize]
      public void InitializeTest( ) {
         data = TableTests.TestData;
         schema = SchemaTests.CreateSchema(ID)
            .Configure(f => f[3].Attributes |= Attribs.Index);

         persons = new Table("People", SchemaTests.CreateSchema(ID), 1);

         foreach(var d in data) {
            persons.Insert(d);
         }
      }

      [TestMethod]
      public void GetRepository( ) {
         var getRepo = Using<Person>(persons);
         Repository<Person> repo;

         Assert.IsNotNull((repo = getRepo()));         
      }

      //[TestMethod]
      //public void QueryAll( ) {

      //}
   }
}
