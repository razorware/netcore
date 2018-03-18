namespace RazorWare.IO {
   public interface IFileEvents {
      event FileClosed Closed;
      event FileOpened Opened;
   }
}