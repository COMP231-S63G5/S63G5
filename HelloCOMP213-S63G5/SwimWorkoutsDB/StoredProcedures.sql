-- =============================================
-- Author:		<Hiren Patel>
-- Create date: <>
-- Description:	<This pulls all stroke values>
-- =============================================

GO
CREATE PROCEDURE getStrokes
AS 
    SET NOCOUNT ON;
    SELECT t.stroke
    FROM tbl_Set t
	Group By t.stroke;
GO


-- =============================================
-- Author:		<Hiren Patel>
-- Create date: <>
-- Description:	<This insert workoutPlan record>
-- =============================================

--IF OBJECT_ID ( 'insertWorkoutPlan', 'P' ) IS NOT NULL 
--    DROP PROCEDURE insertWorkoutPlan;
GO
CREATE PROCEDURE addWorkOutPlan    @date date,
								   @ttl_distance int, 
								   @ttl_duration nvarchar(10)
AS 
    SET NOCOUNT ON;
    INSERT INTO [dbo].[tbl_WorkoutPlan]
           ([planDate],[totalDistance],[totalDuration])
	OUTPUT INSERTED.planID
    VALUES
           (@date,@ttl_distance,@ttl_duration );
GO




-- =============================================
-- Author:		<Hiren Patel>
-- Create date: <>
-- Description:	<This inserts a set>
-- =============================================

--IF OBJECT_ID ( 'insertWorkoutSet', 'P' ) IS NOT NULL 
 --   DROP PROCEDURE insertWorkoutSet;
GO
CREATE PROCEDURE insertWorkoutSet	@SetType nvarchar(10), 
									@planID int,
									@repeats int,
									@stroke varchar(10),
									@pace varchar(10),
									@rest varchar(10),
									@duration varchar(10),
									@distance int,
									@description varchar(1000),
									@energyName varchar(5),
									@tlt_distance int,
									@orderID int,
									@parentID int
AS 
    SET NOCOUNT ON;
	INSERT INTO [dbo].[tbl_Set]
				([setType],
				 [planID],
				 [repeats],
				 [stroke],
				 [pace],
				 [rest],
				 [duration],
				 [distance],
				 [description],
				 [energyName],
				 [totalDistance],
				 [orderID],
				 [parentID])
	OUTPUT INSERTED.setID
    VALUES
           (@SetType
           ,@planID
           ,@repeats
           ,@stroke
           ,@pace
           ,@rest
		   ,@duration
		   ,@distance
		   ,@description
		   ,@energyName
		   ,@tlt_distance
		   ,@orderID
		   ,@parentID
		   );
GO



-- ======================================================================
-- Author: <Hiren Patel>
-- Description: <This stored proc gets Ids of all existing workout plans>
-- ======================================================================
GO
CREATE PROCEDURE getWorkOutPlanIDs
AS 
    SET NOCOUNT ON;
    SELECT WPids.planID
    FROM tbl_WorkoutPlan WPids;
GO



-- ==================================================================
-- Author: <Hiren Patel>
-- Description: <This  stored procedure gets workOut plan info from all tables>
-- ==================================================================
GO
CREATE PROCEDURE getworkoutplan 
    @workoutplanID int
AS 
    SET NOCOUNT ON;
    SELECT  tbl_WorkoutPlan.planID,
			tbl_WorkoutPlan.totalDistance,
			tbl_WorkoutPlan.totalDuration,
			tbl_Set.setID,
			tbl_Set.setType,
			tbl_Set.repeats,
			tbl_Set.stroke,
			tbl_Set.pace,
			tbl_Set.rest,
			tbl_Set.duration,
			tbl_Set.distance,
			tbl_Set.description,
			REPLACE(CONVERT(VARCHAR(25),tbl_WorkoutPlan.planDate,111), '/','-') as planDate, -- planDate needed to be pulled in a specific format for the input-date in view
			tbl_Set.energyName,
			tbl_Set.totalDistance,
			tbl_Set.orderID,
			tbl_Set.parentID
    FROM tbl_set
			inner join tbl_WorkoutPlan ON tbl_WorkoutPlan.planID=tbl_set.planID
	WHERE tbl_WorkoutPlan.planID=@workoutplanID
	ORDER BY orderID ASC;
GO






-- ==================================================================
-- Author: <Hiren Patel>
-- Description: <This stored proc deletes all sets and plan from DB>
-- ==================================================================
GO
CREATE PROCEDURE deleteworkoutplan 
    @workoutplanID int
AS 
    SET NOCOUNT ON;
    DELETE FROM tbl_set 
	WHERE tbl_Set.planID=@workoutplanID;
	DELETE FROM tbl_workoutplan
	WHERE tbl_WorkoutPlan.planID=@workoutplanID;

GO