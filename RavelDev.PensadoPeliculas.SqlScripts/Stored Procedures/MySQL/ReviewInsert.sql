USE `peliculas`;
DROP procedure IF EXISTS `ReviewInsert`;

DELIMITER $$
USE `peliculas`$$
CREATE PROCEDURE `ReviewInsert` (
	titleId INT,
	reviewDate TIMESTAMP,
	reviewTitle LONGTEXT,
	reviewText LONGTEXT,
   isVisible TINYINT,
   reviewAuthor VARCHAR(36),
   reviewRating DECIMAL(3,1),
   reviewSlug VARCHAR(100),
   headerImageUrl VARCHAR(2083)
)
BEGIN
		SET headerImageUrl = coalesce(headerImageUrl, '');
		INSERT INTO Reviews(
        ReviewDate, 
        DateUpdated,
        TitleId,
        ReviewTitle,
        ReviewText,
        IsVisible,
        ReviewAuthor,
        ReviewRating,
        Slug,
        HeaderImageUrl
        ) 
        VALUES (
        reviewDate, 
        UTC_TIMESTAMP(),
        titleId,
        reviewTitle,
        reviewText,
        isVisible,
        reviewAuthor,
        reviewRating,
        reviewSlug,
        headerImageurl
        );
        SELECT LAST_INSERT_ID() INTO @reviewId;
        SELECT * FROM reviews WHERE ReviewId = @reviewId LIMIT 1;
END$$

DELIMITER ;