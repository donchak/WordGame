using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WordGame {
    public partial class FormErrorList : Form {
        public FormErrorList(IList<GameErrorInfo> errors) {
            InitializeComponent();
            gameErrorInfoBindingSource.DataSource = errors;
        }
        public static void ShowErrors(IWin32Window owner, IList<GameErrorInfo> errors){
            using (FormErrorList frmErrorList = new FormErrorList(errors)) {
                frmErrorList.ShowDialog(owner);
            }
        }

        private void btShowError_Click(object sender, EventArgs e) {
            foreach (DataGridViewRow row in errorGrid.SelectedRows) {
                GameErrorInfo errorInfo = row.DataBoundItem as GameErrorInfo;
                if (errorInfo != null) {
                    FormError.ShowError(this, errorInfo);
                }
                break;
            }
        }

        private void errorGrid_DoubleClick(object sender, EventArgs e) {
            btShowError_Click(sender, e);
        }
    }
}
