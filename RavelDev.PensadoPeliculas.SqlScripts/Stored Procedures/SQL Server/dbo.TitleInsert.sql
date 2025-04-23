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
			AND p.name = 'TitlesInsert')
DROP PROCEDURE [dbo].[TitlesInsert] 
GO


CREATE PROCEDURE [dbo].[TitlesInsert]
	@posterImageUrl NVARCHAR(MAX),
	@titleName NVARCHAR(MAX),
	@releaseDate DATE
AS
BEGIN

	SET NOCOUNT ON;
	INSERT INTO dbo.Titles (TitleName, ReleaseDate, PosterThumbnailUrl, PosterUrl)
	VALUES
	(@titleName, @releaseDate, '', @posterImageUrl);

	SELECT SCOPE_IDENTITY()
END
GO
