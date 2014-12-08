CREATE TABLE [dbo].[tbl_Set]
(
	[setID] INT IDENTITY(1000,1) NOT NULL PRIMARY KEY,
	[setType] NVARCHAR(10) not null DEFAULT '',
	[planID] int not null CONSTRAINT "foreignkeyPlanid" FOREIGN KEY REFERENCES tbl_WorkoutPlan([planID]) ON DELETE CASCADE,
	[repeats] int NOT null DEFAULT 0,
	[stroke] varchar(10) NOT null DEFAULT '',
	[pace] varchar(10) NOT NULL DEFAULT '',
	[rest] varchar(10) NOT NULL DEFAULT '',
	[duration] VARCHAR(10) not null DEFAULT '' ,
	[distance] int NOT NULL DEFAULT 0,
	[description] NVARCHAR(1000) NOT NULL DEFAULT ''   ,
	[energyName] VARCHAR(5) NOT NULL DEFAULT '' ,
	[totalDistance] int NOT NULL DEFAULT 0,
	[orderID] int NOT NULL DEFAULT 0 ,
	[parentID] int NOT NULL DEFAULT 0
)
