USE `peliculas`;
DROP procedure IF EXISTS `ReviewGetForSlug`;

DELIMITER $$
USE `peliculas`$$
CREATE PROCEDURE `ReviewGetForSlug` (
   reviewSlug VARCHAR(100)
)
BEGIN
    SELECT r.*, 
    t.TitleId AS ReviewTitleId ,
     t.TitleName as TitleName,
    COALESCE(genre.GenreName, '') as GenreName
    FROM
    reviews r
    LEFT JOIN LATERAL (SELECT GenreName FROM titlegenres tg
    INNER JOIN genres g ON g.GenreId = tg.GenreId
WHERE tg.TitleId = r.TitleId LIMIT 1) genre on TRUE
    INNER JOIN titles t ON t.TitleId = r.TitleId
    WHERE
    r.slug = reviewSlug LIMIT 1;

END$$

DELIMITER ;