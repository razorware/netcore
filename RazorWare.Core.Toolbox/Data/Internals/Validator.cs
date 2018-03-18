using System;
using System.Reflection;
using System.Collections.Generic;
using RazorWare.Data.Annotations;
using static RazorWare.Data.Annotations.DataAttributes;

namespace RazorWare.Data {
   internal partial class Internals {
      private static readonly Type typValidator = typeof(Validator);
      private static readonly Type typInserter = typeof(Inserter);
      private static readonly Type typTblCache = typeof(TableCache);
      private static readonly Type typIndexer = typeof(Indexer);

      private static readonly Dictionary<Type, ConstructorInfo> typeCtorMap = new Dictionary<Type, ConstructorInfo>() {
         { typTblCache, typTblCache.GetConstructor(BindingFlags.NonPublic|BindingFlags.Instance, null, new []{ typeof(int) }, null) },
         { typValidator, typValidator.GetConstructor(BindingFlags.NonPublic|BindingFlags.Instance, null, new []{ typeof(ISchema) }, null) },
         { typInserter, typInserter.GetConstructor(BindingFlags.NonPublic|BindingFlags.Instance, null, new []{ typeof(ISchema), typeof(ICache) }, null) },
         { typIndexer, typIndexer.GetConstructor(BindingFlags.NonPublic|BindingFlags.Instance, null, new []{ typeof(ISchema) }, null) },

      };
      private static readonly Lazy<Internals> internals = new Lazy<Internals>(( ) => new Internals());

      internal static Internals Instance => internals.Value;

      private Internals( ) { }

      internal static ICache GetTableCache(int startRowId) {
         var cache = typeCtorMap[typTblCache].Invoke(new object[] { startRowId });

         return (ICache)cache;
      }

      internal static IInserter GetInserter(ISchema schema, ICache cache) {
         var inserter = typeCtorMap[typInserter].Invoke(new object[] { schema, cache });

         return (IInserter)inserter;
      }

      internal static IIndexer GetIndexer(ISchema schema) {
         var indexer = typeCtorMap[typIndexer].Invoke(new object[] { schema });

         return (IIndexer)indexer;
      }

      internal static IValidator GetValidator(ISchema schema) {
         var validator = typeCtorMap[typValidator].Invoke(new object[] { schema });

         return (IValidator)validator;
      }

      /// <summary>
      /// Validator has the job of checking incoming data and acting on table constraints.
      /// </summary>
      private class Validator : IValidator {
         private readonly ISchema mSchema;

         public IReadOnlyList<IField> Fields => mSchema.Fields;
         public string ErrMsg { get; private set; }

         private Validator(ISchema schema) {
            mSchema = schema;
         }

         public bool Validate(params object[] data) {
            ErrMsg = string.Empty;
            var pass = true;

            if (data.Length != Fields.Count) {
               ErrMsg = $"Parameter count mismatch [{data.Length} != {Fields.Count}]";

               pass = false;
            }

            var idx = 0;
            while (pass && idx < Fields.Count) {
               // get field[idx]
               var fld = Fields[idx];
               // get data[idx]
               var param = data[idx];

               if (fld.Attributes.HasAttribute(NotNull) && param == null) {
                  pass = false;
                  ErrMsg = $"[{fld.Name}] Parameter constraint failed '{NotNull}' [{idx} :: {fld.Attributes}]";
               }
               else if (param == null) {
                  // Null allowed by absence of 'NotNull' attribute
                  pass = true;
                  ++idx;

                  continue;
               }
               if (param is DataValue) {
                  if (pass && !(pass &= fld.Type == ((DataValue)param).Type)) {
                     ErrMsg = $"[{fld.Name}] Parameter type mismatch [{idx} :: {((DataValue)param).Type} != {fld.Type}]";
                  }
               }
               else {
                  if (pass && !(pass &= fld.Type == (DataType)param.GetType())) {
                     ErrMsg = $"[{fld.Name}] Parameter type mismatch [{idx} :: {param.GetType().Name} != {fld.Type}]";
                  }
               }
               if (pass && (param is string) && !(pass &= ((string)param).Length <= fld.Size)) {
                  ErrMsg = $"[{fld.Name}] Parameter constraint failed 'Length' [{idx} :: {((string)param).Length} != {fld.Size}]";
               }

               ++idx;
            }

            return pass;
         }
      }

      internal interface IValidator {
         IReadOnlyList<IField> Fields { get; }
         string ErrMsg { get; }
         
         bool Validate(params object[] data);
      }
   }
}
