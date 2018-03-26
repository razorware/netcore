using System;
using System.Reflection;
using System.Collections.Generic;

using RazorWare.Dynamics;

namespace RazorWare.Reflection {
   public class ReflectorEngine {
      private static readonly Lazy<ReflectorEngine> instance = new Lazy<ReflectorEngine>(( ) => new ReflectorEngine());
      private static readonly Dictionary<TypeData, ModelCache> typeModelMap = new Dictionary<TypeData, ModelCache>();
      
      public static ReflectorEngine Default {
         get { return instance.Value; }
      }

      public void Clear( ) {
         // reset/clear all cached models
         typeModelMap.Clear();
      }

      public IModelCache Register<TModel>( ) {
         var type = typeof(TModel);
         if (!typeModelMap.TryGetValue(type.GetTypeData(), out ModelCache modelCache)) {
            modelCache = new ModelCache(type);
            typeModelMap.Add(modelCache.TypeData, modelCache);
         }

         return modelCache;
      }

      public bool TryGetModelCache<TModel>(out IModelCache model) {
         var result = typeModelMap.TryGetValue(typeof(TModel).GetTypeData(), out ModelCache modelCache);
         model = modelCache;

         return result;
      }

      public static TypeData GetTypeData<TType>( ) {
         return typeof(TType).GetTypeData();
      }
   }
}
