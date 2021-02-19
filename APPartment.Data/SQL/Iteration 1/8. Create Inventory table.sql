USE APPartment

CREATE TABLE [dbo].[Inventory] (
    Id bigint IDENTITY(1, 1) NOT NULL,
	ObjectId bigint NOT NULL,
	Status int NOT NULL,
	HomeId bigint NOT NULL,
	CONSTRAINT PK_Inventory PRIMARY KEY (Id),
	CONSTRAINT FK_ObjectInventory FOREIGN KEY (ObjectId)
    REFERENCES dbo.Object(ObjectId)
);

ALTER TABLE [dbo].[Inventory]
ADD CONSTRAINT FK_HomeInventory FOREIGN KEY (HomeId)
    REFERENCES dbo.Home(Id)