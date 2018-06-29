using System;

namespace RazorWare.CoreDL.Testing.CreateNativeWindow {
   using RazorWare.CoreDL.Internals;

   class Program {

      static void Main(string[] args) {
         Console.WriteLine("Hello EventPump!");

         var nativeWindow = new NativeWindow("Native Window");



         Console.WriteLine();
         Console.Write("Press any key to exit");
         Console.ReadKey();
      }

   }
}
