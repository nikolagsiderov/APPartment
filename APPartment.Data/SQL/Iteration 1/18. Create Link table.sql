USE APPartment2

CREATE TABLE [dbo].[Link] (
    Id bigint NOT NULL,
	ObjectId bigint NOT NULL,
	HomeId bigint NOT NULL,
	LinkTypeId bigint NOT NULL,
	LinkedObjectId bigint NOT NULL,
	TargetObjectId bigint NOT NULL,
	ConnectedLinkTypeId bigint,
	CONSTRAINT PK_Link PRIMARY KEY (Id),
	CONSTRAINT FK_ObjectLink FOREIGN KEY (ObjectId)
    REFERENCES dbo.Object(ObjectId)
);

	ALTER TABLE [dbo].[Link]
ADD CONSTRAINT FK_HomeLink FOREIGN KEY (HomeId)
    REFERENCES dbo.[Home](Id)

	ALTER TABLE [dbo].[Link]
ADD CONSTRAINT FK_LinkTypeLink FOREIGN KEY (LinkTypeId)
    REFERENCES dbo.[LinkType](Id)

	ALTER TABLE [dbo].[Link]
ADD CONSTRAINT FK_LinkedObjectLink FOREIGN KEY (LinkedObjectId)
    REFERENCES dbo.[Object](ObjectId)

	ALTER TABLE [dbo].[Link]
ADD CONSTRAINT FK_TargetObjectLink FOREIGN KEY (TargetObjectId)
    REFERENCES dbo.[Object](ObjectId)