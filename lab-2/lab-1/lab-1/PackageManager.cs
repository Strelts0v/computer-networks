using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COM
{
    class PackageManager
    {
        private static PackageManager INSTANCE;

        private BitStufferFilter bitStuffer;

        private readonly Encoding ENCODING;

        private PackageManager()
        {
            ENCODING = Encoding.UTF8;
            bitStuffer = new BitStufferFilter();
        }

        public static PackageManager GetInstance()
        {
            if(INSTANCE == null)
            {
                INSTANCE = new PackageManager();
            }
            return INSTANCE;
        }

        public byte[] preparePackage(string data)
        {
            byte[] bytes = ENCODING.GetBytes(data);
            return BitStufferFilter.Encode(bytes);
        }

        public byte[] parsePackage(byte[] bytes)
        {
            return BitStufferFilter.Decode(bytes);
        }
    }
}
