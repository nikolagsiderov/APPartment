USE [APPartment]

CREATE TABLE [dbo].[ObjectType] (
    [ID] bigint NOT NULL,
    [Name] nvarchar(255) NOT NULL,
	CONSTRAINT PK_ObjectType PRIMARY KEY ([ID])
);