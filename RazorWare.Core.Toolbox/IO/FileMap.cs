using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using SysFile = System.IO.File;

namespace RazorWare.IO {
   public delegate void FileClosed(IFile File);
   public delegate void FileOpened(IFile File);

   public class FileMap : IDisposable {
      private static readonly Lazy<FileMap> instance = new Lazy<FileMap>(( ) => new FileMap());

      private readonly Dictionary<string, FileHandler> fMap;
      private readonly List<IFile> openFiles;

      public static FileMap Instance => instance.Value;

      private FileMap( ) {
         fMap = new Dictionary<string, FileHandler>();
         openFiles = new List<IFile>();
      }

      /// <summary>
      /// Create an unregistered file handler
      /// </summary>
      /// <param name="filePath">string: full file path</param>
      /// <returns>IFile: file handler interface</returns>
      public static FileInfo CreateFile(string filePath) {
         if (!new FileInfo(filePath).Exists) {
            using (var sys = SysFile.Create(filePath)) { }
         }

         return new FileInfo(filePath);
      }

      /// <summary>
      /// Determines if file path has a file extension.
      /// </summary>
      /// <param name="filePath">string: full file path</param>
      /// <returns>bool: TRUE if has extension; otherwise FALSE</returns>
      public static bool HasExtension(string filePath) {
         return !string.IsNullOrEmpty(new FileInfo(filePath).Extension);
      }

      /// <summary>
      /// Validates file path and initializes a file handler. This handler is not registered
      /// with FileMap event listeners.
      /// </summary>
      /// <param name="fileHandler"></param>
      /// <returns></returns>
      public static bool ValidateFileInfo(string filePath, out IFile fileHandler) {
         return ValidateFileInfo(new FileInfo(filePath), out fileHandler);
      }

      /// <summary>
      /// Validates file path and initializes a file handler. This handler is not registered
      /// with FileMap event listeners.
      /// </summary>
      /// <param name="fileHandler"></param>
      /// <returns></returns>
      public static bool ValidateFileInfo(FileInfo file, out IFile fileHandler) {
         if (!file.Exists) {
            throw new FileNotFoundException($"File not found: {file.FullName}");
         }

         fileHandler = new File(file);

         return fileHandler != null;
      }

      /// <summary>
      /// Recursive: iterates path to ensure directory structure exists; creates non-existing directories
      /// </summary>
      /// <param name="dirPath">Path to construct</param>
      /// <returns>TRUE if entire structure exists; otherwise FALSE outputing error message if applicable</returns>
      public static bool TryConstructPath(string dirPath, out string errMsg) {
         var dir = new DirectoryInfo(dirPath);
         errMsg = string.Empty;

         while (!dir.Exists) {
            if (TryConstructPath(dir.Parent.FullName, out errMsg)) {
               Directory.CreateDirectory(dir.FullName);

               dir = new DirectoryInfo(dir.FullName);
            }
            else {
               errMsg = $"Failed creating directory: {dirPath}";
               return false;
            }
         }

         return true;
      }

      public static void RegisterFileHandler(IFile file) {
         // already in fMap
         if (Instance.fMap.TryGetValue(file.Name, out FileHandler fHandler)) {
            return;
         }
         else {
            fHandler = (FileHandler)file;
         }

         fHandler.Opened += Instance.OnFileOpened;
         fHandler.Closed += Instance.OnFileClosed;
         Instance.fMap.Add(fHandler.Name, fHandler);
      }

      public static void UnregisterFileHandler(IFile file) {
         // remove from instance fMap
         Instance.fMap.Remove(file.Name);

         // unsubscribe file handler events
         ((FileHandler)file).Opened -= Instance.OnFileOpened;
         ((FileHandler)file).Closed -= Instance.OnFileClosed;
      }

      public static IFile Delete(IFile file) {
         ((FileHandler)file).DeleteFile(file.FullName);
         UnregisterFileHandler(file);

         return new File(new FileInfo(file.FullName));
      }

      public IFile Get(string filePath) {
         if (!SysFile.Exists(filePath)) {
            using (var fs = SysFile.Create(filePath)) { }
         }

         var fileInfo = new FileInfo(filePath);

         if (!fMap.TryGetValue(fileInfo.Name, out FileHandler fHandler)) {
            if (ValidateFileInfo(fileInfo, out IFile handler)) {
               RegisterFileHandler(handler);

               fHandler = (File)handler;
            }
         }

         return fHandler;
      }

      public static IFileEvents GetFileEvents(IFile file) {
         return new FileEvents((FileHandler)file);
      }

      public Stream Open(FileHandler file, FileMode fileMode, FileAccess fileAccess, FileOpened handler = null) {
         var fStream = (Stream)file.Open(fileMode, fileAccess);

         handler?.Invoke(file);

         return fStream;
      }

      public bool VerifyIsOpen(IFile file) {
         return openFiles.Contains(file);
      }

      public void Dispose( ) {
         int fIdx = 0;
         while (openFiles.Count > 0) {
            openFiles[fIdx].Dispose();
            openFiles.RemoveAt(fIdx);
         }

         while (fMap.Count > 0) {
            var fKV = fMap.First();
            fKV.Value.Closed -= OnFileClosed;
            fKV.Value.Opened -= OnFileOpened;

            fMap.Remove(fKV.Key);
         }
      }

      private void OnFileClosed(IFile file) {
         openFiles.Remove(file);
      }

      private void OnFileOpened(IFile file) {
         openFiles.Add(file);
      }

      private class File : FileHandler {
         internal File(FileInfo file) : base(file) { }

         internal static IFile Create(string filePath) {
            using (var fStream = SysFile.Create(filePath)) { }

            return new File(new FileInfo(filePath));
         }
      }

      private class FileEvents : IFileEvents {
         private readonly FileHandler mFile;

         public event FileClosed Closed {
            add { mFile.Closed += value; }
            remove { mFile.Closed -= value; }
         }

         public event FileOpened Opened {
            add { mFile.Opened += value; }
            remove { mFile.Opened -= value; }
         }

         internal FileEvents(FileHandler file) {
            mFile = file;
         }
      }
   }
}
