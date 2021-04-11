USE [APPartment]

ALTER TABLE [dbo].[Event]
DROP CONSTRAINT FK_HomeEvent;

ALTER TABLE [dbo].[Event]
DROP COLUMN HomeID;