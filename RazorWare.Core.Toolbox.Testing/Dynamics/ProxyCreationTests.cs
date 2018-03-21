using System;
using System.Linq;
using System.Runtime.Serialization;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using RazorWare.Testing._classes;
using static RazorWare.Testing._classes.TestProxyGenerator;

using RazorWare.Dynamics;
using static RazorWare.Dynamics.DynamicsExtensions;
using System.ComponentModel;

namespace RazorWare.Toolbox.Testing {
   [TestClass]
   public class ProxyCreationTests {

      #region Inadequate methods of proxy creation
      [TestMethod]
      public void ActivatorCreation( ) {
         // Activator must use a concrete object
         var type = typeof(Person);
         var person = (Person)Activator.CreateInstance(type);

         Assert.IsInstanceOfType(person, type);
      }

      [TestMethod]
      [ExpectedException(typeof(MemberAccessException))]
      public void FormatterServicesCreation( ) {
         var type = typeof(IPerson);
         var person = (IPerson)FormatterServices.GetUninitializedObject(type);

         /*
          * The object cannot be executed because an interface is un-instantiable as a concrete object. 
          * Even though FormatterServices does not execute the constructor, the class must be concrete.
          * ***/
      }
      #endregion

      #region TestProxyGenerator
      [TestMethod]
      public void GenerateIFooProxy( ) {
         // Only tests the creation of the proxy, not the methods within
         var today = DateTime.Now.DayOfWeek;
         var fooProxy = GetProxyFor<IFoo>();

         Assert.IsNotNull(fooProxy);
         Assert.IsInstanceOfType(fooProxy, typeof(IFoo));
      }
      #endregion

      [TestMethod]
      public void ProxyTypeBuilderDelegate( ) {
         var bldrDelegate = GetTypeBuilder<IPerson>();
         var actual = bldrDelegate.Target.GetType().GenericTypeArguments[0];
         var expected = typeof(IPerson);

         Assert.AreEqual(expected, actual);

         var typeBuilder = bldrDelegate();

         Assert.IsNotNull(typeBuilder);
         Assert.AreEqual("IPerson", bldrDelegate.Assembly().Name);

         var interfaces = typeBuilder.GetInterfaces().ToList();

         Assert.IsTrue(interfaces.Any());
         Assert.AreEqual(expected, interfaces[0]);

         expected = typeof(Proxy);

         Assert.AreEqual(expected, typeBuilder.BaseType);
      }

      [TestMethod]
      public void AddTypeInterfaceDelegate( ) {
         var bldrDelegate = GetTypeBuilder<IPerson>()
            .AddInterfaces(typeof(IFoo))
            .AddInterfaces(typeof(INotifyPropertyChanged), typeof(IEquatable<IPerson>));

         var typeBuilder = bldrDelegate();
         var interfaces = typeBuilder.GetInterfaces().ToList();

         Assert.IsTrue(interfaces.Any(i => i == typeof(IFoo)));
         Assert.IsTrue(interfaces.Any(i => i == typeof(INotifyPropertyChanged)));
         Assert.IsTrue(interfaces.Any(i => i == typeof(IEquatable<IPerson>)));
      }


   }
}
