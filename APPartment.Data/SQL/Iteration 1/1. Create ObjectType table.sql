USE APPartment

CREATE TABLE [dbo].[ObjectType] (
    Id bigint NOT NULL,
    Name nvarchar(255) NOT NULL,
	CONSTRAINT PK_ObjectType PRIMARY KEY (Id)
);