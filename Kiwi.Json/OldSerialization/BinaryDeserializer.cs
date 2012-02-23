using System;
using System.IO;
using Kiwi.Json.Untyped;

namespace Kiwi.Json.Serialization
{
    public class BinaryDeserializer
    {
        private readonly IJsonFactory _factory;
        private readonly BinaryReader _reader;

        public BinaryDeserializer(BinaryReader reader) : this(reader, JsonFactory.Default)
        {
            _reader = reader;
        }

        public BinaryDeserializer(BinaryReader reader, IJsonFactory factory)
        {
            _reader = reader;
            _factory = factory;
        }

        public IJsonValue Read()
        {
            var kind = (JsonValueKind) _reader.Read();
            switch (kind)
            {
                case JsonValueKind.Array:
                    {
                        var a = _factory.CreateArray();
                        var count = _reader.ReadInt32();
                        for (var i = 0; i < count; ++i)
                        {
                            a.Add(Read());
                        }
                        return a;
                    }
                case JsonValueKind.Bool:
                    return _factory.CreateBool(_reader.ReadBoolean());
                case JsonValueKind.Date:
                    return _factory.CreateDate(DateTime.FromBinary(_reader.ReadInt64()));
                case JsonValueKind.Double:
                    return _factory.CreateNumber(_reader.ReadDouble());
                case JsonValueKind.Integer:
                    return _factory.CreateNumber(_reader.ReadInt64());
                case JsonValueKind.Null:
                    return _factory.CreateNull();
                case JsonValueKind.Object:
                    {
                        var o = _factory.CreateObject();
                        var count = _reader.ReadInt32();
                        for (var i = 0; i < count; ++i)
                        {
                            o.Add(_reader.ReadString(), Read());
                        }
                        return o;
                    }
                case JsonValueKind.String:
                    return _factory.CreateString(_reader.ReadString());
                default:
                    throw new ApplicationException("Illegal tag in binary json data");
            }
        }
    }
}