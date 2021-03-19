USE APPartment

CREATE TABLE [dbo].[Hygiene] (
    Id bigint IDENTITY(1, 1) NOT NULL,
	ObjectId bigint NOT NULL,
	HomeId bigint NOT NULL,
	CONSTRAINT PK_Hygiene PRIMARY KEY (Id),
	CONSTRAINT FK_ObjectHygiene FOREIGN KEY (ObjectId)
    REFERENCES dbo.Object(ObjectId)
);

ALTER TABLE [dbo].[Hygiene]
ADD CONSTRAINT FK_HomeHygiene FOREIGN KEY (HomeId)
    REFERENCES dbo.Home(Id)