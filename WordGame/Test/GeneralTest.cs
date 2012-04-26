#if DEBUGTEST
using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.IO;

namespace WordGame.Tests {
    [TestFixture]
    public class General {
        [Test]
        public void Collision() {
            GameBitmap bitmap1 = new GameBitmap(new bool[][]{
                new bool[] { true, true, true },
                new bool[] { false, true, false }
            });
            GameBitmap bitmap2 = new GameBitmap(new bool[][]{
                new bool[] { false, true, false },
                new bool[] { true, true, true }
            });
            GameTable gt = new GameTable(30, 60);
            GameObject go1bm1 = new GameObject(bitmap1);
            GameObject go1bm2 = new GameObject(bitmap2);
            gt.AddObject(go1bm1, 0, 0);
            gt.AddObject(go1bm2, 0, 3);
            Assert.AreEqual(CanMoveResult.TableBoundsCollision, gt.TryMoveObjectHere(go1bm2, 0, -1));
            Assert.AreEqual(CanMoveResult.ObjectCollision, gt.TryMoveObjectHere(go1bm2, 0, 0));
            Assert.AreEqual(CanMoveResult.ObjectCollision, gt.TryMoveObjectHere(go1bm2, 0, 1));
            Assert.AreEqual(CanMoveResult.Success, gt.TryMoveObjectHere(go1bm2, 0, 2));
            Assert.AreEqual(CanMoveResult.Success, gt.TryMoveObjectHere(go1bm2, 0, 3));
        }
        [Test]
        public void GameWordTest() {
            GameWord word1 = new GameWord("оловя[нн]ый");
            Assert.AreEqual(1, word1.Height);
            Assert.AreEqual(8, word1.Width);
            Assert.AreEqual(false, word1.Bitmap[0, 5]);
            Assert.AreEqual(true, word1.Bitmap[0, 0]);
            Assert.AreEqual("НН", word1.StringMap[0, 5]);
            Assert.AreEqual("О", word1.StringMap[0, 0]);
        }

        [Test]
        public void LoadRules() {
            string rules = @"1=A
2=B
3=C
4=D";
            string rulesFilePath = Path.GetTempFileName();
            File.WriteAllText(rulesFilePath, rules);
            try{
            GameDictionary.LoadRules(rulesFilePath);
            } finally {
                if (File.Exists(rulesFilePath)) File.Delete(rulesFilePath);
            }
            Assert.AreEqual(4, GameDictionary.rulesDictionary.Count);
            Assert.AreEqual("A", GameDictionary.rulesDictionary[1]);
            Assert.AreEqual("B", GameDictionary.rulesDictionary[2]);
            Assert.AreEqual("C", GameDictionary.rulesDictionary[3]);
            Assert.AreEqual("D", GameDictionary.rulesDictionary[4]);
        }

        [Test]
        public void LoadWords() {
            string[] words = new string[]{
                "43(не)прав31(и)льный",
                "ответсве22(нн)ый",
                "м31ороже22(н)ое",
                "э1н2ц3и44л55о6п7ед8(ия)"
            };
            int[][] expectResults = new int[][] {
                new int[] { 43, 31 },
                new int[] { 22 },
                new int[] { 31, 22 },
                new int[] { 1, 2, 3, 44, 55, 6, 7, 8 }
            };
            for (int i = 0; i < words.Length; i++) {
                int[] result = GameWord.GetRules(words[i]);
                Assert.IsTrue(CommonHelper.ArraysAreEquals<int>(result, expectResults[i]));
            }

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < words.Length; i++) {
                sb.AppendLine(words[i]);
            }
            string wordsFilePath = Path.GetTempFileName();
            File.WriteAllText(wordsFilePath, sb.ToString());
            try {
                GameDictionary.LoadWords(wordsFilePath);
            } finally {
                if (File.Exists(wordsFilePath)) File.Delete(wordsFilePath);
            }
            Assert.AreEqual(words.Length, GameDictionary.wordsDictionary.Count);
            for (int i = 0; i < words.Length; i++) {
                Assert.IsTrue(CommonHelper.ArraysAreEquals<int>(expectResults[i], GameDictionary.wordsDictionary[words[i]]));
            }

            string[] words1 = GameDictionary.GetRawWords(new int[] { 22 });
            Assert.AreEqual(2, words1.Length);
            Assert.IsTrue(Array.IndexOf<string>(words1, "ответсве22(нн)ый") >= 0);
            Assert.IsTrue(Array.IndexOf<string>(words1, "м31ороже22(н)ое") >= 0);
            string[] words2 = GameDictionary.GetRawWords(new int[] { 31 });
            Assert.AreEqual(2, words2.Length);
            Assert.IsTrue(Array.IndexOf<string>(words2, "43(не)прав31(и)льный") >= 0);
            Assert.IsTrue(Array.IndexOf<string>(words2, "м31ороже22(н)ое") >= 0);
        }
        [Test]
        public void GameWordConvertByRules() {
            string[] words = new string[]{
                "43(не)прав31(и)льный",
                "ответсве22(нн)ый",
                "м31ороже22(н)ое",
                "э1н2ц3и44л55о6п7ед8(ия)"
            };
            int[][][] rules = new int[][][] {
                new int[][] { new int[] { 43 }, new int[] { 31 }, new int[] { 43, 31 }, new int[] { 18 } },
                new int[][] { new int[] { 22 } },
                new int[][] { new int[] { 31 } , new int[] { 22 }, new int[] { 31, 22 } },
                new int[][] { new int[] { 1 }, //1
                    new int[] { 2 }, //2
                    new int[] { 3 }, //3
                    new int[] { 44 }, //4
                    new int[] { 55 }, //5
                    new int[] { 6 }, //6
                    new int[] { 7 }, //7
                    new int[] { 8 }, //8
                    new int[] { 2, 3, 6 }, //9
                    new int[] { 3, 44, 55, 2 }, //10
                    new int[] { 8, 2, 7, 55 } //11
            }};

            string[][] expectedResults = new string[][]{
                new string[] { "[не]прав(и)льный", "(не)прав[и]льный", "[не]прав[и]льный", "(не)прав(и)льный"  }, 
                new string[] { "ответсве[нн]ый"  }, 
                new string[] { "м[о]роже(н)ое", "мороже[н]ое", "м[о]роже[н]ое"  },
                new string[] { "э[н]цилопед(ия)", //1
                    "эн[ц]илопед(ия)", //2
                    "энц[и]лопед(ия)", //3
                    "энци[л]опед(ия)", //4
                    "энцил[о]пед(ия)", //5
                    "энцило[п]ед(ия)", //6
                    "энцилоп[е]д(ия)", //7
                    "энцилопед[ия]", //8
                    "эн[ц][и]ло[п]ед(ия)", //9
                    "эн[ц][и][л][о]пед(ия)", //10
                    "эн[ц]ил[о]п[е]д[ия]", //11
                },
            };
            for (int i = 0; i < words.Length; i++) {
                for (int j = 0; j < rules[i].Length; j++) {
                    string convertedWord = GameWord.GetConvertedWordByRule(words[i], rules[i][j]);
                    Assert.AreEqual(expectedResults[i][j].ToUpperInvariant(), convertedWord);
                }
            }
        }

        [Test]
        public void LoadLetters() {
            string letters = @"1=н,нн,
2=не ,не,   
3=а,о,
4=е,и,";
            string lettersFilePath = Path.GetTempFileName();
            File.WriteAllText(lettersFilePath, letters);
            try {
                GameDictionary.LoadLetters(lettersFilePath);
            } finally {
                if (File.Exists(lettersFilePath)) File.Delete(lettersFilePath);
            }
            Assert.AreEqual(4, GameDictionary.lettersDictionary.Count);
            Assert.IsTrue(CommonHelper.ArraysAreEquals<string>(new string[] { "Н", "НН" }, GameDictionary.lettersDictionary[1]));
            Assert.IsTrue(CommonHelper.ArraysAreEquals<string>(new string[] { "НЕ ", "НЕ" }, GameDictionary.lettersDictionary[2]));
            Assert.IsTrue(CommonHelper.ArraysAreEquals<string>(new string[] { "А", "О" }, GameDictionary.lettersDictionary[3]));
            Assert.IsTrue(CommonHelper.ArraysAreEquals<string>(new string[] { "Е", "И" }, GameDictionary.lettersDictionary[4]));
        }

        [Test]
        public void GetMaskedWord() {
            string[] words = new string[]{
                    "эн[ц]илопед(ия)", //2
                    "энц[и]лопед(ия)", //3
                    "энци[л]опед(ия)", //4
                    "энцил[о]пед(ия)", //5
                    "энцило[п]ед(ия)", //6
                    "энцилоп[е]д(ия)", //7
                    "энцилопед[ия]", //8
                    "эн[ц][и]ло[п]ед(ия)", //9
                    "эн[ц][и][л][о]пед(ия)", //10
                    "эн[ц]ил[о]п[е]д[ия]", //11
            };
            string[] maskedWords = new string[]{
                    "эн_илопедия", //2
                    "энц_лопедия", //3
                    "энци_опедия", //4
                    "энцил_педия", //5
                    "энцило_едия", //6
                    "энцилоп_дия", //7
                    "энцилопед_", //8
                    "эн__ло_едия", //9
                    "эн____педия", //10
                    "эн_ил_п_д_", //11
            };
            for (int i = 0; i < words.Length; i++) {
                Assert.AreEqual(maskedWords[i].ToUpperInvariant(), GameWord.GetMaskedWord(words[i]));
            }
        }

        [Test]
        public void GetRulesMap() {
            string[] words = new string[]{
                "43(не)прав31(и)льный",
                "ответсве22(нн)ый",
                "м31ороже22(н)ое",
                "э1н2ц3и44л55о6п7ед8(ия)"
            };
            int[][] rules = new int[][]{
                new int[] { 43, -1, -1, -1, -1, 31, -1, -1, -1, -1, -1 },
                new int[] { -1, -1, -1, -1, -1, -1, -1, -1, 22,  -1, -1 },
                new int[] { -1, 31, -1, -1, -1, -1, 22, -1, -1 },
                new int[] { -1, 1, 2, 3, 44, 55, 6, 7, -1, 8 }
            };

            for (int i = 0; i < words.Length; i++) {
                GameMap<int> rulesMap = GameWord.GetRulesMapFromWord(words[i]);
                Assert.AreEqual(1, rulesMap.Height);
                Assert.AreEqual(rules[i].Length, rulesMap.Width);
                for (int j = 0; j < rules[i].Length; j++) {
                    Assert.AreEqual(rules[i][j], rulesMap[0, j]);
                }
            }            
        }
    }
}
#endif