USE APPartment

UPDATE [dbo].[ObjectType]
SET [Area] = 'Surveys'
WHERE [Name] = 'SurveyType';

UPDATE [dbo].[ObjectType]
SET [Area] = NULL
WHERE [Area] = 'default';