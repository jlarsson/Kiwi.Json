using System;
using System.Collections.Generic;
using Microsoft.Isam.Esent.Interop;

namespace Kiwi.Fluesent
{
    public interface ITableKey
    {
        IEnumerable<Action<JET_SESID, JET_TABLEID, MakeKeyGrbit>> Parts { get; }
        void DefineKeyPart(Action<JET_SESID, JET_TABLEID, MakeKeyGrbit> definer);
    }
}