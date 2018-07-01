namespace RazorWare.CoreDL.Internals {
   internal struct EventPumpState {

      internal static EventPumpState Running = (true, "Running");
      internal static EventPumpState Idle = (false, "Idle");

      private bool flag;
      private string id;

      private EventPumpState(bool isRunning, string stateId) {
         flag = isRunning;
         id = stateId;
      }

      public static implicit operator EventPumpState((bool isRunning, string id) stateInfo) {
         return new EventPumpState(stateInfo.isRunning, stateInfo.id);
      }

      public static implicit operator EventPumpState(bool isRunning) {
         EventPumpState evState;

         switch (isRunning) {
            case true:
               evState = Running;

               break;
            case false:
            default:
               evState = Idle;

               break;
         }

         return evState;
      }

      public static implicit operator string (EventPumpState state) {
         return state.id;
      }

      public static implicit operator bool (EventPumpState state) {
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
