USE [APPartment]

CREATE TABLE [dbo].[HomeUser] (
    [ID] bigint IDENTITY(1, 1) NOT NULL,
	[ObjectID] bigint NOT NULL,
	[HomeID] bigint NOT NULL,
	[UserID] bigint NOT NULL,
	CONSTRAINT PK_HomeUser PRIMARY KEY ([ID]),
	CONSTRAINT FK_ObjectHomeUser FOREIGN KEY ([ObjectID])
    REFERENCES [dbo].[Object]([ObjectID])
);

ALTER TABLE [dbo].[HomeUser]
ADD CONSTRAINT FK_HomeHomeUser FOREIGN KEY ([HomeID])
    REFERENCES [dbo].[Home]([ID])

ALTER TABLE [dbo].[HomeUser]
ADD CONSTRAINT FK_UserHomeUser FOREIGN KEY ([UserID])
    REFERENCES [dbo].[User]([ID])