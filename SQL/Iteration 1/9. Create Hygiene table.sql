USE [APPartment]

CREATE TABLE [dbo].[Hygiene] (
    [ID] bigint IDENTITY(1, 1) NOT NULL,
	[ObjectID] bigint NOT NULL,
	[HomeID] bigint NOT NULL,
	CONSTRAINT PK_Hygiene PRIMARY KEY ([ID]),
	CONSTRAINT FK_ObjectHygiene FOREIGN KEY ([ObjectID])
    REFERENCES [dbo].[Object]([ObjectID])
);

ALTER TABLE [dbo].[Hygiene]
ADD CONSTRAINT FK_HomeHygiene FOREIGN KEY ([HomeID])
    REFERENCES [dbo].[Home]([ID])