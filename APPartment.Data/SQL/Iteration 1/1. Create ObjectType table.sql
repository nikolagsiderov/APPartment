USE APPartment

CREATE TABLE [dbo].[ObjectType] (
    Id bigint IDENTITY(1, 1) NOT NULL,
    Name nvarchar(255) NOT NULL,
	CONSTRAINT PK_ObjectType PRIMARY KEY (Id)
);