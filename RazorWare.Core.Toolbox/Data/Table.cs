using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using static RazorWare.Data.Internals;
using RazorWare.IO;

namespace RazorWare.Data {
   public class Table {
      private static readonly Dictionary<ICache, int> rowIdMap = GetRowIdMap();

      private static uint Mark = 0;

      private readonly ISchema mSchema;
      private readonly ICache cache;
      private readonly IValidator validator;
      private readonly IInserter inserter;
      private readonly IIndexer indexer;

      private string mName;
      private bool hasErrors;
      private string file;

      public string Name {
         get { return mName; }
         set {
            mName = value;
            file = $"{mName.ToLower()}.dat";
         }
      }
      public bool HasErrors => hasErrors;
      public int RowCount => cache.RowCount;

      public Table(ISchema schema, int startRowId = 1) : this($"Table_{Mark++}", schema, startRowId) {
         /* It is up to developers to use tables properly. Naming conventions
          * should be implemented in order to prevent table name clashes if tables
          * are persisted in files.
          * ***/
      }
      public Table(string name, ISchema schema, int startRowId) {
         mName = name;
         mSchema = schema;

         if (mSchema != null) {
            hasErrors = false;

            indexer = GetIndexer(schema);
            cache = GetTableCache(startRowId);
            validator = GetValidator(schema);
            inserter = GetInserter(schema, cache);
         }

      }

      public void Clear( ) {
         // TODO: check for dirty records and return enumerated methods/procs to clear

         cache.Clear();
      }

      public bool Insert(object[] parameters) {
         var written = false;

         if ((written = validator.Validate(parameters))) {
            var start = cache.RowCount;
            inserter.Insert(indexer, parameters);
            // was record written???
            written = (cache.RowCount - start) == 1;
         }

         return written;
      }

      public void SetRowId(int newRowId) {
         rowIdMap[cache] = newRowId;
      }

      public IEnumerable<TableRow> Rows( ) {
         foreach(var row in cache) {
            yield return new TableRow(this, row);
         }
      }

      public virtual void Save( ) {
         // create/open/truncate data file
         using (var f = FileMap.Instance.Get(file))
         using (var s = FileMap.Instance.Open((FileHandler)f, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write)) {
            if(s.Length > 0) {
               s.SetLength(0);
            }

            // write schema to file
            mSchema.WriteSchema(s);
         }
      }

      public void Save(string fileName) {
         file = fileName;
         Save();
      }

      private static Dictionary<ICache, int> GetRowIdMap( ) {
         // via reflection, retrieve Dictionary<ICache, int> tblRowIdMap from Internals
         var internals = typeof(Internals);
         var field = internals.GetField("tblRowIdMap", BindingFlags.NonPublic | BindingFlags.Static);

         return (Dictionary<ICache, int>)field.GetValue(Instance);
      }

      private IReadOnlyList<string> Columns( ) {
         return mSchema.Fields
            .Select(f => f.Name)
            .ToArray();
      }

      public class TableRow {
         private readonly Dictionary<string, int> mColumns = new Dictionary<string, int>();
         private readonly object[] mRow;

         public object this[int ordinal] => mRow[ordinal];
         public object this[string name] => mRow[mColumns[name]];
         public int ColCount => mRow.Length;

         internal TableRow(Table table, object[] row) {
            mColumns = PopulateColumns(table);
            mRow = row;
         }

         private Dictionary<string, int> PopulateColumns(Table table) {
            return table.Columns()
               .Select((c, i) => new {
                  col = c,
                  idx = i
               })
               .ToDictionary(k => k.col, v => v.idx);
         }
      }
   }
}
