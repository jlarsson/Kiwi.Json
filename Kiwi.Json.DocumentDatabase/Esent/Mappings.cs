using System;
using Kiwi.Fluesent;
using Kiwi.Fluesent.Ddl;

namespace Kiwi.Json.DocumentDatabase.Esent
{
    public static class Mappings
    {
        public static IDatabaseDefinition DatabaseDefinition =
            new DatabaseDefinition()
                .Table("Document")
                    .Column("DocumentId").AsInt64().AutoIncrement().NotNull()
                    .Column("CollectionId").AsInt64().NotNull()
                    .Column("DocumentKey").AsString().NotNull()
                    .Column("DocumentJson").AsText().NotNull()
                    .Index("PK_Doucment_DocumentId").Asc("DocumentId").Primary().Unique().DisallowNull()
                    .Index("IX_Document_DocumentKey").Asc("DocumentKey").Unique().DisallowNull()
                    .Index("IX_Document_CollectionId").Asc("CollectionId").DisallowNull()
                .Table("Collection")
                    .Column("CollectionId").AsInt64().AutoIncrement().NotNull()
                    .Column("CollectionName").AsString().NotNull()
                    .Index("PK_Collection_CollectionId").Asc("CollectionId").Unique().Primary()
                    .Index("IX_Collection_CollectionName").Asc("CollectionName").Unique()
                .Table("Index")
                    .Column("CollectionId").AsInt64().NotNull().AutoIncrement().Table
                    .Column("JsonPath").AsString().NotNull().Table
                    .Column("JsonDefinition").AsText().NotNull().Table
                    .Index("PK_Collection_Id").Asc("CollectionId").DisallowNull().Table
                    .Index("IX_Collection_Id_JsonPath").Asc("CollectionId", "JsonPath").DisallowNull()
                .Table("IndexValue")
                    .Column("CollectionId").AsInt64().NotNull()
                    .Column("DocumentId").AsInt64().NotNull()
                    .Column("Json").AsString().NotNull()
                    .Index("PK_IndexValue_CollectionId_DocumentId").Asc("CollectionId", "DocumentId").DisallowNull()
                    .Index("IX_IndexValue_Json").Asc("Json").DisallowNull()
                    .Database();

        public class DocumentRecord
        {
            public Int64 DocumentId { get; set; }
            public string DocumentKey { get; set; }
            public string DocumentJson { get; set; }
        }

        public class CollectionRecord
        {
            public Int64 CollectionId { get; set; }
            public string CollectionName { get; set; }
        }

        public class IndexRecord
        {
            public Int64 CollectionId { get; set; }
            public string JsonPath { get; set; }
            public string JsonDefinition { get; set; }
        }

        public class IndexValueRecord
        {
            public Int64 CollectionId { get; set; }
            public Int64 DocumentId { get; set; }
            public string Json { get; set; }
        }

        public static readonly IRecordMapper<DocumentRecord> DocumentRecordMapper = new RecordMapper<DocumentRecord>()
            .Int64("DocumentId", (r, v) => r.DocumentId = v)
            .String("DocumentKey", (r, v) => r.DocumentKey = v)
            .String("DocumentJson", (r, v) => r.DocumentJson = v);

        public static readonly IRecordMapper<CollectionRecord> CollectionRecordMapper = new RecordMapper<CollectionRecord>()
            .Int64("CollectionId", (r, v) => r.CollectionId = v)
            .String("CollectionName", (r, v) => r.CollectionName = v);

        public static readonly IRecordMapper<IndexRecord> IndexRecordMapper = new RecordMapper<IndexRecord>()
            .Int64("CollectionId", (r, v) => r.CollectionId = v)
            .String("JsonPath", (r, v) => r.JsonPath = v)
            .String("JsonDefinition", (r, v) => r.JsonDefinition = v);

        public static readonly IRecordMapper<IndexValueRecord> IndexValueRecordMapper = new RecordMapper<IndexValueRecord>()
            .Int64("CollectionId", (r, v) => r.CollectionId = v)
            .Int64("DocumentId", (r, v) => r.DocumentId = v)
            .String("Json", (r, v) => r.Json = v);
    }
}