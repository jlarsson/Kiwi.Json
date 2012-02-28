using System;
using System.Collections.Generic;
using Kiwi.Json.Untyped;

namespace Kiwi.Json.Conversion
{
    public class ConstructJsonValue : IJsonWriter
    {
        private readonly Stack<string> _memberNames = new Stack<string>();
        private readonly Stack<IJsonValue> _values = new Stack<IJsonValue>();

        #region IJsonWriter Members

        public void WriteString(string value)
        {
            _values.Push(new JsonString(value));
        }

        public void WriteInteger(long value)
        {
            _values.Push(new JsonInteger(value));
        }

        public void WriteDouble(double value)
        {
            _values.Push(new JsonDouble(value));
        }

        public void WriteDate(DateTime value)
        {
            _values.Push(new JsonDate(value));
        }

        public void WriteBool(bool value)
        {
            _values.Push(new JsonBool(value));
        }

        public void WriteNull()
        {
            _values.Push(new JsonNull());
        }

        public void WriteArrayStart()
        {
            _values.Push(new JsonArray());
        }

        public void WriteArrayElementDelimiter()
        {
            var value = _values.Pop();
            ((IJsonArray) _values.Peek()).Add(value);
        }

        public void WriteArrayEnd(int elementCount)
        {
            if (elementCount > 0)
            {
                var value = _values.Pop();
                ((IJsonArray) _values.Peek()).Add(value);
            }
        }

        public void WriteObjectStart()
        {
            _values.Push(new JsonObject());
        }

        public void WriteMember(string memberName)
        {
            _memberNames.Push(memberName);
        }

        public void WriteObjectMemberDelimiter()
        {
            var name = _memberNames.Pop();
            var value = _values.Pop();

            ((IJsonObject) _values.Peek()).Add(name, value);
        }

        public void WriteObjectEnd(int memberCount)
        {
            if (memberCount > 0)
            {
                var value = _values.Pop();
                var name = _memberNames.Pop();
                ((IJsonObject) _values.Peek()).Add(name, value);
            }
        }

        #endregion

        public IJsonValue GetValue()
        {
            return _values.Peek();
        }
    }
}