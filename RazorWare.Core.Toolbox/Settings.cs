using System;
using System.IO;
using System.Data;
using System.Linq;
using System.Text;
using RzWare.IO;
using RzWare.Data;
using static RzWare.Data.DataExtensions;
using RzWare.Serialization;

namespace RzWare {
   public abstract class Settings<TDescendant> : Singleton<TDescendant, ISettings>, ISettings
      where TDescendant : ISettings {
      const string SettingsFile = "app.config.settings";
      const string SettingsName = "App.Config";
      const string SettingsPath = "Settings";
      const string SettingsSection = "Section";
      const string SettingsReadOnly = "ReadOnly";
      const string ApplicationSection = "Application";
      const string UserSection = "User";
      const string SectionState = "State";
      const string SettingsKey = "Key";
      const string SettingsValue = "Value";

      private static readonly string CurrentDirectory = Directory.GetCurrentDirectory();
      private static readonly FileMap fMap = FileMap.Instance;

      private static DataTable mSettingsTable;

      private readonly string mFilePath;
      private readonly string mFileSpec;
      private readonly IFile mFile;

      public bool HasSettingsFile { get; }
      public Encoding Encoder { get; set; } = Encoding.UTF8;

      protected ApplicationSettings Application { get; }
      protected UserSettings User { get; }

      /// <summary>
      /// Default Settings file configuration
      /// </summary>
      protected Settings( ) : this(SettingsFile) { }
      /// <summary>
      /// Settings file configuration
      /// </summary>
      /// <param name="settingsFile"></param>
      protected Settings(string settingsFile) {
         mFilePath = Path.Combine(CurrentDirectory, SettingsPath);
         if (!Directory.Exists(mFilePath)) {
            Directory.CreateDirectory(mFilePath);
         }

         mFileSpec = Path.Combine(mFilePath, settingsFile);
         mFile = fMap.Get(mFileSpec);
         mSettingsTable = LoadSettings(mFile);

         Application = new ApplicationSettings();
         User = new UserSettings();

         HasSettingsFile = mFile.Exists;
      }

      public void Save( ) {
         Save(mSettingsTable, mFile);
      }

      protected static void Save(Settings<TDescendant> settings) {
         Save(mSettingsTable, settings.mFile);
      }

      private static DataTable LoadSettings(IFile file) {
         var table = default(DataTable);

         using (var mStream = new MemoryStream(file.Read(file.Length))) {
            mStream.Seek(0, SeekOrigin.Begin);

            if (mStream.Length == 0) {
               table = CreateSettings();
               Save(table, file);
            }
            else {
               table = new DataTable(SettingsName);
               try {
                  table.ReadXml(mStream);
               }
               catch {
                  // there was a problem reading up the settings so we'll start over
                  table = CreateSettings();
                  Save(table, file);
               }
            }
         }

         return table;
      }

      private static void Save(DataTable table, IFile file) {
         file.Truncate();

         using (var memStream = new MemoryStream()) {
            table.WriteXml(memStream, XmlWriteMode.WriteSchema);
            memStream.Seek(0, SeekOrigin.Begin);

            file.Write(memStream.ToArray());
         }
      }

      private static DataTable CreateSettings( ) {
         var table = new DataTable(SettingsName)
            .WithColumns(new[] {
               new DataColumn(SettingsSection, typeof(string)),
               new DataColumn(SettingsReadOnly, typeof(bool)),
               new DataColumn(SectionState, typeof(DataState)),
               new DataColumn(SettingsPath, typeof(DataTable))
            })
            .WithData(new[] {
               new object[] { ApplicationSection, true, DataState.Unknown,
                  new DataTable(ApplicationSection)
                  .WithColumns(new [] {
                     new DataColumn(SettingsKey, typeof(string)),
                     new DataColumn(SettingsValue, typeof(byte[]))
                  })},
               new object[] { UserSection, false, DataState.Unknown,
                  new DataTable(UserSection)
                  .WithColumns(new [] {
                     new DataColumn(SettingsKey, typeof(string)),
                     new DataColumn(SettingsValue, typeof(byte[]))
                  })}
               });

         return table;
      }

      private static bool HasRow(DataTable table, Func<DataRow, bool> predicate) {
         return table.Rows.Cast<DataRow>()
            .Any(r => predicate(r));
      }

      private static DataRow GetRow(Func<DataRow, bool> predicate) {
         return GetRow(mSettingsTable, predicate);
      }

      private static DataRow GetRow(DataTable table, Func<DataRow, bool> predicate) {
         return table.Rows.Cast<DataRow>()
            .Where(r => predicate(r))
            .FirstOrDefault();
      }

      public class UserSettings : Section {

         internal UserSettings( ) : base(UserSection) { }

         public byte[] this[string key] {
            get {
               DataRow dataRow = GetRow(Settings, row => ((string)row[SettingsKey]).Equals(key));

               return dataRow != null ? (byte[])dataRow[SettingsValue] : new byte[0];
            }
            set {
               SetKey(key, value);
            }
         }
      }

      public class ApplicationSettings : Section {

         internal ApplicationSettings( ) : base(ApplicationSection) { }

         public byte[] this[string key] {
            get {
               DataRow dataRow = GetRow(Settings, row => ((string)row[SettingsKey]).Equals(key));

               return dataRow != null ? (byte[])dataRow[SettingsValue] : new byte[0];
            }
         }
      }

      public abstract class Section {
         private readonly DataTable mSectionSettings;

         protected DataTable Settings => mSectionSettings;

         protected Section(string sectionName) {
            var dataRow = GetRow(row => ((string)row[SettingsSection]).Equals(sectionName));
            mSectionSettings = (DataTable)dataRow[SettingsPath];
         }

         public bool HasKey(string key) {
            return HasRow(mSectionSettings, row => ((string)row[SettingsKey]).Equals(key));
         }

         public void SetKey<T>(string key, T val) {
            var value = default(byte[]);
            if (val is string) {
               value = Instance.Encoder.GetBytes(val as string);
            }
            else if(val is byte[]) {
               value = val as byte[];
            }
            else {
               value = val.Encode();
            }

            SetKey(key, value);
         }

         protected void SetKey(string key, byte[] value) {
            DataRow dataRow = GetRow(Settings, row => ((string)row[SettingsKey]).Equals(key));

            if (dataRow == null) {
               dataRow = Settings.NewRow();
               dataRow[SettingsKey] = key;
               Settings.Rows.Add(dataRow);
            }

            dataRow[SettingsValue] = value;

            dataRow.AcceptChanges();
         }
      }
   }
}
