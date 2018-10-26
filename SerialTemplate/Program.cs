using System;
using System.IO.Ports;
using System.Threading;
using System.Text;

namespace SerialPortTest
{
    class Program
    {

        static SerialPort sp = new SerialPort();

        static string[] ports = SerialPort.GetPortNames();
        static byte[] buffer;
        static void Main(string[] args)
        {
            PrintPorts();
            Console.Write("Choose port > ");
            char key = Console.ReadKey().KeyChar;
            Console.WriteLine();
            Console.WriteLine("Port " + ports[int.Parse(key.ToString())] + " chosen.");
            InitPort(int.Parse(key.ToString()));
            Thread serialTh = new Thread(SerialThread);
            serialTh.Start();

        }

        static void SerialThread()
        {
            while (true)
            {
                buffer = new byte[sp.BytesToRead];
                sp.Read(buffer, 0, buffer.Length);
                string output = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
                if (output.Length != 0)
                {
                    Console.Write(output);
                }
                Thread.Sleep(500);
            }
        }

        static void PrintPorts()
        {
            Console.WriteLine("Ports: ");
            int i = 0;
            foreach (string port in ports)
            {
                Console.WriteLine(i + ") " + port);
                i++;
            }
        }

        static void InitPort(int num)
        {
            try
            {
                // настройки порта
                sp.PortName = ports[num];
                sp.BaudRate = 9600;
                sp.DataBits = 8;
                sp.Parity = System.IO.Ports.Parity.None;
                sp.StopBits = System.IO.Ports.StopBits.One;
                sp.ReadTimeout = 1000;
                sp.WriteTimeout = 1000;
                sp.Open();
                Console.WriteLine("Port ready, receiving data. . .");
            }
            catch (Exception e)
            {
                Console.WriteLine("Some shit happened.");
                return;
            }
        }
    }
}



