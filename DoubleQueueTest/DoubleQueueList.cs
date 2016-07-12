﻿using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using static FluentConsole;

// ===============================================================================
// Author              :    yjq
// Email               :    425527169@qq.com
// Create Time         :    2016/7/11 22:59:39
// ===============================================================================
// Class Version       :    v1.0.0.0
// Class Description   :
// ===============================================================================
// Copyright ©yjq 2016 . All rights reserved.
// ===============================================================================
namespace DoubleQueueTest {

    public class DoubleQueueList {

        /// <summary>
        /// 写队列列表
        /// </summary>
        private List<ConcurrentQueue<User>> _writeQueueList = new List<ConcurrentQueue<User>>();

        /// <summary>
        /// 读队列列表
        /// </summary>
        private List<ConcurrentQueue<User>> _readQueueList = new List<ConcurrentQueue<User>>();

        /// <summary>
        /// 当前执行队列列表
        /// </summary>
        private volatile List<ConcurrentQueue<User>> _currentQueueList = new List<ConcurrentQueue<User>>();

        /// <summary>
        /// 自动阻塞列表
        /// </summary>
        private List<AutoResetEvent> _dataEventList = new List<AutoResetEvent>();

        /// <summary>
        /// 手动阻塞（用于控制写队列是否完成）
        /// </summary>
        private List<ManualResetEvent> _finishedEventList = new List<ManualResetEvent>();

        /// <summary>
        /// 手动阻塞（用于控制读写列表是否交换成功）
        /// </summary>
        private List<ManualResetEvent> _producerEventList = new List<ManualResetEvent>();

        private int _dealCount = 0;

        public DoubleQueueList(int dealCount) {
            if (dealCount <= 0) dealCount = 1;
            if (dealCount >= 10) dealCount = 10;
            _dealCount = dealCount;
            for (int i = 0; i < _dealCount; i++)
            {
                _writeQueueList.Add(new ConcurrentQueue<User>());
                _readQueueList.Add(new ConcurrentQueue<User>());
                _currentQueueList.Add(new ConcurrentQueue<User>());
                _dataEventList.Add(new AutoResetEvent(false));
                _finishedEventList.Add(new ManualResetEvent(true));
                _producerEventList.Add(new ManualResetEvent(true));
            }

            for (int i = 0; i < _dealCount; i++)
            {
                int j = i;//赋值是为了防止闭包
                Task.Factory.StartNew(() => ConsumerQueue(j), TaskCreationOptions.None);
            }
        }

        public void PruducerFunc(User user) {
            int hashCode = user.GetHashCode() % _dealCount;
            _producerEventList[hashCode].WaitOne();
            _finishedEventList[hashCode].Reset();
            _currentQueueList[hashCode].Enqueue(user);
            _dataEventList[hashCode].Set();
            _finishedEventList[hashCode].Set();
        }

        private static Stopwatch watch = Stopwatch.StartNew();

        public void ConsumerQueue(int hashCode) {
            ConcurrentQueue<User> consumerQueue;
            User user;
            int allcount = 0;
            while (true)
            {
                _dataEventList[hashCode].WaitOne();
                if (_currentQueueList[hashCode].Count > 0)
                {
                    _producerEventList[hashCode].Reset();//将写入阻塞
                    _finishedEventList[hashCode].WaitOne();//等待写入的完成
                    consumerQueue = _currentQueueList[hashCode];
                    _currentQueueList[hashCode] = (_currentQueueList[hashCode] == _writeQueueList[hashCode]) ? _readQueueList[hashCode] : _writeQueueList[hashCode];//交互读写队列的引用
                    _producerEventList[hashCode].Set();
                    while (consumerQueue.Count > 0)
                    {
                        if (consumerQueue.TryDequeue(out user))
                        {
                            GetBankGroudColor(hashCode).Background.Red.Line(user.ToString());
                            allcount++;
                        }
                        GetBankGroudColor(hashCode).Background.Red.Line($"当前个数{allcount.ToString()}，花费了{watch.ElapsedMilliseconds.ToString()}ms;");
                        System.Threading.Thread.Sleep(20);
                    }
                }
            }
        }

        public IAfterColorSyntax GetBankGroudColor(int hashCode) {
            IAfterColorSyntax syntax = null;
            switch (hashCode)
            {
                case 0:
                    syntax = FluentConsole.Black;
                    break;

                case 1:
                    syntax = FluentConsole.Blue;
                    break;

                case 2:
                    syntax = FluentConsole.Cyan;
                    break;

                case 3:
                    syntax = FluentConsole.Yellow;
                    break;

                case 4:
                    syntax = FluentConsole.White;
                    break;

                case 5:
                    syntax = FluentConsole.Red;
                    break;

                case 6:
                    syntax = FluentConsole.Magenta;
                    break;

                case 7:
                    syntax = FluentConsole.Green;
                    break;

                case 8:
                    syntax = FluentConsole.Black;
                    break;

                case 9:
                    syntax = FluentConsole.DarkRed;
                    break;

                default:
                    syntax = FluentConsole.DarkCyan;
                    break;
            }
            return syntax;
        }
    }
}