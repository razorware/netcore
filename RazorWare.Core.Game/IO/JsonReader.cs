using System;
using System.IO;

namespace RazorWare.Core.Media.IO {
   using Newtonsoft.Json.Linq;

   using RazorWare.Data;
   using RazorWare.CoreDL.Core;

   internal class JsonReader : IDataReader {
      private FileInfo config;
      private JObject json;

      public string Name => config.Name;
      public bool IsLoaded { get; private set; }

      public JsonReader(FileInfo cfgFile) {
         config = cfgFile;
      }

      public void Load( ) {
         using (var stream = config.Open(FileMode.Open, FileAccess.ReadWrite))
         using (var reader = new StreamReader(stream)) {
            var rawConfig = reader.ReadToEnd();
            json = JObject.Parse(rawConfig);
            IsLoaded = true;
         }
      }

      public TResult Get<TResult>(string jsonPath) {
         // Example: JToken acme = o.SelectToken("$.Manufacturers[?(@.Name == 'Acme Co')]");
         var token = json.SelectToken($"$.{jsonPath}");

         if (typeof(TResult) == typeof(WindowState)) {
            return (TResult)((object)token.Value<uint>());
         }

         return token.Value<TResult>();
      }
   }
}