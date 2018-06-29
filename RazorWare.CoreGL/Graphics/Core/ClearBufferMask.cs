
namespace RazorWare.CoreGL.Graphics.Core {
   public class ClearBufferMask {

      //
      // Summary:
      //     Original was GL_NONE = 0
      public static ClearBufferMask GL_NONE = new ClearBufferMask(0);
      //
      // Summary:
      //     Original was GL_DEPTH_BUFFER_BIT = 0x00000100
      public static ClearBufferMask GL_DEPTH_BUFFER_BIT = new ClearBufferMask(0x00000100);
      //
      // Summary:
      //     Original was GL_ACCUM_BUFFER_BIT = 0x00000200
      public static ClearBufferMask GL_ACCUM_BUFFER_BIT = new ClearBufferMask(0x00000200);
      //
      // Summary:
      //     Original was GL_STENCIL_BUFFER_BIT = 0x00000400
      public static ClearBufferMask GL_STENCIL_BUFFER_BIT = new ClearBufferMask(0x00000400);
      //
      // Summary:
      //     Original was GL_COLOR_BUFFER_BIT = 0x00004000
      public static ClearBufferMask GL_COLOR_BUFFER_BIT = new ClearBufferMask(0x00004000);
      //
      // Summary:
      //     Original was GL_COVERAGE_BUFFER_BIT_NV = 0x00008000
      public static ClearBufferMask GL_COVERAGE_BUFFER_BIT_NV = new ClearBufferMask(0x00008000);

      private readonly int flag;

      private ClearBufferMask(int value) {
         flag = value;
      }

      public static ClearBufferMask operator |(ClearBufferMask glClrBuffMsk1, ClearBufferMask glClrBuffMsk2) {
         return new ClearBufferMask(glClrBuffMsk1.flag | glClrBuffMsk2.flag);
      }
   }
}
