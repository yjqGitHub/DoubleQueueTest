using System.Collections.Concurrent;
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

        public DoubleQueue() {
            _writeQueue1 = new ConcurrentQueue<User>();
            _readQueue1 = new ConcurrentQueue<User>();
            _currentQueue1 = _writeQueue1;

            _dataEvent1 = new AutoResetEvent(false);
            _finishedEvent1 = new ManualResetEvent(true);
            _producerEvent1 = new ManualResetEvent(true);
            Task.Factory.StartNew(() => ConsumerQueue1(), TaskCreationOptions.LongRunning);
        }

        public void Write(User user) {
            HashCode1(user);
        }

        public void HashCode1(User user) {
            _producerEvent1.WaitOne();
            _finishedEvent1.Reset();
            _currentQueue1.Enqueue(user);
            System.Threading.Thread.Sleep(30);
            _dataEvent1.Set();
            _finishedEvent1.Set();
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
    }
}