USE APPartment

CREATE TABLE [dbo].[HomeUser] (
    Id bigint IDENTITY(1, 1) NOT NULL,
	ObjectId bigint NOT NULL,
	HomeId bigint NOT NULL,
	UserId bigint NOT NULL,
	CONSTRAINT PK_HomeUser PRIMARY KEY (Id),
	CONSTRAINT FK_ObjectHomeUser FOREIGN KEY (ObjectId)
    REFERENCES dbo.Object(ObjectId)
);

ALTER TABLE [dbo].[HomeUser]
ADD CONSTRAINT FK_HomeHomeUser FOREIGN KEY (HomeId)
    REFERENCES dbo.Home(Id)

ALTER TABLE [dbo].[HomeUser]
ADD CONSTRAINT FK_UserHomeUser FOREIGN KEY (UserId)
    REFERENCES dbo.[User](Id)