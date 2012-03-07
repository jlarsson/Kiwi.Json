using System;
using System.Collections.Generic;
using System.Data;

namespace Kiwi.Json.DocumentDatabase.Data
{
    public interface IDatabaseCommand
    {
        string CommandText { get; }
        IEnumerable<KeyValuePair<string, object>> Parameters { get; }
        IDatabaseCommand Param(string name, object value);
        void Execute();
        IEnumerable<T> Query<T>(Func<IDataReader, T> map);
    }
}