using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WordGame {
    public partial class FormSettings : Form {
        public FormSettings() {
            InitializeComponent();
            int[] allRules = GameDictionary.GetAllRules();
            for (int i = 0; i < allRules.Length; i++) {
                clbRules.Items.Add(new FormSettingsRuleItem(allRules[i]));
            }
        }

        class FormSettingsRuleItem {
            int rule;
            public int Rule { get { return rule; } }
            string description;
            public string Description { get { return description; } }
            public FormSettingsRuleItem(int rule) {
                this.rule = rule;
                this.description = GameDictionary.GetRuleDescription(rule);
            }
            public override string ToString() {
                return string.Format("{0}) {1}", rule, description);
            }
        }

        void SetSettings(GameSettings settings) {
            tbSpeed.Value = (1100 - settings.LetterDownInterval) / 100;
            Dictionary<int, bool> selectedRules = new Dictionary<int, bool>();
            for (int i = 0; i < settings.Rules.Length; i++) {
                selectedRules[settings.Rules[i]] = true;
            }
            for (int i = 0; i < clbRules.Items.Count; i++) {
                if (selectedRules.ContainsKey(((FormSettingsRuleItem)clbRules.Items[i]).Rule)) {
                    clbRules.SetItemChecked(i, true);
                }
            }
        }
        GameSettings GetSettings() {
            List<int> selectedRuleList = new List<int>();
            foreach (FormSettingsRuleItem ruleItem in clbRules.CheckedItems) {
                selectedRuleList.Add(ruleItem.Rule);
            }
            int[] selectedRules = selectedRuleList.ToArray();
            int wordsCount = GameDictionary.GetRawWordsCount(selectedRules);
            int needWordsCount = GameDictionary.NeedWordsCount(wordsCount);
            int letterDownInterval = (1100 - tbSpeed.Value * 100);
            return new GameSettings(letterDownInterval, letterDownInterval * 10, needWordsCount, selectedRules);
        }


        public static GameSettings EditSettings(IWin32Window owner, GameSettings settings) {
            using (FormSettings settingsForm = new FormSettings()) {
                settingsForm.SetSettings(settings);
                if (settingsForm.ShowDialog(owner) == DialogResult.OK) {
                    return settingsForm.GetSettings();
                }
                return null;
            }
        }

        private void btOk_Click(object sender, EventArgs e) {

        }
    }
}
