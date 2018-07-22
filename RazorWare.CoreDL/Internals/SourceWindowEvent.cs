namespace RazorWare.CoreDL.Core {
   using RazorWare.CoreDL.Internals;

   public class SourceWindowEvent {
      internal SDL_Event EventInfo { get; }

      public object Source { get; }
      public EventType Type { get; }

      public bool Resize { get; private set; }

      internal SourceWindowEvent(object source, SDL_Event sdlEvent) {
         EventInfo = sdlEvent;
         Source = source;
         Type = (EventType)EventInfo.type;

         ProcessSourceEvent();
      }

      private void ProcessSourceEvent() {
         switch (Type) {
            case EventType.Quit:
               break;
            case EventType.Window:
               ProcessWindowEvent(EventInfo.window);

               break;
            case EventType.SysWm:
               break;
            case EventType.KeyDown:
            case EventType.KeyUp:
               ProcessKeyboardEvent(Type, EventInfo.key);

               break;
            case EventType.TextEditing:
               break;
            case EventType.TestInput:
               break;
            case EventType.MouseMotion:
               break;
            case EventType.MouseButtonDown:
               break;
            case EventType.MouseButtonUp:
               break;
            case EventType.MouseWheel:
               break;
            case EventType.JStickAxisMotion:
               break;
            case EventType.JStickBallMotion:
               break;
            case EventType.JStickHatMotion:
               break;
            case EventType.JStickButtonDown:
               break;
            case EventType.JStickButtonUp:
               break;
            case EventType.JStickAdded:
               break;
            case EventType.JStickRemoved:
               break;
            case EventType.ControllerAxisMotion:
               break;
            case EventType.ControllerButtonDown:
               break;
            case EventType.ControllerButtonUp:
               break;
            case EventType.ControllerAdded:
               break;
            case EventType.ControllerRemoved:
               break;
            case EventType.ControllerRemapped:
               break;
            case EventType.FingerMotion:
               break;
            case EventType.FingerDown:
               break;
            case EventType.FingerUp:
               break;
            case EventType.DollarGesture:
               break;
            case EventType.DollarRecord:
               break;
            case EventType.MultiGesture:
               break;
            case EventType.ClipboardUpdate:
               break;
            case EventType.DropFile:
               break;
            case EventType.DropText:
               break;
            case EventType.DropBegin:
               break;
            case EventType.DropComplete:
               break;
            case EventType.AudioDeviceAdded:
               break;
            case EventType.AudioDeviceRemoved:
               break;
            case EventType.RenderTargetsReset:
               break;
            case EventType.RenderDeviceReset:
               break;
            case EventType.User:
               break;
            default:
               break;
         }
      }

      private void ProcessWindowEvent(SDL_WindowEvent window) {
         System.Console.WriteLine($"[SDL_Window.EVENT]: {window.windowEvent}");

         switch (window.windowEvent) {
            case SDL_Window.EVENT.NONE:
               break;
            case SDL_Window.EVENT.SHOWN:
               break;
            case SDL_Window.EVENT.HIDDEN:
               break;
            case SDL_Window.EVENT.EXPOSED:
               break;
            case SDL_Window.EVENT.MOVED:
               break;
            case SDL_Window.EVENT.RESIZED:
               Resize = true;

               break;
            case SDL_Window.EVENT.SIZE_CHANGED:
               break;
            case SDL_Window.EVENT.MINIMIZED:
               break;
            case SDL_Window.EVENT.MAXIMIZED:
               break;
            case SDL_Window.EVENT.RESTORED:
               break;
            case SDL_Window.EVENT.ENTER:
               break;
            case SDL_Window.EVENT.LEAVE:
               break;
            case SDL_Window.EVENT.FOCUS_GAINED:
               break;
            case SDL_Window.EVENT.FOCUS_LOST:
               break;
            case SDL_Window.EVENT.CLOSE:
               break;
            case SDL_Window.EVENT.TAKE_FOCUS:
               break;
            case SDL_Window.EVENT.HIT_TEST:
               break;
            default:
               break;
         }
      }
   }
}
