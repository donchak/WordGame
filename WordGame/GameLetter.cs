using System;
using System.Collections.Generic;
using System.Text;

namespace WordGame {
    class GameLetter : GameObject, IStringMapProvider {
        GameStringMap stringMap;
        public GameStringMap StringMap {
            get { return stringMap; }
        }
        public GameLetter(string letter)
            : base(GetBitmap()) {
            this.stringMap = GetStringMapFromLetter(letter);
        }
        static GameBitmap GetBitmap() {
            GameBitmap gBitmap;
            if (!GameBitmapDictionary.TryGetBitmap("+", out gBitmap)) {
                gBitmap = new GameBitmap(new bool[][] { new bool[] { true } });
                GameBitmapDictionary.AddBitmap("+", gBitmap);
            }
            return gBitmap;
        }
        static GameStringMap GetStringMapFromLetter(string letter) {
            return new GameStringMap(new string[][] { new string[] { letter } });
        }
    }
}
