USE [APPartment]

CREATE TABLE [dbo].[Issue] (
    [ID] bigint IDENTITY(1, 1) NOT NULL,
	[ObjectID] bigint NOT NULL,
	[HomeID] bigint NOT NULL,
	CONSTRAINT PK_Issue PRIMARY KEY ([ID]),
	CONSTRAINT FK_ObjectIssue FOREIGN KEY ([ObjectID])
    REFERENCES [dbo].[Object]([ObjectID])
);

ALTER TABLE [dbo].[Issue]
ADD CONSTRAINT FK_HomeIssue FOREIGN KEY ([HomeID])
    REFERENCES [dbo].[Home]([ID])