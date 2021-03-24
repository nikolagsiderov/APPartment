USE [APPartment]

CREATE TABLE [dbo].[User] (
    [ID] bigint IDENTITY(1, 1) NOT NULL,
	[ObjectID] bigint NOT NULL,
	[Password] nvarchar(max) NOT NULL,
	CONSTRAINT PK_User PRIMARY KEY ([ID]),
	CONSTRAINT FK_ObjectUser FOREIGN KEY ([ObjectID])
    REFERENCES [dbo].[Object]([ObjectID])
);