USE [APPartment]

ALTER TABLE [dbo].[Message]
DROP CONSTRAINT FK_HomeMessage;

ALTER TABLE [dbo].[Message]
DROP COLUMN HomeID;