using System;
using System.Collections.Generic;
using Microsoft.Isam.Esent.Interop;

namespace Kiwi.Fluesent
{
    public interface IEsentTable : IDisposable
    {
        JET_SESID JetSesid { get; }
        JET_TABLEID JetTableid { get; }
        IEsentSession Session { get; }
        IEsentTransaction Transaction { get; }
        IDictionary<string, JET_COLUMNID> ColumnNames { get; }
        IInsertRecord CreateInsertRecord();
        ITableKey CreateKey();
        IEsentCursor CreateCursor(string indexName);
    }
}