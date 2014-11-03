-- ----------------------------
-- Table structure for alias_console_command
-- ----------------------------
CREATE TABLE `alias_console_command` (
Id INTEGER PRIMARY KEY AUTOINCREMENT,
NewCommand TEXT,
BaseCommand TEXT
);

-- ----------------------------
-- Table structure for alias_irc_command
-- ----------------------------
CREATE TABLE `alias_irc_command` (
Id INTEGER PRIMARY KEY AUTOINCREMENT,
ServerId INTEGER DEFAULT 1,
ServerName VARCHAR(40),
NewCommand TEXT,
BaseCommand TEXT
);
