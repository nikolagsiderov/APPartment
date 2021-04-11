USE [APPartment]

ALTER TABLE [dbo].[Object]
ADD HomeID bigint;

ALTER TABLE [dbo].[Object]
ADD CONSTRAINT FK_HomeObject FOREIGN KEY ([HomeID])
    REFERENCES [dbo].[Home]([ID])