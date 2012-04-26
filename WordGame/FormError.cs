using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WordGame {
    public partial class FormError : Form {
        GameErrorInfo error;
        public FormError(GameErrorInfo error) {
            InitializeComponent();
            this.error = error;
            lbLetter.Text = error.Letter;
            lbWord.Text = error.MaskedWord;
            richTextBox1.Rtf = GameDictionary.GetRuleDescriptioRtf(error.Rule);
        }
        public static void ShowError(IWin32Window owner, GameErrorInfo error) {
            using (FormError formError = new FormError(error)) {
                if (owner is Form) {
                    formError.Icon = ((Form)owner).Icon;
                }
                formError.ShowDialog(owner);
            }
        }
    }
}
