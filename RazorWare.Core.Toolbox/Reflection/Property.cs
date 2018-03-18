using System.Reflection;

namespace RazorWare.Reflection {
   public delegate object ReadProperty(object model);
   public delegate void WriteProperty(object model, object value);

   public class Property {
      private readonly string mName;
      private readonly ReadProperty mRead;
      private readonly WriteProperty mWrite;
      private readonly TypeData mTypeData;

      public string Name => mName;
      public TypeData TypeData => mTypeData;
      public ReadProperty Read => mRead;
      public WriteProperty Write => mWrite;
      
      public Property(PropertyInfo propertyInfo) {
         mName = propertyInfo.Name;
         mTypeData = propertyInfo.PropertyType.GetTypeData();
         mRead = propertyInfo.GetValue;
         mWrite = propertyInfo.SetValue;
      }
   }
}
