using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Collections;

namespace WordGame {
    class GameObject {
        GameBitmap bitmap;
        public int Width { get { return bitmap.Width; } }
        public int Height { get { return bitmap.Height; } }
        public GameBitmap Bitmap { get { return bitmap; } }
        public GameObject(GameBitmap bitmap) {
            if (bitmap == null) throw new ArgumentNullException();
            this.bitmap = bitmap;
        }
    }
}