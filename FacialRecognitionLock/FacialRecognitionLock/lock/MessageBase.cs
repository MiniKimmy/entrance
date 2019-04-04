namespace FacialRecognitionLock
{
    /// <summary>
    /// 报文数据实体Base类
    /// </summary>
    abstract class MessageBase
    {
        #region lock controller protocol
        /*
            [0]:type:报文类型
            [1]:functionID:报文功能号
            [2~3]:保留（不用管）
            [4~7]:SN码(9位int，占4个字节,即4个下标index)
            [8-63]:根据不同功能设置
            [40~43]:流水号(可选) [40-43位]，需要异步操作才用。默认0x00000000
        */
        #endregion

        private int messageSize;                       // 报文长度(默认64位)
        private int messageType;                       // 报文类型:[0]
        private int functionID;                        // 功能号:[1]
        private long controllerSN;                     // lock控制器SN(9位数字,占4个byte):[4-7]
        private string controllerIP;                   // lock控制器IP
        private int controllerPort;                    // lock控制器端口号 
        private byte[] cmdMsg = null;                  // byte[]格式命令请求报文

        public int MessageSize { get { return messageSize; } }
        public int MessageType { get { return messageType; } }
        public int FunctionID { get { return functionID; } }
        public long ControllerSN { get { return controllerSN; } }
        public string ControllerIP { get { return controllerIP; } }
        public int ControllerPort { get { return controllerPort; } }
        public byte[] CmdMsg { get { return cmdMsg; } protected set { cmdMsg = value; } }

        public MessageBase() { }
        public MessageBase(int functionID, int controllerPort = AppConst.lockPort)
        {
            this.functionID = functionID;
            this.controllerPort = controllerPort;
            this.controllerSN = long.Parse(LockController.Instance.ControllerSN);
            this.controllerIP = LockController.Instance.ControllerIP;

            CreateByteMsg();
        }

        /// <summary>
        /// 构建byte[]格式的命令请求报文
        /// </summary>
        public virtual void CreateByteMsg()
        {
            this.messageSize = 64;
            this.messageType = 0x17;
            this.cmdMsg = new byte[this.messageSize];

            // [0-7]位 默认设置
            this.cmdMsg[0] = (byte)this.messageType;
            this.cmdMsg[1] = (byte)this.functionID;
            System.Array.Copy(System.BitConverter.GetBytes(controllerSN), 0, this.cmdMsg, 4, 4);
        }

        /// <summary>
        /// 处理接收报文信息
        /// </summary>
        public abstract void HandleReceiveMsg(byte[] recv);
    }
}
