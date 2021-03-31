USE [APPartment]

CREATE TABLE [dbo].[HomeStatus] (
    [ID] bigint IDENTITY(1, 1) NOT NULL,
	[ObjectID] bigint NOT NULL,
	[Status] int NOT NULL,
	[HomeID] bigint NOT NULL,
	[UserID] bigint NOT NULL,
	CONSTRAINT PK_HomeStatus PRIMARY KEY ([ID]),
	CONSTRAINT FK_ObjectHomeStatus FOREIGN KEY ([ObjectID])
    REFERENCES [dbo].[Object]([ObjectID])
);

ALTER TABLE [dbo].[HomeStatus]
ADD CONSTRAINT FK_HomeHomeStatus FOREIGN KEY ([HomeID])
    REFERENCES [dbo].[Home]([ID])

ALTER TABLE [dbo].[HomeStatus]
ADD CONSTRAINT FK_UserHomeStatus FOREIGN KEY ([UserID])
    REFERENCES [dbo].[User]([ID])