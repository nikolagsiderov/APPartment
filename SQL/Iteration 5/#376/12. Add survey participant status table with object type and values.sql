USE APPartment;

CREATE TABLE [dbo].[SurveyParticipantStatus] (
    ID BIGINT NOT NULL PRIMARY KEY,
    [Name] NVARCHAR(MAX),
    [Details] NVARCHAR(MAX)
);

GO

INSERT INTO [dbo].[ObjectType] ([ID], [Name], [Area])
VALUES (26, 'SurveyParticipantStatus', 'Surveys');

INSERT INTO [dbo].[SurveyParticipantStatus] ([ID], [Name])
VALUES (1, 'NotStarted');

INSERT INTO [dbo].[SurveyParticipantStatus] ([ID], [Name])
VALUES (2, 'StartedNotCompleted');

INSERT INTO [dbo].[SurveyParticipantStatus] ([ID], [Name])
VALUES (3, 'Submitted');