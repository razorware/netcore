using System;
using System.Reflection;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RazorWare.Jargon.Testing {
   using RazorWare.Jargon.Domain;
   using RazorWare.Jargon.Interfaces;
   using RazorWare.Jargon.Infrastructure;

   [TestClass]
   public class ParserTests {
      private static string rootDir = GlobalConstraints.RootDirectory;

      private IFileMap fileMap = FileLoader.Map(rootDir);
      private static MethodInfo parseRawNodes;

      [ClassInitialize]
      public static void InitializeTestHarness(TestContext context) {
         var parserType = typeof(Parser);
         parseRawNodes = parserType.GetMethod("BuildRawNodeTree", BindingFlags.NonPublic | BindingFlags.Instance);

         Assert.IsNotNull(parseRawNodes);
      }

      [TestMethod]
      public void ConstructParser( ) {
         var targetFile = "jargon_sample.jss";
         var parser = Parser.Create(fileMap, targetFile);

         Assert.IsNotNull(parser);
      }

      [TestMethod]
      public void ParserFileNotFound( ) {
         var targetFile = "not_found.jss";

         var parser = Parser.Create(fileMap, targetFile);

         Assert.AreEqual(ParserState.Error, parser.State);
      }

      [TestMethod]
      public void InitializeParserTrue( ) {
         var targetFile = "jargon_sample.jss";

         var parser = Parser.Create(fileMap, targetFile);

         Assert.AreEqual(ParserState.Initialized, parser.State);
      }

      [TestMethod]
      public void NamedEmptyContainer( ) {
         var targetFile = "jargon_1.jss";
         NamedValue<(long start, long length)> expected = ("Window", (7, 0));

         var parser = Parser.Create(fileMap, targetFile);
         var nodes = BuildRawNodeTree(parser);
         var actual = nodes[0];

         Assert.AreEqual(expected.Name, actual.Name);
         Assert.AreEqual(expected.Value.start, actual.Value.start);
         Assert.AreEqual(expected.Value.length, actual.Value.length);
      }

      [TestMethod]
      public void MultipleEmptyContainers( ) {
         var targetFile = "jargon_1.jss";
         NamedValue<(long start, long length)> expected = ("Window", (7, 0));

         var parser = Parser.Create(fileMap, targetFile);
         var nodes = BuildRawNodeTree(parser);

         Assert.AreEqual(5, nodes.Count);

         var i = 0;
         foreach (var actual in nodes) {
            Assert.AreEqual(expected.Name, actual.Name,
               $"{i}: {expected.Name} != {actual.Name}");
            Assert.AreEqual(expected.Value.length, actual.Value.length,
               $"{i}");

            ++i;
         }
      }

      [TestMethod]
      public void IgnoreSingleLineComments( ) {
         // ignore '// comments'
         var targetFile = "jargon_2.jss";
         NamedValue<(long start, long length)> expected = ("Window", (40, 0));

         var parser = Parser.Create(fileMap, targetFile);
         var nodes = BuildRawNodeTree(parser);
         var actual = nodes[0];

         Assert.AreEqual(expected.Name, actual.Name);
         Assert.AreEqual(expected.Value.start, actual.Value.start);
         Assert.AreEqual(expected.Value.length, actual.Value.length);
      }

      [TestMethod]
      public void IgnoreMultiLineCommentFormatting( ) {
         // ignore '/* comments */'
         var targetFile = "jargon_3.jss";
         NamedValue<(long start, long length)> expected = ("Window", (46, 0));

         var parser = Parser.Create(fileMap, targetFile);
         var nodes = BuildRawNodeTree(parser);
         var actual = nodes[0];

         Assert.AreEqual(expected.Name, actual.Name);
         Assert.AreEqual(expected.Value.start, actual.Value.start);
         Assert.AreEqual(expected.Value.length, actual.Value.length);
      }

      [TestMethod]
      public void CommentedRawNodeContainers( ) {
         /* related tests for populated raw node containers
          *    - container populated with comment only is still 'empty' (length:0)
          *    - container with comment and raw node (named value) reports proper length
          * ***/
         var targetFile = "jargon_4.jss";
         NamedValue<(long start, long length)> expected = ("Window", (43, 0));

         var parser = Parser.Create(fileMap, targetFile);
         var nodes = BuildRawNodeTree(parser);
         var actual = nodes[0];

         Assert.AreEqual(expected.Name, actual.Name);
         Assert.AreEqual(expected.Value.start, actual.Value.start);
         Assert.AreEqual(expected.Value.length, actual.Value.length);
         Assert.IsTrue(actual.Parent == null);

         expected = ("Window", (106, 0));
         actual = nodes[1];

         Assert.AreEqual(expected.Name, actual.Name);
         Assert.AreEqual(expected.Value.start, actual.Value.start);
         Assert.AreEqual(expected.Value.length, actual.Value.length);
         Assert.IsTrue(actual.Parent == null);

         expected = ("Resources", (192, 0));
         actual = nodes[2];

         Assert.AreEqual(expected.Name, actual.Name);
         Assert.AreEqual(expected.Value.start, actual.Value.start);
         Assert.AreEqual(expected.Value.length, actual.Value.length);
         Assert.IsTrue(actual.Parent == null);
      }

      [TestMethod]
      public void PopulatedNodeContainer( ) {
         var targetFile = "jargon_5.jss";
         NamedValue<(long start, long length)> expParent = ("Window", (73, 1));
         NamedValue<(long start, long length)> expChild = ("child", (80, 0));

         var parser = Parser.Create(fileMap, targetFile);
         var nodes = BuildRawNodeTree(parser);

         var actParent = nodes[0];
         var children = actParent.Nodes;

         Assert.IsTrue(children.Count > 0);

         var actChild = children[0];

         Assert.AreEqual(expParent.Name, actParent.Name);
         Assert.AreEqual(expParent.Value.start, actParent.Value.start);
         Assert.AreEqual(expParent.Value.length, actParent.Value.length);
         Assert.AreEqual(expChild.Name, actChild.Name);
         Assert.AreEqual(expChild.Value.start, actChild.Value.start);
         Assert.AreEqual(expChild.Value.length, actChild.Value.length);
      }

      [TestMethod]
      public void NamedValueNode( ) {
         var targetFile = "jargon_6.jss";
         NamedValue<(long start, long length)> expParent = ("Window", (73, 1));
         NamedValue<(long start, long length)> expChild = ("target", (80, 0));

         var parser = Parser.Create(fileMap, targetFile);
         var nodes = BuildRawNodeTree(parser);

         var actParent = nodes[0];
         var children = actParent.Nodes;
         var actChild = children[0];

      }

      private List<RawNode> BuildRawNodeTree(Parser parser) {
         return (List<RawNode>)parseRawNodes.Invoke(parser, new object[] { null });
      }
   }
}
