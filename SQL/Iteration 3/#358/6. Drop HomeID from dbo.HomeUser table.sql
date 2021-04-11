USE [APPartment]

ALTER TABLE [dbo].[HomeUser]
DROP CONSTRAINT FK_HomeHomeUser;

ALTER TABLE [dbo].[HomeUser]
DROP COLUMN HomeID;