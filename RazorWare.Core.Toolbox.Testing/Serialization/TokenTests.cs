using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RazorWare.Serialization;

namespace RazorWare.Toolbox.Testing.Serialization {
   [TestClass]
   public class TokenTests {
      private const byte Bassign = 61;
      private const char Cassign = '=';

      [TestMethod]
      public void AssignFromByte( ) {
         Token assign = Bassign;

         Assert.AreEqual(Cassign, assign);
      }

      [TestMethod]
      public void AssignFromChar( ) {
         Token assign = Cassign;

         Assert.AreEqual(Bassign, assign);
      }

      private static class _Tokens {
         private static readonly Token Assign = '=';
         private static readonly Token EndStm = ';';
         private static readonly Token Concat = ',';
         private static readonly Token MQuery = ':';
         private static readonly Token GetMny = '*';
         private static readonly Token BegMny = '{';
         private static readonly Token EndMny = '}';
         private static readonly Token BegGrp = '[';
         private static readonly Token EndGrp = ']';
         private static readonly Token VarRpl = '@';
         private static readonly Token Escape = '\\';

         internal static readonly List<Token> TokenList = new List<Token>
         {
                Assign,
                EndStm,
                Concat,
                MQuery,
                GetMny,
                BegMny,
                EndMny,
                BegGrp,
                EndGrp,
                VarRpl,
                Escape
            };
      }
   }
}