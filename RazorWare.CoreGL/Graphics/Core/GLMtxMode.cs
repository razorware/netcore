namespace RazorWare.CoreGL.Graphics.Core {
   public class GLMtxMode {
      public static GLMtxMode GL_MODELVIEW = new GLMtxMode(0x1700);
      public static GLMtxMode GL_MODELVIEW0_EXT = new GLMtxMode(0x1700);
      public static GLMtxMode GL_PROJECTION = new GLMtxMode(0x1701);
      public static GLMtxMode GL_TEXTURE = new GLMtxMode(0x1702);
      public static GLMtxMode GL_COLOR = new GLMtxMode(0x1800);

      private readonly int flag;

      private GLMtxMode(int value) {
         flag = value;
      }
   }
}
