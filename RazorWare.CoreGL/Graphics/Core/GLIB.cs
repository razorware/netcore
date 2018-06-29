using System.Collections.Generic;

namespace RazorWare.CoreGL.Graphics.Core {
   public static class GLIB {
      //public const string PERSPECTIVE_FOV = "perspective_fov";
      //public const string LOOKAT_MATRIX = "lookat_matrix";

      //private static readonly Dictionary<string, Matrix4> matrices = new Dictionary<string, Matrix4>();

      //public static void ClearColor(float red, float green, float blue, float alpha) {
      //   GL.ClearColor(red, green, blue, alpha);
      //}

      //public static void Enable(GLEnable glEnable) {
      //   GL.Enable(glEnable);
      //}

      //public static void Viewport(int x, int y, int width, int height) {
      //   GL.Viewport(x, y, width, height);
      //}

      //public static void MatrixMode(GLMtxMode glMtxMode) {
      //   GL.MatrixMode(glMtxMode);
      //}

      ///// <summary>
      ///// Loads the matrix using the specified name; matrices are removed from cache when loaded.
      ///// </summary>
      ///// <param name="mtxName"></param>
      //public static void LoadMatrix(string mtxName) {
      //   // TODO: check matrices dict for matrix name
      //   var matrix = matrices[mtxName];
      //   GL.LoadMatrix(ref matrix);

      //   matrices.Remove(mtxName);
      //}

      ///// <summary>
      ///// Creates a matrix4 from perspective FoV.
      ///// </summary>
      ///// <param name="fovy"></param>
      ///// <param name="aspect"></param>
      ///// <param name="zNear"></param>
      ///// <param name="zFar"></param>
      ///// <returns>Returns the name of the matrix. Matrices are removed from cache when loaded</returns>
      //public static string CreatePerspectiveFieldOfView(float fovy, float aspect, float zNear, float zFar) {
      //   matrices[PERSPECTIVE_FOV] = Matrix4.CreatePerspectiveFieldOfView(fovy, aspect, zNear, zFar);

      //   return PERSPECTIVE_FOV;
      //}

      //public static string CreateLookAtMatrix(Vector3 eye, Vector3 target, Vector3 up) {
      //   matrices[LOOKAT_MATRIX] = Matrix4.LookAt(eye, target, up);

      //   return LOOKAT_MATRIX;
      //}

      //public static void Clear(ClearBufferMask gLClearBufferMask) {
      //   GL.Clear(gLClearBufferMask);
      //}
   }
}
