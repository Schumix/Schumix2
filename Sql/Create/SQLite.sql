/* SQLite.sql */

PRAGMA foreign_keys = OFF;

-- ----------------------------
-- Table structure for "admins"
-- ----------------------------
CREATE TABLE "admins" (
Id INTEGER PRIMARY KEY AUTOINCREMENT,
ServerId INTEGER DEFAULT 1,
ServerName VARCHAR(40),
Name VARCHAR(20),
Password VARCHAR(40),
Vhost VARCHAR(50),
Flag BIGINT DEFAULT 0
);

-- ----------------------------
-- Table structure for "banned"
-- ----------------------------
CREATE TABLE "banned" (
Id INTEGER PRIMARY KEY AUTOINCREMENT,
ServerId INTEGER DEFAULT 1,
ServerName VARCHAR(40),
Name VARCHAR(50),
Channel VARCHAR(20),
Reason TEXT,
Year INTEGER DEFAULT 0,
Month INTEGER DEFAULT 0,
Day INTEGER DEFAULT 0,
Hour INTEGER DEFAULT 0,
Minute INTEGER DEFAULT 0
);

-- ----------------------------
-- Table structure for calendar
-- ----------------------------
CREATE TABLE "calendar" (
Id INTEGER PRIMARY KEY AUTOINCREMENT,
ServerId INTEGER DEFAULT 1,
ServerName VARCHAR(40),
Name VARCHAR(50),
Channel VARCHAR(20),
Message TEXT,
Loops VARCHAR(5) DEFAULT 'false',
Year INTEGER DEFAULT 0,
Month INTEGER DEFAULT 0,
Day INTEGER DEFAULT 0,
Hour INTEGER DEFAULT 0,
Minute INTEGER DEFAULT 0,
UnixTime INTEGER DEFAULT 0
);

-- ----------------------------
-- Table structure for "channel"
-- ----------------------------
CREATE TABLE "channels" (
Id INTEGER PRIMARY KEY AUTOINCREMENT,
ServerId INTEGER DEFAULT 1,
ServerName VARCHAR(40),
Functions VARCHAR(500) DEFAULT ',greeter:off,log:on,rejoin:on,commands:on,autohl:off,autokick:off,automode:off,antiflood:off,message:off,compiler:off,gamecommands:off,webtitle:off,randomkick:off,chatterbot:off,nameday:off,birthday:off',
Channel VARCHAR(20),
Password VARCHAR(30),
Enabled VARCHAR(5) DEFAULT 'false',
Error TEXT,
Language VARCHAR(4) DEFAULT 'enUS'
);

-- ----------------------------
-- Table structure for "gitinfo"
-- ----------------------------
CREATE TABLE "gitinfo" (
Id INTEGER PRIMARY KEY AUTOINCREMENT,
ServerId INTEGER DEFAULT 1,
ServerName VARCHAR(40),
Name VARCHAR(20),
Type VARCHAR(20),
Link VARCHAR(255),
Website VARCHAR(30),
Channel TEXT
);

-- ----------------------------
-- Table structure for "hginfo"
-- ----------------------------
CREATE TABLE "hginfo" (
Id INTEGER PRIMARY KEY AUTOINCREMENT,
ServerId INTEGER DEFAULT 1,
ServerName VARCHAR(40),
Name VARCHAR(20),
Link VARCHAR(255),
Website VARCHAR(30),
Channel TEXT
);

-- ----------------------------
-- Table structure for "hlmessage"
-- ----------------------------
CREATE TABLE "hlmessage" (
Id INTEGER PRIMARY KEY AUTOINCREMENT,
ServerId INTEGER DEFAULT 1,
ServerName VARCHAR(40),
Name VARCHAR(20),
Info TEXT,
Enabled VARCHAR(3)
);

-- ----------------------------
-- Table structure for "irc_commands"
-- ----------------------------
CREATE TABLE "irc_commands" (
Id INTEGER PRIMARY KEY AUTOINCREMENT,
Language VARCHAR(4) DEFAULT 'enUS',
Command VARCHAR(30),
Text TEXT
);

-- ----------------------------
-- Table structure for "kicklist"
-- ----------------------------
CREATE TABLE "kicklist" (
Id INTEGER PRIMARY KEY AUTOINCREMENT,
ServerId INTEGER DEFAULT 1,
ServerName VARCHAR(40),
Name VARCHAR(20),
Channel VARCHAR(20),
Reason TEXT
);

-- ----------------------------
-- Table structure for "localized_console_command"
-- ----------------------------
CREATE TABLE "localized_console_command" (
Id INTEGER PRIMARY KEY AUTOINCREMENT,
Language VARCHAR(4) DEFAULT 'enUS',
Command TEXT,
Text TEXT
);

-- ----------------------------
-- Table structure for localized_console_command_help
-- ----------------------------
CREATE TABLE "localized_console_command_help" (
Id INTEGER PRIMARY KEY AUTOINCREMENT,
Language VARCHAR(4) DEFAULT 'enUS',
Command TEXT,
Text TEXT
);

-- ----------------------------
-- Table structure for localized_console_warning
-- ----------------------------
CREATE TABLE "localized_console_warning" (
Id INTEGER PRIMARY KEY AUTOINCREMENT,
Language VARCHAR(4) DEFAULT 'enUS',
Command TEXT,
Text TEXT
);

-- ----------------------------
-- Table structure for "localized_command"
-- ----------------------------
CREATE TABLE "localized_command" (
Id INTEGER PRIMARY KEY AUTOINCREMENT,
Language VARCHAR(4) DEFAULT 'enUS',
Command TEXT,
Text TEXT
);

-- ----------------------------
-- Table structure for "localized_command_help"
-- ----------------------------
CREATE TABLE "localized_command_help" (
Id INTEGER PRIMARY KEY AUTOINCREMENT,
Language VARCHAR(4) DEFAULT 'enUS',
Command TEXT,
Rank INTEGER DEFAULT 0,
Text TEXT
);

-- ----------------------------
-- Table structure for "localized_warning"
-- ----------------------------
CREATE TABLE "localized_warning" (
Id INTEGER PRIMARY KEY AUTOINCREMENT,
Language VARCHAR(4) DEFAULT 'enUS',
Command TEXT,
Text TEXT
);

-- ----------------------------
-- Table structure for "message"
-- ----------------------------
CREATE TABLE "message" (
Id INTEGER PRIMARY KEY AUTOINCREMENT,
ServerId INTEGER DEFAULT 1,
ServerName VARCHAR(40),
Name VARCHAR(20),
Channel VARCHAR(20),
Message TEXT,
Wrote VARCHAR(20),
UnixTime INTEGER DEFAULT 0
);

-- ----------------------------
-- Table structure for "modelist"
-- ----------------------------
CREATE TABLE "modelist" (
Id INTEGER PRIMARY KEY AUTOINCREMENT,
ServerId INTEGER DEFAULT 1,
ServerName VARCHAR(40),
Name VARCHAR(20),
Channel VARCHAR(20),
Rank VARCHAR(10)
);

-- ----------------------------
-- Table structure for "notes"
-- ----------------------------
CREATE TABLE "notes" (
Id INTEGER PRIMARY KEY AUTOINCREMENT,
ServerId INTEGER DEFAULT 1,
ServerName VARCHAR(40),
Code TEXT,
Name VARCHAR(20),
Note TEXT
);

-- ----------------------------
-- Table structure for "notes_users"
-- ----------------------------
CREATE TABLE "notes_users" (
Id INTEGER PRIMARY KEY AUTOINCREMENT,
ServerId INTEGER DEFAULT 1,
ServerName VARCHAR(40),
Name VARCHAR(20),
Password VARCHAR(40),
Vhost VARCHAR(50)
);

-- ----------------------------
-- Table structure for "schumix"
-- ----------------------------
CREATE TABLE "schumix" (
Id INTEGER PRIMARY KEY AUTOINCREMENT,
ServerId INTEGER DEFAULT 1,
ServerName VARCHAR(40),
FunctionName VARCHAR(20),
FunctionStatus VARCHAR(3)
);

-- ----------------------------
-- Table structure for "svninfo"
-- ----------------------------
CREATE TABLE "svninfo" (
Id INTEGER PRIMARY KEY AUTOINCREMENT,
ServerId INTEGER DEFAULT 1,
ServerName VARCHAR(40),
Name VARCHAR(20),
Link VARCHAR(255),
Website VARCHAR(30),
Channel TEXT
);

-- ----------------------------
-- Table structure for birthday
-- ----------------------------
CREATE TABLE "birthday" (
Id INTEGER PRIMARY KEY AUTOINCREMENT,
ServerId INTEGER DEFAULT 1,
ServerName VARCHAR(40),
Name VARCHAR(20),
Year INTEGER DEFAULT 0,
Month INTEGER DEFAULT 1,
Day INTEGER DEFAULT 1,
Enabled VARCHAR(5) DEFAULT 'false'
);

-- ----------------------------
-- Table structure for "uptime"
-- ----------------------------
CREATE TABLE "uptime" (
Id INTEGER PRIMARY KEY AUTOINCREMENT,
Date TEXT,
Uptime TEXT,
Memory INTEGER
);

-- ----------------------------
-- Table structure for wordpressinfo
-- ----------------------------
CREATE TABLE `wordpressinfo` (
Id INTEGER PRIMARY KEY AUTOINCREMENT,
ServerId INTEGER DEFAULT 1,
ServerName VARCHAR(40),
Name VARCHAR(20),
Link VARCHAR(255),
Channel TEXT
);

-- ----------------------------
-- Table structure for ignore_channels
-- ----------------------------
CREATE TABLE `ignore_channels` (
Id INTEGER PRIMARY KEY AUTOINCREMENT,
ServerId INTEGER DEFAULT 1,
ServerName VARCHAR(40),
Channel VARCHAR(20)
);

-- ----------------------------
-- Table structure for ignore_nicks
-- ----------------------------
CREATE TABLE `ignore_nicks` (
Id INTEGER PRIMARY KEY AUTOINCREMENT,
ServerId INTEGER DEFAULT 1,
ServerName VARCHAR(40),
Nick VARCHAR(30)
);

-- ----------------------------
-- Table structure for ignore_commands
-- ----------------------------
CREATE TABLE `ignore_commands` (
Id INTEGER PRIMARY KEY AUTOINCREMENT,
ServerId INTEGER DEFAULT 1,
ServerName VARCHAR(40),
Command VARCHAR(30)
);

-- ----------------------------
-- Table structure for ignore_irc_commands
-- ----------------------------
CREATE TABLE `ignore_irc_commands` (
Id INTEGER PRIMARY KEY AUTOINCREMENT,
ServerId INTEGER DEFAULT 1,
ServerName VARCHAR(40),
Command VARCHAR(30)
);

-- ----------------------------
-- Table structure for maffiagame
-- ----------------------------
CREATE TABLE `maffiagame` (
Id INTEGER PRIMARY KEY AUTOINCREMENT,
ServerId INTEGER DEFAULT 1,
ServerName VARCHAR(40),
Game INTEGER DEFAULT 0,
Name VARCHAR(25),
Survivor INTEGER DEFAULT 0,
Job INTEGER DEFAULT 0,
Active INTEGER DEFAULT 0
);

-- ----------------------------
-- Table structure for ignore_addons
-- ----------------------------
CREATE TABLE `ignore_addons` (
Id INTEGER PRIMARY KEY AUTOINCREMENT,
ServerId INTEGER DEFAULT 1,
ServerName VARCHAR(40),
Addon VARCHAR(50)
);
