USE [APPartment]

CREATE TABLE [dbo].[Inventory] (
    [ID] bigint IDENTITY(1, 1) NOT NULL,
	[ObjectID] bigint NOT NULL,
	[HomeID] bigint NOT NULL,
	CONSTRAINT PK_Inventory PRIMARY KEY ([ID]),
	CONSTRAINT FK_ObjectInventory FOREIGN KEY ([ObjectID])
    REFERENCES [dbo].[Object]([ObjectID])
);

ALTER TABLE [dbo].[Inventory]
ADD CONSTRAINT FK_HomeInventory FOREIGN KEY ([HomeID])
    REFERENCES [dbo].[Home]([ID])