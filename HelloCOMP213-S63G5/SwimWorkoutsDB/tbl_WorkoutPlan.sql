CREATE TABLE [dbo].[tbl_WorkoutPlan]
(
	[planID] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[planDate] date not null, 
    [totalDistance] INT NULL, 
    [totalDuration] NVARCHAR(10) NULL
)
