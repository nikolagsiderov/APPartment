USE [APPartment]

ALTER TABLE [dbo].[Inventory]
DROP CONSTRAINT FK_HomeInventory;

ALTER TABLE [dbo].[Inventory]
DROP COLUMN HomeID;