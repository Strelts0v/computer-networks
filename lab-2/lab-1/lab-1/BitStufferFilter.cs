using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COM
{
    public class BitStufferFilter
    {
        private static int COUNT_BIT_STUFFING = 5;
        private static byte START_END_BYTE = 0x7e;
        private static byte[] START_END_SIGNAL_BITS = new byte[] { START_END_BYTE };

        public static byte[] Encode(byte[] data)
        {
            var encodingBytes = EncodingBytes(new BitArray(data));
            return AddFlagsToData(encodingBytes);
        }

        public static byte[] Decode(byte[] data)
        {
            if (!IsDataIsValid(data))
            {
                throw new Exception("Start signal not found");
            }
            data = DeleteFlagsToData(data);
            return DecodingBytes(new BitArray(data));
        }

        private static byte[] EncodingBytes(BitArray bitArray)
        {
            var bits = new List<bool>();
            var currentSuccessivelyOnes = 0;
            foreach (var bit in bitArray)
            {
                if ((bool)bit)
                {
                    if (COUNT_BIT_STUFFING == currentSuccessivelyOnes)
                    {
                        bits.Add(false);
                        bits.Add(true);
                        currentSuccessivelyOnes = 0;
                    }
                    else
                    {
                        bits.Add(true);
                        currentSuccessivelyOnes++;
                    }
                }
                else
                {
                    bits.Add(false);
                    currentSuccessivelyOnes = 0;
                }
            }
            var returnedBytes = BitArrayToByteArray(new BitArray(bits.ToArray()));
            return returnedBytes;
        }

        private static byte[] DecodingBytes(BitArray bitArray)
        {
            var bits = new List<bool>();
            var currentSuccessivelyOnes = 0;
            foreach (var bit in bitArray)
            {
                if ((bool)bit)
                {
                    if(currentSuccessivelyOnes == COUNT_BIT_STUFFING)
                    {
                        bits.RemoveRange(bits.Count - COUNT_BIT_STUFFING, COUNT_BIT_STUFFING);
                        break;
                    }
                    bits.Add(true);
                    currentSuccessivelyOnes++;
                }
                else
                {
                    if (!(currentSuccessivelyOnes == COUNT_BIT_STUFFING))
                    {
                        bits.Add(false);
                    }
                    currentSuccessivelyOnes = 0;
                }
            }
            var returnedBytes = BitArrayToByteArray(new BitArray(bits.ToArray()));
            return returnedBytes;
        }

        private static bool IsDataIsValid(byte[] data)
        {
            return (data[0] == START_END_BYTE);
        }

        private static byte[] AddFlagsToData(byte[] data)
        {
            var retBytes = START_END_SIGNAL_BITS.ToList();
            retBytes.AddRange(data);
            retBytes.AddRange(START_END_SIGNAL_BITS);
            return retBytes.ToArray();
        }

        private static byte[] DeleteFlagsToData(byte[] data)
        {
            return data.Skip(1).Take(data.Length - 1).ToArray();
        }

        private static byte[] BitArrayToByteArray(BitArray bits)
        {
            byte[] ret = new byte[(bits.Length - 1) / 8 + 1];
            bits.CopyTo(ret, 0);
            return ret;
        }
    }
}
