USE APPartment2

CREATE TABLE [dbo].[HomeStatus] (
    Id bigint NOT NULL,
	ObjectId bigint NOT NULL,
	Status int NOT NULL,
	HomeId bigint NOT NULL,
	UserId bigint NOT NULL,
	CONSTRAINT PK_HomeStatus PRIMARY KEY (Id),
	CONSTRAINT FK_ObjectHomeStatus FOREIGN KEY (ObjectId)
    REFERENCES dbo.Object(ObjectId)
);

ALTER TABLE [dbo].[HomeStatus]
ADD CONSTRAINT FK_HomeHomeStatus FOREIGN KEY (HomeId)
    REFERENCES dbo.Home(Id)

ALTER TABLE [dbo].[HomeStatus]
ADD CONSTRAINT FK_UserHomeStatus FOREIGN KEY (UserId)
    REFERENCES dbo.[User](Id)