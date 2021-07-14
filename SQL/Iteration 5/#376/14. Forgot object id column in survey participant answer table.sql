USE APPartment;

ALTER TABLE [dbo].[SurveyParticipantAnswer]
ADD [ObjectID] BIGINT NOT NULL;

ALTER TABLE [dbo].[SurveyParticipantAnswer]
ADD CONSTRAINT FK_ObjectSurveyParticipantAnswer
FOREIGN KEY ([ObjectID]) REFERENCES [dbo].[Object]([ObjectID]);