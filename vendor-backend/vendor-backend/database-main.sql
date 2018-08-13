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

CREATE TABLE tMarket
(
  ID INTEGER PRIMARY KEY AUTOINCREMENT,  
  Name TEXT,
  Address TEXT,
  DayOfWeek INTEGER REFERENCES tDay(ID),
  StartTime TEXT,
  EndTime TEXT,
  CONSTRAINT UN_Name UNIQUE (Name)  
);

INSERT INTO tMarket(Name, Address, DayOfWeek, StartTime, EndTime)
SELECT 'Pedernales Farmers Market', '123 Fake St', 6, '9AM', '2PM'
UNION ALL
SELECT 'Bee Creek Makers Market', '124 Fake St', 7, '9AM', '2PM'
UNION ALL
SELECT 'Lone Star Farmers Market', '125 Fake St', 2, '2PM', '7PM';

CREATE TABLE tDay
(
  ID INTEGER,
  [Day] TEXT,
  CONSTRAINT UN_Day UNIQUE ([Day])  
);

INSERT INTO tDay(ID, [Day])
SELECT 1, 'Monday'
UNION ALL
SELECT 2, 'Tuesday'
UNION ALL
SELECT 3, 'Wednesday'
UNION ALL
SELECT 4, 'Thursday'
UNION ALL
SELECT 5, 'Friday'
UNION ALL
SELECT 6, 'Saturday'
UNION ALL
SELECT 7, 'Sunday';

CREATE TABLE tVendorMarket
(
  ID INTEGER PRIMARY KEY AUTOINCREMENT,
  AccountID INTEGER REFERENCES tAccount(ID),
  MarketID INTEGER REFERENCES tMarket(ID)
);

