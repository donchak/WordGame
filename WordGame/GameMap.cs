using System;
using System.Collections.Generic;
using System.Text;

namespace WordGame {
    class GameMap<T> {
        int width;
        int height;
        protected T[][] map;
        bool allowSet;
        public int Width { get { return width; } }
        public int Height { get { return height; } }
        public T this[int i, int j] {
            get { return map[i][j]; }
            set {
                if (!allowSet) throw new InvalidOperationException("Set is not allowed.");
                map[i][j] = value;
            }
        }
        public GameMap(int width, int height, bool allowSet) {
            if (width == 0) throw new ArgumentException("Map width.");
            if (height == 0) throw new ArgumentException("Map height.");
            map = new T[height][];
            for (int i = 0; i < height; i++) {
                map[i] = new T[width];
            }
            this.allowSet = allowSet;
        }
        public GameMap(T[][] map) {
            if (map == null) throw new ArgumentNullException();
            if (map.Length == 0) throw new ArgumentException("Map height.");
            if (map[0].Length == 0) throw new ArgumentException("Map width.");
            int mapFirstLength = map[0].Length;
            for (int i = 1; i < map.Length; i++) {
                if (map[i] == null) throw new NullReferenceException("Map line.");
                if (mapFirstLength != map[i].Length) throw new ArgumentException("Map width.");
            }
            this.map = map;
            this.height = map.Length;
            this.width = mapFirstLength;
        }
    }
}