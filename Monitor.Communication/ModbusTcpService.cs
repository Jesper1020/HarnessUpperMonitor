using System.Net.Sockets;
using Modbus.Device;

namespace Monitor.Communication
{
    public class ModbusTcpService : IModbusService
    {
        private TcpClient _tcpClient;
        private IModbusMaster _master;

        public bool Connect(string ip, int port)
        {
            try
            {
                _tcpClient = new TcpClient(ip, port);
                _master = ModbusIpMaster.CreateIp(_tcpClient);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("连接失败: " + ex.Message);
                throw;
            }
        }

        public ushort[] ReadInputRegisters(byte slaveId, ushort startAddress, ushort numberOfPoints)
        {
            if (_master == null) throw new Exception("未连接设备");
            return _master.ReadInputRegisters(slaveId, startAddress, numberOfPoints);
        }

        public void Disconnect()
        {
            _master?.Dispose();
            _tcpClient?.Close();
        }
    }
}