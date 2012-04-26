using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WordGame {
    public partial class FormRtf : Form {
        public FormRtf(string title, string rtf) {
            InitializeComponent();
            Text = title;
            richTextBox1.Rtf = rtf;
        }
        public static void ShowOrder(IWin32Window owner, string title, string rtf) {
            using (FormRtf frmRtf = new FormRtf(title, rtf)) {
                if (owner is Form) {
                    frmRtf.Icon = ((Form)owner).Icon;
                }
                frmRtf.ShowDialog(owner);
            }
        }
    }
}
