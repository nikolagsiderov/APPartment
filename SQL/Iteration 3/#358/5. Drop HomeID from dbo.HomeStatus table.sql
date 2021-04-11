USE [APPartment]

ALTER TABLE [dbo].[HomeStatus]
DROP CONSTRAINT FK_HomeHomeStatus;

ALTER TABLE [dbo].[HomeStatus]
DROP COLUMN HomeID;