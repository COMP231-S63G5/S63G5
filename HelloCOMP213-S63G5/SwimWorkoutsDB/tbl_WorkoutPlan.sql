CREATE TABLE [dbo].[tbl_WorkoutPlan]
(
	[planID] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[planDate] date not null, 
    [totalDistance] INT NULL DEFAULT 0, 
    [totalDuration] NVARCHAR(10) NULL default '0', 
    [planName] NVARCHAR(50) NULL DEFAULT ''
)
