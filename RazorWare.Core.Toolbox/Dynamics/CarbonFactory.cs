using System;
using Castle.DynamicProxy;
//
using RazorWare.Reflection;

namespace RazorWare.Dynamics {
   internal static class CarbonFactory {
      private static readonly Lazy<ProxyGenerator> proxyEngine = new Lazy<ProxyGenerator>(( ) => new ProxyGenerator(true));
      private static readonly ReflectorEngine reflector = ReflectorEngine.Default;


      private static ProxyGenerator ProxyEngine {
         get { return proxyEngine.Value; }
      }


      internal static object CreateObservableProxy<TObject>(this IMutator<TObject> mutator) where TObject : class {
         return ProxyEngine
             .CreateClassProxyWithTarget(typeof(TObject),
                 new Type[] {
                        typeof(ICarbonProxy),
                 }, mutator.Options.Target, mutator.Options, mutator.Options.Interceptors);
      }

      //internal static ICarbonCollection CreateCarbonCollectionProxy<TObject>(Mutator mutator) {
      //   return ProxyEngine
      //       .CreateInterfaceProxyWithTarget(typeof(ICarbonCollection),
      //           new Type[] {
      //                  typeof(IList<TObject>),
      //                  typeof(ICollection<TObject>),
      //                  typeof(ISortable),
      //           }, mutator.Settings.Target, mutator.Settings, mutator.Settings.Interceptors) as ICarbonCollection;
      //}

      //private static IModelCache GetReflectionMetadata<TModel>( ) {
      //   var metadata = default(IModelCache);
      //   if (!reflector.TryGetModelCache<TModel>(out metadata)) {
      //      metadata = reflector.Register<TModel>();
      //   }

      //   return metadata;
      //}

      //private static Type GetItemType<TItem>(this IEnumerable<TItem> _) {
      //   return typeof(TItem);
      //}

      //private static object ToConcreteList<TCollection>( ) where TCollection : IEnumerable {
      //   var type = typeof(TCollection).GetGenericArguments()[0];
      //   var listType = typeof(List<>);
      //   var genericListType = listType.MakeGenericType(type);

      //   return Activator.CreateInstance(genericListType);
      //}

      //private static object ToConcreteList(Type ListType) {
      //   var type = ListType.GetGenericArguments()[0];
      //   var listType = typeof(List<>);
      //   var genericListType = listType.MakeGenericType(type);

      //   return Activator.CreateInstance(genericListType);
      //}

      //private static Type ToGenericIListType<TCollection>( ) where TCollection : IEnumerable {
      //   var type = typeof(TCollection).GetGenericArguments()[0];
      //   var listType = typeof(IList<>);
      //   return listType.MakeGenericType(type);
      //}

   }
}
