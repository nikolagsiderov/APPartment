USE APPartment;

ALTER TABLE [dbo].[Survey]
DROP COLUMN IsCompleted;

GO

ALTER TABLE [dbo].[Survey]
ADD [ExpireDate] NVARCHAR(255);