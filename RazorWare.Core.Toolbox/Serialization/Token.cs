using System;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using RazorWare.Data.Comparers;

namespace RazorWare.Serialization {
   public struct Token : IReadOnlyList<byte> {
      private static readonly IEqualityComparer<byte[]> mComparer = new BufferComparer();

      private readonly byte[] mBuffer;

      public static readonly Token Null = new Token(-1, -1, null);

      public static Encoding Encoder = Encoding.UTF8;

      public byte this[int index] {
         get {
            if ((index < 0) || (index > Length)) {
               throw new ArgumentOutOfRangeException();
            }

            return mBuffer[index];
         }
      }

      //  TODO: test 'substring' function
      public string this[int start, int length] {
         get {
            if ((start < 0) || (start > Length) || (start + length > Length)) {
               throw new ArgumentOutOfRangeException();
            }

            return new string(Encoder.GetChars(mBuffer, start, length));
         }
      }

      public long Start { get; }
      public int Count => Length;
      public int Length { get; }
      public int Line { get; }

      public Token(long start, int line, byte[] buffer) {
         Start = start;
         Length = buffer?.Length ?? 0;
         Line = line;
         mBuffer = buffer ?? new byte[0];
      }

      public Token(string word) : this(-1, -1, Encoder.GetBytes(word)) { }

      public static implicit operator Token(byte b) {
         return new[] { b };
      }

      public static implicit operator Token(byte[] buffer) {
         return new Token(-1, -1, buffer);
      }

      public static implicit operator byte[] (Token tkn) {
         return tkn.mBuffer;
      }

      public static implicit operator Token(char c) {
         return Encoder.GetBytes(new[] { c });
      }

      public static implicit operator Token(char[] cBuffer) {
         return Encoder.GetBytes(cBuffer);
      }

      public static implicit operator Token(short value) {
         return BitConverter.GetBytes(value);
      }

      public static implicit operator Token(ushort value) {
         return BitConverter.GetBytes(value);
      }

      public static implicit operator Token(int value) {
         return BitConverter.GetBytes(value);
      }

      public static implicit operator Token(uint value) {
         return BitConverter.GetBytes(value);
      }

      public static implicit operator Token(float value) {
         return BitConverter.GetBytes(value);
      }

      public static implicit operator Token(long value) {
         return BitConverter.GetBytes(value);
      }

      public static implicit operator Token(ulong value) {
         return BitConverter.GetBytes(value);
      }

      public static implicit operator Token(double value) {
         return BitConverter.GetBytes(value);
      }

      public static implicit operator Token(decimal value) {
         return BitConverter.GetBytes((double)value);
      }

      public static implicit operator Token(DateTime value) {
         return BitConverter.GetBytes(value.Ticks);
      }

      //  == Token
      public static bool operator ==(Token tkn1, Token tkn2) {
         return BuffersEqual(tkn1, tkn2);
      }

      public static bool operator !=(Token tkn1, Token tkn2) {
         return !BuffersEqual(tkn1, tkn2);
      }

      //  == string
      public static bool operator ==(Token tkn1, string s1) {
         var other = Null;
         if (!string.IsNullOrEmpty(s1)) {
            other = Encoder.GetBytes(s1);
         }

         return BuffersEqual(tkn1, other);
      }

      public static bool operator !=(Token tkn1, string s1) {
         var other = Null;
         if (!string.IsNullOrEmpty(s1)) {
            other = Encoder.GetBytes(s1);
         }

         return !BuffersEqual(tkn1, other);
      }

      public static bool operator ==(string s1, Token tkn1) {
         return tkn1 == s1;
      }

      public static bool operator !=(string s1, Token tkn1) {
         return tkn1 != s1;
      }

      //  == byte
      public static bool operator ==(Token tkn1, byte b1) {
         return tkn1 == new[] { b1 };
      }

      public static bool operator !=(Token tkn1, byte b1) {
         return tkn1 != new[] { b1 };
      }

      public static bool operator ==(byte b1, Token tkn1) {
         return tkn1 == b1;
      }

      public static bool operator !=(byte b1, Token tkn1) {
         return tkn1 != b1;
      }

      //  == byte[]
      public static bool operator ==(Token tkn1, byte[] b1) {
         return BuffersEqual(tkn1.ToCharArray(), b1);
      }

      public static bool operator !=(Token tkn1, byte[] b1) {
         return !BuffersEqual(tkn1.ToCharArray(), b1);
      }

      public static bool operator ==(byte[] b1, Token tkn1) {
         return tkn1 == b1;
      }

      public static bool operator !=(byte[] b1, Token tkn1) {
         return tkn1 != b1;
      }

      //  == char
      public static bool operator ==(Token tkn1, char c1) {
         return tkn1 == new[] { c1 };
      }

      public static bool operator !=(Token tkn1, char c1) {
         return tkn1 != new[] { c1 };
      }

      public static bool operator ==(char c1, Token tkn1) {
         return tkn1 == c1;
      }

      public static bool operator !=(char c1, Token tkn1) {
         return tkn1 != c1;
      }

      //  == char[]
      public static bool operator ==(Token tkn1, char[] c1) {
         return BuffersEqual(tkn1.ToCharArray(), c1);
      }

      public static bool operator !=(Token tkn1, char[] c1) {
         return !BuffersEqual(tkn1.ToCharArray(), c1);
      }

      public static bool operator ==(char[] c1, Token tkn1) {
         return tkn1 == c1;
      }

      public static bool operator !=(char[] c1, Token tkn1) {
         return tkn1 != c1;
      }

      //  + Token
      public static Token operator +(Token tkn1, Token tkn2) {
         var buffer = new byte[tkn1.mBuffer.Length + tkn2.mBuffer.Length];
         var index = 0;

         Buffer.BlockCopy(tkn1.mBuffer, 0, buffer, index, tkn1.mBuffer.Length);
         index += tkn1.mBuffer.Length;
         Buffer.BlockCopy(tkn2.mBuffer, 0, buffer, index, tkn2.mBuffer.Length);

         return new Token(tkn1.Start, tkn1.Line, buffer);
      }

      public static Token operator +(Token tkn1, string s1) {
         var buffer = new byte[tkn1.mBuffer.Length + s1.Length];
         var index = 0;

         Buffer.BlockCopy(tkn1.mBuffer, 0, buffer, index, tkn1.mBuffer.Length);
         index += tkn1.mBuffer.Length;
         Buffer.BlockCopy(Encoder.GetBytes(s1), 0, buffer, index, s1.Length);

         return new Token(tkn1.Start, tkn1.Line, buffer);
      }

      //  utilities
      public static Token Join(string delimeter, params Token[] args) {
         var stack = new Stack<Token>(args.Reverse());
         var token = stack.Pop();

         while (stack.Count > 0) {
            token += delimeter;
            token += stack.Pop();
         }

         return token;
      }

      public string Word( ) {
         if (mBuffer.Length == 0)
            return DBNull.Value.ToString();

         return Encoder.GetString(mBuffer);
      }
      
      public bool Is<TType>(TType value) {
         if (mBuffer == null || mBuffer.Length == 0) {
            return false;
         }
         if (Equals(value)) {
            return true;
         }

         return mComparer.Equals(mBuffer, Type.GetTypeCode(typeof(TType)).Encode(value));
      }

      public override bool Equals(object obj) {
         if (!(obj is Token)) {
            return false;
         }

         return mComparer.Equals(mBuffer, ((Token)obj).mBuffer);
      }

      public override int GetHashCode( ) {
         return mComparer.GetHashCode(mBuffer);
      }

      public override string ToString( ) {
         return Word();
      }

      private string ToString(bool asFormatted) {
         return asFormatted ? $"({Start}:{Length}:{Line})::{Word()}" : ToString();
      }

      public IEnumerator<byte> GetEnumerator( ) {
         return mBuffer.OfType<byte>().GetEnumerator();
      }

      public char[] ToCharArray( ) {
         if (mBuffer == null || mBuffer.Length == 0) {
            throw new InvalidOperationException();
         }

         return Encoder.GetChars(mBuffer ?? Null.mBuffer);
      }

      IEnumerator IEnumerable.GetEnumerator( ) {
         return GetEnumerator();
      }

      private static bool BuffersEqual(Token tkn1, Token tkn2) {
         var pass = tkn1.mBuffer.Length == tkn2.mBuffer.Length;

         if (pass) {
            var idx = 0;
            while (idx < tkn1.mBuffer.Length) {
               pass &= tkn1.mBuffer[idx] == tkn2.mBuffer[idx];

               if (!pass) break;

               ++idx;
            }
         }

         return pass;
      }

      private static bool BuffersEqual(char[] tkn1, char[] tkn2) {
         var pass = tkn1.Length == tkn2.Length;

         if (pass) {
            var idx = 0;
            while (idx < tkn1.Length) {
               pass &= tkn1[idx] == tkn2[idx];

               if (!pass) break;

               ++idx;
            }
         }

         return pass;
      }
   }
}