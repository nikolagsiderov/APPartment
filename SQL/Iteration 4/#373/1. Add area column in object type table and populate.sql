USE APPartment;

ALTER TABLE [dbo].[ObjectType]
ADD [Area] nvarchar(255);

GO

UPDATE [dbo].[ObjectType]
SET [Area] = 'Roommates'
WHERE [Name] = 'User'

UPDATE [dbo].[ObjectType]
SET [Area] = 'Neighbors'
WHERE [Name] = 'Home'

UPDATE [dbo].[ObjectType]
SET [Area] = 'default'
WHERE [Name] = 'HomeStatus'

UPDATE [dbo].[ObjectType]
SET [Area] = 'default'
WHERE [Name] = 'HomeSetting'

UPDATE [dbo].[ObjectType]
SET [Area] = 'Inventory'
WHERE [Name] = 'Inventory'

UPDATE [dbo].[ObjectType]
SET [Area] = 'Issues'
WHERE [Name] = 'Issue'

UPDATE [dbo].[ObjectType]
SET [Area] = 'default'
WHERE [Name] = 'Message'

UPDATE [dbo].[ObjectType]
SET [Area] = 'default'
WHERE [Name] = 'Comment'

UPDATE [dbo].[ObjectType]
SET [Area] = 'default'
WHERE [Name] = 'Image'

UPDATE [dbo].[ObjectType]
SET [Area] = 'Surveys'
WHERE [Name] = 'Survey'

UPDATE [dbo].[ObjectType]
SET [Area] = 'Chores'
WHERE [Name] = 'Chore'

UPDATE [dbo].[ObjectType]
SET [Area] = 'default'
WHERE [Name] = 'HomeUser'

UPDATE [dbo].[ObjectType]
SET [Area] = 'default'
WHERE [Name] = 'Notification'

UPDATE [dbo].[ObjectType]
SET [Area] = 'default'
WHERE [Name] = 'NotificationParticipant'

UPDATE [dbo].[ObjectType]
SET [Area] = 'default'
WHERE [Name] = 'ObjectParticipant'

UPDATE [dbo].[ObjectType]
SET [Area] = 'default'
WHERE [Name] = 'Event'

UPDATE [dbo].[ObjectType]
SET [Area] = 'default'
WHERE [Name] = 'EventParticipant'

UPDATE [dbo].[ObjectType]
SET [Area] = 'default'
WHERE [Name] = 'ObjectLink'

UPDATE [dbo].[ObjectType]
SET [Area] = 'default'
WHERE [Name] = 'SurveyParticipant'

UPDATE [dbo].[ObjectType]
SET [Area] = 'Surveys'
WHERE [Name] = 'SurveyType'