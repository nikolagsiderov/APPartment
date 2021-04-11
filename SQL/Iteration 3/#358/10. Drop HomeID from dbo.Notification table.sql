USE [APPartment]

ALTER TABLE [dbo].[Notification]
DROP CONSTRAINT FK_HomeNotification;

ALTER TABLE [dbo].[Notification]
DROP COLUMN HomeID;