
-- =============================================
-- Author:		<Azim Ousamnd>
-- Create date: <October 21, 2014>
-- Description:	<This pulls all tbl_stroke values>
-- =============================================

IF OBJECT_ID ( 'getStrokes', 'P' ) IS NOT NULL 
    DROP PROCEDURE getStrokes;
GO
CREATE PROCEDURE getStrokes
AS 
    SET NOCOUNT ON;
    SELECT t.ID, t.Name, t.Description
    FROM tbl_stroke t;
GO
