using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

// ===============================================================================
// Author              :    yjq
// Email               :    425527169@qq.com
// Create Time         :    2016/7/13 21:35:02
// ===============================================================================
// Class Version       :    v1.0.0.0
// Class Description   :
// ===============================================================================
// Copyright ©yjq 2016 . All rights reserved.
// ===============================================================================
namespace DoubleQueueTest {

    public abstract class DoubleBufferQueueList<T> where T : class, new() {

        /// <summary>
        /// 写队列列表
        /// </summary>
        private List<ConcurrentQueue<T>> _writerQueueList = new List<ConcurrentQueue<T>>();

        /// <summary>
        /// 读队列列表
        /// </summary>
        private List<ConcurrentQueue<T>> _readerQueueList = new List<ConcurrentQueue<T>>();

        /// <summary>
        /// 当前执行队列列表
        /// </summary>
        private volatile List<ConcurrentQueue<T>> _currentQueueList = new List<ConcurrentQueue<T>>();

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
        private List<ManualResetEvent> _writerEventList = new List<ManualResetEvent>();

        private int _queueCount = 0;

        public DoubleBufferQueueList(int queueCount) {
            if (queueCount <= 0) queueCount = 1;
            if (queueCount >= 10) queueCount = 10;
            _queueCount = queueCount;
            for (int i = 0; i < _queueCount; i++)
            {
                _writerQueueList.Add(new ConcurrentQueue<T>());
                _readerQueueList.Add(new ConcurrentQueue<T>());
                _currentQueueList.Add(new ConcurrentQueue<T>());
                _dataEventList.Add(new AutoResetEvent(false));
                _finishedEventList.Add(new ManualResetEvent(true));
                _writerEventList.Add(new ManualResetEvent(true));
            }

            for (int i = 0; i < _queueCount; i++)
            {
                int j = i;//赋值是为了防止闭包
                Task.Factory.StartNew(() => ConsumerQueue(j), TaskCreationOptions.None);
            }
        }

        public void ProducerFunc(T info) {
            int hashCode = GetHashCode(info) % _queueCount;
            _writerEventList[hashCode].WaitOne();
            _finishedEventList[hashCode].Reset();
            _currentQueueList[hashCode].Enqueue(info);
            _dataEventList[hashCode].Set();
            _finishedEventList[hashCode].Set();
        }

        public virtual int GetHashCode(T info) {
            return info.GetHashCode();
        }

        public void ConsumerQueue(int hashCode) {
            ConcurrentQueue<T> consumerQueue;
            T info;
            while (true)
            {
                _dataEventList[hashCode].WaitOne();
                if (_currentQueueList[hashCode].Count > 0)
                {
                    _writerEventList[hashCode].Reset();//将写入阻塞
                    _finishedEventList[hashCode].WaitOne();//等待写入的完成
                    consumerQueue = _currentQueueList[hashCode];
                    _currentQueueList[hashCode] = (_currentQueueList[hashCode] == _writerQueueList[hashCode]) ? _readerQueueList[hashCode] : _writerQueueList[hashCode];//交互读写队列的引用
                    _writerEventList[hashCode].Set();
                    while (consumerQueue.Count > 0)
                    {
                        if (consumerQueue.TryDequeue(out info))
                        {
                            Dequeue(info);
                        }
                    }
                }
            }
        }

        public abstract void Dequeue(T info);
    }
}