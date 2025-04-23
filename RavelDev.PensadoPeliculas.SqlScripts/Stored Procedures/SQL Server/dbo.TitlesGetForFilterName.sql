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
			AND p.name = 'TitlesGetForFilterName')
DROP PROCEDURE [dbo].[TitlesGetForFilterName] 
GO


CREATE PROCEDURE [dbo].[TitlesGetForFilterName]
	@filterText NVARCHAR(MAX)
AS
BEGIN

	SET NOCOUNT ON;
	SELECT
	*
	FROM Titles t
	WHERE
		t.TitleName LIKE '%'  + @filterText + '%'
	ORDER BY
	TitleName
END
GO
