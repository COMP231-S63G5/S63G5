
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
CREATE PROCEDURE insertWorkoutPlan @date varchar 
AS 
    SET NOCOUNT ON;
    INSERT INTO [dbo].[tbl_workoutplan]
           ([planDate])
	OUTPUT INSERTED.ID
    VALUES
           (Convert(datetime, @date, 102 ) );
	SELECT SCOPE_IDENTITY();
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
			   ,[restPeriod])
	OUTPUT INSERTED.ID
    VALUES
           (@strokeID
           ,@repeats
           ,@distance
           ,@description
           ,@paceTime
           ,@restPeriod);
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