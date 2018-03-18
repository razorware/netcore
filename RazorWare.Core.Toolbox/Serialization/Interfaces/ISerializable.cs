namespace RazorWare.Serialization {
   public interface ISerializable : ITokenized {
      byte TypeCode { get; }
   }
}
