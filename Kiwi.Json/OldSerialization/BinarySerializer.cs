using System.IO;
using Kiwi.Json.Untyped;

namespace Kiwi.Json.Serialization
{
    public class BinarySerializer : IJsonValueVisitor<IJsonValue>
    {
        private readonly BinaryWriter _writer;

        public BinarySerializer(BinaryWriter writer)
        {
            _writer = writer;
        }

        #region IJsonValueVisitor<IJsonValue> Members

        public IJsonValue VisitArray(IJsonArray value)
        {
            _writer.Write((byte) JsonValueKind.Array);
            _writer.Write(value.Count);
            foreach (var elem in value)
            {
                elem.Visit(this);
            }
            return value;
        }

        public IJsonValue VisitBool(IJsonBool value)
        {
            _writer.Write((byte) JsonValueKind.Bool);
            _writer.Write(value.Value);
            return value;
        }

        public IJsonValue VisitDate(IJsonDate value)
        {
            _writer.Write((byte) JsonValueKind.Date);
            _writer.Write(value.Value.ToBinary());
            return value;
        }

        public IJsonValue VisitDouble(IJsonDouble value)
        {
            _writer.Write((byte) JsonValueKind.Double);
            _writer.Write(value.Value);
            return value;
        }

        public IJsonValue VisitInteger(IJsonInteger value)
        {
            _writer.Write((byte) JsonValueKind.Integer);
            _writer.Write(value.Value);
            return value;
        }

        public IJsonValue VisitNull(IJsonNull value)
        {
            _writer.Write((byte) JsonValueKind.Null);
            return value;
        }

        public IJsonValue VisitObject(IJsonObject value)
        {
            _writer.Write((byte) JsonValueKind.Object);
            _writer.Write(value.Count);
            foreach (var kv in value)
            {
                _writer.Write(kv.Key);
                kv.Value.Visit(this);
            }
            return value;
        }

        public IJsonValue VisitString(IJsonString value)
        {
            _writer.Write((byte) JsonValueKind.String);
            _writer.Write(value.Value);
            return value;
        }

        #endregion
    }
}