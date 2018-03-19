using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Collections.Generic;
using RazorWare.Reflection;

namespace RazorWare.Dynamics {
   public delegate TypeBuilder BuildType<TModel>( );

   public static class DynamicsExtensions {
      private static readonly Dictionary<Type, AssemblyBuilder> typeAsmBuilderMap = new Dictionary<Type, AssemblyBuilder>();
      private static readonly Dictionary<Type, TypeBuilder> typeTypeBuilderMap = new Dictionary<Type, TypeBuilder>();

      public static BuildType<TModel> GetTypeBuilder<TModel>( ) {
         var type = typeof(TModel);

         return ( ) => {
            if (!typeTypeBuilderMap.TryGetValue(type, out TypeBuilder typeBuilder)) {
               var asmBldr = GetAssemblyBuilder<TModel>();
               var moduleBuilder = asmBldr.DefineDynamicModule($"{type.Name}ProxyModule");

               typeTypeBuilderMap[type] = typeBuilder = moduleBuilder.DefineType(type.Name + "_Proxy", TypeAttributes.Public);
               typeBuilder.AddInterfaceImplementation(type);
               typeBuilder.AddInterfaceImplementation(typeof(IProxy));
            }

            return typeBuilder;
         };
      }

      public static BuildType<TModel> AddInterfaces<TModel>(this BuildType<TModel> builder, params Type[] interfaces) {
         foreach (var i in interfaces) {
            builder().AddInterfaceImplementation(typeof(TModel));
         }

         return builder;
      }

      public static AssemblyInfo Assembly<TModel>(this BuildType<TModel> builder) {
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
   }
}
