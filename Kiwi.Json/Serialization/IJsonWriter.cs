using System;

namespace Kiwi.Json.Serialization
{
    public interface IJsonWriter
    {
        void WriteString(string value);
        void WriteInteger(long value);
        void WriteDouble(double value);
        void WriteDate(DateTime value);
        void WriteBool(bool value);
        void WriteNull();
        void WriteArrayStart();
        void WriteArrayElementDelimiter();
        void WriteArrayEnd(int elementCount);
        void WriteObjectStart();
        void WriteMember(string memberName);
        void WriteObjectMemberDelimiter();
        void WriteObjectEnd(int memberCount);
    }
}