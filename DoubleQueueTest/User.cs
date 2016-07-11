// ===============================================================================
// Author              :    yjq
// Email               :    425527169@qq.com
// Create Time         :    2016/7/11 21:56:52
// ===============================================================================
// Class Version       :    v1.0.0.0
// Class Description   :
// ===============================================================================
// Copyright ©yjq 2016 . All rights reserved.
// ===============================================================================
namespace DoubleQueueTest {

    public class User {
        public string Mobile { get; set; }

        public string Pwd { get; set; }

        public override string ToString() {
            return $"{Mobile},{Pwd}";
        }
    }
}