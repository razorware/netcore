using System;

namespace RzWare.Testing._classes {
   public partial class AppSettings : Settings<AppSettings> {

      public static AppSettings Default {
         get { return (AppSettings)Instance; }
      }

      public AppSettings( ) {
         if (!Application.HasKey("SystemSetting")) {
            Application.SetKey("SystemSetting", 1000);
            Save();
         }
      }

      public string DefaultPath {
         get { return Encoder.GetString(User["DefaultPath"]); }
         set { User["DefaultPath"] = Encoder.GetBytes(value); }
      }

   }
}
