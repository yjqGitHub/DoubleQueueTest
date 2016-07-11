using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace DoubleQueueTest {

    internal class Program {
        private static DoubleQueue doubleQueue = new DoubleQueue();
        private static object obj = new object();


        private static DoubleQueueList doubleQueueList = new DoubleQueueList(5);
        private static void Main(string[] args) {

            //        #region 使用双缓冲队列

            //        Parallel.For(0, 500, i =>
            //{
            //    User user = new User()
            //    {
            //        Mobile = i.ToString().PadLeft(11, '0'),
            //        Pwd = i.ToString().PadLeft(8, '8')
            //    };
            //    doubleQueue.Write(user);
            //});

            //        #endregion 使用双缓冲队列


            #region 使用双缓冲队列

            Parallel.For(0, 500, i =>
            {
                User user = new User()
                {
                    Mobile = i.ToString().PadLeft(11, '0'),
                    Pwd = i.ToString().PadLeft(8, '8')
                };
                doubleQueueList.PruducerFunc(user);
            });

            #endregion 使用双缓冲队列

            //Stopwatch watch = Stopwatch.StartNew();
            //int allcount = 0;
            //Parallel.For(0, 1000, i =>
            //{
            //    User user = new User()
            //    {
            //        Mobile = i.ToString().PadLeft(11, '0'),
            //        Pwd = i.ToString().PadLeft(8, '8')
            //    };
            //    System.Threading.Thread.Sleep(30);
            //    lock (obj)
            //    {
            //        FluentConsole.White.Background.Red.Line(user.ToString());
            //        allcount++;
            //        FluentConsole.White.Background.Red.Line($"当前个数{allcount.ToString()}，花费了{watch.ElapsedMilliseconds.ToString()}ms;");
            //        System.Threading.Thread.Sleep(300);
            //    }
            //});
            FluentConsole.Black.Background.Red.Line("执行完成");
            Console.Read();
        }
    }
}