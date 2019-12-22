using System;

namespace Chronometer
{
    class Program
    {
        static void Main(string[] args)
        {
            string command = Console.ReadLine();
            Chronometer chronometer = new Chronometer();
            while (true)
            {
                if (command == "exit")
                {
                    chronometer.Stop();
                    return;
                }

                switch (command)
                {
                    case "start":
                        chronometer.Start();
                        break;
                    case "stop":
                        chronometer.Stop();
                        break;
                    case "lap":
                        Console.WriteLine(chronometer.Lap());
                        break;
                    case "laps":
                        Console.WriteLine(string.Join(Environment.NewLine,chronometer.Laps));
                        break;
                    case "time":
                        Console.WriteLine(chronometer.GetTime);
                        break;
                    case "reset":
                        chronometer.Reset();
                        break;
                }
                command = Console.ReadLine();
            }
        }
    }
}
