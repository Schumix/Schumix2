-- ----------------------------
-- Table structure for birthday
-- ----------------------------
CREATE TABLE "birthday" (
Id INTEGER PRIMARY KEY AUTOINCREMENT,
ServerId INTEGER DEFAULT 1,
ServerName VARCHAR(40),
Name VARCHAR(20),
Month INTEGER DEFAULT 1,
Day INTEGER DEFAULT 1,
Enabled VARCHAR(5) DEFAULT 'false'
);
