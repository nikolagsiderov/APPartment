USE [APPartment]

CREATE TABLE [dbo].[SurveyType] (
    [ID] bigint IDENTITY(1, 1) NOT NULL,
	[ObjectID] bigint NOT NULL,
	CONSTRAINT PK_SurveyType PRIMARY KEY ([ID]),
	CONSTRAINT FK_ObjectSurveyType FOREIGN KEY ([ObjectID])
    REFERENCES [dbo].[Object]([ObjectID])
);

GO

ALTER TABLE [dbo].[Survey]
ADD [SurveyTypeID] bigint NOT NULL DEFAULT(0);

ALTER TABLE [dbo].[Survey]
ADD CONSTRAINT FK_SurveyTypeSurvey
FOREIGN KEY (SurveyTypeID) REFERENCES [dbo].[SurveyType](ID);