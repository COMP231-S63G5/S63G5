CREATE TABLE [dbo].[tbl_Set]
(
	[setID] INT IDENTITY(1000,1) NOT NULL PRIMARY KEY,
	[setType] NVARCHAR(50) not null,
	[planID] int not null CONSTRAINT "foreignkeyPlanid" foreign key references tbl_workoutplan([planID]) on delete cascade,
	[repeats] int null,
	[stroke] varchar(10) null,
	[pace] varchar(10),
	[rest] varchar(10),
	[duration] VARCHAR(10) not null ,
	[distance] int NOT NULL DEFAULT 0,
	[description] NVARCHAR(1000) NOT NULL ,
	[energyName] VARCHAR(5) NOT NULL ,
	[totalDistance] int NOT NULL DEFAULT 0,
	[orderID] int NOT NULL ,
	[parentID] int NOT NULL CONSTRAINT "foreignkeySetid" foreign key references tbl_Set([setID]) on delete cascade,
)
