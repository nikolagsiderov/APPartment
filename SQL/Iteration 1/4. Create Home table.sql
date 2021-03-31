USE [APPartment]

CREATE TABLE [dbo].[Home] (
    [ID] bigint IDENTITY(1, 1) NOT NULL,
	[ObjectID] bigint NOT NULL,
	[Password] nvarchar(max) NOT NULL,
	CONSTRAINT PK_Home PRIMARY KEY ([ID]),
	CONSTRAINT FK_ObjectHome FOREIGN KEY ([ObjectID])
    REFERENCES [dbo].[Object]([ObjectID])
);