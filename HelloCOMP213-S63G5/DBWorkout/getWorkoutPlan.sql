IF OBJECT_ID ( 'getworkoutplan', 'P' ) IS NOT NULL 
    DROP PROCEDURE getworkoutplan;
GO
CREATE PROCEDURE getworkoutplan 
    @workoutplanID int
AS 
    SET NOCOUNT ON;
    SELECT tbl_workout_plan.ID AS WorkOutPlan_ID,tbl_set.ID AS Set_Id,
			tbl_stroke.ID AS Stroke_Id,tbl_stroke.Name,tbl_stroke.Description,
			tbl_workoutplan_member.ID as Member_Id,tbl_workoutplan_member.memberOrder,
			tbl_workout_plan.planDate,
			tbl_set.repeats,
			tbl_set.distance,tbl_set.effortLevel,
			tbl_set.paceTime,tbl_set.restPeriod
    FROM tbl_set
			inner join tbl_workoutplan_member ON tbl_workoutplan_member.childID=tbl_set.ID
			inner join tbl_workout_plan ON tbl_workout_plan.ID=tbl_workoutplan_member.parentID
			inner join tbl_stroke ON tbl_stroke.ID=tbl_set.strokeID
	WHERE tbl_workout_plan.ID=@workoutplanID
	ORDER BY memberOrder ASC;
GO