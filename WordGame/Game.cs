using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using System.Threading;

namespace WordGame {
    class Game {
        bool exit;
        bool gameOver;
        bool win;
        bool pause;
        int gameWords = 0;
        Exception threadsException;
        Random randomize;
        readonly GameSettings settings;
        readonly GameTable mainTable;
        readonly ReadOnlyCollection<string> rawWrods;
        readonly SynchronizationContext syncContext;
        readonly Dictionary<string, int[]> usedWordsDict = new Dictionary<string, int[]>();
        readonly ManualResetEvent suspendEvent = new ManualResetEvent(true);
        readonly AutoResetEvent letterDownEvent = new AutoResetEvent(false);

        GameLetter currentLetter;
        readonly List<GameWord> currentWords = new List<GameWord>();
        readonly List<GameErrorInfo> gameErrorList = new List<GameErrorInfo>();
        readonly ReadOnlyCollection<GameErrorInfo> gameErrorReadOnlyCollection;

        public bool Exit {
            get {
                return exit || gameOver || win;
            }
        }
        public ReadOnlyCollection<GameErrorInfo> GameErrors {
            get {
                return gameErrorReadOnlyCollection;
            }
        }
        public bool GameOver {
            get {
                return gameOver;
            }
        }
        public bool Win {
            get {
                return win;
            }
        }

        public int GameWords {
            get {
                return gameWords;
            }
        }
        public int GameWordsLeft {
            get {
                return settings.WinWordsCount - gameWords;
            }
        }
        public GameTable MainTable {
            get { return mainTable; }
        }

        public bool Pause {
            get {
                return pause;
            }
            set {
                pause = value;
            }
        }

        public event StartLetterDieAnimationHandler OnStartLetterDieAnimation;
        public event StartWordAndLetterDieAnimationHandler OnStartWordAndLetterDieAnimation;
        public event EventHandler OnUpdateScreen;

        public Game(int width, int height, GameSettings settings) {
            this.settings = settings;
            this.mainTable = new GameTable(width, height);
            this.gameErrorReadOnlyCollection = new ReadOnlyCollection<GameErrorInfo>(gameErrorList);
            this.rawWrods = new ReadOnlyCollection<string>(GameDictionary.GetRawWords(settings.Rules));
            if (rawWrods.Count == 0) throw new InvalidOperationException("Не нашел слов для выбранного набора правил.");
            this.syncContext = SynchronizationContext.Current;
            if (this.syncContext == null) throw new InvalidOperationException("Current thread has no synchronization context.");
            this.randomize = new Random(DateTime.Now.GetHashCode());
        }
        Thread threadNextWordLine;
        Thread threadLetterDown;
        public bool StartGameThreads() {
            lock (this) {
                if (threadNextWordLine != null || threadLetterDown != null) return false;
                threadNextWordLine = new Thread(new ThreadStart(WorkerNextWordLineAdd));
                threadNextWordLine.IsBackground = true;
                threadNextWordLine.Start();
                try {
                    threadLetterDown = new Thread(new ThreadStart(WorkerLetterDown));
                    threadLetterDown.IsBackground = true;
                    threadLetterDown.Start();
                } catch {
                    threadNextWordLine.Abort();
                    threadNextWordLine = null;
                    threadLetterDown = null;
                }
                return true;
            }
        }
        public void Stop() {
            exit = true;
            threadNextWordLine.Abort();
            threadLetterDown.Abort();
            OnStartLetterDieAnimation = null;
            OnStartWordAndLetterDieAnimation = null;
            OnUpdateScreen = null;
        }

        public void WorkerNextWordLineAdd() {
            while (!Exit) {
                if (!pause) {
                    try {
                        syncContext.Send(new SendOrPostCallback(NextWordLineAdd), null);
                    } catch (ThreadAbortException tex) {
                        //TODO Log
                    } catch (Exception ex) {
                        //TODO Log
                    }
                }
                suspendEvent.WaitOne();
                Thread.Sleep(settings.NewWordLineInsertInterval);
            }
        }
        public void WorkerLetterDown() {
            while (!Exit) {
                if (!pause) {
                    try {
                        syncContext.Send(new SendOrPostCallback(LetterDown), null);
                    } catch (ThreadAbortException tex) {
                        //TODO Log
                    } catch (Exception ex) {
                        //TODO Log
                    }
                }
                suspendEvent.WaitOne();
                letterDownEvent.WaitOne(settings.LetterDownInterval, false);
            }
        }

        void RollbackWordsShift(int collisionIndex) {
            for (int r = collisionIndex - 1; r >= 0; r--) {
                if (mainTable.TryMoveObject(currentWords[r], 1, 0) != CanMoveResult.Success) {
                    throw new InvalidOperationException("ObjectCollition.");
                }
            }
        }
        void NextWordLineAdd(object state) {
            try {
                int insertColumn = 0;
                int insertWidth;
                List<GameWord> wordsToInsert = GetWordsToInsert(out insertWidth);
                if (wordsToInsert.Count > 0)
                    insertColumn = randomize.Next(mainTable.Width - insertWidth);
                bool doRepeat = false;
                do {
                    bool doCurrentRepeat = false;
                    for (int i = 0; i < currentWords.Count; i++) {
                        CanMoveResult result = mainTable.TryMoveObject(currentWords[i], -1, 0);
                        switch (result) {
                            case CanMoveResult.ObjectCollision: {
                                    if (doRepeat) throw new InvalidOperationException("ObjectCollition.");
                                    RollbackWordsShift(i);
                                    LetterDown(state);
                                    doCurrentRepeat = true;
                                }
                                break;
                            case CanMoveResult.ObjectNotFound:
                                throw new InvalidOperationException(string.Format(WordGame.Properties.Resources.GivenGameTableHasNoCurrentWord, currentWords[i].Word));
                            case CanMoveResult.TableBoundsCollision:
                                gameOver = true;
                                return;
                        }
                        if (doCurrentRepeat) break;
                    }
                    if (!doCurrentRepeat && wordsToInsert.Count > 0) {
                        int currentInsertColumn = insertColumn;
                        for (int i = 0; i < wordsToInsert.Count; i++) {
                            CanMoveResult result = mainTable.CanMoveObjectHere(wordsToInsert[i], mainTable.Height - 1, currentInsertColumn);
                            switch(result){
                                case CanMoveResult.ObjectCollision: {
                                        if (doRepeat) throw new InvalidOperationException("ObjectCollition.");
                                        RollbackWordsShift(i);
                                        LetterDown(state);
                                        doCurrentRepeat = true;
                                    }
                                    break;
                                case CanMoveResult.Success:
                                    break;
                                default:
                                    throw new InvalidOperationException("Internal error.");
                            }
                            if (doCurrentRepeat) break;
                            currentInsertColumn += wordsToInsert[i].Width;
                        }
                        if (!doCurrentRepeat) {
                            currentInsertColumn = insertColumn;
                            for (int i = 0; i < wordsToInsert.Count; i++) {
                                mainTable.AddObject(wordsToInsert[i], mainTable.Height - 1, currentInsertColumn);
                                currentWords.Add(wordsToInsert[i]);
                                currentInsertColumn += wordsToInsert[i].Width;
                            }
                        }
                    }
                    doRepeat = doCurrentRepeat;
                } while (doRepeat);
                UpdateScreen();
            } catch (Exception ex) {
                threadsException = ex;
                exit = true;
            }
        }
        public void LetterStop() {
            if (pause) return;
            if (currentLetter == null) return;
            GameErrorInfo gameError;
            GameWord resultWord = CheckLetterStop(out gameError);
            if (gameError != null) gameErrorList.Add(gameError);
            if (resultWord == null) {
                StartLetterDieAnimationInternal(currentLetter);
            } else {
                StartWordAndLetterDieAnimationInternal(resultWord, currentLetter);
            }
        }
        public void LetterDown(object state) {
            if (pause) return;
            try {
                if (currentLetter == null) {
                    string nextLetterString = GetNextLetter();
                    if (string.IsNullOrEmpty(nextLetterString)) return;
                    GameLetter nextLetter = new GameLetter(nextLetterString);
                    mainTable.AddObject(nextLetter, 0, mainTable.Width / 2);
                    currentLetter = nextLetter;
                } else {
                    GameCellPos position;
                    if (!mainTable.TryGetObjectPosition(currentLetter, out position)) {
                        throw new InvalidOperationException("Current letter not found in table.");
                    }
                    position.MoveDown();
                    CanMoveResult moveResult = mainTable.TryMoveObjectHere(currentLetter, position.Row, position.Column);
                    switch (moveResult) {
                        case CanMoveResult.ObjectCollision:
                        case CanMoveResult.TableBoundsCollision: {
                                LetterStop();
                                break;
                            }
                        default:
                            break;
                    }
                }
                UpdateScreen();
            } catch (Exception ex) {
                threadsException = ex;
                exit = true;
            }
        }

        GameWord CheckLetterStop(out GameErrorInfo gameErrorInfo) {
            GameCellPos letterPos;
            gameErrorInfo = null;
            if (currentLetter == null) return null;
            if (!mainTable.TryGetObjectPosition(currentLetter, out letterPos)) return null;
            if (letterPos.Column - 1 >= 0) {
                GameCell cell = mainTable[letterPos.Row, letterPos.Column - 1];
                GameErrorInfo currentGameError;
                GameWord result = CheckLetterStopCell(letterPos, cell, out currentGameError);
                if (result != null) {
                    gameErrorInfo = null;
                    return result;
                }
                gameErrorInfo = currentGameError;
            }
            if (letterPos.Column + 1 < mainTable.Width) {
                GameCell cell = mainTable[letterPos.Row, letterPos.Column + 1];
                GameErrorInfo currentGameError;
                GameWord result = CheckLetterStopCell(letterPos, cell, out currentGameError);
                if (result != null) {
                    gameErrorInfo = null;
                    return result;
                }
                gameErrorInfo = currentGameError;
            }
            return null;
        }
        GameWord CheckLetterStopCell(GameCellPos letterPos, GameCell cell, out GameErrorInfo gameError) {
            GameWord word = cell.GameObject as GameWord;
            gameError = null;
            if (word != null) {
                GameCellPos wordPos;
                if (mainTable.TryGetObjectPosition(word, out wordPos) && ((wordPos.Column == letterPos.Column) || (wordPos.Column + word.Width > letterPos.Column))) {
                    int wordRelColumn = letterPos.Column - wordPos.Column;
                    if (wordRelColumn >= 0 && wordRelColumn < word.Width){
                        if (string.Equals(word.StringMap[0, wordRelColumn],
                                            currentLetter.StringMap[0, 0], StringComparison.InvariantCultureIgnoreCase)) {
                            return word;
                        } else {
                            gameError = new GameErrorInfo(word.RulesMap[0, wordRelColumn], currentLetter.StringMap[0, 0], word.Word);
                        }
                    }
                }
            }
            return null;
        }
        string GetNextLetter() {
            if (currentWords.Count == 0) return null;
            List<int> rulesList = new List<int>();
            int firstRow = -1;
            for (int i = 0; i < currentWords.Count; i++) {
                GameCellPos pos;
                if (!mainTable.TryGetObjectPosition(currentWords[i], out pos)) continue;
                if (firstRow == -1) firstRow = pos.Row;
                else if (firstRow != pos.Row && (firstRow + 1) != pos.Row)
                    continue;
                int[] rules;
                if (!usedWordsDict.TryGetValue(currentWords[i].Word, out rules)) continue;
                rulesList.AddRange(rules);
            }
            int currentRule = rulesList[randomize.Next(rulesList.Count)];
            string[] currentRuleLetters = GameDictionary.GetLetters(currentRule);
            string nextLetter = currentRuleLetters[randomize.Next(currentRuleLetters.Length)];
            return nextLetter;
        }
        string GetNextWord(out int[] rules, out GameMap<int> rulesMap) {
            int tryCount = 100;
            do {
                int nextWordIndex = randomize.Next(rawWrods.Count);
                string nextRawWord = rawWrods[nextWordIndex];
                int[] nextWordRules = GameDictionary.GetRules(nextRawWord);
                int nextRuleIndex = randomize.Next(nextWordRules.Length);
                int[] nextRules = new int[] { nextWordRules[nextRuleIndex] };
                string nextWord = GameWord.GetConvertedWordByRule(nextRawWord, nextRules);
                if (!usedWordsDict.ContainsKey(nextWord)) {
                    rules = nextRules;
                    rulesMap = GameWord.GetRulesMapFromWord(nextRawWord);
                    return nextWord;
                }
            } while (tryCount-- > 0);
            rules = null;
            rulesMap = null;
            return null;
        }
        List<GameWord> GetWordsToInsert(out int insertWidth) {
            insertWidth = 0;
            List<GameWord> wordsToInsert = new List<GameWord>();
            do {
                int repeats = 3;
                GameWord word;
                int[] wordRules;
                GameMap<int> wordRulesMap;
                do {
                    string nextWord = GetNextWord(out wordRules, out wordRulesMap);
                    if (string.IsNullOrEmpty(nextWord)) {
                        return wordsToInsert;
                    }
                    word = new GameWord(nextWord, wordRulesMap);
                } while ((insertWidth + word.Width) > mainTable.Width && repeats-- > 3);
                if ((insertWidth + word.Width) > mainTable.Width)
                    break;
                insertWidth += word.Width;
                wordsToInsert.Add(word);
                usedWordsDict.Add(word.Word, wordRules);
            } while (true);
            return wordsToInsert;
        }

        protected virtual void UpdateScreen() {
            if (OnUpdateScreen == null) return;
            OnUpdateScreen(this, new EventArgs());
        }
        void StartLetterDieAnimationInternal(GameLetter letter) {
            GameCellPos letterPosition;
            if (!mainTable.TryGetObjectPosition(letter, out letterPosition)) return;
            try {
                if (StartLetterDieAnimation(letter, letterPosition)) {
                    suspendEvent.Reset();
                    return;
                }
            } catch (Exception ex) {
                //TODO Log
            }
            EndLetterDieAnimation(letter);
        }
        void StartWordAndLetterDieAnimationInternal(GameWord word, GameLetter letter) {
            GameCellPos wordPosition;
            if (!mainTable.TryGetObjectPosition(word, out wordPosition)) return;
            try {
                if (StartWordAndLetterDieAnimation(word, letter, wordPosition)) {
                    suspendEvent.Reset();
                    return;
                }
            } catch (Exception ex) {
                //TODO Log
            }
            EndWordAndLetterDieAnimation(word, letter);
        }
        protected virtual bool StartLetterDieAnimation(GameLetter letter, GameCellPos letterPosition) {
            if(OnStartLetterDieAnimation == null) return false;
            return OnStartLetterDieAnimation(this, new LetterDieEventArgs(letter, letterPosition));
        }
        protected virtual bool StartWordAndLetterDieAnimation(GameWord word, GameLetter letter, GameCellPos wordPosition) {
            if (OnStartWordAndLetterDieAnimation == null) return false;
            return OnStartWordAndLetterDieAnimation(this, new WordAndLetterDieEventArgs(word, letter, wordPosition));
        }
        public void EndLetterDieAnimation(GameLetter letter) {
            try {
                if (currentLetter == letter) {
                    mainTable.RemoveObject(letter);
                    currentLetter = null;
                }
            } finally {
                suspendEvent.Set();
            }
        }
        public void EndWordAndLetterDieAnimation(GameWord word, GameLetter letter) {
            try {
                if (currentLetter == letter) {
                    mainTable.RemoveObject(letter);
                    currentLetter = null;
                    int wordIndex;
                    if ((wordIndex = currentWords.IndexOf(word)) >= 0) {
                        currentWords.RemoveAt(wordIndex);
                        mainTable.RemoveObject(word);
                        gameWords++;
                        if (gameWords >= settings.WinWordsCount) {
                            win = true;
                        }
                    }
                }
            } finally {
                suspendEvent.Set();
            }
        }
        public void ProcessExceptions() {
            if (threadsException != null) {
                throw threadsException;
            }
        }

        public void MoveLetterDown() {
            if (pause) return;
            letterDownEvent.Set();
        }
        public void MoveLetterLeft() {
            if (pause) return;
            MoveLetterSide(-1); 
        }
        public void MoveLetterRight() {
            if (pause) return;
            MoveLetterSide(1);
        }
        void MoveLetterSide(int shift) {
            if(currentLetter == null) return;
            CanMoveResult result = mainTable.TryMoveObject(currentLetter, 0, shift);
            if(result == CanMoveResult.ObjectNotFound) throw new InvalidOperationException("Current letter not found in mainTable.");
        }
    }

    class LetterDieEventArgs: EventArgs{
        GameLetter letter;
        GameCellPos letterPosition;
        public GameLetter Letter {
            get {
                return letter;
            }
        }
        public GameCellPos LetterPosition {
            get {
                return letterPosition;
            }
        }
        public LetterDieEventArgs(GameLetter letter, GameCellPos letterPosition) {
        		this.letter = letter;
        		this.letterPosition = letterPosition;
        }
    }
    class WordAndLetterDieEventArgs: EventArgs{
        GameWord word;
        GameLetter letter;
        GameCellPos wordPosition;
        public GameWord Word {
            get {
                return word;
            }
        }
        public GameLetter Letter {
            get {
                return letter;
            }
        }
        public GameCellPos WordPosition {
            get {
                return wordPosition;
            }
        }
        public WordAndLetterDieEventArgs(GameWord word, GameLetter letter, GameCellPos wordPosition) {
        		this.word = word;
        		this.letter = letter;
        		this.wordPosition = wordPosition;
        }
    }
    public class GameErrorInfo {
        int rule;
        string letter;
        string maskedWord;
        string ruleDescription;
        public int Rule {
            get {
                return rule;
            }
        }
        public string RuleDescription {
            get {
                if (ruleDescription == null) {
                    ruleDescription = GameDictionary.GetRuleDescription(rule);
                }
                return ruleDescription;
            }
        }
        public string Letter {
            get {
                return letter;
            }
        }
        public string MaskedWord {
            get {
                return maskedWord;
            }
        }
        public GameErrorInfo(int rule, string letter, string word) {
            this.rule = rule;
            this.letter = letter;
            this.maskedWord = GameWord.GetMaskedWord(word);
        }
    }
     delegate bool StartLetterDieAnimationHandler(object sender, LetterDieEventArgs e);
     delegate bool StartWordAndLetterDieAnimationHandler(object sender, WordAndLetterDieEventArgs e);
}
