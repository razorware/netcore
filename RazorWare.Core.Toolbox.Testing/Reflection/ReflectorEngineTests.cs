using System.Linq;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using RazorWare.Testing._classes;

using RazorWare.Dynamics;
using RazorWare.Reflection;

namespace RazorWare.Toolbox.Testing {
   [TestClass]
   public class ReflectorEngineTests {
      private static readonly ReflectorEngine reflector = ReflectorEngine.Default;

      [TestInitialize]
      public void InitializeTest( ) {
         Assert.IsNotNull(reflector);

         reflector.Clear();
      }

      [TestMethod]
      public void RegisterDataModel( ) {
         IModelCache model = reflector.Register<Customer>();

         Assert.IsNotNull(model);
      }

      [TestMethod]
      public void GetModelCache( ) {
         reflector.Register<Customer>();

         Assert.IsTrue(reflector.TryGetModelCache<Customer>(out IModelCache model));
      }

      [TestMethod]
      public void ValidateModelCacheProperties( ) {
         reflector.Register<Customer>();
         var expProperties = new Dictionary<string, TypeData>() {
                { "Id", TypeData.Int32 },
                { "Forename", TypeData.String },
                { "Surname", TypeData.String },
                { "IsActive", TypeData.Bool },
                { "Email", TypeData.String },
                { "AddressId", TypeData.Int32 },
            };

         reflector.TryGetModelCache<Customer>(out IModelCache model);

         CollectionAssert.AreEqual(expProperties.Keys, model.PropertyNames.ToArray());
         CollectionAssert.AreEqual(expProperties.Values, model.PropertyTypes.ToArray());
      }
   }
}
