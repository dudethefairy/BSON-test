using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSON_test
{
    internal class BsonDecoder
    {
        private BinaryReader reader;
        private Dictionary<string, object> data;

        public BsonDecoder(BinaryReader reader)
        {
            this.reader = reader;
            this.data = new Dictionary<string, object>();
        }

        public Dictionary<string, object> Decode()
        {
            DecodeDocument();

            return data;
        }

        void DecodeDocument()
        {
            int size = reader.ReadInt32();

            DecodeEList();

            int stop = reader.ReadByte();
            if (stop != 0)
                throw new BsonDecoderException("Missing \\x00 after document");
        }

        void DecodeEList()
        {
            while (reader.PeekChar() != 0)
            {
                DecodeElement();
            }
        }
        void DecodeElement()
        {
            int type = reader.ReadByte();
            string name = DecodeCString();
            
            switch (type)
            {
                case 1:
                    double value = reader.ReadDouble();
                    data.Add(name, value);
                    break;
                case 2:
                    string value2 = DecodeString();
                    data.Add(name, value2);
                    break;
                case 3:
                case 4:
                    Dictionary<string,object> value34 = new BsonDecoder(reader).Decode();
                    data.Add(name, value34);
                    break;
                case 16:
                    int value16 = reader.ReadInt32();
                    data.Add(name, value16);
                    break;
                default:
                    throw new BsonDecoderException("Unhadled element type");
            }
        }

        private string DecodeString()
        {
            string str = "";
            int len = reader.ReadInt32()-1;

            for (int i = 0; i < len; i++)
                str += (char)reader.ReadByte();

            reader.ReadByte();

            return str;
        }
        private string DecodeCString()
        {
            string str = "";
            while (reader.PeekChar() != 0)
                str += reader.ReadChar();

            reader.ReadByte();
            return str;
        }
    }
}
