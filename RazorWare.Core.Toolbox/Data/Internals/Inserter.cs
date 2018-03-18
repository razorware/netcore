using System;
using System.Collections.Generic;
using RazorWare.Data.Annotations;
using Attribs = RazorWare.Data.Annotations.DataAttributes;

namespace RazorWare.Data {
   internal partial class Internals {
      /// <summary>
      /// Inserter has the sole responsibility of appending data to volatile cache.
      /// </summary>
      private class Inserter : IInserter {
         private readonly TimeStamp pid;
         private readonly ISchema mSchema;
         private readonly ICache mCache;
         private readonly Func<int> mIndexer;

         private int rowId;

         public int CurrentRowId => rowId;
         public string ErrMsg { get; private set; }
         public IReadOnlyList<IField> Fields => mSchema.Fields;

         private Inserter(ISchema schema, ICache cache) : this(schema, cache, null) { }
         private Inserter(ISchema schema, ICache cache, Func<int> indexer) {
            mSchema = schema;
            mCache = cache;
            rowId = mCache.RowCount - 1;
            mIndexer = indexer ?? (( ) => mCache.NextRowId);

            pid = TimeStamp.Now;
         }

         public void Insert(IIndexer indexer, object[] data) {
            if (!CheckConstraints(indexer, data, out DataValue key)) {
               // TODO: check error state - make this more robust
               throw new InvalidOperationException("data constraints failed check");
            }

            rowId = mCache.Append(data);
         }

         /* 
          * If data does not have an ID then a key is generated on the fly.
          * ***/
         private bool CheckConstraints(IIndexer indexer, object[] data, out DataValue key) {
            var passConstraints = true;
            key = new DataValue(DataType.Int, Attribs.PKey);

            var idx = 0;
            while (passConstraints && idx < data.Length) {
               var field = Fields[idx];
               IIndex index;

               if (field.Is(Attribs.PKey)) {
                  key = new DataValue(field.Type, field.Attributes);

                  if (field.Is(Attribs.AutoIncr)) {
                     // even if a real value is supplied, an AutoIncr attribute will
                     // overwrite with the appropriate value
                     if (data[idx] is DataValue) {
                        key = ((DataValue)data[idx]).Value(mIndexer());
                     }
                     else {
                        key = key.Value(mIndexer());
                     }

                     passConstraints = (data[idx] = key.Value()) != null;

                     // check indexer constraints - true if valid
                     if (indexer.TryGetIndex(key, out index)) {
                        passConstraints &= index.Add(mCache.NextRowId, key);
                     }
                  }
               }
               else if (field.Is(Attribs.AutoIncr)) {
                  // even if a real value is supplied, an AutoIncr attribute will
                  // overwrite with the appropriate value
                  if (data[idx] is DataValue) {
                     key = ((DataValue)data[idx]).Value(mIndexer());
                  }
                  else {
                     key = key.Value(mIndexer());
                  }

                  passConstraints = (data[idx] = key.Value()) != null;

                  // check indexer constraints - true if valid
                  if (indexer.TryGetIndex(key, out index)) {
                     passConstraints &= index.Add(mCache.NextRowId, key);
                  }
               }
               // check if assign default value
               if (field.Is(Attribs.Default)) {
                  data[idx] = field.Data.Value();
               }
               // check add Index
               if (!field.Is(Attribs.PKey) && field.Is(Attribs.Index)) {
                  key = new DataValue(field.Type, field.Attributes).Value(field.Name);
                  if (!indexer.TryGetIndex(key, out index)) {
                     // TODO: index error, missing index
                     passConstraints &= false;

                     break;
                  }

                  key = key.Value(data[idx]);
                  passConstraints &= index.Add(mCache.NextRowId, key);
               }

               ++idx;
            }

            if (!passConstraints) {
               passConstraints = (key = key.Value(mIndexer())).Value() != null;
            }

            return passConstraints;
         }

         public override bool Equals(object obj) {
            if (!(obj is Inserter)) {
               return false;
            }

            var other = (Inserter)obj;

            return pid == other.pid;
         }

         public override int GetHashCode( ) {
            return pid.GetHashCode();
         }
      }

      internal interface IInserter {
         int CurrentRowId { get; }
         string ErrMsg { get; }
         IReadOnlyList<IField> Fields { get; }

         void Insert(IIndexer indexer, object[] data);
      }
   }
}
