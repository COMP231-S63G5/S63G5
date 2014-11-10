CREATE TABLE [dbo].[tbl_set]
(
	ID INT IDENTITY(1000,1) NOT NULL PRIMARY KEY,
	strokeID int NOT NULL FOREIGN KEY REFERENCES tbl_stroke(ID),
	repeats int not null,
	distance int null,
	description varchar(100) null,
	paceTime varchar(10),
	restPeriod varchar(10),
	E1 int not null default 0,
	E2 int NOT NULL DEFAULT 0,
	E3 int NOT NULL DEFAULT 0,
	S1 int NOT NULL DEFAULT 0,
	S2 int NOT NULL DEFAULT 0,
	S3 int NOT NULL DEFAULT 0,
	REC int NOT NULL DEFAULT 0, 
    [duration] INT NOT NULL DEFAULT 0
);