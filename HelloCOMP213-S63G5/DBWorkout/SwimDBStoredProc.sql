
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

-- =============================================
-- Author:		<Azim Ousamnd>
-- Create date: <October 21, 2014>
-- Description:	<This pulls all tbl_stroke values>
-- =============================================

IF OBJECT_ID ( 'getStroke', 'P' ) IS NOT NULL 
    DROP PROCEDURE getStroke;
GO
CREATE PROCEDURE getStroke @id int 
AS 
    SET NOCOUNT ON;
    SELECT t.ID, t.Name, t.Description
    FROM tbl_stroke t
	WHERE t.ID = @id;
GO
