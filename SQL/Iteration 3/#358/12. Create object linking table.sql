USE [APPartment]

CREATE TABLE [dbo].[ObjectLink] (
    [ID] bigint IDENTITY(1, 1) NOT NULL,
	[ObjectID] bigint NOT NULL,
	[ObjectBID] bigint NOT NULL,
	[ObjectLinkType] nvarchar(255) NOT NULL,
	[TargetObjectID] bigint NOT NULL,
	CONSTRAINT PK_ObjectLink PRIMARY KEY ([ID]),
	CONSTRAINT FK_ObjectObjectLink FOREIGN KEY ([ObjectID])
    REFERENCES [dbo].[Object]([ObjectID])
);

ALTER TABLE [dbo].[ObjectLink]
ADD CONSTRAINT FK_TargetObjectObjectLink FOREIGN KEY ([TargetObjectID])
    REFERENCES [dbo].[Object]([ObjectID])

ALTER TABLE [dbo].[ObjectLink]
ADD CONSTRAINT FK_TargetObjectBObjectLink FOREIGN KEY ([ObjectBID])
    REFERENCES [dbo].[Object]([ObjectID])