-- ----------------------------
-- Table structure for "rssinfo"
-- ----------------------------
CREATE TABLE "rssinfo" (
Id INTEGER PRIMARY KEY AUTOINCREMENT,
ServerId INTEGER DEFAULT 1,
ServerName VARCHAR(40),
Name VARCHAR(20),
Link VARCHAR(255),
Website VARCHAR(30),
ShortUrl VARCHAR(5) DEFAULT 'false',
Colors VARCHAR(5) DEFAULT 'true',
Channel TEXT
);
