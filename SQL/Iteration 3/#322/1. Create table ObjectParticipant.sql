USE [APPartment]

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