USE [APPartment]

CREATE TABLE [dbo].[Survey] (
    [ID] bigint IDENTITY(1, 1) NOT NULL,
	[ObjectID] bigint NOT NULL,
	IsCompleted bit NOT NULL,
	[HomeID] bigint NOT NULL,
	CONSTRAINT PK_Survey PRIMARY KEY ([ID]),
	CONSTRAINT FK_ObjectSurvey FOREIGN KEY ([ObjectID])
    REFERENCES [dbo].[Object]([ObjectID])
);

ALTER TABLE [dbo].[Survey]
ADD CONSTRAINT FK_HomeSurvey FOREIGN KEY ([HomeID])
    REFERENCES [dbo].[Home]([ID])