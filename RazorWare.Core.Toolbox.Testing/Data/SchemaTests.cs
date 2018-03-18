using System.IO;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RazorWare.Data;
using RazorWare.Data.Annotations;
using Attribs = RazorWare.Data.Annotations.DataAttributes;
using RazorWare.Testing._classes;

namespace RazorWare.Toolbox.Testing {
   [TestClass]
   public class SchemaTests {
      const int ID = 1;

      [TestMethod]
      public void InitializeSchema( ) {
         var schema = Schema.Initialize(ID);

         Assert.AreEqual(ID, schema.Id);
      }

      [TestMethod]
      public void AddSchemaProperties( ) {
         // Property refers to a table column
         ISchema schema = Schema.Initialize(ID)
            .AddTypes(DataType.Int, DataType.String, DataType.String);

         Assert.AreEqual(3, schema.Count);
         Assert.AreEqual(12, schema.Size);
         Assert.AreEqual(15, schema.Width);
      }

      [TestMethod]
      public void ConfigureSchema( ) {
         ISchema schema = Schema.Initialize(ID)
            .AddTypes(DataType.Int, DataType.String, DataType.String)
            .Configure(p => p[1].Size = 64);

         Assert.AreEqual(72, schema.Size);
         Assert.AreEqual(15, schema.Width);
      }

      [TestMethod]
      public void ConfigureField( ) {
         IField property = Schema.Initialize(ID)
            .AddTypes(DataType.Int, DataType.String, DataType.String)
            .ConfigureField(p => p[1]);

         Assert.AreEqual(DataType.String, property.Type);
      }

      [TestMethod]
      public void ConfigPropAttribName( ) {
         ISchema schema = Schema.Initialize(ID)
            .AddTypes(DataType.Int, DataType.String, DataType.String);
         var idProp = schema.ConfigureField(c => c[0]);

         idProp.Name = "id";

         Assert.AreEqual(Schema.RC_ERROR, schema.Validate(out (IField prop, string msg)[] fields));
         Assert.AreEqual(2, fields.Length);
         Assert.AreEqual(fields[0].prop.Type, DataType.String);
         Assert.AreEqual(fields[0].msg, "Name is null");
         Assert.AreEqual(1, schema.Ordinal(fields[0].prop));
         Assert.AreEqual(fields[1].prop.Type, DataType.String);
         Assert.AreEqual(fields[1].msg, "Name is null");
         Assert.AreEqual(2, schema.Ordinal(fields[1].prop));
      }

      [TestMethod]
      public void SerializeSchema( ) {
         /* as constraint implementations are completed, the RC value may change
          * 
          * this is the schema for catalogs loaded from sys.dat
          *   Catalogs    [id(int), schema_id(int), name(string:64), file(string:512)]
          *                0         0               "master"         "master.dat"
          * ***/
         var schema = Schema.Initialize(128)
           .AddTypes(DataType.Int, DataType.Int, DataType.Resize(DataType.String, 64), DataType.Resize(DataType.String, 512));
         // configure properties
         schema.Configure(p => p[0].Name = "id")
            // setting PKey or FKey trigger additional constraints - e.g., NotNull, Unique, Index
            .Configure(p => p[0].Attributes = Attribs.PKey | Attribs.AutoIncr)
            .Configure(p => p[1].Name = "schema_id")
            .Configure(p => p[1].Attributes = Attribs.NotNull | Attribs.FKey)
            .Configure(p => p[2].Name = "name")
            .Configure(p => p[2].Attributes = Attribs.NotNull)
            .Configure(p => p[3].Name = "file")
            .Configure(p => p[3].Attributes = Attribs.NotNull);

         Assert.AreEqual(Schema.RC_OKAY, schema.Validate(out (IField prop, string msg)[] errMsg), "Validation Failed");

         var buffer = new MemoryStream();
         schema.WriteSchema(buffer);
         buffer.Seek(0, SeekOrigin.Begin);

         Assert.IsTrue(buffer.Length > 0);

         using (var reader = new BinaryReader(buffer)) {
            // read schema ID
            Assert.AreEqual(128, reader.ReadInt32(), "id read failed");
            // read number of properties
            Assert.AreEqual(4, reader.ReadInt32(), "property count read failed");
            // for each property in schema
            var idx = 0;
            while (idx < schema.Fields.Count) {
               var field = schema.Fields[idx];

               // read type (5 bytes)
               DataType type = reader.ReadBytes(5);
               // read attribs (1 byte)
               Attribs attribs = reader.ReadByte();
               // read name
               string name = reader.ReadString();

               Assert.AreEqual(field.Name, name, $"[{idx}] name failed");
               Assert.AreEqual(field.Type, type, $"[{idx}] type failed");
               Assert.AreEqual(field.Size, type.Size, $"[{idx}] size failed");
               Assert.AreEqual(field.Attributes, attribs, $"[{idx}] attribs failed");

               ++idx;
            }
         }
      }

      [TestMethod]
      public void TypedSchema( ) {
         /*
           int Id
           string Forename 
           string MiddleName 
           string Surname
           DateTime Birthdate
           DateTime Created
          * ***/
         var pNames = new List<(DataType type, string name)> {
            (DataType.Int, "Id"), (DataType.String, "Forename"),
            (DataType.String, "MiddleName"), (DataType.String, "Surname"),
            (DataType.TimeStamp, "Birthdate"), (DataType.TimeStamp, "Created")
         };
         var schema = Schema.FromType(ID, typeof(Person));

         var idx = 0;
         while (idx < schema.Fields.Count) {
            var field = schema.Fields[idx];

            Assert.AreEqual(pNames[idx].type, field.Type);
            Assert.AreEqual(pNames[idx].name, field.Name);

            ++idx;
         }
      }

      [TestMethod]
      public void ConfigureTypedSchema( ) {
         const int expSize = 64;

         var schemaCfg = Schema.FromType<Person>(ID);
         schemaCfg.Property(p => p.Forename)
            .Configure(f => f.Size = expSize)
            .Configure(f => f.Attributes = Attribs.NotNull);

         var schema = schemaCfg();
         var field = schema.Fields
            .Where(f => f.Name == "Forename");

         Assert.AreEqual(expSize, field.First().Size);
         Assert.AreEqual(Attribs.NotNull, field.First().Attributes);
      }

      [TestMethod]
      public void ConfigureSchemaWithTypedProperty( ) {
         const int expSize = 64;

         var schema = Schema.FromType(ID, typeof(Person));
         BuildSchema<Person> schemaCfg = ( ) => schema;
         schemaCfg.Property(p => p.Forename)
            .Configure(f => f.Size = expSize)
            .Configure(f => f.Attributes = Attribs.NotNull);

         var field = schema.Fields
            .Where(f => f.Name == "Forename")
            .First();

         Assert.AreEqual(expSize, field.Size);
         Assert.AreEqual(Attribs.NotNull, field.Attributes);
      }

      internal static ISchema CreateSchema(int schemaId) {
         /*
               int Id 
               string Forename 
               string MiddleName 
               string Surname 
               DateTime Birthdate 
               DateTime Created 
          * ***/

         var schema = Schema.FromType(ID, typeof(Person));
         BuildSchema<Person> schemaCfg = ( ) => schema;
         schemaCfg.Property(m => m.Id)
            .Configure(f => f.Attributes = Attribs.PKey | Attribs.AutoIncr);
         schemaCfg.Property(m => m.Forename)
            .Configure(f => f.Size = 32)
            .Configure(f => f.Attributes = Attribs.NotNull);
         schemaCfg.Property(m => m.MiddleName)
            .Configure(f => f.Size = 32);
         schemaCfg.Property(m => m.Surname)
            .Configure(f => f.Size = 48)
            .Configure(f => f.Attributes = Attribs.NotNull);
         schemaCfg.Property(m => m.Birthdate)
            .Configure(f => f.Attributes = Attribs.NotNull);
         schemaCfg.Property(m => m.Created)
            .Configure(f => f.Attributes = Attribs.NotNull);

         return schema;
      }
   }
}
