USE [APPartment]

CREATE TABLE [dbo].[Event] (
    [ID] bigint IDENTITY(1, 1) NOT NULL,
	[ObjectID] bigint NOT NULL,
	[StartDate] DATETIME2 NOT NULL,
	[EndDate] DATETIME2 NOT NULL,
	[TargetObjectID] bigint NOT NULL,
	CONSTRAINT PK_Event PRIMARY KEY ([ID]),
	CONSTRAINT FK_ObjectEvent FOREIGN KEY ([ObjectID])
    REFERENCES [dbo].[Object]([ObjectID])
);

ALTER TABLE [dbo].[Event]
ADD CONSTRAINT FK_TargetObjectEvent FOREIGN KEY ([TargetObjectID])
    REFERENCES [dbo].[Object]([ObjectID])