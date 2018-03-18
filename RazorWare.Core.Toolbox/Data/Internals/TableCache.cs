using System.IO;
using System.Collections.Generic;
using System.Collections;

namespace RazorWare.Data {
   internal partial class Internals {
      private static readonly Dictionary<ICache, int> tblRowIdMap = new Dictionary<ICache, int>();

      private class TableCache : ICache {
         private readonly long pid;
         private readonly List<object[]> mRecords;

         public int RowCount => mRecords.Count;
         public int NextRowId => tblRowIdMap[this];

         private TableCache(int startRowId) {
            pid = TimeStamp.Now;
            mRecords = new List<object[]>();
            tblRowIdMap.Add(this, startRowId);
         }

         public void Clear( ) {
            mRecords.Clear();
         }

         public int Append(object[] data) {
            mRecords.Add(data);

            return tblRowIdMap[this]++;
         }

         public BinaryReader ReadInterface( ) {
            throw new System.NotImplementedException();
         }

         public BinaryWriter WriteInterface( ) {
            throw new System.NotImplementedException();
         }

         public override bool Equals(object obj) {
            if (!(obj is TableCache)) {
               return false;
            }

            var other = (TableCache)obj;

            return pid == other.pid;
         }

         public override int GetHashCode( ) {
            return pid.GetHashCode();
         }

         public IEnumerator<object[]> GetEnumerator( ) {
            return mRecords.GetEnumerator();
         }

         IEnumerator IEnumerable.GetEnumerator( ) {
            return GetEnumerator();
         }

         private class DataWriter : BinaryWriter {
            //private readonly TableCache mCache;

            //internal DataWriter(TableCache cache) : base (cache.memory, Encoding.UTF8, true) {
            //   mCache = cache;
            //}

            //public override void Write(byte[] buffer) {
            //   base.Write(buffer);
            //   mCache.lastWriteEnd = BaseStream.Position;
            //}
         }
      }

      internal interface ICache : IEnumerable<object[]> {
         int RowCount { get; }
         int NextRowId { get; }

         /// <summary>
         /// Appends data to collection and advances the next row identifier
         /// </summary>
         /// <param name="data">The data</param>
         /// <returns>Current row id for inserted data</returns>
         int Append(object[] data);
         void Clear( );
      }
   }
}
