using System.Text;

namespace RazorWare {
   public interface ISettings {
      bool HasSettingsFile { get; }
      Encoding Encoder { get; set; }

      void Save( );
   }
}
