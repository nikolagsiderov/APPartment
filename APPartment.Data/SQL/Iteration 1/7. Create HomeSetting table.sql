USE APPartment

CREATE TABLE [dbo].[HomeSetting] (
    Id bigint IDENTITY(1, 1) NOT NULL,
	ObjectId bigint NOT NULL,
	RentDueDateDay int,
	HomeName nvarchar(max),
	HomeId bigint NOT NULL,
	CONSTRAINT PK_HomeSetting PRIMARY KEY (Id),
	CONSTRAINT FK_ObjectHomeSetting FOREIGN KEY (ObjectId)
    REFERENCES dbo.Object(ObjectId)
);

ALTER TABLE [dbo].[HomeSetting]
ADD CONSTRAINT FK_HomeHomeSetting FOREIGN KEY (HomeId)
    REFERENCES dbo.Home(Id)