USE `peliculas`;
DROP procedure IF EXISTS `ReviewUpdate`;

DELIMITER $$
USE `peliculas`$$
CREATE PROCEDURE `ReviewUpdate` (
    reviewId INT,
	titleId INT,
	dateUpdated TIMESTAMP,
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
		UPDATE Reviews
        SET 
        DateUpdated = dateUpdated,
        TitleId = titleId,
        ReviewTitle = reviewTitle,
        ReviewText = reviewText,
        IsVisible = isVisible,
        ReviewAuthor = reviewAuthor,
        ReviewRating = reviewRating,
        Slug = reviewSlug,
        HeaderImageUrl = headerImageUrl
        WHERE
			ReviewId = reviewId;

        SELECT * FROM reviews WHERE ReviewId = reviewId LIMIT 1;
END$$

DELIMITER ;