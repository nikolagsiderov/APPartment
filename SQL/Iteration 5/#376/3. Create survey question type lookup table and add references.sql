USE APPartment;

CREATE TABLE [dbo].[SurveyQuestionType] (
    ID BIGINT NOT NULL PRIMARY KEY,
    [Name] NVARCHAR(MAX),
    [Details] NVARCHAR(MAX)
);

GO

ALTER TABLE [dbo].[SurveyQuestion]
ADD [TypeID] BIGINT;

GO

ALTER TABLE [dbo].[SurveyQuestion]
ADD CONSTRAINT FK_SurveyQuestionTypeSurveyQuestion
FOREIGN KEY ([TypeID]) REFERENCES [dbo].[SurveyQuestionType]([ID]);