USE [APPartment]

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