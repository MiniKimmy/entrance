namespace EntranceGuard
{
    /// <summary>
    /// 报文数据实体Base类
    /// </summary>
    class MessageBase 
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
        public long ControllerSN { get { return controllerSN; } set { controllerSN = value; } }

        /// <summary>
        /// 控制器IP
        /// </summary>
        private string controllerIP;
        public string ControllerIP { get { return controllerIP; } set { controllerIP = value; } }

        /// <summary>
        /// 控制器端口号
        /// </summary>
        private int controllerPort;
        public int ControllerPort { get { return controllerPort; } }

        /// <summary>
        /// bytep[]格式报文
        /// </summary>
        private byte[] message = null;
        public byte[] Message { get { return message; } protected set { message = value; } }

        public MessageBase() { }
        public MessageBase(int functionID, int controllerPort = AppConst.controllerPort)
        {
            this.functionID = functionID;
            this.controllerPort = controllerPort;
            this.controllerSN = long.Parse(Controller.Instance.Text_ControllerSN);
            this.controllerIP = Controller.Instance.Text_ControllerIP;

            CreateByteMsg(); /// 构造byte[]格式报文
        }

        /// <summary>
        /// 构建byte[]格式的报文
        /// </summary>
        public virtual void CreateByteMsg()
        {
            this.message = new byte[messageSize];
            this.message[0] = (byte)this.MessageType;
            this.message[1] = (byte)this.FunctionID;
            System.Array.Copy(System.BitConverter.GetBytes(controllerSN), 0, this.message, 4, 4);
        }
    }
}
