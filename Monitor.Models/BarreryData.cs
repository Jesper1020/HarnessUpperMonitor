namespace Monitor.Models
{
    public class BatteryData
    {
        public float Voltage { get; set; }
        public float Current { get; set; }
        public float Temperature { get; set; }
        public bool IsRunning { get; set; }
        public DateTime UpdateTime { get; set; }

    }
}
