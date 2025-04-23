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
			AND p.name = 'TitlesGetAll')
DROP PROCEDURE [dbo].[TitlesGetAll] 
GO


CREATE PROCEDURE [dbo].[TitlesGetAll]

AS
BEGIN

	SET NOCOUNT ON;
	SELECT
	*
	FROM Titles t
	ORDER BY
	TitleName
END
GO
