/**
**/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


IF EXISTS ( SELECT 1 
            FROM sys.procedures p 
			JOIN sys.schemas s 
			ON p.schema_id = s.schema_id
            WHERE s.name = 'dbo' 
			AND p.name = 'ReviewsGetRecent')
DROP PROCEDURE [dbo].[ReviewsGetRecent] 
GO


CREATE PROCEDURE [dbo].[ReviewsGetRecent]

AS
BEGIN

	SET NOCOUNT ON;
	SELECT TOP 10
	*
	FROM Reviews rev
	INNER JOIN Titles t ON t.TitleId  = rev.TitleId
	WHERE
		rev.IsVisible = 1
	ORDER BY
		rev.ReviewDate DESC
END
GO
