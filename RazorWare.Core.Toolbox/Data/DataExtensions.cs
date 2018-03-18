using System;
using System.Linq.Expressions;
using RazorWare.Data.Annotations;
using static RazorWare.Data.Internals;

namespace RazorWare.Data {
   public delegate ISchema BuildSchema<TModel>( );
   public delegate IField BuildField<TProperty>( );

   public static class DataExtensions {

      public enum DataState {
         Unknown = -1,
         Dirty = 0,
         Ready = 1
      }

      public static bool Is(this IField field, DataAttributes attribs) {
         return HasAttribute(field.Attributes, attribs);
      }

      public static bool HasAttribute(this DataAttributes dataAttribs, DataAttributes attribs) {
         return (dataAttribs & attribs) == attribs;
      }

      public static int TypeCode(this DataType dbType) {
         return dbType;
      }

      public static BuildField<TProperty> Property<TModel, TProperty>(this BuildSchema<TModel> build, Expression<Func<TModel, TProperty>> propExpr) {
         var schema = build();
         var member = propExpr.Body as MemberExpression;
         var prop = member.Member;
         
         return () => schema.ConfigureField(arr => Array.Find(arr, f => f.Name == prop.Name));
      }

      public static BuildField<TProperty> Configure<TProperty>(this BuildField<TProperty> propCfg, Action<IField> config) {
         config(propCfg());

         return propCfg;
      }

      // internal extension methods - IIndexer is an internal interface
      internal static bool TryGetIndex<TModel, TProperty>(this IIndexer indexer, Expression<Func<TModel, TProperty>> propExpr, out IIndex index) {
         index = null;
         // we can get the name of the property
         var member = propExpr.Body as MemberExpression;
         var prop = member.Member;

         return indexer.TryGetIndex(new DataValue(typeof(TProperty), DataAttributes.Index).Value(prop.Name), out index);
      }

      //public static IEnumerable<IField> Where(this IEnumerator<IField> iterator, Func<IField, bool> where) {
      //   while (iterator.MoveNext()) {
      //      if (where(iterator.Current)) {
      //         yield return iterator.Current;

      //         break;
      //      }
      //   }
      //}

      //public static IEnumerable<IField> Select(this IEnumerator<IField> iterator) {
      //   while (iterator.MoveNext()) {
      //      yield return iterator.Current;
      //   }
      //}

      //public static IEnumerable<TKey> Select<TKey>(this IEnumerator<IField> iterator, Func<IField, TKey> keySelector) {
      //   while (iterator.MoveNext()) {
      //      yield return keySelector(iterator.Current);
      //   }
      //}

      //public static IEnumerable<TKey> Select<TKey>(this IEnumerator<IField> iterator, Func<IField, TKey> keySelector, Func<TKey, bool> where) {
      //   while (iterator.MoveNext()) {
      //      var key = keySelector(iterator.Current);
      //      if (where(key)) {
      //         yield return key;
      //      }
      //   }
      //}
   }
}
