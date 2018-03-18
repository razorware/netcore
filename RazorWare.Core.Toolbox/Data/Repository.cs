using System;
using System.Linq;
using System.Collections.Generic;
using static RazorWare.Data.Table;

namespace RazorWare.Data {
   public delegate Repository<TModel> RepoHandler<TModel>( );
   public delegate Table QueryProvider( );

   public static class Repositories {

      public static RepoHandler<TModel> Using<TModel>(Table table) {
         return ( ) => new Repository<TModel>(table);
      }

      public static QueryProvider Using(Table table) {
         return () => table;
      }

      public static IEnumerable<TableRow> Select(this QueryProvider provider) {
         var iterator = provider().Rows().GetEnumerator();
         while (iterator.MoveNext()) {
            yield return iterator.Current;
         }
      }
   }

   public class Repository<TModel> {
      private static readonly Dictionary<Type, ISchema> modelMaps = new Dictionary<Type, ISchema>();

      private readonly Table mTable;

      internal Repository(Table table) {
         modelMaps.Add(typeof(TModel), Schema.FromType<TModel>(0)());

         mTable = table;
      }
   }
}
