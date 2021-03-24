USE [APPartment]

ALTER TABLE [dbo].[Inventory]
ADD [IsSupplied] BIT NOT NULL DEFAULT(0);