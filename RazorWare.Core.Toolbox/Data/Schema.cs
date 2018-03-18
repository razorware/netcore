using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using RazorWare.Data.Annotations;
using Attribs = RazorWare.Data.Annotations.DataAttributes;

namespace RazorWare.Data {
   public class Schema : ISchema {
      internal const byte RC_OKAY = 0x00;
      internal const byte RC_ERROR = 0x01;

      private static Dictionary<string, ISchema> tableSchemaMap = new Dictionary<string, ISchema>();
      private static readonly Encoding Encoder = Encoding.UTF8;

      private readonly int id;
      private readonly List<Field> fields;

      /// <summary>
      /// Get the schema ID
      /// </summary>
      public int Id => id;
      /// <summary>
      /// Get number of properties in schema
      /// </summary>
      public int Count => fields.Count;
      /// <summary>
      /// Get the raw data size of all properties
      /// </summary>
      public int Size => fields.Sum(p => p.Size);
      /// <summary>
      /// Get total row width including property headers
      /// </summary>
      public int Width => fields.Count * DataType.TYPESIZE;

      /// <summary>
      /// Retrieves a read only list of fields in the current schema
      /// </summary>
      public IReadOnlyList<IField> Fields => fields.AsReadOnly();

      private Schema(int schemaId) {
         id = schemaId;
         fields = new List<Field>();
      }

      private ISchema FromType(Type model) {
         var props = model.GetProperties(BindingFlags.Public | BindingFlags.Instance);

         foreach(var p in props) {
            var tpCode = Type.GetTypeCode(p.PropertyType);
            DataType type = tpCode;

            if(type == DataType.None) {
               // we some things to figure out
            }

            Action<IField> config = (f) => f.Name = p.Name;
            AddType(type, config);
         }

         return this;
      }

      public static ISchema Initialize(int id) {
         return new Schema(id);
      }

      public static ISchema FromType(int id, Type model) {
         return new Schema(id)
            .FromType(model);
      }

      public static BuildSchema<TModel> FromType<TModel>(int id) {
         var schema = new Schema(id).FromType(typeof(TModel));

         return ( ) => schema;                  
      }

      public ISchema AddType(DataType fieldType) {
         fields.Add(new Field(fieldType));

         return this;
      }

      public ISchema AddType(DataType fieldType, Action<IField> config) {
         var property = new Field(fieldType);
         fields.Add(property);
         config(property);

         return this;
      }

      public ISchema AddTypes(params DataType[] fieldTypes) {
         if (fieldTypes.Length == 0) {
            return this;
         }

         fields.AddRange(fieldTypes.Select((sp, i) => new Field(sp)));

         return this;
      }

      public ISchema Configure<TProp>(Func<IField[], TProp> func) {
         var res = func(fields.ToArray());

         return this;
      }

      public IField ConfigureField(Func<IField[], IField> func) {
         return func(fields.ToArray());
      }

      public static ISchema FromTable(string name) {
         return tableSchemaMap[name];
      }

      public int Ordinal(IField field) {
         return fields.IndexOf((Field)field);
      }

      public byte Validate(out (IField prop, string msg)[] errFields) {
         var list = new List<(IField prop, string msg)>();
         var rc = RC_OKAY;

         foreach(var p in fields) {
            // first rule - Name cannot be null or empty
            if (string.IsNullOrEmpty(p.Name)) {
               list.Add((p, "Name is null"));
               rc = RC_ERROR;
            }
         }

         errFields = list.ToArray();

         return rc;
      }

      public void WriteSchema(Stream stream) {
         /*
            Schemas     [id(int), count(int), property[...](byte[6*])]
                          0        4           [int(+Name),int(+Name),string:64(+Name),string:512(+Name)]
          */
         using (var binWriter = new BinaryWriter(stream, Encoder, true)) {
            // write schema ID
            binWriter.Write(id);
            // write property count
            binWriter.Write(fields.Count);
            // write each property
            foreach(var f in fields) {
               // write type
               binWriter.Write((byte[])f.Type);
               // write attributes
               binWriter.Write((byte)f.Attributes);
               // write name (binary writer prfixes string w/ length)
               binWriter.Write(f.Name);
            }
         }
      }

      public override bool Equals(object obj) {
         if(!(obj is Schema)) {
            return false;
         }

         var other = (Schema)obj;

         return Id == other.Id &&
            fields.SequenceEqual(other.fields);
      }

      public override int GetHashCode( ) {
         return base.GetHashCode();
      }

      internal static void SetTable(Table table, ISchema schema) {
         tableSchemaMap[table.Name] = schema;
      }

      internal class Field : IField {
         private static int PID = 0;

         private int pid;
         private DataType dataType;
         private string name;
         private Attribs attribs;
         
         public string Name {
            get { return name; }
            set { name = value; }
         }
         public int Size {
            get { return dataType.Size; }
            set { dataType = DataType.Resize(dataType, value); }
         }
         public DataType Type => dataType;
         public Attribs Attributes {
            get { return attribs; }
            set { SetAttributes(value); }
         }
         public DataValue Data { get; set; }

         internal Field(DataType propType) : this() {
            dataType = propType;
         }

         internal Field(byte[] propInfo) : this() {
            using (var bReader = new BinaryReader(new MemoryStream(propInfo), Encoder)) {
               dataType = DataType.FromBinaryReader(bReader);
               attribs = Attribs.FromBinaryReader(bReader);
               name = bReader.ReadString();
            }
         }

         private Field( ) {
            pid = PID++;
         }

         internal Field Configure<TProp>(Func<Field, TProp> func) {
            func(this);

            return this;
         }

         public override bool Equals(object obj) {
            if(!(obj is Field)) {
               return false;
            }

            var other = (Field)obj;

            return pid == other.pid && 
               dataType == other.dataType;
         }

         public override int GetHashCode( ) {
            return pid.GetHashCode();
         }

         private void SetAttributes(Attribs propAttribs) {
            if(propAttribs.HasAttribute(Attribs.PKey)) {
               propAttribs |= Attribs.NotNull | Attribs.Unique | Attribs.Index;
            }
            else if (propAttribs.HasAttribute(Attribs.FKey)) {
               propAttribs |= Attribs.NotNull;
            }

            attribs = propAttribs;
         }
      }
   }
}
