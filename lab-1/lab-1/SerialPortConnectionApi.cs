using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO; // for work with COM port
using System.IO.Ports;

namespace lab_1
{
    class SerialPortConnectionApi
    {
        private static SerialPortConnectionApi INSTANCE;

        private SerialPort senderPort;

        private SerialPort receiverPort;

        private readonly static int SERIAL_PORT_READ_TIMEOUT = 500;

        private readonly static int SERIAL_PORT_WRITE_TIMEOUT = 500;

        private readonly static string SENDER_SERIAL_PORT_NAME = "COM1";

        private readonly static string RECEIVER_SERIAL_PORT_NAME = "COM2";

        // bits per sec
        private readonly static int DEFAULT_SERIAL_PORT_SPEED = 9600;

        private readonly static Encoding ENCODING = Encoding.UTF8;

        private readonly static int BUFFER_SIZE = 256;

        private SerialPortConnectionApi()
        {
            senderPort = new SerialPort(
                SENDER_SERIAL_PORT_NAME 
            );

            receiverPort = new SerialPort(
                RECEIVER_SERIAL_PORT_NAME
            );

            // configure serial ports
            senderPort.ReadTimeout = SERIAL_PORT_READ_TIMEOUT;
            senderPort.WriteTimeout = SERIAL_PORT_WRITE_TIMEOUT;
            senderPort.BaudRate = DEFAULT_SERIAL_PORT_SPEED;
            receiverPort.ReadTimeout = SERIAL_PORT_READ_TIMEOUT;
            receiverPort.WriteTimeout = SERIAL_PORT_WRITE_TIMEOUT;
            receiverPort.BaudRate = DEFAULT_SERIAL_PORT_SPEED;

            senderPort.Open();
            receiverPort.Open();
        }

        public static SerialPortConnectionApi GetInstance()
        {
            if (INSTANCE == null)
            {
                INSTANCE = new SerialPortConnectionApi();
            }
            return INSTANCE;
        }

        public string Read()
        {
            byte[] buffer = new byte[BUFFER_SIZE];
            Int32 offset = 0;
            Int32 count = BUFFER_SIZE;
            int readbytes;
            try
            {
                readbytes = receiverPort.Read(buffer, offset, count);
            }
            catch (TimeoutException) { }
            return ENCODING.GetString(buffer);
        }

        public void Write(string data)
        {
            byte[] bytes = ENCODING.GetBytes(data);
            Int32 count = bytes.Length;
            for(int offset = 0; offset < count; offset++)
            {
                senderPort.Write(bytes, offset, 1);
            }
        }

        public void SetBaudRate(int portBaudRate)
        {
            senderPort.BaudRate = portBaudRate;
        }

        public void SetReceptionSpeed(int receptionSpeed)
        {
            receiverPort.BaudRate = receptionSpeed;
        }
    }
}
