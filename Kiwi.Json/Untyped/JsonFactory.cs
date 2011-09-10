using System;
using System.Globalization;

namespace Kiwi.Json.Untyped
{
    public class JsonFactory : IJsonFactory
    {
        private readonly IJsonValue _false = new JsonBool(false);
        private readonly IJsonValue _null = new JsonNull();
        private readonly IJsonValue _true = new JsonBool(true);

        static JsonFactory()
        {
            Default = new JsonFactory();
        }

        public static IJsonFactory Default { get; private set; }

        #region IJsonFactory Members

        public IJsonValue TryCreatePrimitiveValue(object value)
        {
            if ((value == null) || (value is DBNull))
            {
                return _null;
            }
            if ((value is string) || (value is char))
            {
                return new JsonString(value.ToString());
            }
            if ((value is int)
                || (value is long)
                || (value is byte)
                || (value is short)
                || (value is sbyte)
                || (value is ushort)
                || (value is uint)
                || (value is ulong))
            {
                return new JsonInteger(((IConvertible) value).ToInt32(NumberFormatInfo.InvariantInfo));
            }
            if ((value is double)
                || (value is float)
                || (value is decimal))
            {
                return new JsonDouble(((IConvertible) value).ToDouble(NumberFormatInfo.InvariantInfo));
            }
            if (value is DateTime)
            {
                return new JsonDate((DateTime) value);
            }
            return null;
/*
            if (value == null)
            {
                return _null;
            }
            var convertible = value as IConvertible;
            if (convertible == null)
            {
                return null;
            }

            switch (convertible.GetTypeCode())
            {
                case TypeCode.Boolean:
                    return CreateBool((bool) value);
                case TypeCode.Byte:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                    return CreateNumber((convertible).ToInt32(NumberFormatInfo.InvariantInfo));
                case TypeCode.Char:
                case TypeCode.String:
                    return CreateString(value.ToString());
                case TypeCode.DBNull:
                case TypeCode.Empty:
                    return _null;
                case TypeCode.DateTime:
                    return CreateDate(convertible.ToDateTime(DateTimeFormatInfo.InvariantInfo));
                case TypeCode.Double:
                case TypeCode.Single:
                    return CreateNumber((convertible).ToDouble(NumberFormatInfo.InvariantInfo));
                default:
                    return null;
            }
 */
        }

        public IJsonObject CreateObject()
        {
            return new JsonObject();
        }

        public IJsonArray CreateArray()
        {
            return new JsonArray();
        }

        public IJsonValue CreateString(string value)
        {
            return new JsonString(value);
        }

        public IJsonValue CreateNumber(int value)
        {
            return new JsonInteger(value);
        }

        public IJsonValue CreateNumber(double value)
        {
            return new JsonDouble(value);
        }

        public IJsonValue CreateBool(bool value)
        {
            return value ? _true : _false;
        }

        public IJsonValue CreateDate(DateTime value)
        {
            return new JsonDate(value);
        }

        public IJsonValue CreateNull()
        {
            return _null;
        }

        #endregion
    }
}