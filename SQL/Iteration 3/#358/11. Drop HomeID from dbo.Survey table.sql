USE [APPartment]

ALTER TABLE [dbo].[Survey]
DROP CONSTRAINT FK_HomeSurvey;

ALTER TABLE [dbo].[Survey]
DROP COLUMN HomeID;