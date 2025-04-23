USE `peliculas`;
DROP procedure IF EXISTS `ReviewsGetRecent`;

DELIMITER $$
USE `peliculas`$$
CREATE PROCEDURE `ReviewsGetRecent` ()
BEGIN
		SELECT
	*,
    u.SiteDisplayName as ReviewAuthor
	FROM Reviews rev
	INNER JOIN Titles t ON t.TitleId  = rev.TitleId
    INNER JOIN Users u ON u.UserId = rev.ReviewAuthor
	WHERE
		rev.IsVisible = 1
	ORDER BY
		rev.ReviewDate DESC
	LIMIT 10;
END$$

DELIMITER ;