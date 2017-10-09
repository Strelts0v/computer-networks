using System;
using System.Collections.Generic;
using System.Linq;

namespace COM
{
    class ByteStuffing
    {
        private const byte Flag = 0x7E;
        private const byte Esc = 0x7D;
        private const byte FlagCh = 0x5E;

        public static byte[] Decode(byte[] data)
        {
            if (!SendValidation(data)) return null;            

            int dataLength = data.Length;
            int count = 0;
            for (int i=0; i<dataLength; i++)
                if (data[i] == Esc)
                    count++;

            int newDataLength = dataLength - 4 - count;
            byte[] newData = new byte[newDataLength];
            int j = 0;
            for (int i = 3; i < dataLength - 1; i++)
            {
                if (data[i] == Flag)
                {
                    if (data[i++] == FlagCh)
                        newData[j++] = Flag;
                    else
                        newData[j++] = Esc;
                }
                else
                {
                    newData[j++] = data[i];
                }
            }
            return newData;
        }

        public static byte[] Encode(byte[] data)
        {
            int newDataLength = GetNewDataLength(data);     
            byte[] newData = new byte[newDataLength];
            InitializeBytes(ref newData);

            int j = 3;
            for (int i = 0; i < data.Length; i++)
            {
                switch (data[i])
                {
                    case Flag:
                        newData[j++] = Esc;
                        newData[j++] = FlagCh; 
                        break;
                    case Esc:
                        newData[j++] = Esc;
                        newData[j++] = Esc;
                        break;
                    default:
                        newData[j++] = data[i];
                        break; 
                }
            }
            return newData;
        }
        
        private static bool SendValidation(byte[] data)
        {
            return data[1] == 0xFF && data[2] == 0x00;          
        }

        private static void InitializeBytes(ref byte[] data)
        {
            data[0] = Flag;
            data[1] = 0xFF;
            data[2] = 0x00;
            data[data.Length - 1] = Flag;
        }

        private static int GetNewDataLength(byte[] data)
        {
            int count = 0;
            for (int i = 0; i < data.Length; i++)
                if ((data[i] == Flag) || (data[i] == Esc))
                    count++;
            return data.Length + count + 4;
        }

    }

    public class ByteStuffer
    {
        private const byte Flag = 0x7E; //Control flag
        private const byte Esc = 0x7D; //Escape byte
        private const byte FlagCh = 0x5E; //byte to replace flag
        private const int DataBytes = 16; //payload
        private static readonly byte[] Header = { Flag, 0xFF, 03, 0x00 }; //header of packet

        /// <summary>
        ///     Еncode bytes
        /// </summary>
        /// <param name="data">bytes to encode</param>
        /// <returns>Encoded bytes</returns>
        public static byte[] Encode(byte[] data)
        {
            var t = new Crc16();
            var chunks = data.ToList().ChunkBy(DataBytes);
            var resBytes = new List<byte>();
            foreach (var chunk in chunks) //encode data
            {
                chunk.AddRange(t.ComputeChecksumBytes(chunk.ToArray())); //Add crc16
                for (var i = 0; i < chunk.Count; i++) //Encode
                    switch (chunk[i])
                    {
                        case Flag:
                            chunk[i] = Esc;
                            i++;
                            chunk.Insert(i, FlagCh);
                            continue;
                        case Esc:
                            i++;
                            chunk.Insert(i, Esc);
                            break;
                    }
                /*Form packet*/
                resBytes.AddRange(Header);
                resBytes.AddRange(chunk);
                resBytes.Add(Flag);
            }
            return resBytes.ToArray();
        }

        /// <summary>
        ///     Decode data
        /// </summary>
        /// <param name="input">Bytes to decode</param>
        /// <returns>Decoded bytes</returns>
        public static byte[] Decode(byte[] input)
        {
            var inp = input.ToList();
            var resBytes = new List<byte>();
            var counter = 0;
            var t = new Crc16();
            var prevPos = 0;
            for (var i = 0; i < inp.Count; i++)
            {
                if (inp[i] == Flag) counter++;
                if (counter == 2)
                {
                    var chunk = inp.GetRange(prevPos, i + 1 - prevPos);
                    var adress = chunk[1];
                    chunk.RemoveRange(0, 4);
                    chunk.RemoveAt(chunk.Count - 1); //Get rid from header and ending flag
                    for (var j = 0; j < chunk.Count; j++) //Decode
                        if (chunk[j] == Esc)
                        {
                            chunk.RemoveAt(j);
                            if (chunk[j] == FlagCh) chunk[j] = Flag;
                        }
                    if (!t.ComputeChecksumBytes(chunk.GetRange(0, chunk.Count - 2).ToArray())
                            .SequenceEqual(chunk.GetRange(chunk.Count - 2, 2)) &&
                        adress != 0xFF) //Check crc16 and adress
                    {
                        prevPos = i + 1; //Skip packet if wrong crc
                        counter = 0;
                        continue;
                    }
                    chunk.RemoveRange(chunk.Count - 2, 2); //Get rid from crc bytes
                    resBytes.AddRange(chunk);
                    prevPos = i + 1; //go to next packet
                    counter = 0;
                }
            }
            return resBytes.ToArray();
        }
    }

    public class Crc16
    {
        private const ushort Polynomial = 0xA001;
        private readonly ushort[] _table = new ushort[256];

        public Crc16()
        {
            for (ushort i = 0; i < _table.Length; ++i)
            {
                ushort value = 0;
                var temp = i;
                for (byte j = 0; j < 8; ++j)
                {
                    if (((value ^ temp) & 0x0001) != 0)
                        value = (ushort)((value >> 1) ^ Polynomial);
                    else
                        value >>= 1;
                    temp >>= 1;
                }
                _table[i] = value;
            }
        }

        public ushort ComputeChecksum(byte[] bytes)
        {
            ushort crc = 0;
            foreach (var t in bytes)
            {
                var index = (byte)(crc ^ t);
                crc = (ushort)((crc >> 8) ^ _table[index]);
            }
            return crc;
        }

        public byte[] ComputeChecksumBytes(byte[] bytes)
        {
            var crc = ComputeChecksum(bytes);
            return BitConverter.GetBytes(crc);
        }
    }

    public static class ListExtensions
    {
        /// <summary>
        /// Srinks lits to smaller lists
        /// </summary>
        /// <typeparam name="T">Тypе оf elements</typeparam>
        /// <param name="source">List to chunk</param>
        /// <param name="chunkSize">Elements in new lists</param>
        /// <returns>List of new lists</returns>
        public static List<List<T>> ChunkBy<T>(this List<T> source, int chunkSize) => source
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / chunkSize)
                .Select(x => x.Select(v => v.Value).ToList())
                .ToList();
    }
}
