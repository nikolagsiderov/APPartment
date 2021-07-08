USE APPartment;

ALTER TABLE [dbo].[SurveyQuestion]
DROP COLUMN [EnableOneCorrectAnswer];

GO

ALTER TABLE [dbo].[SurveyQuestion]
DROP COLUMN [EnableManyCorrectAnswers];

GO

ALTER TABLE [dbo].[SurveyQuestion]
DROP COLUMN [EnableOpenEndedAnswer];

GO

ALTER TABLE [dbo].[SurveyQuestion]
DROP COLUMN [EnableRatingAnswer];

GO

ALTER TABLE [dbo].[SurveyQuestion]
DROP COLUMN [EnableQuestionWithNoIncorrectAnswers];