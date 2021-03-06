USE APPartment

CREATE TABLE [dbo].[Comment] (
    Id bigint IDENTITY(1, 1) NOT NULL,
	ObjectId bigint NOT NULL,
	TargetObjectId bigint NOT NULL,
	CONSTRAINT PK_Comment PRIMARY KEY (Id),
	CONSTRAINT FK_ObjectComment FOREIGN KEY (ObjectId)
    REFERENCES dbo.Object(ObjectId)
);

ALTER TABLE [dbo].[Comment]
ADD CONSTRAINT FK_TargetObjectComment FOREIGN KEY (TargetObjectId)
    REFERENCES dbo.[Object](ObjectId)