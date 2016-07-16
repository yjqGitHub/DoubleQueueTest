using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

// ===============================================================================
// Author              :    yjq
// Email               :    425527169@qq.com
// Create Time         :    2016/7/13 21:12:27
// ===============================================================================
// Class Version       :    v1.0.0.0
// Class Description   :
// ===============================================================================
// Copyright ©yjq 2016 . All rights reserved.
// ===============================================================================
namespace DoubleQueueTest {

    public abstract class DoubleBufferQueue<T> where T : class, new() {
        private ConcurrentQueue<T> _writerQueue;
        private ConcurrentQueue<T> _readerQueue;
        private volatile ConcurrentQueue<T> _currentWriterQueue;

        private AutoResetEvent _dataEvent;

        public DoubleBufferQueue() {
            _writerQueue = new ConcurrentQueue<T>();
            _readerQueue = new ConcurrentQueue<T>();
            _currentWriterQueue = _writerQueue;

            _dataEvent = new AutoResetEvent(false);

            Task.Factory.StartNew(() => ConsumerFunc(), TaskCreationOptions.None);
        }

        public void ProducerFunc(T info) {
            _currentWriterQueue.Enqueue(info);
            _dataEvent.Set();
        }

        public void ConsumerFunc() {
            ConcurrentQueue<T> readerQueue;
            T info;
            while (true)
            {
                _dataEvent.WaitOne();
                readerQueue = _currentWriterQueue;
                _currentWriterQueue = (_currentWriterQueue == _writerQueue) ? _readerQueue : _writerQueue;
                readerQueue = (_currentWriterQueue == _writerQueue) ? _readerQueue : _writerQueue;
                while (!readerQueue.IsEmpty)
                {
                    if (readerQueue.TryDequeue(out info))
                        Dequeue(info);
                }
            }
        }

        public abstract void Dequeue(T info);
    }
}