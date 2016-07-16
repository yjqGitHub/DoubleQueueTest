using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
* Author              :    yjq
* Email               :    425527169@qq.com
* Create Time         :    2016/7/14 17:32:29
* Class Version       :    v1.0.0.0
* Class Description   :
* Copyright @yjq 2016 . All rights reserved.
*/

namespace DoubleQueueTest {

    public sealed class BlockingCollectionQueue {
        private BlockingCollection<User> _writerQueue;
        private BlockingCollection<User> _readerQueue;
        private BlockingCollection<User> _currentQueue;

        public BlockingCollectionQueue() {
            _writerQueue = new BlockingCollection<User>();
            _readerQueue = new BlockingCollection<User>();
            _currentQueue = _writerQueue;
            Task.Factory.StartNew(() => ConsumerFunc(), TaskCreationOptions.None);
        }

        public void ProducerFunc(User user) {
            _currentQueue.Add(user);
        }

        public void ConsumerFunc() {
            int allCount = 0;
            User user;
            while (!_currentQueue.IsAddingCompleted)
            {
                while (_writerQueue.Count > 0)
                {
                    user = _writerQueue.Take();
                    if (user != null)
                    {
                        allCount++;
                        FluentConsole.White.Background.Red.Line(user.ToString());
                        FluentConsole.White.Background.Red.Line(allCount.ToString());
                        System.Threading.Thread.Sleep(20);
                    }
                }
            }
        }
    }
}