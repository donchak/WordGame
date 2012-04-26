using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Collections;

namespace WordGame {
    class GameBitmap: GameMap<bool> {
        public GameBitmap(bool[][] bitmap)
            : base(bitmap) {
        }
        public override int GetHashCode() {
            uint hash = (uint)map.Length;
            for (int i = 0; i < Height; i++) {
                hash = (((hash << 8) & 0xFFFFFF00) | ((hash >> 24) & 0xFF)) ^ (uint)map[i].Length;
                int bitIndex = 0;
                uint bitMask = 0;
                for (int j = 0; j < Width; j++) {
                    if (map[i][j]) {
                        bitMask = bitMask | (1u << bitIndex++);
                    }
                    if (bitIndex >= 32) {
                        hash = (((hash << 8) & 0xFFFFFF00) | ((hash >> 24) & 0xFF)) ^ bitMask;
                        bitMask = 0;
                    }
                }
                if (bitIndex > 0) {
                    hash = (((hash << 8) & 0xFFFFFF00) | ((hash >> 24) & 0xFF)) ^ bitMask;
                }
            }
            return (int)hash;
        }
        public override bool Equals(object obj) {
            GameBitmap other = obj as GameBitmap;
            if (other == null) return false;
            if (Width != other.Width || Height != other.Height) return false;
            for (int i = 0; i < Height; i++) {
                for (int j = 0; j < Width; j++) {
                    if (map[i][j] != other.map[i][j]) return false;
                }
            }
            return true;
        }
    }
}
