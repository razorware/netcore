using System;
using System.Linq;
using System.Collections.Generic;

using RazorWare.Reflection;

namespace RazorWare.Dynamics {
   internal class ModelCache : IModelCache {
      private readonly TypeData type;
      private readonly IReadOnlyDictionary<string, TypeData> cache;


      public TypeData TypeData => type;
      public IReadOnlyList<string> PropertyNames => cache.Keys.ToArray();
      public IReadOnlyList<TypeData> PropertyTypes => cache.Values.ToArray();


      public ModelCache(Type modelType) {
         type = modelType.GetTypeData();
         cache = BuildModelCache();
      }


      private Dictionary<string, TypeData> BuildModelCache( ) {
         var propCache = new Dictionary<string, TypeData>();

         foreach (var p in type.Type.GetProperties()) {
            propCache[p.Name] = p.PropertyType.GetTypeData();
         }

         return propCache;
      }
   }
}
