using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Resources;
using System.Reflection;
using System.IO;

namespace WordGame {
    public partial class MainGameForm : Form {
        Game game;
        GameAppearence gameAppearence;
        GameSettings gameSettings;
        public MainGameForm() {
            InitializeComponent();
            this.Icon = CommonHelper.GetIconFromResource("WordGame.Data.Program.ico");
            GameDictionary.LoadDictionary();
            gameSettings = GameSettings.LoadSettigs();
            if (gameSettings == null) {
                int needWordsCount = GameDictionary.NeedWordsCount(GameDictionary.GetAllWordsCount());
                gameSettings = new GameSettings(1000, 10000, needWordsCount, GameDictionary.GetAllRules());
            }
        }


        void GameCreateAndStart() {
            try {
                game = new Game(20, 15, gameSettings);
            } catch (InvalidOperationException ex) {
                MessageBox.Show(this, ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            gameAppearence = new GameAppearence(game);
            gameAppearence.MainFont = Font;
            gameAppearence.GrayCellImageData = CommonHelper.GetResourceData("WordGame.Data.cell.bmp");
            game.OnStartLetterDieAnimation += new StartLetterDieAnimationHandler(game_OnStartLetterDieAnimation);
            game.OnStartWordAndLetterDieAnimation += new StartWordAndLetterDieAnimationHandler(game_OnStartWordAndLetterDieAnimation);
            game.OnUpdateScreen += new EventHandler(game_OnUpdateScreen);
            gameAppearence.UpdateScreenSize(gamePanel.ClientSize);
            game.StartGameThreads();
            timer1.Enabled = true;
            tsmiPause.Enabled = true;
        }
        void GameClear() {
            if (game != null) {
                game.Stop();
                gameAppearence.ClearCache();
                game = null;
                gameAppearence = null;
            }
            tsmiPause.Enabled = false;
            gameStatusLeftWords.Text = "Осталось слов:";
            gameStatusErrors.Text = "Ошибок:";
        }
        void GameDrawTableInvalidate(Graphics g) {
            if (game == null) return;
            gameAppearence.Invalidate();
            gameAppearence.DrawTable(g);
            UpdateStatusBar();
        }
        void GameDrawTableCreateGraphics() {
            if (this.IsDisposed) return;
            if (game == null) return;
            using (Graphics g = gamePanel.CreateGraphics()) {
                gameAppearence.DrawTable(g);
            }
            UpdateStatusBar();
        }
        void GameResizeAndDrawTable() {
            if (game == null) return;
            gameAppearence.UpdateScreenSize(gamePanel.ClientSize);
            GameDrawTableCreateGraphics();
        }
        void UpdateStatusBar() {
            if(game == null) return;
            gameStatusErrors.Text = "Ошибок: " + game.GameErrors.Count.ToString();
            gameStatusLeftWords.Text = "Осталось слов: " + game.GameWordsLeft.ToString();
        }
        bool GameAllowAction {
            get {
                return !(game == null || game.Exit || game.Pause);
            }
        }

        void game_OnUpdateScreen(object sender, EventArgs e) {
            GameDrawTableCreateGraphics();
        }
        bool game_OnStartLetterDieAnimation(object sender, LetterDieEventArgs e) {
            if(gameAppearence == null) return false;
            gameAppearence.RemoveObjectFromColorCache(e.Letter);
            return false;
        }
        bool game_OnStartWordAndLetterDieAnimation(object sender, WordAndLetterDieEventArgs e) {
            if (gameAppearence == null) return false;
            gameAppearence.RemoveObjectFromColorCache(e.Letter);
            gameAppearence.RemoveObjectFromColorCache(e.Word);
            return false;
        }

        private void MainGameForm_SizeChanged(object sender, EventArgs e) {
            GameResizeAndDrawTable();
        }
        private void MainGameForm_Paint(object sender, PaintEventArgs e) {
            GameDrawTableInvalidate(e.Graphics);
        }
        private void MainGameForm_FormClosed(object sender, FormClosedEventArgs e) {
            GameClear();
            GameSettings.SaveSettings(gameSettings);
        }

        private void MainGameForm_KeyPress(object sender, KeyPressEventArgs e) {
            switch (e.KeyChar) {
                case 's':
                case ' ':
                    game.LetterStop();
                    break;
            }
        }


        private void timer1_Tick(object sender, EventArgs e) {
            if (game == null) {
                timer1.Enabled = false;
                return;
            }
            if (!game.Exit) return;
            timer1.Enabled = false;
            if (game.Win) MessageBox.Show("Победа!");
            if (game.GameOver) MessageBox.Show("Попробуй еще разок.");
            try {
                game.ProcessExceptions();
            } catch (Exception ex) {
                MessageBox.Show(ex.ToString());
            }
        }

        private void MainGameForm_KeyDown(object sender, KeyEventArgs e) {
            if (!GameAllowAction) return;
            switch (e.KeyCode) {
                case Keys.Left:
                    timerLeft.Enabled = false;
                    timerLeft.Interval = 1000;
                    game.MoveLetterLeft();
                    GameDrawTableCreateGraphics();
                    timerLeft.Enabled = true;
                    break;
                case Keys.Right:
                    timerRight.Enabled = false;
                    timerRight.Interval = 1000;
                    game.MoveLetterRight();
                    GameDrawTableCreateGraphics();
                    timerRight.Enabled = true;
                    break;
                case Keys.Down:
                    timerDown.Enabled = false;
                    timerDown.Interval = 1000;
                    game.MoveLetterDown();
                    GameDrawTableCreateGraphics();
                    timerDown.Enabled = true;
                    break;
            }
        }

        private void MainGameForm_KeyUp(object sender, KeyEventArgs e) {
            if (!GameAllowAction) return;
            switch (e.KeyCode) {
                case Keys.Left:
                    timerLeft.Enabled = false;
                    break;
                case Keys.Right:
                    timerRight.Enabled = false;
                    break;
                case Keys.Down:
                    timerDown.Enabled = false;
                    break;
            }
        }

        private void timerLeft_Tick(object sender, EventArgs e) {
            if (!GameAllowAction) return;
            timerLeft.Interval = 200;
            game.MoveLetterLeft();
            GameDrawTableCreateGraphics();
        }

        private void timerRight_Tick(object sender, EventArgs e) {
            if (!GameAllowAction) return;
            timerRight.Interval = 200;
            game.MoveLetterRight();
            GameDrawTableCreateGraphics();
        }

        private void timerDown_Tick(object sender, EventArgs e) {
            if (!GameAllowAction) return;
            timerDown.Interval = 200;
            game.MoveLetterDown();
            GameDrawTableCreateGraphics();
        }

        private void MainGameForm_Resize(object sender, EventArgs e) {
            MainGameForm_SizeChanged(sender, e);
        }

        private void tsmiExit_Click(object sender, EventArgs e) {
            Close();
        }

        private void tsmiPause_Click(object sender, EventArgs e) {
            if (game == null || game.Exit) return;
            SetPauseState(!game.Pause);
        }

        private void SetPauseState(bool state) {
            if (game == null) return;
            if (game.Exit) return;
            game.Pause = state;
            tsmiPause.Checked = game.Pause;
            gameStatus.Text = game.Pause ? "Пауза" : "";
        }

        private void tsmiNewGame_Click(object sender, EventArgs e) {
            SetPauseState(true);
            try {
                GameSettings newGameSettings;
                if ((newGameSettings = FormSettings.EditSettings(this, gameSettings)) != null) {
                    GameClear();
                    gameSettings = newGameSettings;
                    GameCreateAndStart();
                }
            } finally {
                SetPauseState(false);
            }
        }

        private void MainGameForm_Deactivate(object sender, EventArgs e) {
            SetPauseState(true);
        }

        private void gameStatusBar_PanelClick(object sender, StatusBarPanelClickEventArgs e) {
            if (e.StatusBarPanel == gameStatusErrors) {
                if(game != null && game.GameErrors.Count > 0){
                    SetPauseState(true);
                    try {
                        FormError.ShowError(this, game.GameErrors[game.GameErrors.Count - 1]);
                    } finally {
                        SetPauseState(false);
                    }
                }
            }
        }

        private void tsmiShowErrors_Click(object sender, EventArgs e) {
            if (game != null && game.GameErrors.Count > 0) {
                SetPauseState(true);
                try {
                    FormErrorList.ShowErrors(this, game.GameErrors);
                } finally {
                    SetPauseState(false);
                }
            }
        }

        private void tsmiRules_Click(object sender, EventArgs e) {
            SetPauseState(true);
            try {
                FormRtf.ShowOrder(this, "Правила", GameDictionary.GetGameOrderRtf());
            } finally {
                SetPauseState(false);
            }
        }

        private void tsmiAbout_Click(object sender, EventArgs e) {
            SetPauseState(true);
            try {
                using (AboutBox about = new AboutBox()) {
                    about.ShowDialog(this);
                }
            } finally {
                SetPauseState(false);
            }
        }

    }
}
