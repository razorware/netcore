using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace RazorWare.Collections.Specialized {
   internal class ByteStack : IByteStack, IBufferStack {
      private Stack<byte> mStack;

      public ByteStack(IEnumerable<byte> source) {
         mStack = new Stack<byte>(source.Reverse());
         Index = StackState.Initial;
         State = StackState.Initial;
      }

      internal ByteStack( ) {
         Index = StackState.Initial;
         State = StackState.Initial;
      }

      public ByteStack(Stack<byte> srcStack) {
         //  make a copy so original is not consumed
         mStack = new Stack<byte>(srcStack);
         Index = StackState.Initial;
         State = StackState.Initial;
      }

      byte[] IBufferStack.Current => new[] { Current };

      int IBufferStack.Element => Index;

      bool IBufferStack.NextBuffer( ) {
         return MoveNext();
      }

      bool IBufferStack.Peek(out byte[] buffer) {
         buffer = new byte[1];
         var success = Peek(out buffer[0]);

         return success;
      }

      public byte Current { get; private set; }

      object IEnumerator.Current => Current;
      public sbyte State { get; private set; }

      public int Index { get; private set; }

      public bool MoveNext( ) {
         switch (State) {
            case StackState.Disposed:
               return false;
            case StackState.Initial:
               State = StackState.Ready;

               return MoveNext();
            case StackState.Ready:
               if (mStack.Count == 0) {
                  Reset();
                  break;
               }

               Current = mStack.Pop();
               ++Index;

               return true;
         }

         return false;
      }

      public bool Peek(out byte value) {
         value = 0;

         if ((State == StackState.Ready) && (mStack.Count > 0)) {
            value = mStack.Peek();

            return true;
         }

         return false;
      }

      public void Reset( ) {
         mStack?.Clear();
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

      public void Reset(byte[] source) {
         Reset();
         mStack = new Stack<byte>(source.Reverse());
      }

      public override string ToString( ) {
         return $"{(char)Current}";
      }
   }
}