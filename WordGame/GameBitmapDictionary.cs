using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace WordGame {
    static class GameBitmapDictionary {
        static Dictionary<GameBitmap, string> bitmapUniqueDict = new Dictionary<GameBitmap, string>();
        static Dictionary<string, GameBitmap> bitmapNameDict = new Dictionary<string, GameBitmap>();
        public static GameBitmap GetBitmap(string name) {
            GameBitmap bitmap;
            if (!bitmapNameDict.TryGetValue(name, out bitmap)) return null;
            return bitmap;
        }
        public static string GetBitmapName(GameBitmap bitmap) {
            string name;
            if (!bitmapUniqueDict.TryGetValue(bitmap, out name)) return null;
            return name;
        }
        public static bool TryGetBitmap(string name, out GameBitmap bitmap) {
            return bitmapNameDict.TryGetValue(name, out bitmap);
        }
        public static bool TryGetBitmapName(GameBitmap bitmap, out string name) {
            return bitmapUniqueDict.TryGetValue(bitmap, out name);
        }
        public static bool AddBitmap(string name, GameBitmap bitmap) {
            if (bitmapUniqueDict.ContainsKey(bitmap)) return false;
            if (bitmapNameDict.ContainsKey(name)) return false;
            bitmapNameDict.Add(name, bitmap);
            bitmapUniqueDict.Add(bitmap, name);
            return true;
        }

        public static bool RemoveBitmap(string name) {
            GameBitmap bitmap;
            if (!bitmapNameDict.TryGetValue(name, out bitmap)) return false;
            bitmapUniqueDict.Remove(bitmap);
            bitmapNameDict.Remove(name);
            return true;
        }
        public static bool RemoveBitmap(GameBitmap bitmap) {
            string name;
            if (!bitmapUniqueDict.TryGetValue(bitmap, out name)) return false;
            bitmapUniqueDict.Remove(bitmap);
            bitmapNameDict.Remove(name);
            return true;
        }
    }
}
