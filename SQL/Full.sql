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