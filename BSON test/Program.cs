using System;
using System.Text;


namespace BSON_test // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        public const string fileName = "meteorites.bson";
        static void Main(string[] args)
        {
            if (File.Exists(fileName))
            {
                using (var stream = File.Open(fileName, FileMode.Open))
                {
                    BinaryReader reader = new BinaryReader(stream);
                    BsonDecoder decoder = new BsonDecoder(reader);
                    try
                    {
                        Dictionary<string, object> document = decoder.Decode();
                        foreach (KeyValuePair<string, object> item in document)
                        {
                            var record = item.Value as Dictionary<string, object>;

                            Dictionary<string, object> geolocation;
                            if (record.ContainsKey("geolocation"))
                            {
                                geolocation = record["geolocation"] as Dictionary<string, object>;

                            }
                            else
                            {
                                geolocation = null;
                            }

                            string coords = "";
                            if (geolocation != null)
                            {
                                Dictionary<string, object> coordinates = geolocation["coordinates"] as Dictionary<string, object>;
                                //coords = $"{coordinates["0"]} {coordinates["1"]}";
                                coords = coordinates["0"] + " " + coordinates["1"];
                            }

                            var year = record.ContainsKey("year") ? record["year"] : "-no year-";

                            Console.WriteLine($"{record["name"]} {year} - {coords}");
                        }
                    }
                    catch (BsonDecoderException ex)
                    {
                        Console.WriteLine("Error when parsing document");
                        Console.WriteLine(ex.Message);
                    }

                }

            }
        }
    }
}