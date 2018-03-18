using System;

namespace RazorWare {
   public abstract class Singleton<TSingleton> {
      private static readonly Lazy<TSingleton> instance = new Lazy<TSingleton>((TSingleton)Activator.CreateInstance(typeof(TSingleton), true));

      public static TSingleton Instance => instance.Value;
   }

   public abstract class Singleton<TSingleton, TInterface> where TSingleton : TInterface {
      private static readonly Lazy<TInterface> instance = new Lazy<TInterface>((TSingleton)Activator.CreateInstance(typeof(TSingleton), true));

      public static TInterface Instance => instance.Value;
   }
}
