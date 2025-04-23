USE `peliculas`;
DROP procedure IF EXISTS `ReviewGetForId`;

DELIMITER $$
USE `peliculas`$$
CREATE PROCEDURE `ReviewGetForId` (
   reviewId INT
)
BEGIN
    SELECT *, t.TitleId AS ReviewTitleId FROM
    reviews r
    INNER JOIN titles t ON t.TitleId = r.TitleId
    WHERE
    r.ReviewId = reviewId LIMIT 1;
END$$

DELIMITER ;