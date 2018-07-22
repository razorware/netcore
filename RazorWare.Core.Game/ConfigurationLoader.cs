using System;

namespace RazorWare.Core.Media {
   using RazorWare.CoreDL.Core;
   using RazorWare.Data;

   internal class ConfigurationLoader {
      private readonly IDataReader reader;

      public static ConfigurationLoader Empty => new ConfigurationLoader(null);

      public string Name => reader != null ? reader.Name : "<null>";

      public ConfigurationLoader(IDataReader dataReader) {
         reader = dataReader;
      }

      public Game.Configuration Load( ) {
         if (!reader.IsLoaded) {
            reader.Load();
         }

         var config = Game.Configuration.Empty;

         try {
            Game.Configuration.Window.Title = reader.Get<string>("window.title");
            Game.Configuration.Window.Width = reader.Get<int>("window.width");
            Game.Configuration.Window.Height = reader.Get<int>("window.height");
            Game.Configuration.Window.State = reader.Get<WindowState>("window.state");

            config = new Game.Configuration(this);
         }
         catch (Exception) {
            throw;
         }

         return config;
      }
   }
}
