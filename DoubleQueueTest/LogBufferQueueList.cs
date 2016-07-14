// ===============================================================================
// Author              :    yjq
// Email               :    425527169@qq.com
// Create Time         :    2016/7/13 21:45:11
// ===============================================================================
// Class Version       :    v1.0.0.0
// Class Description   :
// ===============================================================================
// Copyright ©yjq 2016 . All rights reserved.
// ===============================================================================
namespace DoubleQueueTest {

    public sealed class LogBufferQueueList : DoubleBufferQueueList<User> {

        public LogBufferQueueList(int queueCount) : base(queueCount) {
        }

        public override int GetHashCode(User info) {
            return base.GetHashCode(info);
        }

        public override void Dequeue(User info) {
            FluentConsole.White.Background.Red.Line(info.ToString());
        }
    }
}