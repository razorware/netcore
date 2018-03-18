using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzWare.Testing._classes;

namespace RzWare.Toolbox.Testing {
   [TestClass]
   public class SettingsTests {
      private static readonly AppSettings mSettings = AppSettings.Default;
      private static readonly string mDefault = @".\default";

      [TestInitialize]
      public void InitializeTest( ) {
         if (string.IsNullOrEmpty(mSettings.DefaultPath)) {
            mSettings.DefaultPath = mDefault;
            mSettings.Save();
         }
      }

      [TestCleanup]
      public void CleanupTest( ) {
         mSettings.DefaultPath = mDefault;
         mSettings.Save();
      }

      [TestMethod]
      public void InitializeAppSettings( ) {
         Assert.IsNotNull(mSettings);
         Assert.IsTrue(mSettings.HasSettingsFile);
         Assert.AreEqual(mDefault, mSettings.DefaultPath);
      }

      [TestMethod]
      public void SetDefaultPathValue( ) {
         var expPath = @".\testPath";
         mSettings.DefaultPath = expPath;
         mSettings.Save();

         Assert.AreEqual(expPath, mSettings.DefaultPath);
      }
   }
}
