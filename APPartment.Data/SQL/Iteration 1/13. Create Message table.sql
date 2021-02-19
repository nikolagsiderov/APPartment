USE APPartment

CREATE TABLE [dbo].[Message] (
    Id bigint IDENTITY(1, 1) NOT NULL,
	ObjectId bigint NOT NULL,
	HomeId bigint NOT NULL,
	CONSTRAINT PK_Message PRIMARY KEY (Id),
	CONSTRAINT FK_ObjectMessage FOREIGN KEY (ObjectId)
    REFERENCES dbo.Object(ObjectId)
);

ALTER TABLE [dbo].[Message]
ADD CONSTRAINT FK_HomeMessage FOREIGN KEY (HomeId)
    REFERENCES dbo.Home(Id)