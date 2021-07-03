USE APPartment;

ALTER TABLE [dbo].[SurveyQuestion]
ADD EnableOneCorrectAnswer BIT;

ALTER TABLE [dbo].[SurveyQuestion]
ADD EnableManyCorrectAnswers BIT;

ALTER TABLE [dbo].[SurveyQuestion]
ADD EnableOpenEndedAnswer BIT;

ALTER TABLE [dbo].[SurveyQuestion]
ADD EnableRatingAnswer BIT;

ALTER TABLE [dbo].[SurveyQuestion]
ADD EnableQuestionWithNoIncorrectAnswers BIT;