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
			AND p.name = 'ReviewsInsertReview')
DROP PROCEDURE [dbo].[ReviewsInsertReview] 
GO


CREATE PROCEDURE [dbo].[ReviewsInsertReview]
	@reviewTitleId INT,
	@reviewText NVARCHAR(MAX),
	@reviewTitle NVARCHAR(MAX),
	@reviewScore DECIMAL(18,2)
AS
BEGIN

	SET NOCOUNT ON;
	INSERT INTO dbo.Reviews (ReviewDate, DateUpdated, TitleId, ReviewTitle, ReviewText, ReviewScore, IsVisible, ReviewAuthor)
	VALUES
	(SYSUTCDATETIME(), SYSUTCDATETIME(), @reviewTitleId, @reviewTitle, @reviewText, @reviewScore, 1, 'hawk');

	SELECT SCOPE_IDENTITY()
END
GO
