USE [APPartment]

CREATE TABLE [dbo].[Image] (
    [ID] bigint IDENTITY(1, 1) NOT NULL,
	[ObjectID] bigint NOT NULL,
	[TargetObjectID] bigint NOT NULL,
	FileSize nvarchar(max) NOT NULL,
	CONSTRAINT PK_Image PRIMARY KEY ([ID]),
	CONSTRAINT FK_ObjectImage FOREIGN KEY ([ObjectID])
    REFERENCES [dbo].[Object]([ObjectID])
);

ALTER TABLE [dbo].[Image]
ADD CONSTRAINT FK_TargetObjectImage FOREIGN KEY ([TargetObjectID])
    REFERENCES [dbo].[Object]([ObjectID])