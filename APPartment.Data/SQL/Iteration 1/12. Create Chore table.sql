USE APPartment

CREATE TABLE [dbo].[Chore] (
    Id bigint IDENTITY(1, 1) NOT NULL,
	ObjectId bigint NOT NULL,
	AssignedToUserId bigint,
	HomeId bigint NOT NULL,
	CONSTRAINT PK_Chore PRIMARY KEY (Id),
	CONSTRAINT FK_ObjectChore FOREIGN KEY (ObjectId)
    REFERENCES dbo.Object(ObjectId)
);

ALTER TABLE [dbo].[Chore]
ADD CONSTRAINT FK_HomeChore FOREIGN KEY (HomeId)
    REFERENCES dbo.Home(Id)

ALTER TABLE [dbo].[Chore]
ADD CONSTRAINT FK_UserChore FOREIGN KEY (AssignedToUserId)
    REFERENCES dbo.[User](Id)