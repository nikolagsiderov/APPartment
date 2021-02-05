USE APPartment2

CREATE TABLE [dbo].[Issue] (
    Id bigint NOT NULL,
	ObjectId bigint NOT NULL,
	Status int NOT NULL,
	HomeId bigint NOT NULL,
	CONSTRAINT PK_Issue PRIMARY KEY (Id),
	CONSTRAINT FK_ObjectIssue FOREIGN KEY (ObjectId)
    REFERENCES dbo.Object(ObjectId)
);

ALTER TABLE [dbo].[Issue]
ADD CONSTRAINT FK_HomeIssue FOREIGN KEY (HomeId)
    REFERENCES dbo.Home(Id)