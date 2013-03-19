UPDATE uptime SET Memory = REPLACE(Memory, ' MB', '');
UPDATE uptime SET Memory = REPLACE(Memory, 'MB', '');
ALTER TABLE uptime CHANGE column `Memory` `Memory` int(20) NOT NULL default 0;
UPDATE uptime SET Memory = Memory*1024*1024;
