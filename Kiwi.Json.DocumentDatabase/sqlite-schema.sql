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
    CollectionId INT REFERENCES DocumentCollection (CollectionId) ON DELETE CASCADE, 
    JsonPath TEXT COLLATE NOCASE,
	Definition TEXT,
	TableName TEXT,
	PRIMARY KEY (CollectionId, JsonPath)
);

