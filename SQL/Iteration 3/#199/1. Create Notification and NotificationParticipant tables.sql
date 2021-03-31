USE [APPartment]

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