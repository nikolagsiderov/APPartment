USE APPartment;

CREATE TABLE [dbo].[SurveyAnswer] (
    [ID] BIGINT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    [Answer] NVARCHAR(MAX),
    [IsCorrect] BIT NOT NULL DEFAULT(0),
    [ChoiceCap] INT NULL,
	[QuestionID] BIGINT NOT NULL,
    CONSTRAINT FK_SurveyQuestionSurveyAnswer FOREIGN KEY ([QuestionID])
    REFERENCES [dbo].[SurveyQuestion]([ID])
);

GO

CREATE TABLE [dbo].[SurveyParticipantAnswer] (
    [ID] BIGINT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    [SurveyParticipantID] BIGINT NOT NULL,
    [AnswerID] BIGINT NOT NULL,
    CONSTRAINT FK_SurveyParticipantSurveyParticipantAnswer FOREIGN KEY ([SurveyParticipantID])
    REFERENCES [dbo].[SurveyParticipant]([ID])
);

GO

ALTER TABLE [dbo].[SurveyParticipantAnswer]
ADD CONSTRAINT FK_SurveyAnswerSurveyParticipantAnswer
FOREIGN KEY ([AnswerID]) REFERENCES [dbo].[SurveyAnswer]([ID]);

GO

INSERT INTO [dbo].[ObjectType] ([ID], [Name], [Area])
VALUES (24, 'SurveyAnswer', 'Surveys');

INSERT INTO [dbo].[ObjectType] ([ID], [Name], [Area])
VALUES (25, 'SurveyParticipantAnswer', 'Surveys');