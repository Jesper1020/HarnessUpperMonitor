namespace Monitor.Communication
{
    public interface IModbusService
    {
        bool Connect(string ip, int port);
        void Disconnect();
        ushort[] ReadInputRegisters(byte slaveId, ushort startAddress, ushort numberOfPoints);
    }
}