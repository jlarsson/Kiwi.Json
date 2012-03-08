CREATE TABLE IF NOT EXISTS DocumentCollection(
    CollectionId INTEGER PRIMARY KEY AUTOINCREMENT, 
    CollectionName TEXT COLLATE NOCASE UNIQUE
);

CREATE TABLE IF NOT EXISTS Document( 
	DocumentId INTEGER PRIMARY KEY AUTOINCREMENT,
    [Key] TEXT COLLATE NOCASE, 
    Json TEXT, 
    CollectionId INTEGER REFERENCES DocumentCollection (CollectionId) ON DELETE CASCADE,
    UNIQUE (CollectionId, [Key])
);

CREATE TABLE IF NOT EXISTS CollectionIndex(
	CollectionIndexId INTEGER PRIMARY KEY,
    CollectionId INT REFERENCES DocumentCollection (CollectionId) ON DELETE CASCADE, 
    JsonPath TEXT COLLATE NOCASE,
	Definition TEXT,
	UNIQUE (CollectionId, JsonPath)
);

CREATE TABLE IF NOT EXISTS CollectionIndexValue(
	CollectionIndexValueId INTEGER PRIMARY KEY AUTOINCREMENT,
	CollectionIndexId INT REFERENCES CollectionIndex (CollectionIndexId) ON DELETE CASCADE,
	DocumentId INTEGER REFERENCES Document (DocumentId) ON DELETE CASCADE,
	Json TEXT COLLATE NOCASE
);
CREATE INDEX IX_CollectionIndexValue_Json ON CollectionIndexValue(Json);
CREATE INDEX IX_CollectionIndexValue_DocumentId ON CollectionIndexValue(DocumentId);
CREATE INDEX IX_CollectionIndexValue_CollectionIndexId ON CollectionIndex(CollectionIndexId);

