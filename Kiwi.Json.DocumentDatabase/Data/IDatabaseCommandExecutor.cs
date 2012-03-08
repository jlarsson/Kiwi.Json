using System;
using System.Collections.Generic;
using System.Data;

namespace Kiwi.Json.DocumentDatabase.Data
{
    public interface IDatabaseCommandExecutor
    {
        void Execute(IDatabaseCommand command);
        IEnumerable<T> Query<T>(IDatabaseCommand command, Func<IAccessor, T> map);
    }
}