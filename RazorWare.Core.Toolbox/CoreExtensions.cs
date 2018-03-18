using System;

namespace RazorWare {
   public delegate TType ReturnWith<TType>( );

   public static class CoreExtensions {

      public static ReturnWith<TType> With<TType>(this TType model) {
         return ( ) => model;
      }

      public static void Act<TType>(this ReturnWith<TType> with, Action<TType> action) {
         action(with());
      }

      public static ReturnWith<TType> ActOn<TType>(this ReturnWith<TType> with, Func<TType, TType> function) {
         return () => function(with());
      }
   }
}
