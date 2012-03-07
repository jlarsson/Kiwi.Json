CREATE TABLE IF NOT EXISTS DocumentCollections(
    DocumentCollectionId INTEGER PRIMARY KEY AUTOINCREMENT, 
    DocumentCollectionName TEXT COLLATE NOCASE UNIQUE
);

CREATE TABLE IF NOT EXISTS Documents( 
    DocumentKey TEXT COLLATE NOCASE, 
    DocumentAsJson TEXT, 
    DocumentCollectionId INTEGER REFERENCES DocumentCollections (DocumentCollectionId) ON DELETE CASCADE,
    PRIMARY KEY (DocumentCollectionId, DocumentKey)
);

CREATE TABLE IF NOT EXISTS CollectionIndices(
    DocumentCollectionId INT REFERENCES DocumentCollections (DocumentCollectionId) ON DELETE CASCADE, 
    JPath TEXT,
	PRIMARY KEY (DocumentCollectionId, JPath)
);

