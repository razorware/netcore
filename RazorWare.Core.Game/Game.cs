using System;
using System.IO;
using System.Drawing;

namespace RazorWare.Core.Media {
   using RazorWare.CoreDL.Core;

   using RazorWare.Core.Media.IO;

   public class Game : Context {
      private const string JSON_EXT = ".json";
      private const string JARGON_EXT = ".jss";

      private static ConfigurationLoader loader;

      private Configuration configuration;
      private Window window;

      public static Game Instance { get; private set; }

      public string ConfigFile => configuration.File;

      public Game(string configFile) : base(GetConfigurationLoader(configFile)) {
         Instance = this;

         // if Name == null we can assume config file is invalid; otherwise check configuration assignment
         if (loader == null) {
            Console.WriteLine($"configuration file '{Path.Combine(Directory.GetCurrentDirectory(), configFile)}' not found");
         }

         configuration = loader.Load();
      }

      public override void Run( ) {
         Run(window = new Window());
      }

      private static string GetConfigurationLoader(string configFile) {
         try {
            return (loader = Configuration.Loader(configFile))?.Name;
         }
         catch (Exception ex) {
            Console.WriteLine(ex.Message);

            return (loader = ConfigurationLoader.Empty).Name;
         }
      }

      internal sealed class Window : GameWindow {
         internal Window( )
            : base(Configuration.Window.Width, Configuration.Window.Height, Configuration.Window.Title) {
            Background = Color.CornflowerBlue;
         }

         protected override void OnInitialized( ) {
            base.OnInitialized();

            SetWindowState(Configuration.Window.State);
         }

         protected override void OnWindowError(string Message) {
            Console.WriteLine(Message);
         }
      }

      internal class Configuration {
         public static Configuration Empty => new Configuration("", -1, -1) {
            File = string.Empty
         };

         internal string File { get; set; }
         internal sealed class Window {
            internal static string Title { get; set; }
            internal static int Width { get; set; }
            internal static int Height { get; set; }
            internal static WindowState State { get; set; }
         }
         internal class Player {
            public static string Name { get; internal set; }
         }

         private Configuration(string title, int width, int height) {
            Window.Title = title;
            Window.Width = width;
            Window.Height = height;
         }
         internal Configuration(ConfigurationLoader loader) {
            File = loader.Name;
         }

         internal static ConfigurationLoader Loader(string configFile) {
            if (!IsFile(configFile)) {

               return null;
            }
            var config = new FileInfo(configFile);
            var loader = default(ConfigurationLoader);

            switch (config.Extension) {
               case JSON_EXT:
                  Console.WriteLine($"loading '{JSON_EXT}' file");
                  loader = new ConfigurationLoader(new JsonReader(config));

                  break;
               case JARGON_EXT:
                  Console.WriteLine($"loading '{JARGON_EXT}' file");

                  //Loader = new ConfigurationLoader(new JargonReader(config));

                  //break;
                  throw new NotSupportedException($"'{JARGON_EXT}' file type not yet supported.");
               default:
                  throw new InvalidOperationException($"Invalid file type: {config.Extension}");

                  // TODO: -- base raise exception method --
                  //          Raise(new InvalidFileTypeException(config.Extension));
            }

            return loader;
         }

         private static bool IsFile(string configFile) {
            return System.IO.File.Exists(configFile);
         }
      }
   }
}
