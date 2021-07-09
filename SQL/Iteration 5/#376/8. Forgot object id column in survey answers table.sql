USE APPartment;

ALTER TABLE [dbo].[SurveyAnswer]
ADD [ObjectID] BIGINT NOT NULL;

ALTER TABLE [dbo].[SurveyAnswer]
ADD CONSTRAINT FK_ObjectSurveyAnswer
FOREIGN KEY ([ObjectID]) REFERENCES [dbo].[Object]([ObjectID]);