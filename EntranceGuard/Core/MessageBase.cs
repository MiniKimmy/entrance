namespace EntranceGuard
{
    /// <summary>
    /// 报文数据实体Base类
    /// </summary>
    abstract class MessageBase 
    {
        #region
        /*
            [0]:type:报文类型
            [1]:functionID:报文功能号
            [2~3]:保留（不用管）
            [4~7]:SN码(9位int，占4个字节,即4个下标index)
            [8-63]:根据不同功能设置.
            [40~43]:流水号(可选) [40-43位]，需要异步操作才用。默认0x00000000
        */
        #endregion

        /// <summary>
        /// 报文长度:默认64位
        /// </summary>
        private int messageSize = 64;
        public int MessageSize { get { return messageSize; } }

        /// <summary>
        /// 报文类型:[0]
        /// </summary>
        private int messageType = 0x17;
        public int MessageType { get { return messageType; } protected set { messageType = value; } }

        /// <summary>
        /// 功能号:[1]
        /// </summary>
        private int functionID;
        public int FunctionID { get { return functionID; } }

        /// <summary>
        /// 控制器SN(9位数字,占4个byte):[4-7]
        /// </summary>
        private long controllerSN;
        public long ControllerSN { get { return controllerSN; } }

        /// <summary>
        /// 控制器IP
        /// </summary>
        private string controllerIP;
        public string ControllerIP { get { return controllerIP; } }

        /// <summary>
        /// 控制器端口号
        /// </summary>
        private int controllerPort;
        public int ControllerPort { get { return controllerPort; } }

        /// <summary>
        /// byte[]格式命令请求报文
        /// </summary>
        private byte[] cmdMsg = null;
        public byte[] CmdMsg { get { return cmdMsg; } protected set { cmdMsg = value; } }

        public MessageBase() { }
        public MessageBase(int functionID, int controllerPort = AppConst.controllerPort)
        {
            this.functionID = functionID;
            this.controllerPort = controllerPort;
            this.controllerSN = long.Parse(Controller.Instance.ControllerSN);
            this.controllerIP = Controller.Instance.ControllerIP;

            CreateByteMsg(); 
        }

        /// <summary>
        /// 构建byte[]格式的命令请求报文
        /// </summary>
        public virtual void CreateByteMsg()
        {
            this.cmdMsg = new byte[this.messageSize];

            // [0-7] 默认
            this.cmdMsg[0] = (byte)this.MessageType;
            this.cmdMsg[1] = (byte)this.FunctionID;
            System.Array.Copy(System.BitConverter.GetBytes(controllerSN), 0, this.cmdMsg, 4, 4);
        }

        /// <summary>
        /// 处理接收报文信息
        /// </summary>
        public abstract void HandleReceiveMsg(byte[] recv);
    }
}
