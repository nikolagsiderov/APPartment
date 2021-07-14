USE APPartment;

ALTER TABLE [dbo].[SurveyParticipantAnswer]
ADD [MarkedAsCorrect] BIT NOT NULL DEFAULT(0);