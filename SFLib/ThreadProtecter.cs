using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace SFLib
{
    public class ThreadProtecter
    {
        private Thread _runProtectThread = null;

        private int _killCount = 30;
        private static int _originalKillCount = 0;

        public int KillCount
        {
            get { return _killCount; }
            set { _killCount = value; }
        }

        private bool _isTimeout = false;

        public bool IsTimeout
        {
            get { return _isTimeout; }
            set { _isTimeout = value; }
        }

        private static ThreadProtecter _threadProtecter = new ThreadProtecter();

        private ThreadProtecter() { }

        public static ThreadProtecter getInstance()
        {
            return _threadProtecter;
        }

        public static ThreadProtecter getInstance(int originalKillCount)
        {
            ThreadProtecter threadProtecter =  new ThreadProtecter();
            threadProtecter.KillCount = originalKillCount;
            return threadProtecter;
        }

        public void Start(Thread doFileThread)
        {
            IsTimeout = false;

            _runProtectThread = new Thread(new ThreadStart(() =>
            {
                while (_killCount == 0)
                {
                    Thread.Sleep(1000);
                }

                while (_killCount > 0)
                {
                    _killCount--;
                    Thread.Sleep(1000);//每秒检查一次
                }

                IsTimeout = true;
                doFileThread.Abort();
            }));
            _runProtectThread.IsBackground = true;
            _runProtectThread.Start();
        }

        public void Stop()
        {
            if (_runProtectThread != null && _runProtectThread.ThreadState != ThreadState.Aborted)
            {
                _runProtectThread.Abort();
            }
        }

    }
}
