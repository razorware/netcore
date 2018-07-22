using System;
using System.IO;

namespace RazorWare.Data {
   public interface IDataReader {
      string Name { get; }
      bool IsLoaded { get; }

      void Load( );
      TResult Get<TResult>(string jsonPath);
   }
}
