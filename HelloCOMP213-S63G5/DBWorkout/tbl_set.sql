CREATE TABLE [dbo].[tbl_set]
(
	ID varchar(10) NOT NULL PRIMARY KEY,
	workoutID varchar(10) NOT NULL FOREIGN KEY REFERENCES tbl_workout(ID),
	repeats int not null,
	pace varchar(25) not null,
	distance int not null,
	setDuration varchar(10),
	breakTime varchar(10)
);