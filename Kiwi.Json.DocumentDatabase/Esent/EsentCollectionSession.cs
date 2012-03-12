using System;
using System.Collections.Generic;
using System.Linq;
using Kiwi.Fluesent;
using Kiwi.Json.DocumentDatabase.Data;
using Kiwi.Json.Untyped;

namespace Kiwi.Json.DocumentDatabase.Esent
{
    public class EsentCollectionSession : ICollectionSession
    {
        private readonly string _collectionName;
        public IEsentDatabase Database { get; protected set; }
        private IEsentInstance _instance;
        private IEsentSession _session;
        private IEsentTransaction _transaction;

        public EsentCollectionSession(IEsentDatabase database, string collectionName)
        {
            _collectionName = collectionName;
            Database = database;
        }

        public void Dispose()
        {
            if (_transaction != null)
            {
                _transaction.Dispose();
            }
            if (_session != null)
            {
                _session.Dispose();
            }
            if (_instance != null)
            {
                _instance.Dispose();
            }
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

        public void EnsureIndex(IndexDefinition definition)
        {
            EnsureSessionStarted();
            throw new System.NotImplementedException();
        }

        public IEnumerable<KeyValuePair<string, T>> Find<T>(IJsonValue filter)
        {
            throw new System.NotImplementedException();
        }

        public T Get<T>(string key)
        {
            EnsureSessionStarted();

            using (var documenTable = _transaction.OpenTable("Document"))
            {
                var search = documenTable.CreateSearch(Mappings.DocumentRecordMapper);
                search.IndexName = "IX_Document_DocumentKey";
                var found = search.FindEq(documenTable.CreateKey().String(key.ToLowerInvariant())).FirstOrDefault();

                return found == null ? default(T) : JSON.Read<T>(found.DocumentJson);
            }
        }

        public void Put(string key, IJsonValue document)
        {
            EnsureSessionStarted();

            var collectionId = EnsureCollection();

            using (var documentTable = _transaction.OpenTable("Document"))
            {
                documentTable.CreateInsertRecord()
                    .Int64("CollectionId", collectionId)
                    .AddString("DocumentKey", key.ToLowerInvariant())
                    .AddString("DocumentJson", JSON.Write(document))
                    .Insert();
            }
        }

        private Int64 EnsureCollection()
        {
            using (var table = _transaction.OpenTable("Collection"))
            {
                var search = table.CreateSearch(Mappings.CollectionRecordMapper);
                search.IndexName = "IX_Collection_CollectionName";
                var collection = search.FindEq(table.CreateKey().String(_collectionName)).FirstOrDefault();
                if (collection != null)
                {
                    return collection.CollectionId;
                }

                return table.CreateInsertRecord()
                    .AddString("CollectionName", _collectionName)
                    .InsertWithAutoIncrement64("CollectionId");
            }
        }

        private void EnsureSessionStarted()
        {
            if (_instance == null)
            {
                _instance = Database.CreateInstance();
            }
            if (_session == null)
            {
                _session = _instance.CreateSession();
            }
            if (_transaction == null)
            {
                _transaction = _session.CreateTransaction();
            }
        }

    }
}