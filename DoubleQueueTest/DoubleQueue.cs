using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

// ===============================================================================
// Author              :    yjq
// Email               :    425527169@qq.com
// Create Time         :    2016/7/11 21:56:05
// ===============================================================================
// Class Version       :    v1.0.0.0
// Class Description   :
// ===============================================================================
// Copyright ©yjq 2016 . All rights reserved.
// ===============================================================================
namespace DoubleQueueTest {

    public class DoubleQueue {
        private static ConcurrentQueue<User> _writeQueue1;
        private static ConcurrentQueue<User> _readQueue1;
        private static ConcurrentQueue<User> _currentQueue1;

        private static AutoResetEvent _dataEvent1;
        private static ManualResetEvent _finishedEvent1;
        private static ManualResetEvent _producerEvent1;


        private static ConcurrentQueue<User> _writeQueue2;
        private static ConcurrentQueue<User> _readQueue2;
        private static ConcurrentQueue<User> _currentQueue2;

        private static AutoResetEvent _dataEvent2;
        private static ManualResetEvent _finishedEvent2;
        private static ManualResetEvent _producerEvent2;
        public DoubleQueue() {
            _writeQueue1 = new ConcurrentQueue<User>();
            _readQueue1 = new ConcurrentQueue<User>();
            _currentQueue1 = _writeQueue1;

            _dataEvent1 = new AutoResetEvent(false);
            _finishedEvent1 = new ManualResetEvent(true);
            _producerEvent1 = new ManualResetEvent(true);
            Task.Factory.StartNew(() => ConsumerQueue1(), TaskCreationOptions.None);

            _writeQueue2 = new ConcurrentQueue<User>();
            _readQueue2 = new ConcurrentQueue<User>();
            _currentQueue2 = _writeQueue2;

            _dataEvent2 = new AutoResetEvent(false);
            _finishedEvent2 = new ManualResetEvent(true);
            _producerEvent2 = new ManualResetEvent(true);
            Task.Factory.StartNew(() => ConsumerQueue2(), TaskCreationOptions.None);
        }

        public void Write(User user) {
            switch (user.GetHashCode() %2+1)
            {
                case 1:
                    HashCode1(user);
                    break;
                case 2:
                    HashCode2(user);
                    break;
            }
        }

        public void HashCode1(User user) {
            _producerEvent1.WaitOne();
            _finishedEvent1.Reset();
            _currentQueue1.Enqueue(user);
            System.Threading.Thread.Sleep(30);
            _dataEvent1.Set();
            _finishedEvent1.Set();
        }
        public void HashCode2(User user) {
            _producerEvent2.WaitOne();
            _finishedEvent2.Reset();
            _currentQueue2.Enqueue(user);
            System.Threading.Thread.Sleep(30);
            _dataEvent2.Set();
            _finishedEvent2.Set();
        }
        public void ConsumerQueue1() {
            ConcurrentQueue<User> consumerQueue;
            User user;
            int allcount = 0;
            Stopwatch watch = Stopwatch.StartNew();
            while (true)
            {
                _dataEvent1.WaitOne();
                if (_currentQueue1.Count > 0)
                {
                    _producerEvent1.Reset();
                    _finishedEvent1.WaitOne();
                    consumerQueue = _currentQueue1;
                    _currentQueue1 = (_currentQueue1 == _writeQueue1) ? _readQueue1 : _writeQueue1;
                    _producerEvent1.Set();
                    while (consumerQueue.Count > 0)
                    {
                        if (consumerQueue.TryDequeue(out user))
                        {
                            FluentConsole.White.Background.Red.Line(user.ToString());
                            allcount++;
                        }
                        FluentConsole.White.Background.Red.Line($"当前个数{allcount.ToString()}，花费了{watch.ElapsedMilliseconds.ToString()}ms;");
                        System.Threading.Thread.Sleep(30);
                    }
                }
            }
        }
        
        public void ConsumerQueue2() {
            ConcurrentQueue<User> consumerQueue;
            User user;
            int allcount = 0;
            Stopwatch watch = Stopwatch.StartNew();
            while (true)
            {
                _dataEvent2.WaitOne();
                if (_currentQueue2.Count > 0)
                {
                    _producerEvent2.Reset();
                    _finishedEvent2.WaitOne();
                    consumerQueue = _currentQueue2;
                    _currentQueue2 = (_currentQueue2 == _writeQueue2) ? _readQueue2 : _writeQueue2;
                    _producerEvent2.Set();
                    while (consumerQueue.Count > 0)
                    {
                        if (consumerQueue.TryDequeue(out user))
                        {
                            FluentConsole.Blue.Background.Red.Line(user.ToString());
                            allcount++;
                        }
                        FluentConsole.Blue.Background.Red.Line($"当前个数{allcount.ToString()}，花费了{watch.ElapsedMilliseconds.ToString()}ms;");
                        System.Threading.Thread.Sleep(30);
                    }
                }
            }
        }
    }
}