USE [APPartment]

CREATE TABLE [dbo].[Object] (
    [ObjectID] bigint IDENTITY(1, 1) NOT NULL,
	[ObjectTypeID] bigint NOT NULL,
    [Name] nvarchar(MAX) NOT NULL,
	[Details] nvarchar(MAX),
	[CreatedByID] bigint NOT NULL,
	[CreatedDate] datetime2(7) NOT NULL,
	[ModifiedByID] bigint,
	[ModifiedDate] datetime2(7)
	CONSTRAINT PK_Object PRIMARY KEY ([ObjectID]),
	CONSTRAINT FK_ObjectTypeObject FOREIGN KEY ([ObjectTypeID])
    REFERENCES [dbo].[ObjectType](ID)
);