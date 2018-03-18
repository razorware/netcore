using RazorWare.Data.Annotations;

namespace RazorWare.Data {
   public interface IField {
      DataAttributes Attributes { get; set; }
      string Name { get; set; }
      int Size { get; set; }
      DataType Type { get; }
      DataValue Data { get; set; }
   }
}