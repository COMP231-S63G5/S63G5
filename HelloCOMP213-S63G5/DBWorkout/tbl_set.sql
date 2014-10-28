CREATE TABLE [dbo].[tbl_set]
(
	ID INT IDENTITY(1000,1) NOT NULL PRIMARY KEY,
	strokeID int NOT NULL FOREIGN KEY REFERENCES tbl_stroke(ID),
	repeats int not null,
	distance int not null,
	description varchar(50) not null,
	paceTime varchar(10),
	restPeriod varchar(10)
);