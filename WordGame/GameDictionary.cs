using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Diagnostics;
using System.IO;

namespace WordGame {
    class GameDictionary {
        readonly static string programPath;
        readonly static string dataFolderPath;
#if DEBUGTEST
        public
#endif
        static Dictionary<int, string> rulesDictionary = new Dictionary<int, string>();
#if DEBUGTEST
        public
#endif
        static Dictionary<string, int[]> wordsDictionary = new Dictionary<string, int[]>();
#if DEBUGTEST
        public
#endif
        static Dictionary<int, string[]> lettersDictionary = new Dictionary<int, string[]>();
        static GameDictionary() {
            programPath = Process.GetCurrentProcess().MainModule.FileName;
            dataFolderPath = Path.Combine(Path.GetDirectoryName(programPath), "Data");
        }
        public static void LoadDictionary(){
            string rulesFilePath = Path.Combine(dataFolderPath, "rules.dat");
            string wordsFilePath = Path.Combine(dataFolderPath, "words.dat");
            string lettersFilePath = Path.Combine(dataFolderPath, "letters.dat");
            if (!File.Exists(rulesFilePath)) return;
            LoadRules(rulesFilePath);
            LoadWords(wordsFilePath);
            LoadLetters(lettersFilePath);
        }
        public static string GetRuleDescriptionRtfFileName(int rule) {
            return Path.Combine(dataFolderPath, string.Format("{0}.rtf", rule));
        }
        public static string GetGameOrderRtf() {
            return File.ReadAllText(Path.Combine(dataFolderPath, "GameOrder.rtf"));
        }
        public static string GetRuleDescriptioRtf(int rule) {
            return File.ReadAllText(GetRuleDescriptionRtfFileName(rule));
        }
#if DEBUGTEST
        public
#endif
        static void LoadRules(string rulesFilePath) {
            using (FileStream file = new FileStream(rulesFilePath, FileMode.Open, FileAccess.Read)) {
                using (StreamReader reader = new StreamReader(file)) {
                    while (!reader.EndOfStream) {
                        string line = reader.ReadLine();
                        int eqPos = line.IndexOf("=");
                        if (eqPos > 0) {
                            int num;
                            if (int.TryParse(line.Substring(0, eqPos), out num)) {
                                rulesDictionary[num] = line.Substring(eqPos + 1);
                            }
                        }
                    }
                }
            }
        }
#if DEBUGTEST
        public
#endif
        static void LoadWords(string wordsFilePath) {
            using (FileStream file = new FileStream(wordsFilePath, FileMode.Open, FileAccess.Read)) {
                using (StreamReader reader = new StreamReader(file)) {
                    while (!reader.EndOfStream) {
                        string word = reader.ReadLine();
                        int[] rules = GameWord.GetRules(word);
                        wordsDictionary[word] = rules;
                    }
                }
            }            
        }

#if DEBUGTEST
        public
#endif
        static void LoadLetters(string lettersFilePath) {
            using (FileStream file = new FileStream(lettersFilePath, FileMode.Open, FileAccess.Read)) {
                using (StreamReader reader = new StreamReader(file)) {
                    while (!reader.EndOfStream) {
                        string line = reader.ReadLine().TrimEnd(' ');
                        int eqPos = line.IndexOf("=");
                        if (eqPos > 0) {
                            int num;
                            if (int.TryParse(line.Substring(0, eqPos), out num)) {
                                string[] letters = line.Substring(eqPos + 1).Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                                for (int i = 0; i < letters.Length; i++) {
                                    letters[i] = letters[i].ToUpperInvariant();
                                }
                                lettersDictionary[num] = letters;
                            }
                        }
                    }
                }
            }
        }
        static int[] emptyRules = new int[0];
        static string[] emptyLetters = new string[0];
        public static string GetRuleDescription(int rule){
            string description;
            if (!rulesDictionary.TryGetValue(rule, out description)) {
                return string.Empty;
            }
            return description;
        }
        public static int[] GetAllRules() {
            List<int> ruleList = new List<int>();
            foreach (int rule in rulesDictionary.Keys) {
                ruleList.Add(rule);
            }
            return ruleList.ToArray();
        }
        public static int[] GetRules(string word) {
            int[] containsRules;
            if (wordsDictionary.TryGetValue(word, out containsRules)) {
                int[] result = new int[containsRules.Length];
                Array.Copy(containsRules, result, containsRules.Length);
                return containsRules;
            }
            return emptyRules;
        }
        public static string[] GetLetters(int rule) {
            string[] containsLetters;
            if (lettersDictionary.TryGetValue(rule, out containsLetters)) {
                string[] result = new string[containsLetters.Length];
                Array.Copy(containsLetters, result, containsLetters.Length);
                return containsLetters;
            }
            return emptyLetters;
        }
        public static string[] GetRawWords(int[] rules) {
            List<string> words = new List<string>();
            if (rules != null) {
                Dictionary<int, bool> rulesDict = new Dictionary<int, bool>();
                for (int i = 0; i < rules.Length; i++) {
                    rulesDict[rules[i]] = true;
                }
                foreach (KeyValuePair<string, int[]> pair in wordsDictionary) {
                    for (int i = 0; i < pair.Value.Length; i++) {
                        if (rulesDict.ContainsKey(pair.Value[i])) {
                            words.Add(pair.Key);
                            break;
                        }
                    }
                }
            }
            return words.ToArray();
        }
        public static int GetRawWordsCount(int[] rules) {
            int count = 0;
            if (rules != null) {
                Dictionary<int, bool> rulesDict = new Dictionary<int, bool>();
                for (int i = 0; i < rules.Length; i++) {
                    rulesDict[rules[i]] = true;
                }
                foreach (KeyValuePair<string, int[]> pair in wordsDictionary) {
                    for (int i = 0; i < pair.Value.Length; i++) {
                        if (rulesDict.ContainsKey(pair.Value[i])) {
                            count++;
                            break;
                        }
                    }
                }
            }
            return count;
        }
        public static int GetAllWordsCount() {
            return wordsDictionary.Count;
        }
        public static int NeedWordsCount(int wordsCount) {
            int halfWordsCount = wordsCount / 2;
            int needWordsCount = (halfWordsCount < 15) ? wordsCount : halfWordsCount;
            return needWordsCount;
        }
    }
}
