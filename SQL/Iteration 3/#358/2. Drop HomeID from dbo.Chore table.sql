USE [APPartment]

ALTER TABLE [dbo].[Chore]
DROP CONSTRAINT FK_HomeChore;

ALTER TABLE [dbo].[Chore]
DROP COLUMN HomeID;