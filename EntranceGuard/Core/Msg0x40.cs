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
            this.Message[8] = 0x01;
        }
    }
}
