USE APPartment2

CREATE TABLE [dbo].[Home] (
    Id bigint NOT NULL,
	ObjectId bigint NOT NULL,
	Password nvarchar(max) NOT NULL,
	CONSTRAINT PK_Home PRIMARY KEY (Id),
	CONSTRAINT FK_ObjectHome FOREIGN KEY (ObjectId)
    REFERENCES dbo.Object(ObjectId)
);