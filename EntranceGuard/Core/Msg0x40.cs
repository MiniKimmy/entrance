namespace EntranceGuard
{
    /// <summary>
    /// 远程开门:0x40
    /// </summary>
    class Msg0x40 : MessageBase
    {
        public Msg0x40(int functionID, int controllerPort = 60000) : base(functionID, controllerPort){ }

        public override void CreateByteMsg()
        {
            base.CreateByteMsg();
            this.CmdMsg[8] = 0x01;
        }
        
        public override void HandleReceiveMsg(byte[] recv)
        {
            if(recv[8] == 1)
            {
                FacadeTool.Debug("remote opendoor:success");
            }
            else
            {
                FacadeTool.Debug("remote opendoor:failed");
            }
        }
    }
}
