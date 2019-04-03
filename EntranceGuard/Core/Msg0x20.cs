namespace EntranceGuard
{
    /// <summary>
    /// 查询状态:0x20
    /// </summary>
    class Msg0x20 : MessageBase
    {
        public Msg0x20(int functionID, int controllerPort = 60000) : base(functionID, controllerPort) { }

        public override void HandleReceiveMsg(byte[] recv)
        {
            // 0 : close ; 1: openning
            Controller.Instance.isLockOpenning = recv[49] == 1? true:false;
        }
    }
}
