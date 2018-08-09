CREATE TABLE tAccount 
(
  ID INTEGER PRIMARY KEY AUTOINCREMENT,
  Name TEXT NOT NULL,
  Email TEXT NOT NULL,
  PasswordHash TEXT NOT NULL,
  Salt TEXT NOT NULL,
  Token TEXT NULL,
  Active BOOLEAN NOT NULL DEFAULT 1 CHECK (Active IN (0,1)),
  CONSTRAINT UN_Email UNIQUE (Email),
  CONSTRAINT UN_Name UNIQUE (Name)  
);

CREATE TABLE tLocation
(
  ID INTEGER PRIMARY KEY AUTOINCREMENT,  
  Location TEXT,
  CONSTRAINT UN_Location UNIQUE (Location)  
);

CREATE TABLE tVendorLocation
(
  ID INTEGER PRIMARY KEY AUTOINCREMENT,
  AccountID INTEGER REFERENCES tAccount(ID),
  LocationID INTEGER REFERENCES tLocation(ID)
);

INSERT INTO tLocation(Location)
SELECT 'Pedernales Farmers Market'
UNION ALL
SELECT 'Bee Creek Makers Market'
UNION ALL
SELECT 'Lone Star Farmers Market'
