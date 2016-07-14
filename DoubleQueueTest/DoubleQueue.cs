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
        private ConcurrentQueue<User> _writeQueue;
        private ConcurrentQueue<User> _readQueue;
        private volatile ConcurrentQueue<User> _currentQueue;

        private AutoResetEvent _dataEvent;
        private ManualResetEvent _finishedEvent;
        private ManualResetEvent _producerEvent;

        public DoubleQueue() {
            _writeQueue = new ConcurrentQueue<User>();
            _readQueue = new ConcurrentQueue<User>();
            _currentQueue = _writeQueue;

            _dataEvent = new AutoResetEvent(false);
            _finishedEvent = new ManualResetEvent(true);
            _producerEvent = new ManualResetEvent(true);
            Task.Factory.StartNew(() => ConsumerQueue(), TaskCreationOptions.None);
        }

        public void ProducerFunc(User user) {
            _producerEvent.WaitOne();
            _finishedEvent.Reset();
            _currentQueue.Enqueue(user);
            _dataEvent.Set();
            _finishedEvent.Set();
        }

        public void ConsumerQueue() {
            ConcurrentQueue<User> consumerQueue;
            User user;
            int allcount = 0;
            Stopwatch watch = Stopwatch.StartNew();
            while (true)
            {
                _dataEvent.WaitOne();
                if (_currentQueue.Count > 0)
                {
                    _producerEvent.Reset();
                    _finishedEvent.WaitOne();
                    consumerQueue = _currentQueue;
                    _currentQueue = (_currentQueue == _writeQueue) ? _readQueue : _writeQueue;
                    _producerEvent.Set();
                    while (consumerQueue.Count > 0)
                    {
                        if (consumerQueue.TryDequeue(out user))
                        {
                            FluentConsole.White.Background.Red.Line(user.ToString());
                            allcount++;
                        }
                        FluentConsole.White.Background.Red.Line($"当前个数{allcount.ToString()}，花费了{watch.ElapsedMilliseconds.ToString()}ms;");
                        System.Threading.Thread.Sleep(20);
                    }
                }
            }
        }
    }
}