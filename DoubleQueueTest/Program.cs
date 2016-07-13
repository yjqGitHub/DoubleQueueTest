using System;
using System.Threading.Tasks;

namespace DoubleQueueTest {

    internal class Program {

        private static void Main(string[] args) {
            LogBufferQueue logBufferQueue = new LogBufferQueue();

            Parallel.For(0, 10000, i =>
            {
                User user = new User()
                {
                    Mobile = i.ToString().PadLeft(11, '0'),
                    Pwd = i.ToString().PadLeft(8, '8')
                };
                logBufferQueue.ProducerFunc(user);
            });

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