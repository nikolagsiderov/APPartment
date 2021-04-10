USE [APPartment]

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