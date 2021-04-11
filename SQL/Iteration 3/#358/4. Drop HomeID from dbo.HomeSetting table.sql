USE [APPartment]

ALTER TABLE [dbo].[HomeSetting]
DROP CONSTRAINT FK_HomeHomeSetting;

ALTER TABLE [dbo].[HomeSetting]
DROP COLUMN HomeID;