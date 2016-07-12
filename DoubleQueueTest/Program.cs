using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace DoubleQueueTest {

    internal class Program {
        private static DoubleQueue doubleQueue = new DoubleQueue();
        private static object obj = new object();

        private static DoubleQueueList doubleQueueList = new DoubleQueueList(5);

        private static void Main(string[] args) {

            #region 使用双缓冲队列

            Parallel.For(0, 10000, i =>
    {
        User user = new User()
        {
            Mobile = i.ToString().PadLeft(11, '0'),
            Pwd = i.ToString().PadLeft(8, '8')
        };
        doubleQueue.Write(user);
    });

            #endregion 使用双缓冲队列

            //#region 使用双缓冲队列 多个消费者

            //Parallel.For(0, 10000, i =>
            //{
            //    User user = new User()
            //    {
            //        Mobile = i.ToString().PadLeft(11, '0'),
            //        Pwd = i.ToString().PadLeft(8, '8')
            //    };
            //    doubleQueueList.PruducerFunc(user);
            //});

            //#endregion 使用双缓冲队列 多个消费者

            Stopwatch watch = Stopwatch.StartNew();
            int allcount = 0;
            Parallel.For(0, 10000, i =>
            {
                User user = new User()
                {
                    Mobile = i.ToString().PadLeft(11, '0'),
                    Pwd = i.ToString().PadLeft(8, '8')
                };
                lock (obj)
                {
                    FluentConsole.White.Background.Red.Line(user.ToString());
                    allcount++;
                    FluentConsole.White.Background.Red.Line($"当前个数{allcount.ToString()}，花费了{watch.ElapsedMilliseconds.ToString()}ms;");
                    System.Threading.Thread.Sleep(20);
                }
            });
            FluentConsole.Black.Background.Red.Line("执行完成");
            Console.Read();
        }
    }
}