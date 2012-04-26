namespace WordGame {
    partial class MainGameForm {
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
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.timerLeft = new System.Windows.Forms.Timer(this.components);
            this.timerRight = new System.Windows.Forms.Timer(this.components);
            this.timerDown = new System.Windows.Forms.Timer(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.tsmiGame = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiNewGame = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiPause = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiShowErrors = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiExit = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiRules = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.gamePanel = new System.Windows.Forms.Panel();
            this.gameStatusBar = new System.Windows.Forms.StatusBar();
            this.gameStatusLeftWords = new System.Windows.Forms.StatusBarPanel();
            this.gameStatusErrors = new System.Windows.Forms.StatusBarPanel();
            this.gameStatus = new System.Windows.Forms.StatusBarPanel();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gameStatusLeftWords)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gameStatusErrors)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gameStatus)).BeginInit();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // timerLeft
            // 
            this.timerLeft.Interval = 1000;
            this.timerLeft.Tick += new System.EventHandler(this.timerLeft_Tick);
            // 
            // timerRight
            // 
            this.timerRight.Interval = 1000;
            this.timerRight.Tick += new System.EventHandler(this.timerRight_Tick);
            // 
            // timerDown
            // 
            this.timerDown.Interval = 1000;
            this.timerDown.Tick += new System.EventHandler(this.timerDown_Tick);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiGame,
            this.tsmiHelp});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.menuStrip1.Size = new System.Drawing.Size(624, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // tsmiGame
            // 
            this.tsmiGame.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiNewGame,
            this.toolStripSeparator1,
            this.tsmiPause,
            this.tsmiShowErrors,
            this.toolStripSeparator2,
            this.tsmiExit});
            this.tsmiGame.Name = "tsmiGame";
            this.tsmiGame.Size = new System.Drawing.Size(46, 20);
            this.tsmiGame.Text = "Игра";
            // 
            // tsmiNewGame
            // 
            this.tsmiNewGame.Name = "tsmiNewGame";
            this.tsmiNewGame.Size = new System.Drawing.Size(172, 22);
            this.tsmiNewGame.Text = "Новая игра";
            this.tsmiNewGame.Click += new System.EventHandler(this.tsmiNewGame_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(169, 6);
            // 
            // tsmiPause
            // 
            this.tsmiPause.Name = "tsmiPause";
            this.tsmiPause.Size = new System.Drawing.Size(172, 22);
            this.tsmiPause.Text = "Пауза";
            this.tsmiPause.Click += new System.EventHandler(this.tsmiPause_Click);
            // 
            // tsmiShowErrors
            // 
            this.tsmiShowErrors.Name = "tsmiShowErrors";
            this.tsmiShowErrors.Size = new System.Drawing.Size(172, 22);
            this.tsmiShowErrors.Text = "Показать ошибки";
            this.tsmiShowErrors.Click += new System.EventHandler(this.tsmiShowErrors_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(169, 6);
            // 
            // tsmiExit
            // 
            this.tsmiExit.Name = "tsmiExit";
            this.tsmiExit.Size = new System.Drawing.Size(172, 22);
            this.tsmiExit.Text = "Выход";
            this.tsmiExit.Click += new System.EventHandler(this.tsmiExit_Click);
            // 
            // tsmiHelp
            // 
            this.tsmiHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiRules,
            this.tsmiAbout});
            this.tsmiHelp.Name = "tsmiHelp";
            this.tsmiHelp.Size = new System.Drawing.Size(68, 20);
            this.tsmiHelp.Text = "Помощь";
            // 
            // tsmiRules
            // 
            this.tsmiRules.Name = "tsmiRules";
            this.tsmiRules.Size = new System.Drawing.Size(152, 22);
            this.tsmiRules.Text = "Правила";
            this.tsmiRules.Click += new System.EventHandler(this.tsmiRules_Click);
            // 
            // tsmiAbout
            // 
            this.tsmiAbout.Name = "tsmiAbout";
            this.tsmiAbout.Size = new System.Drawing.Size(152, 22);
            this.tsmiAbout.Text = "Об игре";
            this.tsmiAbout.Click += new System.EventHandler(this.tsmiAbout_Click);
            // 
            // gamePanel
            // 
            this.gamePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gamePanel.Location = new System.Drawing.Point(0, 24);
            this.gamePanel.Name = "gamePanel";
            this.gamePanel.Size = new System.Drawing.Size(624, 395);
            this.gamePanel.TabIndex = 1;
            this.gamePanel.Paint += new System.Windows.Forms.PaintEventHandler(this.MainGameForm_Paint);
            this.gamePanel.Resize += new System.EventHandler(this.MainGameForm_Resize);
            this.gamePanel.SizeChanged += new System.EventHandler(this.MainGameForm_SizeChanged);
            // 
            // gameStatusBar
            // 
            this.gameStatusBar.Location = new System.Drawing.Point(0, 419);
            this.gameStatusBar.Name = "gameStatusBar";
            this.gameStatusBar.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
            this.gameStatusLeftWords,
            this.gameStatusErrors,
            this.gameStatus});
            this.gameStatusBar.ShowPanels = true;
            this.gameStatusBar.Size = new System.Drawing.Size(624, 23);
            this.gameStatusBar.TabIndex = 0;
            this.gameStatusBar.PanelClick += new System.Windows.Forms.StatusBarPanelClickEventHandler(this.gameStatusBar_PanelClick);
            // 
            // gameStatusLeftWords
            // 
            this.gameStatusLeftWords.Name = "gameStatusLeftWords";
            this.gameStatusLeftWords.Text = "Осталось слов:";
            this.gameStatusLeftWords.Width = 130;
            // 
            // gameStatusErrors
            // 
            this.gameStatusErrors.Name = "gameStatusErrors";
            this.gameStatusErrors.Text = "Ошибок:";
            // 
            // gameStatus
            // 
            this.gameStatus.Name = "gameStatus";
            // 
            // MainGameForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 442);
            this.Controls.Add(this.gamePanel);
            this.Controls.Add(this.gameStatusBar);
            this.Controls.Add(this.menuStrip1);
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainGameForm";
            this.Text = "Орфографический тетрис";
            this.Deactivate += new System.EventHandler(this.MainGameForm_Deactivate);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainGameForm_FormClosed);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.MainGameForm_KeyPress);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.MainGameForm_KeyUp);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainGameForm_KeyDown);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gameStatusLeftWords)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gameStatusErrors)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gameStatus)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Timer timerLeft;
        private System.Windows.Forms.Timer timerRight;
        private System.Windows.Forms.Timer timerDown;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem tsmiGame;
        private System.Windows.Forms.ToolStripMenuItem tsmiNewGame;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem tsmiExit;
        private System.Windows.Forms.ToolStripMenuItem tsmiPause;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.Panel gamePanel;
        private System.Windows.Forms.StatusBar gameStatusBar;
        private System.Windows.Forms.StatusBarPanel gameStatusLeftWords;
        private System.Windows.Forms.StatusBarPanel gameStatusErrors;
        private System.Windows.Forms.StatusBarPanel gameStatus;
        private System.Windows.Forms.ToolStripMenuItem tsmiShowErrors;
        private System.Windows.Forms.ToolStripMenuItem tsmiHelp;
        private System.Windows.Forms.ToolStripMenuItem tsmiRules;
        private System.Windows.Forms.ToolStripMenuItem tsmiAbout;

    }
}

