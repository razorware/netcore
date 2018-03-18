using System.Collections.Generic;
using RazorWare.Data.Annotations;
using RazorWare.Data.Serialization;

namespace RazorWare.Data {
   internal static class ValueExtensions {
      private static Dictionary<DataType, string> dbTypesMap = new Dictionary<DataType, string>() {
         { DataType.None, "None" },
         { DataType.Null, "Null" },
         { DataType.Boolean, "Boolean" },
         { DataType.Byte, "Byte" },
         { DataType.SByte, "SignedByte" },
         { DataType.Char, "Char" },
         { DataType.Short, "Short" },
         { DataType.UShort, "UnsignedShort" },
         { DataType.Int, "Integer" },
         { DataType.UInt, "UnsignedInteger" },
         { DataType.Float, "Float" },
         { DataType.Long, "Long" },
         { DataType.ULong, "UnsignedLong" },
         { DataType.Double, "Double" },
         { DataType.TimeStamp, "TimeStamp" },
         { DataType.String, "String" },
         { DataType.Text, "Text" },
         { DataType.Blob, "Blob" }
      };
      private static Dictionary<Tag, string> datHeadersMap = new Dictionary<Tag, string>() {
         { Tag.TAG_UNKNOWN, "Unknown" },
         { Tag.TAG_SCHEMA, "Schema" },
         { Tag.TAG_CATALOG, "Catalog" },
         { Tag.TAG_DATA, "Data" }
      };
      private static Dictionary<DataAttributes, string> dbAttribsMap = new Dictionary<DataAttributes, string>() {
         { DataAttributes.Name, string.Empty },
         { DataAttributes.NotNull, "Not Null" },
         { DataAttributes.Default, "Default Value" },
         { DataAttributes.PKey, "Primary Key" },
         { DataAttributes.FKey, "Foreign Key" },
         { DataAttributes.Unique, "Unique" },
         { DataAttributes.Index, "Index" },
         { DataAttributes.AutoIncr, "Auto Increment" },
         { DataAttributes.VarLen, "Variable Length" }
      };

      internal static string Name(this DataType dbType) {
         return dbTypesMap[dbType];
      }

      internal static string Name(this Tag dbType) {
         return datHeadersMap[dbType];
      }

      internal static string Name(this DataAttributes dbAttrib) {
         // 'Name [0]' is a given; therefore, concrete application removes redundancy
         if(dbAttrib == 0x00) {
            return string.Empty;
         }

         var attribs = new List<string>();

         // since this is a flag we hav to find all values and concatenate the string
         foreach(var key in dbAttribsMap.Keys) {
            if(key == DataAttributes.Name) {
               continue;
            }
            else if((dbAttrib & key) == key) {
               attribs.Add(dbAttribsMap[key]);
            }
         }

         return $"{(string.Join(" | ", attribs.ToArray()))} [{(byte)dbAttrib}]";
      }
   }
}
