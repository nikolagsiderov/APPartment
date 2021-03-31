USE [APPartment]

CREATE TABLE [dbo].[Comment] (
    [ID] bigint IDENTITY(1, 1) NOT NULL,
	[ObjectID] bigint NOT NULL,
	[TargetObjectID] bigint NOT NULL,
	CONSTRAINT PK_Comment PRIMARY KEY ([ID]),
	CONSTRAINT FK_ObjectComment FOREIGN KEY ([ObjectID])
    REFERENCES [dbo].[Object]([ObjectID])
);

ALTER TABLE [dbo].[Comment]
ADD CONSTRAINT FK_TargetObjectComment FOREIGN KEY ([TargetObjectID])
    REFERENCES [dbo].[Object]([ObjectID])