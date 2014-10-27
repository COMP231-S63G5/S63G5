IF OBJECT_ID ( 'getWorkOutPlanIDs', 'P' ) IS NOT NULL 
    DROP PROCEDURE getWorkOutPlanIDs;
GO
CREATE PROCEDURE getWorkOutPlanIDs
AS 
    SET NOCOUNT ON;
    SELECT WPids.ID
    FROM tbl_workout_plan WPids;
GO