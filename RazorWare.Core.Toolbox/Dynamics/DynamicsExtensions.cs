using Castle.DynamicProxy;

namespace RazorWare.Dynamics {

   public static class DynamicsExtensions {

      public static bool IsCarbonProxy(this object target) {
         return (target as IProxyTargetAccessor) != null;
      }

      public static Template<TObject>.Mutator GetMutator<TObject>( ) {
         return ( ) => new Template<TObject>.Recombinator();
      }

      public static object Mutate<TObject>(this Template<TObject>.Mutator mutator) where TObject : class {
         return CarbonFactory.CreateObservableProxy(mutator());
      }

      public class Template<TObject> {
         public delegate IMutator<TObject> Mutator( );

         internal class Recombinator : IMutator<TObject> {
            private readonly FiberOptions options;

            public FiberOptions Options => options;

            internal Recombinator(FiberOptions fiberOptions = null) {
               options = fiberOptions ?? new FiberOptions();
               options.SetTarget<TObject>();
            }
         }
      }
   }
}
