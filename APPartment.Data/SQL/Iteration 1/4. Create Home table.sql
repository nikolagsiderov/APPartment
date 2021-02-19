USE APPartment

CREATE TABLE [dbo].[Home] (
    Id bigint IDENTITY(1, 1) NOT NULL,
	ObjectId bigint NOT NULL,
	Password nvarchar(max) NOT NULL,
	CONSTRAINT PK_Home PRIMARY KEY (Id),
	CONSTRAINT FK_ObjectHome FOREIGN KEY (ObjectId)
    REFERENCES dbo.Object(ObjectId)
);