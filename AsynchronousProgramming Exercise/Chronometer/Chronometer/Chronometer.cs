using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Chronometer
{
    class Chronometer : IChronometer
    {
        private int milliseconds;
        private bool isRunning;

        public Chronometer()
        {
            this.isRunning = false;
            this.Laps = new List<string>();
        }
        public string GetTime => new TimeSpan(0, 0, 0, 0, this.milliseconds).ToString();

        public List<string> Laps {get; }

        public string Lap()
        {
            var milliseconds = this.milliseconds;
            this.Laps.Add(new TimeSpan(0, 0, 0, 0, this.milliseconds).ToString());
            return new TimeSpan(0, 0, 0, 0, this.milliseconds).ToString();

        }

        public void Reset()
        {
            this.milliseconds = 0;
            this.Laps.Clear();
        }

        public void Start()
        {
            this.isRunning = true;
            Task.Run(() =>
            {
                while (isRunning)
                {
                    Thread.Sleep(1);
                    this.milliseconds++;
                }
            });
        }

        public void Stop()
        {
            this.isRunning = false;
        }
    }
}
