using Monitor.Communication;


namespace Monitor.UI
{
    public partial class Form1 : Form
    {
        private IModbusService _modbusService;
        private bool _isRunning = false;
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
        public Form1()
        {
            InitializeComponent();
            _modbusService = new ModbusTcpService(); // 实例化通信接口
        }


        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (_isRunning) return; // 防止重复点击
            try
            {
                // 1. 尝试连接（你的 Slave 模拟设备在本地 127.0.0.1:502）
                if (_modbusService.Connect("127.0.0.1", 502))
                {
                    _isRunning = true;
                    btnConnect.Text = "采集中...";

                    // 2. 开启后台任务读取数据，不阻塞 UI 线程
                    Task.Run(() =>
                    {
                        while (_isRunning)
                        {
                            try
                            {
                                // 读输入寄存器：站号1，地址0，读1个
                                ushort[] values = _modbusService.ReadInputRegisters(1, 0, 1);
                                double voltage = values[0] / 100.0;

                                // 3. 【核心】跨线程更新 UI（在 WinForm 里绝对不能直接改控件）
                                this.Invoke(new Action(() =>
                                {
                                    lblVoltage.Text = $"{voltage:F2} V";
                                }));
                            }
                            catch (Exception ex)
                            {
                                // 通信断开处理
                                this.Invoke(new Action(() =>
                                {
                                    lblVoltage.Text = "通信异常";
                                    btnConnect.Text = "重新连接";
                                    _isRunning = false;
                                }));
                                break;
                            }
                            Thread.Sleep(1000); // 1秒读一次
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                // 这里会捕获到真正的报错原因
                MessageBox.Show("连接失败的真实原因：\n" + ex.Message, "错误");
            }
        }
        }
    }



