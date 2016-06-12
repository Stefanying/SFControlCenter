using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;

namespace SFLib
{
    /// <summary>
    /// 检测运行类
    /// </summary>
    public class CheckRunner
    {
        public delegate bool CheckDelegate();
        public delegate void RunDelegate();

        Timer _timer = new Timer();
        int _attemptCount;
        bool isInfiniteLoop = false;
        CheckDelegate _checkFunction;
        RunDelegate _runFunction;

        volatile bool _isEnter = false;
        volatile bool _isRunned = false;

        /// <summary>
        /// 以指定的时间间隔调用检测函数，若检测函数返回true，则执行运行函数，
        /// 若检测函数被调用一定次数后仍然返回false，则不执行运行函数。
        /// </summary>
        /// <param name="interval">间隔时间。每隔Interval毫秒，checkFunction就会被调用一次</param>
        /// <param name="checkFunction">检测函数。返回true代表检测成功，反之失败</param>
        /// <param name="runFunction">运行函数。</param>
        /// <param name="maxCheckCount">最大检测次数。如果超过了maxCheckCount，checkFunction仍然返回false，则runFunction不会被调用
        ///                     <para>当检测次数为0时，则为无限循环，直到checkFunction满足条件后方才退出</para></param>
        public CheckRunner(int interval, CheckDelegate checkFunction, RunDelegate runFunction, int maxCheckCount)
        {
            _attemptCount = maxCheckCount;
            if (_attemptCount == 0)
            {
                isInfiniteLoop = true;
            }
            _checkFunction = checkFunction;
            _runFunction = runFunction;

            _timer.Elapsed += new ElapsedEventHandler(delegate(object sender, ElapsedEventArgs e)
            {
                if (_isEnter)
                {
                    return;
                }
                _isEnter = true;

                if (!isInfiniteLoop)
                {
                    _attemptCount--;
                }

                if (_checkFunction())
                {
                    if (!_isRunned)
                    {
                        _runFunction();
                        _isRunned = true;
                    }
                    _timer.Enabled = false;
                }
                else if (_attemptCount <= 0 && !isInfiniteLoop)
                {
                    _timer.Enabled = false;
                }

                _isEnter = false;
            });
            _timer.Interval = interval;
            _timer.Enabled = true;
        }
    }
}
