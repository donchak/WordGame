using System;
using System.Collections.Generic;
using System.Text;

namespace WordGame {
    class GameWord : GameObject, IStringMapProvider {
        string word;
        GameStringMap stringMap;
        public string Word {
            get { return word; }
        }
        public GameStringMap StringMap {
            get { return stringMap; }
        }
        GameMap<int> rulesMap;
        public GameMap<int> RulesMap {
            get { return rulesMap; }
        }
        /// <summary>
        /// Initializes a new instance of the GameWord class.
        /// </summary>
        public GameWord(string word)
            : base(GetBitmapFromWord(word)) {
            this.word = word;
            this.stringMap = GetStringMapFromWord(word);
            this.rulesMap = new GameMap<int>(stringMap.Width, stringMap.Height, false);
        }
        public GameWord(string word, GameMap<int> rulesMap)
            : base(GetBitmapFromWord(word)) {
            this.word = word;
            this.stringMap = GetStringMapFromWord(word);
            this.rulesMap = rulesMap;
        }
        public GameStringMap GetStringMapFromWord(string word) {
            string[][] stringMap = new string[1][];
            List<string> stringList = new List<string>();
            StringBuilder currentString = new StringBuilder();
            IterateWord(word, new IterateWordHandler(delegate(char c, IterateWordAction action) {
                switch (action) {
                    case IterateWordAction.AccumulateChar:
                        currentString.Append(c);
                        break;
                    case IterateWordAction.EndUnion:
                        stringList.Add(currentString.ToString());
                        currentString.Remove(0, currentString.Length);
                        break;
                    case IterateWordAction.AddChar:
                        stringList.Add(new string(c, 1));
                        break;
                    case IterateWordAction.EndSkip:
                        stringList.Add(currentString.ToString());
                        currentString.Remove(0, currentString.Length);
                        break;
                    default:
                        break;
                }
            }));
            stringMap[0] = stringList.ToArray();
            return new GameStringMap(stringMap);
        }
        public static GameBitmap GetBitmapFromWord(string word) {
            List<char> bitmapNameList = new List<char>();
            IterateWord(word, new IterateWordHandler(delegate(char c, IterateWordAction action) {
                switch (action) {
                    case IterateWordAction.EndUnion:
                    case IterateWordAction.AddChar:
                        bitmapNameList.Add('+');
                        break;
                    case IterateWordAction.EndSkip:
                        bitmapNameList.Add('-');
                        break;
                    default:
                        break;
                }
            }));
            string bitmapName = new string(bitmapNameList.ToArray());
            GameBitmap gBitmap;
            if (!GameBitmapDictionary.TryGetBitmap(bitmapName, out gBitmap)) {
                bool[][] bitmapArray = new bool[1][];
                bitmapArray[0] = new bool[bitmapNameList.Count];
                for (int i = 0; i < bitmapNameList.Count; i++) {
                    if (bitmapNameList[i] == '+')
                        bitmapArray[0][i] = true;
                }
                gBitmap = new GameBitmap(bitmapArray);
                GameBitmapDictionary.AddBitmap(bitmapName, gBitmap);
            }
            return gBitmap;
        }

        public static GameMap<int> GetRulesMapFromWord(string word) {
            List<int> rules = new List<int>();
            StringBuilder currentNumSB = new StringBuilder();
            IterateWord(word, new IterateWordHandler(delegate(char c, IterateWordAction action) {
                switch (action) {
                    case IterateWordAction.EndSkip:
                    case IterateWordAction.EndUnion:
                    case IterateWordAction.AddChar:
                        if (currentNumSB.Length > 0) {
                            int num;
                            if (int.TryParse(currentNumSB.ToString(), out num)) {
                                rules.Add(num);
                            }
                            currentNumSB.Remove(0, currentNumSB.Length);
                        } else {
                            rules.Add(-1);
                        }
                        break;
                    case IterateWordAction.AccumulateNum:
                        currentNumSB.Append(c);
                        break;
                    default:
                        break;
                }
            }));
            return new GameMap<int>(new int[][] { rules.ToArray() });
        }

        public static string GetMaskedWord(string word) {
            StringBuilder result = new StringBuilder();
            StringBuilder tempString = new StringBuilder();
            IterateWord(word, new IterateWordHandler(delegate(char c, IterateWordAction action) {
                switch (action) {
                    case IterateWordAction.EndSkip:
                        if (tempString.Length > 0) {
                            result.Append("_");
                            tempString.Remove(0, tempString.Length);
                        }
                        break;
                    case IterateWordAction.EndUnion:
                        if (tempString.Length > 0) {
                            result.Append(tempString);
                            tempString.Remove(0, tempString.Length);
                        }
                        break;
                    case IterateWordAction.AddChar:
                        result.Append(c);
                        break;
                    case IterateWordAction.AccumulateChar:
                        tempString.Append(c);
                        break;
                    default:
                        break;
                }
            }));
            return result.ToString();
        }
        public static int[] GetRules(string word) {
            List<int> rules = new List<int>();
            StringBuilder currentNumSB = new StringBuilder();
            IterateWord(word, new IterateWordHandler(delegate(char c, IterateWordAction action) {
                switch (action) {
                    case IterateWordAction.AccumulateNum:
                        currentNumSB.Append(c);
                        break;
                    case IterateWordAction.EndNum:
                        if (currentNumSB.Length > 0) {
                            int num;
                            if (int.TryParse(currentNumSB.ToString(), out num)) {
                                rules.Add(num);
                            }
                            currentNumSB.Remove(0, currentNumSB.Length);
                        }
                        break;
                    default:
                        break;
                }
            }));
            return rules.ToArray();
        }

        public static string GetConvertedWordByRule(string word, int[] rules) {
            StringBuilder currentNumSB = new StringBuilder();
            StringBuilder currentWordSB = new StringBuilder();
            bool nextUnionIsRightRule = false;
            bool bracketsAreOpened = false;
            IterateWord(word, new IterateWordHandler(delegate(char c, IterateWordAction action) {
                switch (action) {
                    case IterateWordAction.AccumulateNum:
                        currentNumSB.Append(c);
                        break;
                    case IterateWordAction.EndNum:
                        if (currentNumSB.Length > 0) {
                            if (rules != null) {
                                int num;
                                if (int.TryParse(currentNumSB.ToString(), out num)) {
                                    if (Array.IndexOf<int>(rules, num) >= 0) {
                                        nextUnionIsRightRule = true;
                                        bracketsAreOpened = false;
                                    }
                                }
                            }
                            currentNumSB.Remove(0, currentNumSB.Length);
                        }
                        break;
                    case IterateWordAction.AccumulateChar:
                        if (nextUnionIsRightRule) {
                            if (!bracketsAreOpened) {
                                bracketsAreOpened = true;
                                currentWordSB.Append("[");
                            }
                        } else {
                            if (!bracketsAreOpened) {
                                bracketsAreOpened = true;
                                currentWordSB.Append("(");
                            }
                        }
                        currentWordSB.Append(c);
                        break;
                    case IterateWordAction.AddChar:
                        if (bracketsAreOpened) throw new InvalidOperationException(string.Format("Parser error: {0}[ERROR]", currentWordSB.ToString()));
                        if (nextUnionIsRightRule) {
                            currentWordSB.AppendFormat("[{0}]", c);
                            nextUnionIsRightRule = false;
                        } else {
                            currentWordSB.Append(c);
                        }
                        break;
                    case IterateWordAction.EndSkip:
                    case IterateWordAction.EndUnion:
                        if (!bracketsAreOpened) throw new InvalidOperationException(string.Format("Parser error: {0}[ERROR]", currentWordSB.ToString()));
                        bracketsAreOpened = false;
                        if (nextUnionIsRightRule) {
                            currentWordSB.Append("]");
                            nextUnionIsRightRule = false;
                        } else {
                            currentWordSB.Append(")");
                        }
                        break;
                    default:
                        break;
                }
            }));
            return currentWordSB.ToString();
        }

        public static void IterateWord(string word, IterateWordHandler handler) {
            string upperWord = word.ToUpperInvariant();
            bool skip = false;
            bool union = false;
            bool num = false;
            for (int i = 0; i < upperWord.Length; i++) {
                char c = upperWord[i];
                if (c >= '0' && c <= '9') {
                    num = true;
                    if (handler != null) handler(c, IterateWordAction.AccumulateNum);
                    continue;
                }
                if (num) {
                    if (handler != null) handler((char)0, IterateWordAction.EndNum);
                    num = false;
                }
                if (c >= 'А' && c <= 'Я' || c == ' ' || c == '-') {
                    if (skip || union) {
                        if (handler != null) handler(c, IterateWordAction.AccumulateChar);
                        continue;
                    }
                    if (handler != null) handler(c, IterateWordAction.AddChar);
                    continue;
                }
                switch (c) {
                    case '[':
                        skip = true;
                        break;
                    case ']':
                        if (skip) {
                            if (handler != null) handler((char)0, IterateWordAction.EndSkip);
                        }
                        skip = false;
                        break;
                    case '(':
                        union = true;
                        break;
                    case ')':
                        if (union) {
                            if (handler != null) handler((char)0, IterateWordAction.EndUnion);
                        }
                        union = false;
                        break;
                }
            }
        }

        public delegate void IterateWordHandler(char c, IterateWordAction action);

        public enum IterateWordAction {
            AddChar,
            AccumulateChar,
            EndSkip,
            EndUnion,
            AccumulateNum,
            EndNum
        }

        public class GameWordByTopComparer : IComparer<GameWord> {
            GameTable gameTable;
            public GameWordByTopComparer(GameTable gameTable) {
                this.gameTable = gameTable;
            }
            public int Compare(GameWord x, GameWord y) {
                if (x == y) return 0;
                if (x == null) return 1;
                if (y == null) return -1;
                GameCellPos positionX;
                if (!gameTable.TryGetObjectPosition(x, out positionX)){
                    throw new InvalidOperationException(string.Format(WordGame.Properties.Resources.GivenGameTableHasNoCurrentWord, x.Word));
                }
                GameCellPos positionY;
                if(!gameTable.TryGetObjectPosition(y, out positionY)) {
                    throw new InvalidOperationException(string.Format(WordGame.Properties.Resources.GivenGameTableHasNoCurrentWord, y.Word));
                }
                return positionX.Row.CompareTo(positionY.Row);
            }
        }
    }
}