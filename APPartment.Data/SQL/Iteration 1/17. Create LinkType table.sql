USE APPartment

CREATE TABLE [dbo].[LinkType] (
    Id bigint IDENTITY(1, 1) NOT NULL,
	ObjectId bigint NOT NULL,
	HomeId bigint NOT NULL,
	ConnectedLinkTypeId bigint,
	CONSTRAINT PK_LinkType PRIMARY KEY (Id),
	CONSTRAINT FK_ObjectLinkType FOREIGN KEY (ObjectId)
    REFERENCES dbo.Object(ObjectId)
);

	ALTER TABLE [dbo].[LinkType]
ADD CONSTRAINT FK_HomeLinkType FOREIGN KEY (HomeId)
    REFERENCES dbo.[Home](Id)