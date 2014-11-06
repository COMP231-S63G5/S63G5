
-- =============================================
-- Author:		<Azim Ousamnd>
-- Create date: <October 21, 2014>
-- Description:	<This pulls all tbl_stroke values>
-- =============================================

--IF OBJECT_ID ( 'getStrokes', 'P' ) IS NOT NULL 
  --  DROP PROCEDURE getStrokes;
GO
CREATE PROCEDURE getStrokes
AS 
    SET NOCOUNT ON;
    SELECT t.ID, t.Name, t.Description
    FROM tbl_stroke t;
GO

-- =============================================
-- Author:		<Azim Ousamnd>
-- Create date: <October 26, 2014>
-- Description:	<This pulls a tbl_stroke value for a given ID>
-- =============================================

--IF OBJECT_ID ( 'getStroke', 'P' ) IS NOT NULL 
 --   DROP PROCEDURE getStroke;
GO
CREATE PROCEDURE getStroke @id int 
AS 
    SET NOCOUNT ON;
    SELECT t.ID, t.Name, t.Description
    FROM tbl_stroke t
	WHERE t.ID = @id;
GO


-- =============================================
-- Author:		<Azim Ousamnd>
-- Create date: <October 28, 2014>
-- Description:	<This pulls a tbl_stroke value for a given ID>
-- =============================================

--IF OBJECT_ID ( 'insertWorkoutPlan', 'P' ) IS NOT NULL 
--    DROP PROCEDURE insertWorkoutPlan;
GO
CREATE PROCEDURE insertWorkoutPlan @date date 
AS 
    SET NOCOUNT ON;
    INSERT INTO [dbo].[tbl_workoutplan]
           ([planDate])
	OUTPUT INSERTED.ID
    VALUES
           (@date );
GO


-- =============================================
-- Author:		<Azim Ousamnd>
-- Create date: <October 28, 2014>
-- Description:	<This inserts a set>
-- =============================================

--IF OBJECT_ID ( 'insertWorkoutSet', 'P' ) IS NOT NULL 
 --   DROP PROCEDURE insertWorkoutSet;
GO
CREATE PROCEDURE insertWorkoutSet	@strokeID int, 
									@repeats int,
									@distance int,
									@description varchar,
									@E1 int,
									@E2 int,
									@E3 int,
									@S1 int,
									@S2 int,
									@S3 int,
									@REC int,
									@paceTime varchar = null,
									@restPeriod varchar = null 
AS 
    SET NOCOUNT ON;
	INSERT INTO [dbo].[tbl_set]
			   ([strokeID]
			   ,[repeats]
			   ,[distance]
			   ,[description]
			   ,[paceTime]
			   ,[restPeriod]
			   ,[E1]
			   ,[E2]
			   ,[E3]
			   ,[S1]
			   ,[S2]
			   ,[S3]
			   ,[REC])
	OUTPUT INSERTED.ID
    VALUES
           (@strokeID
           ,@repeats
           ,@distance
           ,@description
           ,@paceTime
           ,@restPeriod
		   ,@E1
		   ,@E2
		   ,@E3
		   ,@S1
		   ,@S2
		   ,@S3
		   ,@REC);
GO


-- =============================================
-- Author:		<Azim Ousamnd>
-- Create date: <October 28, 2014>
-- Description:	<This inserts a record into tbl_workoutplan_member>
-- =============================================

--IF OBJECT_ID ( 'insertWorkoutMember', 'P' ) IS NOT NULL 
--    DROP PROCEDURE insertWorkoutMember;
GO
CREATE PROCEDURE insertWorkoutMember	@parentID int, 
										@childID int,
										@memberOrder int
AS 
    SET NOCOUNT ON;
	INSERT INTO [dbo].[tbl_workoutplan_member]
           ([parentID]
           ,[childID]
           ,[memberOrder])
     VALUES
           (@parentID
           ,@childID
           ,@memberOrder)
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
    SELECT  tbl_workoutplan.ID AS WorkOutPlan_ID,
			tbl_set.ID AS Set_Id,
			tbl_stroke.ID AS Stroke_Id,tbl_stroke.Name,tbl_stroke.Description AS Stroke_Desc,
			tbl_workoutplan_member.ID as Member_Id,tbl_workoutplan_member.memberOrder,
			tbl_workoutplan.planDate,
			tbl_set.repeats,
			tbl_set.distance,tbl_set.description AS Set_Desc,
			tbl_set.paceTime,tbl_set.restPeriod,
			tbl_set.E1,tbl_set.E2,tbl_set.E3,tbl_set.S1,tbl_set.S2,tbl_set.S3,tbl_set.REC
    FROM tbl_set
			inner join tbl_workoutplan_member ON tbl_workoutplan_member.childID=tbl_set.ID
			inner join tbl_workoutplan ON tbl_workoutplan.ID=tbl_workoutplan_member.parentID
			inner join tbl_stroke ON tbl_stroke.ID=tbl_set.strokeID
	WHERE tbl_workoutplan.ID=@workoutplanID
	ORDER BY memberOrder ASC;
GO

-- ======================================================================
-- Author: <Hiren Patel>
-- Description: <This stored proc gets Ids of all existing workout plans>
-- ======================================================================
GO
CREATE PROCEDURE getWorkOutPlanIDs
AS 
    SET NOCOUNT ON;
    SELECT WPids.ID
    FROM tbl_workoutplan WPids;
GO