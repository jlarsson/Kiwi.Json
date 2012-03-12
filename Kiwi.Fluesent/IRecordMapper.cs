using System;
using System.Collections.Generic;
using Microsoft.Isam.Esent.Interop;

namespace Kiwi.Fluesent
{
    public interface IRecordMapper<T>
    {
        void DefineMapping(Action<JET_SESID, JET_TABLEID,IDictionary<string, JET_COLUMNID>, T> map);
        T Map(JET_SESID session, JET_TABLEID table, T instance);
    }
}