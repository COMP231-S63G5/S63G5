CREATE TABLE [dbo].[tbl_workoutplan_member]
(
	ID INT IDENTITY(1000,1) not null primary key,
	parentID int not null foreign key references tbl_workoutplan(ID),
	childID int not null foreign key references tbl_set,
	memberOrder int not null
);

