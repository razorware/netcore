using System.Linq;
using System.Collections;
using System.Collections.Generic;
using RazorWare.Serialization;

namespace RazorWare.Collections.Specialized {
   internal class BufferStack : IBufferStack, IByteStack {
      private readonly ByteStack bIter;
      private readonly Stack<byte[]> mStack;

      private byte[] mCurrent;

      public BufferStack(IEnumerable<byte[]> source) {
         mStack = new Stack<byte[]>(source.Reverse());
         bIter = new ByteStack();
         Index = StackState.Initial;
         State = StackState.Initial;
      }

      public sbyte State { get; private set; }
      byte[] IBufferStack.Current => mCurrent;
      public int Index { get; private set; }

      public int Element => bIter.Index;

      /// <summary>
      ///     Iterates the current buffer
      /// </summary>
      /// <returns></returns>
      public bool MoveNext( ) {
         switch (State) {
            case StackState.Disposed:
               return false;
            case StackState.Initial:
               if (NextBuffer())
                  return MoveNext();

               break;
            case StackState.Ready:
               return bIter.MoveNext();
         }
         return false;
      }

      /// <summary>
      ///     Advances to next buffer if available
      /// </summary>
      /// <returns></returns>
      public bool NextBuffer( ) {
         switch (State) {
            case StackState.Disposed:
               return false;
            case StackState.Initial:
               State = StackState.Ready;

               return NextBuffer();
            case StackState.Ready:
               if (mStack.Count == 0) {
                  Reset();

                  break;
               }

               mCurrent = mStack.Pop();
               bIter.Reset(mCurrent);
               ++Index;

               return true;
         }
         return false;
      }

      public bool Peek(out byte value) {
         return bIter.Peek(out value);
      }

      bool IBufferStack.Peek(out byte[] buffer) {
         buffer = null;

         if (mStack.Count > 0) buffer = mStack.Peek();

         return buffer != null;
      }

      public byte Current => bIter.Current;
      object IEnumerator.Current => mCurrent;

      public void Reset( ) {
         mStack.Clear();
         Index = State = StackState.Initial;
      }

      public void Dispose( ) {
         mStack.Clear();
         Index = State = StackState.Disposed;
      }

      public IEnumerator<byte> GetEnumerator( ) {
         return this;
      }

      IEnumerator IEnumerable.GetEnumerator( ) {
         return this;
      }

      public override string ToString( ) {
         return Token.Encoder.GetString(mCurrent);
      }
   }
}