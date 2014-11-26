CREATE TABLE [dbo].[tbl_workoutplan]
(
	[planID] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	planDate date not null, 
    [totalDistance] INT NULL, 
    [totalDuration] NCHAR(10) NULL
);

