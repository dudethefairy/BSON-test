//using System.IO;
//using System.Runtime.Serialization;

//class Program
//{
//    static void Main(string[] args)
//    {
//        using (var file = File.Open("meteorites.bson", FileMode.Open))
//        {
//            var reader = new BinaryReader(file);
//            var decoder = new BsonDecoder(reader);
//            try
//            {
//                var document = decoder.Decode();
//                foreach (var item in document)
//                {
//                    var record = item.Value as Dictionary<string, object>;

//                    var geolocation = record.ContainsKey("geolocation") ? record["geolocation"] as Dictionary<string, object> : null;
//                    string coords = "";
//                    if (geolocation != null)
//                    {
//                        var coordinates = geolocation["coordinates"] as Dictionary<string, object>;
//                        coords = $"{coordinates["0"]} {coordinates["1"]}";
//                    }

//                    var year = record.ContainsKey("year") ? record["year"] : "-";

//                    Console.WriteLine($"{record["name"]} {year} - {coords}");
//                }
//            }
//            catch (BsonDecoderException ex)
//            {
//                Console.WriteLine("Error when parsing document");
//                Console.WriteLine(ex.Message);
//            }
//        }
//    }
//}

//class BsonDecoderException : Exception
//{
//    public BsonDecoderException()
//    {
//    }

//    public BsonDecoderException(string? message) : base(message)
//    {
//    }

//    public BsonDecoderException(string? message, Exception? innerException) : base(message, innerException)
//    {
//    }

//    protected BsonDecoderException(SerializationInfo info, StreamingContext context) : base(info, context)
//    {
//    }
//}


//class BsonDecoder
//{
//    private BinaryReader reader;
//    private Dictionary<string, object> data;

//    public BsonDecoder(BinaryReader reader)
//    {
//        this.reader = reader;
//        this.data = new Dictionary<string, object>();
//    }

//    public Dictionary<string, object> Decode()
//    {
//        DecodeDocument();

//        return data;
//    }

//    void DecodeDocument()
//    {
//        int size = reader.ReadInt32();

//        DecodeEList();

//        int stop = reader.ReadByte();
//        if (stop != 0)
//            throw new BsonDecoderException("Missing \\x00 after document");
//    }

//    void DecodeEList()
//    {
//        while (reader.PeekChar() != 0)
//        {
//            DecodeElement();
//        }
//    }
//    void DecodeElement()
//    {
//        int type = reader.ReadByte();
//        string name = DecodeCString();

//        if (type == 1)
//        {
//            var value = reader.ReadDouble();
//            data.Add(name, value);
//        }
//        else
//        if (type == 2)
//        {
//            var value = DecodeString();
//            data.Add(name, value);
//        }
//        else
//        if (type == 3)
//        {
//            var value = new BsonDecoder(reader).Decode();
//            data.Add(name, value);
//        }
//        else
//        if (type == 4)
//        {
//            var value = new BsonDecoder(reader).Decode();
//            data.Add(name, value);
//        }
//        else
//        if (type == 16)
//        {
//            var value = reader.ReadInt32();
//            data.Add(name, value);
//        }
//        else
//        {
//            throw new BsonDecoderException("Unhadled element type");
//        }
//    }

//    private string DecodeString()
//    {
//        string str = "";
//        int len = reader.ReadInt32() - 1;

//        for (int i = 0; i < len; i++)
//            str += (char)reader.ReadByte();

//        reader.ReadByte();

//        return str;
//    }
//    private string DecodeCString()
//    {
//        string str = "";
//        while (reader.PeekChar() != 0)
//            str += reader.ReadChar();

//        reader.ReadByte();
//        return str;
//    }
//}
