namespace RazorWare.CoreDL.Internals {
   using RazorWare.CoreDL.Core;

   internal struct EventPumpState {

      internal static EventPumpState Running = (true, "Running");
      internal static EventPumpState Idle = (false, "Idle");

      private bool flag;

      public string Tag { get; }
      public int Id { get; }

      private EventPumpState(bool isRunning, string stateTag) {
         flag = isRunning;
         Tag = stateTag;
         Id = Tag.GetHashCode();
      }

      public static implicit operator EventPumpState((bool isRunning, string id) stateInfo) {
         return new EventPumpState(stateInfo.isRunning, stateInfo.id);
      }

      public static implicit operator EventPumpState(bool isRunning) {
         if (isRunning) {
               return Running;
         }

         return Idle;
      }

      public static implicit operator DispatchState(EventPumpState evState) {
         if (evState.flag) {
            return DispatchState.Running;
         }

         return DispatchState.Idle;
      }

      public static implicit operator EventPumpState(DispatchState dispState) {
         switch (dispState) {
            case DispatchState.Running:
               return Running;
            case DispatchState.Idle:
            default:
               return Idle;
         }
      }

      public static implicit operator string(EventPumpState state) {
         return state.Tag;
      }

      public static implicit operator bool(EventPumpState state) {
         return state.flag;
      }

      public static bool operator ==(EventPumpState state1, EventPumpState state2) {
         return state1.flag == state2.flag;
      }

      public static bool operator !=(EventPumpState state1, EventPumpState state2) {
         return state1.flag != state2.flag;
      }

      public override bool Equals(object obj) {
         if (!(obj is EventPumpState)) {
            return false;
         }

         return this == (EventPumpState)obj;
      }

      public override int GetHashCode( ) {
         return flag.GetHashCode();
      }
   }
}
