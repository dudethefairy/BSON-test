using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace BSON_test
{
    internal class BsonDecoderException : Exception
    {
        public BsonDecoderException()
        {
        }

        public BsonDecoderException(string? message) : base(message)
        {
        }

        public BsonDecoderException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected BsonDecoderException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
