using System;

namespace RazorWare.Core.Media {
   using RazorWare.CoreDL.Core;

   public class Game : Context {
      private static Configuration configuration;

      public static Game Instance { get; private set; }

      public Game(string configFile) : base(LoadConfiguration(configFile)) {
         Instance = this;
      }

      private static string LoadConfiguration(string configFile) {
         return (configuration = Configuration.Load(configFile)).Name;
      }

      private class Configuration {
         private string config;

         public string Name { get; private set; }

         private Configuration(string configFile) {
            config = configFile;
         }

         internal static Configuration Load(string configFile) {
            configuration = new Configuration(configFile);


            return configuration;
         }
      }
   }
}
