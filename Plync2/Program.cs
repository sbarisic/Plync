using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace Plync2 {
	static class Program {
		public static Action<string> Printer;
		static ConcurrentQueue<Action> JobQueue;

		[STAThread]
		static void Main() {
			JobQueue = new ConcurrentQueue<Action>();
			Thread WorkThread = new Thread(DoWork);
			WorkThread.IsBackground = true;
			WorkThread.Start();

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new Plync2Frm());
		}

		static void DoWork() {
			Action A = null;

			while (true) {
				if (JobQueue.TryDequeue(out A)) {
#if !DEBUG
					try {
#endif
					A();
#if !DEBUG
				} catch (Exception E) {
						if (Printer != null)
							Printer("Exception: " + E.Message);
					}
#endif
				}
			}
		}

		public static void AddJob(Action A) {
			JobQueue.Enqueue(A);
		}
	}
}