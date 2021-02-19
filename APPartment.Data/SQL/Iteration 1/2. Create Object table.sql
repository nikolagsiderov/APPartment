USE APPartment

CREATE TABLE [dbo].[Object] (
    ObjectId bigint IDENTITY(1, 1) NOT NULL,
	ObjectTypeId bigint NOT NULL,
    Name nvarchar(MAX) NOT NULL,
	Details nvarchar(MAX),
	CreatedById bigint NOT NULL,
	CreatedDate datetime2(7) NOT NULL,
	ModifiedById bigint,
	ModifiedDate datetime2(7)
	CONSTRAINT PK_Object PRIMARY KEY (ObjectId),
	CONSTRAINT FK_ObjectTypeObject FOREIGN KEY (ObjectTypeId)
    REFERENCES dbo.ObjectType(Id)
);