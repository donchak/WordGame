using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading;

namespace WordGame {
    static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args) {
            //if (args.Length > 0 && args[0] == "/debugtest") {
            //    DebugTest = true;
            //}
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainGameForm());
        }
#if DEBUGTEST
        public static bool DebugTest;
#endif
    }
}
