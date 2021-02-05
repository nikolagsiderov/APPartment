USE APPartment2

CREATE TABLE [dbo].[Image] (
    Id bigint NOT NULL,
	ObjectId bigint NOT NULL,
	TargetObjectId bigint NOT NULL,
	FileSize nvarchar(max) NOT NULL,
	CONSTRAINT PK_Image PRIMARY KEY (Id),
	CONSTRAINT FK_ObjectImage FOREIGN KEY (ObjectId)
    REFERENCES dbo.Object(ObjectId)
);

ALTER TABLE [dbo].[Image]
ADD CONSTRAINT FK_TargetObjectImage FOREIGN KEY (TargetObjectId)
    REFERENCES dbo.[Object](ObjectId)