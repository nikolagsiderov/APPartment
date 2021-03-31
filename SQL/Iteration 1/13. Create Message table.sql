USE [APPartment]

CREATE TABLE [dbo].[Message] (
    [ID] bigint IDENTITY(1, 1) NOT NULL,
	[ObjectID] bigint NOT NULL,
	[HomeID] bigint NOT NULL,
	CONSTRAINT PK_Message PRIMARY KEY ([ID]),
	CONSTRAINT FK_ObjectMessage FOREIGN KEY ([ObjectID])
    REFERENCES [dbo].[Object]([ObjectID])
);

ALTER TABLE [dbo].[Message]
ADD CONSTRAINT FK_HomeMessage FOREIGN KEY ([HomeID])
    REFERENCES [dbo].[Home]([ID])