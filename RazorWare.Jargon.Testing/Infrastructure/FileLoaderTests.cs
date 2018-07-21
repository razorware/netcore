using System.IO;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RazorWare.Jargon.Testing {
   [TestClass]
   public class FileLoaderTests {
      private static string rootDir = GlobalConstraints.RootDirectory;

      [TestMethod]
      public void LoadFileMap( ) {
         var fMap = Infrastructure.FileLoader.Map(rootDir);
         var root = fMap.Root();

         Assert.AreEqual(Path.GetFullPath(rootDir), root);
         Assert.AreEqual(@"D:\Git.Repos\netcore\jargon_samples", Path.GetDirectoryName(root));
      }

      [TestMethod]
      public void RootContainsFile( ) {
         var fileName = "jargon_sample.jss";
         var fileMap = Infrastructure.FileLoader.Map(rootDir);

         var fileList = fileMap.Contents()
            .ToList();

         Assert.IsTrue(fileList.Any(f => f.Name == fileName));
      }

      [TestMethod]
      public void RootContainsDirectory( ) {
         var dirName = "test_directory";
         var fileMap = Infrastructure.FileLoader.Map(rootDir);

         Assert.IsTrue(fileMap.Contents().Any(f => f.Name == dirName));
      }
   }
}
