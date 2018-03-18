using System;
using System.IO;
using System.Collections.Generic;
using RazorWare.Data.Annotations;

namespace RazorWare.Data {
   public interface ISchema {
      int Count { get; }
      IReadOnlyList<IField> Fields { get; }
      int Id { get; }
      int Size { get; }
      int Width { get; }

      ISchema AddType(DataType propType);
      ISchema AddType(DataType propType, Action<IField> config);
      ISchema AddTypes(params DataType[] dataTypes);
      ISchema Configure<TProp>(Func<IField[], TProp> func);
      IField ConfigureField(Func<IField[], IField> func);
      int Ordinal(IField field);
      byte Validate(out (IField prop, string msg)[] errProps);
      void WriteSchema(Stream buffer);
   }
}