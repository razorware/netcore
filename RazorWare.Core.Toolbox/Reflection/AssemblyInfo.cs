using System;
using System.Linq;
using System.Reflection;

namespace RazorWare.Reflection {
   public class AssemblyInfo {
      private readonly Assembly mAssembly;

      public string Name => mAssembly.GetName().Name;
      public string FullName => mAssembly.FullName;

      public AssemblyInfo(string assemblyName) {
         mAssembly = GetAssembly(assemblyName);
      }

      public Type[] TypesDescendingFrom(Type ancestorType) {
         return mAssembly.GetTypes()
            .Where(t => t != ancestorType)
            .Where(t => !t.IsInterface)
            .Where(t => !t.IsAbstract)
            .Where(t => ancestorType.IsAssignableFrom(t))
            .ToArray();
      }

      private static Assembly GetAssembly(string assemblyName) {
         return AppDomain.CurrentDomain.GetAssemblies()
            .Where(asm => asm.GetName().Name.Equals(assemblyName))
            .FirstOrDefault();
      }
   }
}
