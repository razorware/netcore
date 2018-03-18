using System.Linq;
using System.Collections;
using System.Collections.Generic;
using RazorWare.Serialization;

namespace RazorWare.Collections.Specialized {
   public class TokenStack : ITokenStack {
      private readonly Stack<Token> mStack;

      private sbyte mState;

      public TokenStack(IEnumerable<Token> source) {
         mStack = new Stack<Token>(source.Reverse());
         Index = StackState.Initial;
         mState = StackState.Initial;
      }

      public TokenStack(Stack<Token> tknStack) {
         //  make a copy so original is not consumed
         mStack = new Stack<Token>(tknStack);
         Index = StackState.Initial;
         mState = StackState.Initial;
      }

      public sbyte State => mState;
      public int Index { get; private set; }
      public Token Current { get; private set; }
      public Token Token => Current;
      public string Word => Current.Word();
      byte[] ITokenStack.Current => Current;
      object IEnumerator.Current => Current;
      public int Length => mStack.Count;

      public bool MoveNext( ) {
         switch (mState) {
            case StackState.Disposed:
               return false;
            case StackState.Initial:
               mState = StackState.Ready;

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

      public bool Peek(out Token token) {
         token = Token.Null;
         //var hasNext = mStack.Count > 0;

         if (mStack.Count > 0)
            token = mStack.Peek();

         return !Equals(token, Token.Null);
      }

      public void Reset( ) {
         mStack.Clear();
         Index = mState = StackState.Initial;
      }

      public void Dispose( ) {
         mStack.Clear();
         Index = mState = StackState.Disposed;
      }

      public IEnumerator<Token> GetEnumerator( ) {
         return this;
      }

      IEnumerator IEnumerable.GetEnumerator( ) {
         return this;
      }

      public override string ToString( ) {
         return Current.ToString();
      }
   }
}