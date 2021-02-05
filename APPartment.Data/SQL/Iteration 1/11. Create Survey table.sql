USE APPartment2

CREATE TABLE [dbo].[Survey] (
    Id bigint NOT NULL,
	ObjectId bigint NOT NULL,
	Status int NOT NULL,
	IsCompleted bit NOT NULL,
	HomeId bigint NOT NULL,
	CONSTRAINT PK_Survey PRIMARY KEY (Id),
	CONSTRAINT FK_ObjectSurvey FOREIGN KEY (ObjectId)
    REFERENCES dbo.Object(ObjectId)
);

ALTER TABLE [dbo].[Survey]
ADD CONSTRAINT FK_HomeSurvey FOREIGN KEY (HomeId)
    REFERENCES dbo.Home(Id)