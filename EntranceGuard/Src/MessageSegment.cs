namespace EntranceGuard
{
    using System;
    using System.Collections.Generic;

    class MessageSegment : IDisposable
    {
        public int functionID;		                          // 功能号
        public long iDevSn;                                   // 设备序列号9位SN码.
        public string IP;                                     // 控制器的IP地址
        static long sequenceId;                               // 序列号	

        public byte[] data = new byte[AppConst.dataSize];     // 56字节,的数据 [含流水号]
        public byte[] recv = new byte[AppConst.messageSize];  // 64字节,接收到的数据
        WG3000_COMM.Core.wgMjController controller = new WG3000_COMM.Core.wgMjController();

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {

            System.GC.Collect();
            System.GC.WaitForPendingFinalizers();

            Dispose(true);
            GC.SuppressFinalize(this);

            System.GC.Collect();
            System.GC.WaitForPendingFinalizers();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (controller != null)
                {
                    controller.Dispose();
                }
            }
        }

        /// 关闭
        public void close()
        {
            controller.Dispose();
        }
    }
}
