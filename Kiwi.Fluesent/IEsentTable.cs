using System;
using Microsoft.Isam.Esent.Interop;

namespace Kiwi.Fluesent
{
    public interface IEsentTable : IDisposable
    {
        Table Table { get; }
        IEsentTransaction Transaction { get; }
        IInsertRecord CreateInsertRecord();
        ITableKey CreateKey();
        IEsentTableSearch<T> CreateSearch<T>(IRecordMapper<T> mapper) where T : new();
    }
}