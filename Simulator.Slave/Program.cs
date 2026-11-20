//引入命名空间
//为了打字少，让我们可以直接使用 ModbusTcpSlave等类，而不需要每次都写完整的命名空间，如不用写modbus.Device.ModbusTcpSlave
using System.Net;
using System.Net.Sockets;
using Modbus.Data;
using Modbus.Device;
// 模拟一个 Modbus TCP 从站设备，监听 502 端口，模拟电压数据变化

// 1. 监听 502 端口
TcpListener listener = new TcpListener(IPAddress.Any, 502);
listener.Start();
Console.WriteLine("模拟设备已启动，在端口 502 等着上位机来连...");

// 2. 创建从站并设置数据存储
var slave = ModbusTcpSlave.CreateTcp(1, listener);
slave.DataStore = DataStoreFactory.CreateDefaultDataStore();

// 3. 模拟数据变化：电压从 3.00V 慢慢涨
Task.Run(() =>
{
    ushort voltage = 300; // 300 代表 3.00V
    while (true)
    {
        voltage = (ushort)Math.Min(voltage + 1, 420);
        slave.DataStore.InputRegisters[1] = voltage;
        Thread.Sleep(1000);
    }
});

// 4. 开始监听
slave.Listen();

// 【加上下面这两行，防止黑窗口一闪而过】
Console.WriteLine("按任意键退出模拟设备...");
Console.ReadKey();