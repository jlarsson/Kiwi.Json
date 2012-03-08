using System.Data.Common;
using System.Data.SQLite;

namespace Kiwi.Json.DocumentDatabase.Sqlite
{
    public class MemoryDatabase : AbstractDatabase, ITxFactory
    {
        public MemoryDatabase()
        {
            Connection = new SQLiteConnection(@"Data Source=:memory:");
            Connection.Open();

            EnforceSchema();
        }

        protected SQLiteConnection Connection { get; private set; }

        protected override ITxFactory TxFactory
        {
            get { return this; }
        }

        #region ITxFactory Members

        ITx ITxFactory.CreateTransaction()
        {
            return new Tx(Connection);
        }

        #endregion

        #region Nested type: Tx

        private class Tx : ITx
        {
            private readonly SQLiteConnection _connection;
            private SQLiteTransaction _transaction;

            public Tx(SQLiteConnection connection)
            {
                _connection = connection;
            }

            #region ITx Members

            public DbCommand CreateCommand()
            {
                if (_transaction == null)
                {
                    _transaction = _connection.BeginTransaction();
                }
                var command = _connection.CreateCommand();
                command.Transaction = _transaction;
                return command;
            }

            public void Commit()
            {
                if (_transaction != null)
                {
                    _transaction.Commit();
                }
            }

            public void Rollback()
            {
                if (_transaction != null)
                {
                    _transaction.Rollback();
                }
            }

            public void Dispose()
            {
                if (_transaction != null)
                {
                    _transaction.Dispose();
                }
            }

            #endregion
        }

        #endregion
    }
}