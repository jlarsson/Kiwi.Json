using System;
using System.Collections.Generic;
using Microsoft.Isam.Esent.Interop;

namespace Kiwi.Fluesent
{
    public interface IRecordMapper<T>
    {
        void DefineMapping(Action<IEsentSession, IEsentTable,IDictionary<string, JET_COLUMNID>, T> map);
        T Map(IEsentSession session, IEsentTable table, T instance);
    }
}