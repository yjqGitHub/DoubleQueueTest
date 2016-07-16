using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace DoubleQueueTest {

    internal class Program {
        private static object obj = new object();

        private static void Main(string[] args) {
            //BlockingCollectionQueue blockingCollectionQueue = new BlockingCollectionQueue();
            //Parallel.For(0, 3000, i =>
            //{
            //    User user = new User()
            //    {
            //        Mobile = i.ToString().PadLeft(11, '0'),
            //        Pwd = i.ToString().PadLeft(8, '8')
            //    };
            //    blockingCollectionQueue.ProducerFunc(user);
            //});

            DoubleQueue doubleQueue = new DoubleQueue();
            Parallel.For(0, 3000, i =>
            {
                User user = new User()
                {
                    Mobile = i.ToString().PadLeft(11, '0'),
                    Pwd = i.ToString().PadLeft(8, '8')
                };
                doubleQueue.ProducerFunc(user);
            });

            //Stopwatch watch = Stopwatch.StartNew();
            //int allcount = 0;
            //Parallel.For(0, 3000, i =>
            //{
            //    User user = new User()
            //    {
            //        Mobile = i.ToString().PadLeft(11, '0'),
            //        Pwd = i.ToString().PadLeft(8, '8')
            //    };
            //    lock (obj)
            //    {
            //        FluentConsole.White.Background.Red.Line(user.ToString());
            //        allcount++;
            //        FluentConsole.White.Background.Red.Line($"当前个数{allcount.ToString()}，花费了{watch.ElapsedMilliseconds.ToString()}ms;");
            //        System.Threading.Thread.Sleep(20);
            //    }
            //});

            //LogBufferQueue logBufferQueue = new LogBufferQueue();

            //Parallel.For(0, 3000, i =>
            //{
            //    User user = new User()
            //    {
            //        Mobile = i.ToString().PadLeft(11, '0'),
            //        Pwd = i.ToString().PadLeft(8, '8')
            //    };
            //    logBufferQueue.ProducerFunc(user);
            //});

            //LogBufferQueueList logBufferQueueList = new LogBufferQueueList(5);
            //Parallel.For(0, 10000, i =>
            //{
            //    User user = new User()
            //    {
            //        Mobile = i.ToString().PadLeft(11, '0'),
            //        Pwd = i.ToString().PadLeft(8, '8')
            //    };
            //    logBufferQueueList.ProducerFunc(user);
            //});
            FluentConsole.Black.Background.Red.Line("执行完成");
            Console.Read();
        }
    }
}