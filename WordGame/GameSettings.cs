using System;
using System.Text;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.IO;

namespace WordGame {
    [XmlRoot("gameSettings")]
    public class GameSettings {
        private static readonly string settingsPath;
        private static readonly string settingsName;
        private int letterDownInterval;
        [XmlElement("letterDownInterval")]
        public int LetterDownInterval {
            get { return letterDownInterval; }
            set {
                letterDownInterval = value;
            }
        }
        private int newWordLineInsertInterval;
        [XmlElement("newWordLineInsertInterval")]
        public int NewWordLineInsertInterval {
            get { return newWordLineInsertInterval; }
            set {
                newWordLineInsertInterval = value;
            }
        }
        private int winWordsCount;
        [XmlElement("winWordsCount")]
        public int WinWordsCount {
            get { return winWordsCount; }
            set {
                winWordsCount = value;
            }
        }
        private int[] rules;
        [XmlElement("rule")]
        public int[] Rules {
            get { return rules; }
            set { rules = value; }
        }

        static GameSettings() {
            settingsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), 
                "DonchakWordGame");
            settingsName = Path.Combine(settingsPath, "settings.xml");
        }
        public GameSettings() { }
        public GameSettings(int letterDownInterval, int newWordLineInsertInterval, int winWordsCount, int[] rules) {
            this.letterDownInterval = letterDownInterval;
            this.newWordLineInsertInterval = newWordLineInsertInterval;
            this.winWordsCount = winWordsCount;
            this.rules = rules;
        }

        public static GameSettings LoadSettigs() {
            if (!File.Exists(settingsName)) return null;
            using (FileStream file = new FileStream(settingsName, FileMode.Open, FileAccess.Read)) {
                XmlSerializer serializer = new XmlSerializer(typeof(GameSettings));
                return (GameSettings)serializer.Deserialize(file);
            }
        }
        public static void SaveSettings(GameSettings settings) {
            if (!Directory.Exists(settingsPath)) Directory.CreateDirectory(settingsPath);
            using (FileStream file = new FileStream(settingsName, FileMode.Create, FileAccess.Write)) {
                XmlSerializer serializer = new XmlSerializer(typeof(GameSettings));
                serializer.Serialize(file, settings);
            }
        }
    }
}
