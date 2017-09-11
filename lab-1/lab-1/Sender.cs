using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COM
{
    class Sender
    {
        private SerialPortConnectionApi connectionApi;

        public Sender(SerialPortConnectionApi connectionApi)
        {
            this.connectionApi = connectionApi;
        }

        public void Send(string data)
        {
            this.connectionApi.Write(data);
        }

        public void SetBaudRate(int baudRate)
        {
            this.connectionApi.SetBaudRate(baudRate);
        }
    }
}
