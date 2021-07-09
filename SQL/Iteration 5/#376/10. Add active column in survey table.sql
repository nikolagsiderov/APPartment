USE APPartment;

ALTER TABLE [dbo].[Survey]
ADD Active BIT NOT NULL DEFAULT(0);