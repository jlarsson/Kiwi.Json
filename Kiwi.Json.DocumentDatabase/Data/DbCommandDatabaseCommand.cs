using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Kiwi.Json.DocumentDatabase.Data
{
    public class DbCommandDatabaseCommand : IDatabaseCommand
    {
        public DbCommandDatabaseCommand(string commandText, IDatabaseCommandExecutor executor)
        {
            CommandText = commandText;
            Executor = executor;
            Parameters = new Dictionary<string, object>();
        }

        public IDatabaseCommandExecutor Executor { get; private set; }

        public Dictionary<string, object> Parameters { get; private set; }

        #region IDatabaseCommand Members

        public string CommandText { get; set; }

        IEnumerable<KeyValuePair<string, object>> IDatabaseCommand.Parameters
        {
            get { return Parameters; }
        }

        public IDatabaseCommand Param(string name, object value)
        {
            Parameters.Add(name, value);
            return this;
        }

        public void Execute()
        {
            Executor.Execute(this);
        }

        public IEnumerable<T> Query<T>(Func<IAccessor, T> map)
        {
            return Executor.Query(this, map);
        }

        #endregion
    }
}