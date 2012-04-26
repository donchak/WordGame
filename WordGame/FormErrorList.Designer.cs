namespace WordGame {
    partial class FormErrorList {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            this.errorGrid = new System.Windows.Forms.DataGridView();
            this.btOK = new System.Windows.Forms.Button();
            this.btShowError = new System.Windows.Forms.Button();
            this.letterDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.maskedWordDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ruleDescriptionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gameErrorInfoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.errorGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gameErrorInfoBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // errorGrid
            // 
            this.errorGrid.AllowUserToAddRows = false;
            this.errorGrid.AllowUserToDeleteRows = false;
            this.errorGrid.AllowUserToResizeColumns = false;
            this.errorGrid.AllowUserToResizeRows = false;
            this.errorGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.errorGrid.AutoGenerateColumns = false;
            this.errorGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.errorGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.errorGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.letterDataGridViewTextBoxColumn,
            this.maskedWordDataGridViewTextBoxColumn,
            this.ruleDescriptionDataGridViewTextBoxColumn});
            this.errorGrid.DataSource = this.gameErrorInfoBindingSource;
            this.errorGrid.Location = new System.Drawing.Point(12, 12);
            this.errorGrid.MultiSelect = false;
            this.errorGrid.Name = "errorGrid";
            this.errorGrid.ReadOnly = true;
            this.errorGrid.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.errorGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.errorGrid.Size = new System.Drawing.Size(569, 316);
            this.errorGrid.TabIndex = 0;
            this.errorGrid.DoubleClick += new System.EventHandler(this.errorGrid_DoubleClick);
            // 
            // btOK
            // 
            this.btOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btOK.Location = new System.Drawing.Point(506, 334);
            this.btOK.Name = "btOK";
            this.btOK.Size = new System.Drawing.Size(75, 23);
            this.btOK.TabIndex = 1;
            this.btOK.Text = "OK";
            this.btOK.UseVisualStyleBackColor = true;
            // 
            // btShowError
            // 
            this.btShowError.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btShowError.Location = new System.Drawing.Point(12, 334);
            this.btShowError.Name = "btShowError";
            this.btShowError.Size = new System.Drawing.Size(149, 23);
            this.btShowError.TabIndex = 2;
            this.btShowError.Text = "Показать ошибку";
            this.btShowError.UseVisualStyleBackColor = true;
            this.btShowError.Click += new System.EventHandler(this.btShowError_Click);
            // 
            // letterDataGridViewTextBoxColumn
            // 
            this.letterDataGridViewTextBoxColumn.DataPropertyName = "Letter";
            this.letterDataGridViewTextBoxColumn.HeaderText = "Неправильная буква";
            this.letterDataGridViewTextBoxColumn.Name = "letterDataGridViewTextBoxColumn";
            this.letterDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // maskedWordDataGridViewTextBoxColumn
            // 
            this.maskedWordDataGridViewTextBoxColumn.DataPropertyName = "MaskedWord";
            this.maskedWordDataGridViewTextBoxColumn.HeaderText = "Слово";
            this.maskedWordDataGridViewTextBoxColumn.Name = "maskedWordDataGridViewTextBoxColumn";
            this.maskedWordDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // ruleDescriptionDataGridViewTextBoxColumn
            // 
            this.ruleDescriptionDataGridViewTextBoxColumn.DataPropertyName = "RuleDescription";
            this.ruleDescriptionDataGridViewTextBoxColumn.HeaderText = "Правило";
            this.ruleDescriptionDataGridViewTextBoxColumn.Name = "ruleDescriptionDataGridViewTextBoxColumn";
            this.ruleDescriptionDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // gameErrorInfoBindingSource
            // 
            this.gameErrorInfoBindingSource.DataSource = typeof(WordGame.GameErrorInfo);
            // 
            // FormErrorList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(593, 369);
            this.Controls.Add(this.btShowError);
            this.Controls.Add(this.btOK);
            this.Controls.Add(this.errorGrid);
            this.Name = "FormErrorList";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Ошибки";
            ((System.ComponentModel.ISupportInitialize)(this.errorGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gameErrorInfoBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView errorGrid;
        private System.Windows.Forms.BindingSource gameErrorInfoBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn letterDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn maskedWordDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ruleDescriptionDataGridViewTextBoxColumn;
        private System.Windows.Forms.Button btOK;
        private System.Windows.Forms.Button btShowError;

    }
}