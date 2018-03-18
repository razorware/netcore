using System;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RazorWare.Data;
using RazorWare.Data.Annotations;
using static RazorWare.Data.Internals;
using Attribs = RazorWare.Data.Annotations.DataAttributes;

namespace RazorWare.Toolbox.Testing {
   [TestClass]
   public class ValidatorTests {
      const int ID = 128;
      const string VALIDATOR = "Validator";

      private static readonly Type internals = typeof(Internals);
      private static Type[] nested;
      private static ISchema schema;
      private static IIndexer indexer;

      [ClassInitialize]
      public static void InitializeHarness(TestContext context) {
         nested = internals.GetNestedTypes(BindingFlags.NonPublic);

         Assert.IsTrue(nested.Any(t => t.Name == VALIDATOR));
      }

      [TestInitialize]
      public void InitializeTest( ) {
         schema = SchemaTests.CreateSchema(ID);
         indexer = GetIndexer(schema);
      }

      [TestMethod]
      public void InitializeValidatorObject( ) {
         // tests reflected initializer method
         var type = nested.First(t => t.Name == VALIDATOR);
         var ctor = type.GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, new[] { typeof(ISchema) }, null);

         Assert.IsNotNull(ctor, "no ctor info");

         var validator = ctor.Invoke(new object[] { schema });

         Assert.IsNotNull(validator, "no validator object");
      }

      [TestMethod]
      public void ValidatorFields( ) {
         // introduced IValidator interface as an internal
         var validator = GetValidator(schema);

         CollectionAssert.AreEqual(schema.Fields.ToList(), validator.Fields.ToList());
      }

      [TestMethod]
      public void ValidateDataFail_NotEnoughParameters( ) {
         /* parameter count mismatch (too few parameters)
          * ***/
         var validator = GetValidator(schema);

         var data = new object[] {
            1, "Peter", "Gilbert", "Fraub", /* new DateTime(1968, 12, 2), */ DateTime.Now
         };
         var expErrMsg = $"Parameter count mismatch [{data.Length} != {validator.Fields.Count}]";

         Assert.IsFalse(validator.Validate(data));
         Assert.AreEqual(expErrMsg, validator.ErrMsg);
      }

      [TestMethod]
      public void ValidateDataFail_TypeMismatch( ) {
         /* parameter type mismatch
          * ***/
         var validator = GetValidator(schema);

         var data = new object[] {
            1, "Peter", "Gilbert", "Fraub", new DateTime(1968, 12, 2).ToString(), DateTime.Now
         };
         var expErrMsg = $"[{validator.Fields[4].Name}] Parameter type mismatch [{4} :: {data[4].GetType().Name} != {validator.Fields[4].Type}]";

         Assert.IsFalse(validator.Validate(data));
         Assert.AreEqual(expErrMsg, validator.ErrMsg);
      }

      [TestMethod]
      public void ValidateDataFail_NotNullConstraint( ) {
         /* parameter type mismatch
          * ***/
         var validator = GetValidator(schema);

         var data = new object[] {
            1, "Peter", "Gilbert", null, new DateTime(1968, 12, 2), DateTime.Now
         };
         var expErrMsg = $"[{validator.Fields[3].Name}] Parameter constraint failed '{Attribs.NotNull}' [{3} :: {validator.Fields[4].Attributes}]";

         Assert.IsFalse(validator.Validate(data));
         Assert.AreEqual(expErrMsg, validator.ErrMsg);
      }

      [TestMethod]
      public void ValidateDataPass_NullAllowed( ) {
         var validator = GetValidator(schema);
         var pkey = new DataValue(schema.Fields[0].Type, schema.Fields[1].Attributes);

         var data = new object[] {
            pkey, "Peter", null, "Fraub", new DateTime(1968, 12, 2), DateTime.Now
         };

         Assert.IsTrue(validator.Validate(data));
      }

      [TestMethod]
      public void ValidateDataFail_DataLengthConstraint( ) {
         /* parameter length mismatch
          * ***/
         var validator = GetValidator(schema);

         var data = new object[] {
            1, "Peter",
            // length configured to 32
            "abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz",
            "Fraub", new DateTime(1968, 12, 2), DateTime.Now
         };
         var expErrMsg = $"[{validator.Fields[2].Name}] Parameter constraint failed 'Length' [{2} :: {((string)data[2]).Length} != {validator.Fields[2].Size}]";

         Assert.IsFalse(validator.Validate(data));
         Assert.AreEqual(expErrMsg, validator.ErrMsg);
      }

      [TestMethod]
      public void ValidateDataPass( ) {
         /* all data is provided per schema requirements (field attributes)
          * 
          * TODO: remove ID insertion since it is an AutoIncr field (future feature impl.)
          * 
          * ***/
         var validator = GetValidator(schema);

         var data = new object[] {
            1, "Peter", "Gilbert", "Fraub", new DateTime(1968, 12, 2), DateTime.Now
         };

         Assert.IsTrue(validator.Validate(data));
      }

      [TestMethod]
      public void ValidateDataPass_PKeyAutoIncrDataValue( ) {
         var validator = GetValidator(schema);
         var pkey = new DataValue(schema.Fields[0].Type, schema.Fields[1].Attributes);

         var data = new object[] {
            pkey, "Peter", "Gilbert", "Fraub", new DateTime(1968, 12, 2), DateTime.Now
         };

         Assert.IsTrue(validator.Validate(data));
      }

      [TestMethod]
      public void ValidateWithPKeyFail_TypeMismatch( ) {
         var validator = GetValidator(schema);
         var badKey = new DataValue(DataType.Boolean, Attribs.PKey);

         var data = new object[] {
            badKey, "Peter", "Gilbert", "Fraub", new DateTime(1968, 12, 2), DateTime.Now
         };
         var expErrMsg = $"[{validator.Fields[0].Name}] Parameter type mismatch [{0} :: {((DataValue)data[0]).Type} != {validator.Fields[0].Type}]";

         Assert.IsFalse(validator.Validate(data));
         Assert.AreEqual(expErrMsg, validator.ErrMsg);
      }

      [TestMethod]
      public void ValidateDataPass_DefaultValue( ) {
         var expDefault = true;
         // modify schema to have default
         Action<IField> defCfg = (f) => {
            f.Name = "Active";
            f.Attributes = Attribs.Default;
            f.Data = new DataValue(f.Type, f.Attributes).Value(expDefault);
         };
         schema.AddType(DataType.Boolean, defCfg);

         var validator = GetValidator(schema);
         var pkey = new DataValue(schema.Fields[0].Type, schema.Fields[1].Attributes);
         var defVal = new DataValue(schema.Fields[6].Type, schema.Fields[6].Attributes);

         var data = new object[] {
            pkey, "Peter", "Gilbert", "Fraub", new DateTime(1968, 12, 2), DateTime.Now, defVal
         };

         Assert.IsTrue(validator.Validate(data));
      }
   }
}
