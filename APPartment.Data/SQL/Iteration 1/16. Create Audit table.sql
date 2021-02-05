USE APPartment2

CREATE TABLE [dbo].[Audit] (
    Id bigint NOT NULL,
	ObjectId bigint NOT NULL,
	TargetObjectId bigint NOT NULL,
	TableName nvarchar(max),
	[When] datetime2(7) NOT NULL,
	KeyValues nvarchar(max),
	OldValues nvarchar(max),
	NewValues nvarchar(max),
	HomeId bigint NOT NULL,
	UserId bigint NOT NULL,
	CONSTRAINT PK_Audit PRIMARY KEY (Id),
	CONSTRAINT FK_ObjectAudit FOREIGN KEY (ObjectId)
    REFERENCES dbo.Object(ObjectId)
);

ALTER TABLE [dbo].[Audit]
ADD CONSTRAINT FK_TargetObjectAudit FOREIGN KEY (TargetObjectId)
    REFERENCES dbo.[Object](ObjectId)

	ALTER TABLE [dbo].[Audit]
ADD CONSTRAINT FK_HomeAudit FOREIGN KEY (HomeId)
    REFERENCES dbo.[Home](Id)

	ALTER TABLE [dbo].[Audit]
ADD CONSTRAINT FK_UserAudit FOREIGN KEY (UserId)
    REFERENCES dbo.[User](Id)