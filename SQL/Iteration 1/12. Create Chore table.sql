USE [APPartment]

CREATE TABLE [dbo].[Chore] (
    [ID] bigint IDENTITY(1, 1) NOT NULL,
	[ObjectID] bigint NOT NULL,
	[AssignedToUserID] bigint,
	[HomeID] bigint NOT NULL,
	CONSTRAINT PK_Chore PRIMARY KEY ([ID]),
	CONSTRAINT FK_ObjectChore FOREIGN KEY ([ObjectID])
    REFERENCES [dbo].[Object]([ObjectID])
);

ALTER TABLE [dbo].[Chore]
ADD CONSTRAINT FK_HomeChore FOREIGN KEY ([HomeID])
    REFERENCES [dbo].[Home]([ID])

ALTER TABLE [dbo].[Chore]
ADD CONSTRAINT FK_UserChore FOREIGN KEY ([AssignedToUserID])
    REFERENCES [dbo].[User]([ID])