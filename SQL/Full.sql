USE [APPartment]
GO

CREATE TABLE [dbo].[ObjectType] (
    [ID] bigint NOT NULL,
    [Name] nvarchar(255) NOT NULL,
	CONSTRAINT PK_ObjectType PRIMARY KEY ([ID])
);

GO

CREATE TABLE [dbo].[Object] (
    [ObjectID] bigint IDENTITY(1, 1) NOT NULL,
	[ObjectTypeID] bigint NOT NULL,
    [Name] nvarchar(MAX) NOT NULL,
	[Details] nvarchar(MAX),
	[CreatedByID] bigint NOT NULL,
	[CreatedDate] datetime2(7) NOT NULL,
	[ModifiedByID] bigint,
	[ModifiedDate] datetime2(7)
	CONSTRAINT PK_Object PRIMARY KEY ([ObjectID]),
	CONSTRAINT FK_ObjectTypeObject FOREIGN KEY ([ObjectTypeID])
    REFERENCES [dbo].[ObjectType](ID)
);

GO

CREATE TABLE [dbo].[User] (
    [ID] bigint IDENTITY(1, 1) NOT NULL,
	[ObjectID] bigint NOT NULL,
	[Password] nvarchar(max) NOT NULL,
	CONSTRAINT PK_User PRIMARY KEY ([ID]),
	CONSTRAINT FK_ObjectUser FOREIGN KEY ([ObjectID])
    REFERENCES [dbo].[Object]([ObjectID])
);

GO

CREATE TABLE [dbo].[Home] (
    [ID] bigint IDENTITY(1, 1) NOT NULL,
	[ObjectID] bigint NOT NULL,
	[Password] nvarchar(max) NOT NULL,
	CONSTRAINT PK_Home PRIMARY KEY ([ID]),
	CONSTRAINT FK_ObjectHome FOREIGN KEY ([ObjectID])
    REFERENCES [dbo].[Object]([ObjectID])
);

GO

CREATE TABLE [dbo].[HomeUser] (
    [ID] bigint IDENTITY(1, 1) NOT NULL,
	[ObjectID] bigint NOT NULL,
	[HomeID] bigint NOT NULL,
	[UserID] bigint NOT NULL,
	CONSTRAINT PK_HomeUser PRIMARY KEY ([ID]),
	CONSTRAINT FK_ObjectHomeUser FOREIGN KEY ([ObjectID])
    REFERENCES [dbo].[Object]([ObjectID])
);

ALTER TABLE [dbo].[HomeUser]
ADD CONSTRAINT FK_HomeHomeUser FOREIGN KEY ([HomeID])
    REFERENCES [dbo].[Home]([ID])

ALTER TABLE [dbo].[HomeUser]
ADD CONSTRAINT FK_UserHomeUser FOREIGN KEY ([UserID])
    REFERENCES [dbo].[User]([ID])

GO

CREATE TABLE [dbo].[HomeStatus] (
    [ID] bigint IDENTITY(1, 1) NOT NULL,
	[ObjectID] bigint NOT NULL,
	[Status] int NOT NULL,
	[HomeID] bigint NOT NULL,
	[UserID] bigint NOT NULL,
	CONSTRAINT PK_HomeStatus PRIMARY KEY ([ID]),
	CONSTRAINT FK_ObjectHomeStatus FOREIGN KEY ([ObjectID])
    REFERENCES [dbo].[Object]([ObjectID])
);

ALTER TABLE [dbo].[HomeStatus]
ADD CONSTRAINT FK_HomeHomeStatus FOREIGN KEY ([HomeID])
    REFERENCES [dbo].[Home]([ID])

ALTER TABLE [dbo].[HomeStatus]
ADD CONSTRAINT FK_UserHomeStatus FOREIGN KEY ([UserID])
    REFERENCES [dbo].[User]([ID])

GO

CREATE TABLE [dbo].[HomeSetting] (
    [ID] bigint IDENTITY(1, 1) NOT NULL,
	[ObjectID] bigint NOT NULL,
	RentDueDateDay int,
	HomeName nvarchar(max),
	[HomeID] bigint NOT NULL,
	CONSTRAINT PK_HomeSetting PRIMARY KEY ([ID]),
	CONSTRAINT FK_ObjectHomeSetting FOREIGN KEY ([ObjectID])
    REFERENCES [dbo].[Object]([ObjectID])
);

ALTER TABLE [dbo].[HomeSetting]
ADD CONSTRAINT FK_HomeHomeSetting FOREIGN KEY ([HomeID])
    REFERENCES [dbo].[Home]([ID])

GO

CREATE TABLE [dbo].[Inventory] (
    [ID] bigint IDENTITY(1, 1) NOT NULL,
	[ObjectID] bigint NOT NULL,
	[HomeID] bigint NOT NULL,
	CONSTRAINT PK_Inventory PRIMARY KEY ([ID]),
	CONSTRAINT FK_ObjectInventory FOREIGN KEY ([ObjectID])
    REFERENCES [dbo].[Object]([ObjectID])
);

ALTER TABLE [dbo].[Inventory]
ADD CONSTRAINT FK_HomeInventory FOREIGN KEY ([HomeID])
    REFERENCES [dbo].[Home]([ID])

GO

CREATE TABLE [dbo].[Hygiene] (
    [ID] bigint IDENTITY(1, 1) NOT NULL,
	[ObjectID] bigint NOT NULL,
	[HomeID] bigint NOT NULL,
	CONSTRAINT PK_Hygiene PRIMARY KEY ([ID]),
	CONSTRAINT FK_ObjectHygiene FOREIGN KEY ([ObjectID])
    REFERENCES [dbo].[Object]([ObjectID])
);

ALTER TABLE [dbo].[Hygiene]
ADD CONSTRAINT FK_HomeHygiene FOREIGN KEY ([HomeID])
    REFERENCES [dbo].[Home]([ID])

GO

CREATE TABLE [dbo].[Issue] (
    [ID] bigint IDENTITY(1, 1) NOT NULL,
	[ObjectID] bigint NOT NULL,
	[HomeID] bigint NOT NULL,
	CONSTRAINT PK_Issue PRIMARY KEY ([ID]),
	CONSTRAINT FK_ObjectIssue FOREIGN KEY ([ObjectID])
    REFERENCES [dbo].[Object]([ObjectID])
);

ALTER TABLE [dbo].[Issue]
ADD CONSTRAINT FK_HomeIssue FOREIGN KEY ([HomeID])
    REFERENCES [dbo].[Home]([ID])

GO

CREATE TABLE [dbo].[Survey] (
    [ID] bigint IDENTITY(1, 1) NOT NULL,
	[ObjectID] bigint NOT NULL,
	IsCompleted bit NOT NULL,
	[HomeID] bigint NOT NULL,
	CONSTRAINT PK_Survey PRIMARY KEY ([ID]),
	CONSTRAINT FK_ObjectSurvey FOREIGN KEY ([ObjectID])
    REFERENCES [dbo].[Object]([ObjectID])
);

ALTER TABLE [dbo].[Survey]
ADD CONSTRAINT FK_HomeSurvey FOREIGN KEY ([HomeID])
    REFERENCES [dbo].[Home]([ID])

GO

CREATE TABLE [dbo].[Chore] (
    [ID] bigint IDENTITY(1, 1) NOT NULL,
	[ObjectID] bigint NOT NULL,
	[AssignedToUserID] bigint,
	[HomeID] bigint NOT NULL,
	CONSTRAINT PK_Chore PRIMARY KEY ([ID]),
	CONSTRAINT FK_ObjectChore FOREIGN KEY ([ObjectID])
    REFERENCES [dbo].[Object]([ObjectID])
);

ALTER TABLE [dbo].[Chore]
ADD CONSTRAINT FK_HomeChore FOREIGN KEY ([HomeID])
    REFERENCES [dbo].[Home]([ID])

ALTER TABLE [dbo].[Chore]
ADD CONSTRAINT FK_UserChore FOREIGN KEY ([AssignedToUserID])
    REFERENCES [dbo].[User]([ID])

GO

CREATE TABLE [dbo].[Message] (
    [ID] bigint IDENTITY(1, 1) NOT NULL,
	[ObjectID] bigint NOT NULL,
	[HomeID] bigint NOT NULL,
	CONSTRAINT PK_Message PRIMARY KEY ([ID]),
	CONSTRAINT FK_ObjectMessage FOREIGN KEY ([ObjectID])
    REFERENCES [dbo].[Object]([ObjectID])
);

ALTER TABLE [dbo].[Message]
ADD CONSTRAINT FK_HomeMessage FOREIGN KEY ([HomeID])
    REFERENCES [dbo].[Home]([ID])

GO

CREATE TABLE [dbo].[Comment] (
    [ID] bigint IDENTITY(1, 1) NOT NULL,
	[ObjectID] bigint NOT NULL,
	[TargetObjectID] bigint NOT NULL,
	CONSTRAINT PK_Comment PRIMARY KEY ([ID]),
	CONSTRAINT FK_ObjectComment FOREIGN KEY ([ObjectID])
    REFERENCES [dbo].[Object]([ObjectID])
);

ALTER TABLE [dbo].[Comment]
ADD CONSTRAINT FK_TargetObjectComment FOREIGN KEY ([TargetObjectID])
    REFERENCES [dbo].[Object]([ObjectID])

GO

CREATE TABLE [dbo].[Image] (
    [ID] bigint IDENTITY(1, 1) NOT NULL,
	[ObjectID] bigint NOT NULL,
	[TargetObjectID] bigint NOT NULL,
	FileSize nvarchar(max) NOT NULL,
	CONSTRAINT PK_Image PRIMARY KEY ([ID]),
	CONSTRAINT FK_ObjectImage FOREIGN KEY ([ObjectID])
    REFERENCES [dbo].[Object]([ObjectID])
);

ALTER TABLE [dbo].[Image]
ADD CONSTRAINT FK_TargetObjectImage FOREIGN KEY ([TargetObjectID])
    REFERENCES [dbo].[Object]([ObjectID])

GO

INSERT INTO [dbo].[ObjectType] ([ID], [Name])
VALUES (1, 'User');

INSERT INTO [dbo].[ObjectType] ([ID], [Name])
VALUES (2, 'Home');

INSERT INTO [dbo].[ObjectType] ([ID], [Name])
VALUES (3, 'HomeStatus');

INSERT INTO [dbo].[ObjectType] ([ID], [Name])
VALUES (4, 'HomeSetting');

INSERT INTO [dbo].[ObjectType] ([ID], [Name])
VALUES (5, 'Inventory');

INSERT INTO [dbo].[ObjectType] ([ID], [Name])
VALUES (6, 'Hygiene');

INSERT INTO [dbo].[ObjectType] ([ID], [Name])
VALUES (7, 'Issue');

INSERT INTO [dbo].[ObjectType] ([ID], [Name])
VALUES (8, 'Message');

INSERT INTO [dbo].[ObjectType] ([ID], [Name])
VALUES (9, 'Comment');

INSERT INTO [dbo].[ObjectType] ([ID], [Name])
VALUES (10, 'Image');

INSERT INTO [dbo].[ObjectType] ([ID], [Name])
VALUES (11, 'Survey');

INSERT INTO [dbo].[ObjectType] ([ID], [Name])
VALUES (12, 'Chore');

INSERT INTO [dbo].[ObjectType] ([ID], [Name])
VALUES (13, 'HomeUser');

GO

ALTER TABLE [dbo].[Inventory]
ADD [IsSupplied] BIT NOT NULL DEFAULT(0);

GO

ALTER TABLE [dbo].[Issue]
ADD [IsClosed] BIT NOT NULL DEFAULT(0);

GO

ALTER TABLE [dbo].[Hygiene]
ADD [IsDone] BIT NOT NULL DEFAULT(0);

GO

ALTER TABLE [dbo].[Chore]
ADD [IsDone] BIT NOT NULL DEFAULT(0);

GO

CREATE TABLE [dbo].[Event] (
    [ID] bigint IDENTITY(1, 1) NOT NULL,
	[ObjectID] bigint NOT NULL,
	[StartDate] DATETIME2 NOT NULL,
	[EndDate] DATETIME2 NOT NULL,
	[HomeID] bigint NOT NULL,
	[TargetObjectID] bigint NOT NULL,
	CONSTRAINT PK_Event PRIMARY KEY ([ID]),
	CONSTRAINT FK_ObjectEvent FOREIGN KEY ([ObjectID])
    REFERENCES [dbo].[Object]([ObjectID])
);

ALTER TABLE [dbo].[Event]
ADD CONSTRAINT FK_TargetObjectEvent FOREIGN KEY ([TargetObjectID])
    REFERENCES [dbo].[Object]([ObjectID])

ALTER TABLE [dbo].[Event]
ADD CONSTRAINT FK_HomeEvent FOREIGN KEY ([HomeID])
    REFERENCES [dbo].[Home]([ID])

GO

INSERT INTO [dbo].[ObjectType] ([ID], [Name])
VALUES (17, 'Event');

GO

CREATE TABLE [dbo].[EventParticipant] (
    [ID] bigint IDENTITY(1, 1) NOT NULL,
	[ObjectID] bigint NOT NULL,
	[EventID] bigint NOT NULL,
	[UserID] bigint NOT NULL,
	CONSTRAINT PK_EventParticipant PRIMARY KEY ([ID]),
	CONSTRAINT FK_ObjectEventParticipant FOREIGN KEY ([ObjectID])
    REFERENCES [dbo].[Object]([ObjectID])
);

ALTER TABLE [dbo].[EventParticipant]
ADD CONSTRAINT FK_EventEventParticipant FOREIGN KEY ([EventID])
    REFERENCES [dbo].[Event]([ID])

ALTER TABLE [dbo].[EventParticipant]
ADD CONSTRAINT FK_TargetUserEventParticipant FOREIGN KEY ([UserID])
    REFERENCES [dbo].[User]([ID])

GO

INSERT INTO [dbo].[ObjectType] ([ID], [Name])
VALUES (18, 'EventParticipant');

GO

CREATE TABLE [dbo].[Notification] (
    [ID] bigint IDENTITY(1, 1) NOT NULL,
	[ObjectID] bigint NOT NULL,
	[HomeID] bigint NOT NULL,
	CONSTRAINT PK_Notification PRIMARY KEY ([ID]),
	CONSTRAINT FK_ObjectNotification FOREIGN KEY ([ObjectID])
    REFERENCES [dbo].[Object]([ObjectID])
);

ALTER TABLE [dbo].[Notification]
ADD CONSTRAINT FK_HomeNotification FOREIGN KEY ([HomeID])
    REFERENCES [dbo].[Home]([ID])

GO

CREATE TABLE [dbo].[NotificationParticipant] (
    [ID] bigint IDENTITY(1, 1) NOT NULL,
	[ObjectID] bigint NOT NULL,
	[NotificationID] bigint NOT NULL,
	[UserID] bigint NOT NULL,
	CONSTRAINT PK_NotificationParticipant PRIMARY KEY ([ID]),
	CONSTRAINT FK_ObjectNotificationParticipant FOREIGN KEY ([ObjectID])
    REFERENCES [dbo].[Object]([ObjectID])
);

ALTER TABLE [dbo].[NotificationParticipant]
ADD CONSTRAINT FK_NotificationNotificationParticipant FOREIGN KEY ([NotificationID])
    REFERENCES [dbo].[Notification]([ID])

ALTER TABLE [dbo].[NotificationParticipant]
ADD CONSTRAINT FK_UserNotificationParticipant FOREIGN KEY ([UserID])
    REFERENCES [dbo].[User]([ID])

GO

INSERT INTO [dbo].[ObjectType] ([ID], [Name])
VALUES (14, 'Notification');

INSERT INTO [dbo].[ObjectType] ([ID], [Name])
VALUES (15, 'NotificationParticipant');

GO

CREATE TABLE [dbo].[ObjectParticipant] (
    [ID] bigint IDENTITY(1, 1) NOT NULL,
	[ObjectID] bigint NOT NULL,
	[TargetObjectID] bigint NOT NULL,
	[UserID] bigint NOT NULL,
	CONSTRAINT PK_ObjectParticipant PRIMARY KEY ([ID]),
	CONSTRAINT FK_ObjectObjectParticipant FOREIGN KEY ([ObjectID])
    REFERENCES [dbo].[Object]([ObjectID])
);

ALTER TABLE [dbo].[ObjectParticipant]
ADD CONSTRAINT FK_TargetObjectObjectParticipant FOREIGN KEY ([TargetObjectID])
    REFERENCES [dbo].[Object]([ObjectID])

ALTER TABLE [dbo].[ObjectParticipant]
ADD CONSTRAINT FK_TargetUserObjectParticipant FOREIGN KEY ([UserID])
    REFERENCES [dbo].[User]([ID])

GO

INSERT INTO [dbo].[ObjectType] ([ID], [Name])
VALUES (16, 'ObjectParticipant');

GO

DELETE FROM [dbo].[ObjectType] WHERE [ID] = 6;

GO

/****** Object:  Table [dbo].[Hygiene]    Script Date: 09-Apr-21 4:02:49 PM ******/
DROP TABLE [dbo].[Hygiene]
GO

ALTER TABLE [dbo].[Object]
ADD HomeID bigint;

ALTER TABLE [dbo].[Object]
ADD CONSTRAINT FK_HomeObject FOREIGN KEY ([HomeID])
    REFERENCES [dbo].[Home]([ID])

GO

ALTER TABLE [dbo].[Chore]
DROP CONSTRAINT FK_HomeChore;

ALTER TABLE [dbo].[Chore]
DROP COLUMN HomeID;

GO

ALTER TABLE [dbo].[Event]
DROP CONSTRAINT FK_HomeEvent;

ALTER TABLE [dbo].[Event]
DROP COLUMN HomeID;

GO

ALTER TABLE [dbo].[HomeSetting]
DROP CONSTRAINT FK_HomeHomeSetting;

ALTER TABLE [dbo].[HomeSetting]
DROP COLUMN HomeID;

GO

ALTER TABLE [dbo].[HomeStatus]
DROP CONSTRAINT FK_HomeHomeStatus;

ALTER TABLE [dbo].[HomeStatus]
DROP COLUMN HomeID;

GO

ALTER TABLE [dbo].[HomeUser]
DROP CONSTRAINT FK_HomeHomeUser;

ALTER TABLE [dbo].[HomeUser]
DROP COLUMN HomeID;

GO

ALTER TABLE [dbo].[Inventory]
DROP CONSTRAINT FK_HomeInventory;

ALTER TABLE [dbo].[Inventory]
DROP COLUMN HomeID;

GO

ALTER TABLE [dbo].[Issue]
DROP CONSTRAINT FK_HomeIssue;

ALTER TABLE [dbo].[Issue]
DROP COLUMN HomeID;

GO

ALTER TABLE [dbo].[Message]
DROP CONSTRAINT FK_HomeMessage;

ALTER TABLE [dbo].[Message]
DROP COLUMN HomeID;

GO

ALTER TABLE [dbo].[Notification]
DROP CONSTRAINT FK_HomeNotification;

ALTER TABLE [dbo].[Notification]
DROP COLUMN HomeID;

GO

ALTER TABLE [dbo].[Survey]
DROP CONSTRAINT FK_HomeSurvey;

ALTER TABLE [dbo].[Survey]
DROP COLUMN HomeID;

GO

CREATE TABLE [dbo].[ObjectLink] (
    [ID] bigint IDENTITY(1, 1) NOT NULL,
	[ObjectID] bigint NOT NULL,
	[ObjectBID] bigint NOT NULL,
	[ObjectLinkType] nvarchar(255) NOT NULL,
	[TargetObjectID] bigint NOT NULL,
	CONSTRAINT PK_ObjectLink PRIMARY KEY ([ID]),
	CONSTRAINT FK_ObjectObjectLink FOREIGN KEY ([ObjectID])
    REFERENCES [dbo].[Object]([ObjectID])
);

ALTER TABLE [dbo].[ObjectLink]
ADD CONSTRAINT FK_TargetObjectObjectLink FOREIGN KEY ([TargetObjectID])
    REFERENCES [dbo].[Object]([ObjectID])

ALTER TABLE [dbo].[ObjectLink]
ADD CONSTRAINT FK_TargetObjectBObjectLink FOREIGN KEY ([ObjectBID])
    REFERENCES [dbo].[Object]([ObjectID])

GO

INSERT INTO [dbo].[ObjectType] ([ID], [Name])
VALUES (19, 'ObjectLink');

GO

CREATE TABLE [dbo].[SurveyParticipant] (
    [ID] bigint IDENTITY(1, 1) NOT NULL,
	[ObjectID] bigint NOT NULL,
	[SurveyID] bigint NOT NULL,
	[UserID] bigint NOT NULL,
	CONSTRAINT PK_SurveyParticipant PRIMARY KEY ([ID]),
	CONSTRAINT FK_ObjectSurveyParticipant FOREIGN KEY ([ObjectID])
    REFERENCES [dbo].[Object]([ObjectID])
);

ALTER TABLE [dbo].[SurveyParticipant]
ADD CONSTRAINT FK_SurveySurveyParticipant FOREIGN KEY ([SurveyID])
    REFERENCES [dbo].[Survey]([ID])

ALTER TABLE [dbo].[SurveyParticipant]
ADD CONSTRAINT FK_TargetUserSurveyParticipant FOREIGN KEY ([UserID])
    REFERENCES [dbo].[User]([ID])

GO

INSERT INTO [dbo].[ObjectType] ([ID], [Name])
VALUES (20, 'SurveyParticipant');

GO

CREATE TABLE [dbo].[SurveyType] (
    [ID] bigint IDENTITY(1, 1) NOT NULL,
	[ObjectID] bigint NOT NULL,
	CONSTRAINT PK_SurveyType PRIMARY KEY ([ID]),
	CONSTRAINT FK_ObjectSurveyType FOREIGN KEY ([ObjectID])
    REFERENCES [dbo].[Object]([ObjectID])
);

GO

ALTER TABLE [dbo].[Survey]
ADD [SurveyTypeID] bigint NOT NULL DEFAULT(0);

ALTER TABLE [dbo].[Survey]
ADD CONSTRAINT FK_SurveyTypeSurvey
FOREIGN KEY (SurveyTypeID) REFERENCES [dbo].[SurveyType](ID);

GO

INSERT INTO [dbo].[ObjectType] ([ID], [Name])
VALUES (21, 'SurveyType');

GO

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

GO

ALTER TABLE [dbo].[Object]
ADD MainID BIGINT NOT NULL DEFAULT(0);

GO

UPDATE obj
SET MainID = mainObj.[ID]
FROM [dbo].[Object] AS obj
INNER JOIN [dbo].[Chore] AS mainObj
ON obj.[ObjectID] = mainObj.[ObjectID]

UPDATE obj
SET MainID = mainObj.[ID]
FROM [dbo].[Object] AS obj
INNER JOIN [dbo].[Comment] AS mainObj
ON obj.[ObjectID] = mainObj.[ObjectID]

UPDATE obj
SET MainID = mainObj.[ID]
FROM [dbo].[Object] AS obj
INNER JOIN [dbo].[Event] AS mainObj
ON obj.[ObjectID] = mainObj.[ObjectID]

UPDATE obj
SET MainID = mainObj.[ID]
FROM [dbo].[Object] AS obj
INNER JOIN [dbo].[EventParticipant] AS mainObj
ON obj.[ObjectID] = mainObj.[ObjectID]

UPDATE obj
SET MainID = mainObj.[ID]
FROM [dbo].[Object] AS obj
INNER JOIN [dbo].[Home] AS mainObj
ON obj.[ObjectID] = mainObj.[ObjectID]

UPDATE obj
SET MainID = mainObj.[ID]
FROM [dbo].[Object] AS obj
INNER JOIN [dbo].[HomeSetting] AS mainObj
ON obj.[ObjectID] = mainObj.[ObjectID]

UPDATE obj
SET MainID = mainObj.[ID]
FROM [dbo].[Object] AS obj
INNER JOIN [dbo].[HomeStatus] AS mainObj
ON obj.[ObjectID] = mainObj.[ObjectID]

UPDATE obj
SET MainID = mainObj.[ID]
FROM [dbo].[Object] AS obj
INNER JOIN [dbo].[HomeUser] AS mainObj
ON obj.[ObjectID] = mainObj.[ObjectID]

UPDATE obj
SET MainID = mainObj.[ID]
FROM [dbo].[Object] AS obj
INNER JOIN [dbo].[Image] AS mainObj
ON obj.[ObjectID] = mainObj.[ObjectID]

UPDATE obj
SET MainID = mainObj.[ID]
FROM [dbo].[Object] AS obj
INNER JOIN [dbo].[Inventory] AS mainObj
ON obj.[ObjectID] = mainObj.[ObjectID]

UPDATE obj
SET MainID = mainObj.[ID]
FROM [dbo].[Object] AS obj
INNER JOIN [dbo].[Issue] AS mainObj
ON obj.[ObjectID] = mainObj.[ObjectID]

UPDATE obj
SET MainID = mainObj.[ID]
FROM [dbo].[Object] AS obj
INNER JOIN [dbo].[Message] AS mainObj
ON obj.[ObjectID] = mainObj.[ObjectID]

UPDATE obj
SET MainID = mainObj.[ID]
FROM [dbo].[Object] AS obj
INNER JOIN [dbo].[Notification] AS mainObj
ON obj.[ObjectID] = mainObj.[ObjectID]

UPDATE obj
SET MainID = mainObj.[ID]
FROM [dbo].[Object] AS obj
INNER JOIN [dbo].[NotificationParticipant] AS mainObj
ON obj.[ObjectID] = mainObj.[ObjectID]

UPDATE obj
SET MainID = mainObj.[ID]
FROM [dbo].[Object] AS obj
INNER JOIN [dbo].[ObjectLink] AS mainObj
ON obj.[ObjectID] = mainObj.[ObjectID]

UPDATE obj
SET MainID = mainObj.[ID]
FROM [dbo].[Object] AS obj
INNER JOIN [dbo].[ObjectParticipant] AS mainObj
ON obj.[ObjectID] = mainObj.[ObjectID]

UPDATE obj
SET MainID = mainObj.[ID]
FROM [dbo].[Object] AS obj
INNER JOIN [dbo].[Survey] AS mainObj
ON obj.[ObjectID] = mainObj.[ObjectID]

UPDATE obj
SET MainID = mainObj.[ID]
FROM [dbo].[Object] AS obj
INNER JOIN [dbo].[SurveyParticipant] AS mainObj
ON obj.[ObjectID] = mainObj.[ObjectID]

UPDATE obj
SET MainID = mainObj.[ID]
FROM [dbo].[Object] AS obj
INNER JOIN [dbo].[SurveyType] AS mainObj
ON obj.[ObjectID] = mainObj.[ObjectID]

UPDATE obj
SET MainID = mainObj.[ID]
FROM [dbo].[Object] AS obj
INNER JOIN [dbo].[User] AS mainObj
ON obj.[ObjectID] = mainObj.[ObjectID]

GO

DROP TABLE [dbo].[HomeSetting]

DROP TABLE [dbo].[HomeStatus]

DELETE FROM [dbo].[ObjectType] WHERE [ID] = 3;

DELETE FROM [dbo].[ObjectType] WHERE [ID] = 4;

GO

CREATE TABLE [dbo].[SurveyQuestion] (
    [ID] bigint IDENTITY(1, 1) NOT NULL,
	[SurveyID] bigint NOT NULL Default(0),
	[ObjectID] bigint NOT NULL,
	CONSTRAINT PK_SurveyQuestion PRIMARY KEY ([ID]),
	CONSTRAINT FK_ObjectSurveyQuestion FOREIGN KEY ([ObjectID])
    REFERENCES [dbo].[Object]([ObjectID])
);

ALTER TABLE [dbo].[SurveyQuestion]
ADD CONSTRAINT FK_SurveySurveyQuestion FOREIGN KEY ([SurveyID])
    REFERENCES [dbo].[Survey]([ID])

GO

INSERT INTO [dbo].[ObjectType] ([ID], [Name])
VALUES (22, 'SurveyQuestion');