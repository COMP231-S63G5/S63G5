CREATE TABLE [dbo].[tbl_workoutplan_member]
(
	ID varchar(10) not null primary key,
	parentID varchar(10) not null foreign key references tbl_workout_plan(ID),
	childID varchar(10) not null foreign key references tbl_set,
	memberOrder int not null
);

