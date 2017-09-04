using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab_1
{
    class Receiver
    {
        private SerialPortConnectionApi connectionApi;

        public Receiver(SerialPortConnectionApi connectionApi)
        {
            this.connectionApi = connectionApi;
        }

        public string Receive()
        {
            return connectionApi.Read();
        }

        public void SetReceptionSpeed(int receptionSpeed)
        {
            connectionApi.SetReceptionSpeed(receptionSpeed);
        }
    }
}
