using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace RazorWare.Testing._classes {
   public static class TestProxyGenerator {
      public static T GetProxyFor<T>( ) {
         Type typeOfT = typeof(T);

         AssemblyName assName = new AssemblyName("testAssembly");
         var asmBuilder = AssemblyBuilder.DefineDynamicAssembly(assName, AssemblyBuilderAccess.Run);
         var moduleBuilder = asmBuilder.DefineDynamicModule("testModule");

         var typeBuilder = moduleBuilder.DefineType(typeOfT.Name + "Proxy", TypeAttributes.Public);
         typeBuilder.AddInterfaceImplementation(typeOfT);

         var ctorBuilder = typeBuilder.DefineConstructor(
                   MethodAttributes.Public,
                   CallingConventions.Standard,
                   new Type[] { });
         var ilGenerator = ctorBuilder.GetILGenerator();
         ilGenerator.EmitWriteLine("Creating Proxy instance");
         ilGenerator.Emit(OpCodes.Ret);


         var methodInfos = typeOfT.GetMethods();
         foreach (var methodInfo in methodInfos) {
            var methodBuilder = typeBuilder.DefineMethod(
                methodInfo.Name,
                MethodAttributes.Public | MethodAttributes.Virtual,
                methodInfo.ReturnType,
                methodInfo.GetParameters().Select(p => p.GetType()).ToArray()
                );
            var methodILGen = methodBuilder.GetILGenerator();
            if (methodInfo.ReturnType == typeof(void)) {
               methodILGen.Emit(OpCodes.Ret);
            }
            else {
               if (methodInfo.ReturnType.IsValueType || methodInfo.ReturnType.IsEnum) {
                  MethodInfo getMethod = typeof(Activator).GetMethod("CreateInstance",
                     new Type[] { typeof(Type) });
                  LocalBuilder lb = methodILGen.DeclareLocal(methodInfo.ReturnType);
                  methodILGen.Emit(OpCodes.Ldtoken, lb.LocalType);
                  methodILGen.Emit(OpCodes.Call, typeof(Type).GetMethod("GetTypeFromHandle"));
                  methodILGen.Emit(OpCodes.Callvirt, getMethod);
                  methodILGen.Emit(OpCodes.Unbox_Any, lb.LocalType);
               }
               else {
                  methodILGen.Emit(OpCodes.Ldnull);
               }
               methodILGen.Emit(OpCodes.Ret);
            }

            typeBuilder.DefineMethodOverride(methodBuilder, methodInfo);
         }

         Type constructedType = typeBuilder.CreateType();
         var instance = Activator.CreateInstance(constructedType);
         return (T)instance;
      }
   }
}
