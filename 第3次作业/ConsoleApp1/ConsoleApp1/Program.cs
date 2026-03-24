using System;
using System.Threading;

namespace AlarmClockConsole
{
    // 嘀嗒事件参数：传递当前时间
    public class TickEventArgs : EventArgs
    {
        public DateTime CurrentTime { get; }
        public TickEventArgs(DateTime currentTime)
        {
            CurrentTime = currentTime;
        }
    }

    // 响铃事件参数：传递闹钟时间
    public class AlarmEventArgs : EventArgs
    {
        public DateTime AlarmTime { get; }
        public AlarmEventArgs(DateTime alarmTime)
        {
            AlarmTime = alarmTime;
        }
    }

    // 闹钟核心类
    public class AlarmClock
    {
        // 当前时间
        public DateTime CurrentTime { get; private set; }
        // 闹钟设定时间
        public DateTime AlarmTime { get; set; }

        // 定义事件：嘀嗒（每秒触发）
        public event EventHandler<TickEventArgs> Tick;
        // 定义事件：响铃（到达闹钟时间时触发）
        public event EventHandler<AlarmEventArgs> Alarm;

        public AlarmClock()
        {
            CurrentTime = DateTime.Now;
        }

        // 触发Tick事件的保护方法（遵循.NET事件规范）
        protected virtual void OnTick(TickEventArgs e)
        {
            Tick?.Invoke(this, e);
        }

        // 触发Alarm事件的保护方法
        protected virtual void OnAlarm(AlarmEventArgs e)
        {
            Alarm?.Invoke(this, e);
        }

        // 启动闹钟
        public void Start()
        {
            Console.WriteLine(" 闹钟启动成功！");
            while (true)
            {
                // 更新当前时间
                CurrentTime = DateTime.Now;
                // 触发Tick事件
                OnTick(new TickEventArgs(CurrentTime));

                // 精确到秒判断是否到达闹钟时间
                if (CurrentTime.Hour == AlarmTime.Hour &&
                    CurrentTime.Minute == AlarmTime.Minute &&
                    CurrentTime.Second == AlarmTime.Second)
                {
                    OnAlarm(new AlarmEventArgs(AlarmTime));
                    break; // 响铃后停止循环
                }

                // 等待1秒（模拟时钟走时）
                Thread.Sleep(1000);
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // 1. 创建闹钟实例
            AlarmClock clock = new AlarmClock();
            // 2. 设置闹钟时间（当前时间+10秒，方便测试）
            clock.AlarmTime = DateTime.Now.AddSeconds(10);
            Console.WriteLine($"闹钟已设定：将在 {clock.AlarmTime:HH:mm:ss} 响铃");

            // 3. 订阅Tick事件（处理嘀嗒提示）
            clock.Tick += (sender, e) =>
            {
                Console.WriteLine($"[嘀嗒] {e.CurrentTime:HH:mm:ss}");
            };

            // 4. 订阅Alarm事件（处理响铃提示）
            clock.Alarm += (sender, e) =>
            {
                Console.WriteLine($"闹钟响了，时间：{e.AlarmTime:HH:mm:ss}");
            };

            // 5. 启动闹钟
            clock.Start();

            Console.WriteLine("按任意键退出...");
            Console.ReadKey();
        }
    }
}