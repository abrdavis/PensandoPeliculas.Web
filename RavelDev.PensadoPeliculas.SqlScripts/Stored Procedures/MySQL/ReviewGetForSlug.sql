USE `peliculas`;
DROP procedure IF EXISTS `ReviewGetForSlug`;

DELIMITER $$
USE `peliculas`$$
CREATE PROCEDURE `ReviewGetForSlug` (
   reviewSlug VARCHAR(100)
)
BEGIN
    SELECT *, t.TitleId AS ReviewTitleId FROM
    reviews r
    INNER JOIN titles t ON t.TitleId = r.TitleId
    WHERE
    r.slug = reviewSlug LIMIT 1;
END$$

DELIMITER ;