using System;

namespace RazorWare.Core.Business {
   using RazorWare.CoreDL.Core;

   public class Application : Context {

      public static Application Default { get; private set; }

      public Application( ) : this("BusinessAppContext") { }
      public Application(string name) : base(name) {
         Default = this;
      }

   }
}
