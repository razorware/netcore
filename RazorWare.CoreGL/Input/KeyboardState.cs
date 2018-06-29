using System;
using System.Linq;

namespace RazorWare.CoreGL.Input {
   using RazorWare.CoreGL.Utility;

   public class KeyboardState {
      // Allocate bytes to store recognized keys
      const byte BitCount = 8;
      const byte NumBytes = (Key.NumKeys + BitCount) / BitCount;

      private byte[] keys = new byte[NumBytes];
      private ILogger logger;

      public byte[] KeyMask => keys;

      public void Logger(ILogger stateLogger) {
         logger = stateLogger;
      }

      public void SetKey(Key key, bool mask) {
         if (key == Key.Unknown) {
            return;
         }

         var keyInfo = WriteBit(key, mask);

         //logger.Log($"{($"Key[{key}:{(mask ? "DN" : "UP")}]"),-25}{string.Join("-", keys.Select(k => Convert.ToString(k, 2).PadLeft(8, '0')).ToArray())}");
         logger.Log($"{($"Key[{key}:{(mask ? "DN" : "UP")}]"),-25}{BitConverter.ToString(keys)}");
      }

      public bool IsKeyDown(Key key) {
         if (key == Key.Unknown) {
            return false;
         }

         return ReadBit(key);
      }


      private bool ReadBit(byte key) {
         if (keys.All(k => k == Key.Unknown)) {
            return false;
         }

         int index = key / BitCount;
         byte bit = (byte)((BitCount - 1) - key % BitCount);

         //logger.Log($"Reading [{index}:{bit}] :: ({keys[index]} & {(1 << bit)}) == {(keys[index] & (1 << bit))}");

         return (keys[index] & (1 << bit)) != 0u;
      }

      private (int, int) WriteBit(byte key, bool on) {
         int index = key / BitCount;
         byte bit = (byte)((BitCount - 1) - key % BitCount);

         //logger.Log($"Writing [{index}:{bit}] :: ({keys[index]} & {(1 << bit)}) == {(keys[index] & (1 << bit))}");

         if (on) {
            keys[index] |= (byte)(1 << bit);
         }
         else {
            keys[index] &= (byte)(~(1 << bit));
         }

         return (index, bit);
      }
   }
}
