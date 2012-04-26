using System;
using System.Collections.Generic;
using System.Text;

namespace WordGame {
    interface IStringMapProvider {
        GameStringMap StringMap { get; }
    }
}
