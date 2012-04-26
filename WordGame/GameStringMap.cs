using System;
using System.Collections.Generic;
using System.Text;

namespace WordGame {
    class GameStringMap : GameMap<string> {
        /// <summary>
        /// Initializes a new instance of the GameStringMap class.
        /// </summary>
        public GameStringMap(string[][] map)
            : base(map) {
        }
    }
}
