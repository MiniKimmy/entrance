namespace FacialRecognitionLock
{
    /// <summary>
    /// 报文创建工厂
    /// </summary>
    class MessageFactory
    {
        public MessageFactory() { }

        public MessageBase CreateMessage(LockController.Funtion functionID)
        {
            MessageBase ret = null;
            switch (functionID)
            {
                case LockController.Funtion.REMOTEOPEN:
                    ret = new Msg0x40((byte)functionID);
                    break;
                case LockController.Funtion.STATUS:
                    ret = new Msg0x20((byte)functionID);
                    break;
                default:
                    break;
            }

            return ret;
        }
    }
}
