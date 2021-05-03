USE [APPartment]

CREATE TABLE [dbo].[SurveyQuestion] (
    [ID] bigint IDENTITY(1, 1) NOT NULL,
	[SurveyID] bigint NOT NULL Default(0),
	[ObjectID] bigint NOT NULL,
	CONSTRAINT PK_SurveyQuestion PRIMARY KEY ([ID]),
	CONSTRAINT FK_ObjectSurveyQuestion FOREIGN KEY ([ObjectID])
    REFERENCES [dbo].[Object]([ObjectID])
);

ALTER TABLE [dbo].[SurveyQuestion]
ADD CONSTRAINT FK_SurveySurveyQuestion FOREIGN KEY ([SurveyID])
    REFERENCES [dbo].[Survey]([ID])