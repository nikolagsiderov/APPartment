USE [APPartment]

CREATE TABLE [dbo].[SurveyParticipant] (
    [ID] bigint IDENTITY(1, 1) NOT NULL,
	[ObjectID] bigint NOT NULL,
	[SurveyID] bigint NOT NULL,
	[UserID] bigint NOT NULL,
	CONSTRAINT PK_SurveyParticipant PRIMARY KEY ([ID]),
	CONSTRAINT FK_ObjectSurveyParticipant FOREIGN KEY ([ObjectID])
    REFERENCES [dbo].[Object]([ObjectID])
);

ALTER TABLE [dbo].[SurveyParticipant]
ADD CONSTRAINT FK_SurveySurveyParticipant FOREIGN KEY ([SurveyID])
    REFERENCES [dbo].[Survey]([ID])

ALTER TABLE [dbo].[SurveyParticipant]
ADD CONSTRAINT FK_TargetUserSurveyParticipant FOREIGN KEY ([UserID])
    REFERENCES [dbo].[User]([ID])