namespace RazorWare.IO {
   public static class Extensions {

      public static void Delete(this IFile file, out IFile deleted) {
         deleted = FileMap.Delete(file);
      }
   }
}
