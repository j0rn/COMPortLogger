using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Management;

namespace COMPortLogger
{
    class Program
    {
        private static void PrintPorts(List<ComPort> ports, bool first)
        {
            int x = Console.CursorLeft;
            int y = Console.CursorTop;
            Console.SetCursorPosition(0, 0);
            Console.ForegroundColor = ConsoleColor.Gray;

            ports.ForEach(z =>
            {
                Console.Write(z.Added);
                Console.Write("\t");
                Console.Write("COM" + z.Port.ToString());
                Console.Write("\t");
                Console.Write(z.Description);
                while (Console.CursorLeft < 70) Console.Write(" "); // To clear rest of line
                Console.WriteLine();
            });
            Console.WriteLine("".PadRight(70));

            if (first == false)
                Console.SetCursorPosition(x, y);
        }

        static void Main(string[] args)
        {
            List<ComPort> AllPorts = ComPort.GetAllPorts();

            AllPorts.Sort();
            PrintPorts(AllPorts, true);
            Console.CursorVisible = false;
            Console.SetCursorPosition(0, 10);

            while (true)
            {
                Console.WindowWidth = 80;
                Console.WindowHeight = 25;
                Console.SetBufferSize(Console.WindowWidth, Console.WindowHeight);

                List<ComPort> ports = ComPort.GetAllPorts();

                // Removed ports
                Console.ForegroundColor = ConsoleColor.DarkRed;
                AllPorts.ForEach(x =>
                {
                    ComPort p = ports.Find(z => z.Port == x.Port);
                    if (p != null)
                    {
                        p.Added = x.Added;
                    }
                    else
                    {
                        Console.WriteLine(DateTime.Now + "\tCOM" + x.Port + "\t" + x.Description);
                    }
                });

                Console.ForegroundColor = ConsoleColor.Green;
                // Added ports
                ports.ForEach(x =>
                {
                    ComPort p = AllPorts.Find(z => z.Port == x.Port);
                    if (p == null)
                    {
                        Console.WriteLine(DateTime.Now + "\tCOM" + x.Port + "\t" + x.Description);
                    }
                });


                AllPorts = ports;

                ports.Sort();
                PrintPorts(ports, false);
                System.Threading.Thread.Sleep(500);
            }

        }
    }
}
