namespace EntranceGuard
{
    /// <summary>
    /// 报文创建工厂
    /// </summary>
    class MessageFactory
    {
        public MessageFactory() { }
        public MessageBase CreateMessage(AppConst.Funtion functionID)
        {
            MessageBase ret = null;
            switch (functionID)
            {
                case AppConst.Funtion.REMOTEOPEN: 
                    ret = new Msg0x40((byte)functionID);
                    break;
                default:
                    break;
            }

            return ret;
        }
    }
}
