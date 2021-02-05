USE APPartment2

CREATE TABLE [dbo].[User] (
    Id bigint NOT NULL,
	ObjectId bigint NOT NULL,
	Password nvarchar(max) NOT NULL,
	CONSTRAINT PK_User PRIMARY KEY (Id),
	CONSTRAINT FK_ObjectUser FOREIGN KEY (ObjectId)
    REFERENCES dbo.Object(ObjectId)
);