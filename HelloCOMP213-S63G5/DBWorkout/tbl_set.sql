CREATE TABLE [dbo].[tbl_set]
(
	ID varchar(10) NOT NULL PRIMARY KEY,
	strokeID varchar(10) NOT NULL FOREIGN KEY REFERENCES tbl_workout(ID),
	repeats int not null,
	distance int not null,
	effortLevel varchar(25) not null,
	paceTime varchar(10),
	restPeriod varchar(10)
);