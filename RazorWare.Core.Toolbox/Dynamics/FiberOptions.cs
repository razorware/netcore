using System;
using System.Collections.Generic;
//
using Castle.DynamicProxy;

namespace RazorWare.Dynamics {
   public class FiberOptions : ProxyGenerationOptions {
      private readonly List<IInterceptor> interceptors = new List<IInterceptor>();

      private Func<object> target;


      public IInterceptor[] Interceptors {
         get { return interceptors.ToArray(); }
      }

      public object Target {
         get { return target(); }
      }

      public FiberOptions(params IInterceptor[] Interceptors) {
         interceptors.AddRange(Interceptors);
      }


      public void AddInterceptor(IInterceptor Interceptor) {
         interceptors.Add(Interceptor);
      }

      public void SetTarget<TTarget>(params object[] args) {
         SetTarget(typeof(TTarget), args);
      }

      public void SetTarget(Type TargetType, params object[] args) {
         target = ( ) => {
            return Activator.CreateInstance(TargetType, args);
         };
      }

      public void SetTarget(object Target) {
         target = ( ) => { return Target; };
      }
   }
}
