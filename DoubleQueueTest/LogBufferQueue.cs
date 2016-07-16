﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// ===============================================================================
// Author              :    yjq
// Email               :    425527169@qq.com
// Create Time         :    2016/7/13 21:29:36
// ===============================================================================
// Class Version       :    v1.0.0.0
// Class Description   :
// ===============================================================================
// Copyright ©yjq 2016 . All rights reserved.
// ===============================================================================
namespace DoubleQueueTest {

    public sealed class LogBufferQueue : DoubleBufferQueue<User> {

        public LogBufferQueue() : base() {
        }

        public override void Dequeue(User info) {
            FluentConsole.White.Background.Red.Line(info.ToString());
            System.Threading.Thread.Sleep(20);
        }
    }
}