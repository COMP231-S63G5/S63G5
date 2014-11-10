CREATE TABLE [dbo].[tbl_workoutplan_member]
(
	ID INT IDENTITY(1000,1) not null primary key,
	parentID int not null foreign key references tbl_workoutplan(ID) on delete cascade,
	childID int not null foreign key references tbl_set on delete cascade,
	memberOrder int not null
);

