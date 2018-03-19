using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Collections.Generic;
using RazorWare.Reflection;

namespace RazorWare.Dynamics {
   internal delegate TypeBuilder BuildType<TModel>( );

   public static class DynamicsExtensions {
      private static readonly Dictionary<Type, AssemblyBuilder> typeAsmBuilderMap = new Dictionary<Type, AssemblyBuilder>();
      private static readonly Dictionary<Type, TypeBuilder> typeTypeBuilderMap = new Dictionary<Type, TypeBuilder>();
      private static readonly Dictionary<Type, MethodInfo[]> typeMethodInfosMap = new Dictionary<Type, MethodInfo[]>() {
         { typeof(IProxy), typeof(IProxy).GetMethods() }
      };

      internal static BuildType<TModel> GetTypeBuilder<TModel>( ) {
         var type = typeof(TModel);

         BuildType<TModel> builder = ( ) => {
            if (!typeTypeBuilderMap.TryGetValue(type, out TypeBuilder typeBuilder)) {
               var asmBldr = GetAssemblyBuilder<TModel>();
               var moduleBuilder = asmBldr.DefineDynamicModule($"{type.Name}ProxyModule");

               typeTypeBuilderMap[type] = typeBuilder = moduleBuilder.DefineType(type.Name + "_Proxy", TypeAttributes.Public);
            }

            return typeBuilder;
         };

         builder.AddInterfaces(type, typeof(IProxy));

         return builder;
      }

      internal static BuildType<TModel> AddInterfaces<TModel>(this BuildType<TModel> builder, params Type[] interfaces) {
         foreach (var iType in interfaces) {
            builder().AddInterfaceImplementation(iType);
         }

         return builder;
      }

      internal static AssemblyInfo Assembly<TModel>(this BuildType<TModel> builder) {
         return new AssemblyInfo(builder().Name);
      }

      private static AssemblyBuilder GetAssemblyBuilder<TModel>( ) {
         var type = typeof(TModel);

         if (!typeAsmBuilderMap.TryGetValue(type, out AssemblyBuilder asmBuilder)) {
            var methodInfos = type.GetMethods();
            var asmName = new AssemblyName($"{type.Name}_Proxy");

            typeAsmBuilderMap[type] = asmBuilder = AssemblyBuilder.DefineDynamicAssembly(asmName, AssemblyBuilderAccess.Run);
         }

         return asmBuilder;
      }

      private static BuildType<TModel> BuildConstructor<TModel>(this BuildType<TModel> builder) {
         var ctorBuilder = builder().DefineConstructor(
          MethodAttributes.Public,
          CallingConventions.Standard,
          new Type[] { });

         var ilGenerator = ctorBuilder.GetILGenerator();
         ilGenerator.EmitWriteLine($"Creating {builder().Name} instance");

         ilGenerator.Emit(OpCodes.Ret);

         return builder;
      }
   }
}
