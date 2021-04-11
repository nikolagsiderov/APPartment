USE [APPartment]

ALTER TABLE [dbo].[Issue]
DROP CONSTRAINT FK_HomeIssue;

ALTER TABLE [dbo].[Issue]
DROP COLUMN HomeID;