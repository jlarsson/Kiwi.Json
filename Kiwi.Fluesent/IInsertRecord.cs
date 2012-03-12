using System;
using System.Collections.Generic;
using Microsoft.Isam.Esent.Interop;

namespace Kiwi.Fluesent
{
    public interface IInsertRecord
    {
        void DefineValue(Action<JET_SESID, JET_TABLEID, IDictionary<string, JET_COLUMNID>> adder);
        void Insert();
    }
}