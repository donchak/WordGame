using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace WordGame {
    class CommonHelper {
        public static bool ArraysAreEquals<T>(T[] a, T[] b) {
            if (a == b) return true;
            if (a == null || b == null) return false;
            if (a.Length != b.Length) return false;
            for (int i = 0; i < a.Length; i++) {
                if (!a[i].Equals(b[i])) return false;
            }
            return true;
        }
        public static byte[] GetResourceData(string name) {
            using (Stream resourceStream = typeof(CommonHelper).Assembly.GetManifestResourceStream(name)) {
                byte[] raw = new byte[resourceStream.Length];
                resourceStream.Read(raw, 0, raw.Length);
                return raw;
            }
        }
        public static Icon GetIconFromResource(string name) {
            using (Stream resourceStream = typeof(CommonHelper).Assembly.GetManifestResourceStream(name)) {
                return new Icon(resourceStream);
            }
        }
        public static Image GetColorImage(byte[] rawImage, Color color) {
            Image newImage;
            using (MemoryStream ms = new MemoryStream(rawImage)) {
                newImage = Image.FromStream(ms);
            }
            ColorPalette palette = newImage.Palette;
            int entriesCount = palette.Entries.Length;
            for (int i = 0; i < entriesCount; i++) {
                palette.Entries[i] = Color.FromArgb(
                    color.R * i / entriesCount,
                    color.G * i / entriesCount,
                    color.B * i / entriesCount);
            }
            newImage.Palette = palette;
            return newImage; 
        }
    }
}
