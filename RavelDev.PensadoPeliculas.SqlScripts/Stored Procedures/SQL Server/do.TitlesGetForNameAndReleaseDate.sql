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
			AND p.name = 'TitlesGetForNameAndReleaseDate')
DROP PROCEDURE [dbo].[TitlesGetForNameAndReleaseDate] 
GO


CREATE PROCEDURE [dbo].[TitlesGetForNameAndReleaseDate]
	@titleName NVARCHAR(MAX),
	@releaseDate DATE
AS
BEGIN

	SET NOCOUNT ON;
	SELECT
	*
	FROM Titles t
	WHERE
		t.TitleName = @titleName
		AND ReleaseDate = @releaseDate
	ORDER BY
	TitleName
END
GO
