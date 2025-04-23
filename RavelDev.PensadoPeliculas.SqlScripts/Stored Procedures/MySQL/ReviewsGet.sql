USE `peliculas`;
DROP procedure IF EXISTS `ReviewsGet`;

DELIMITER $$
USE `peliculas`$$
CREATE PROCEDURE `ReviewsGet` (
   resultCount INT,
   resultOffset INT,
   orderByColumn VARCHAR(200)
)
BEGIN
	DECLARE limitValue INT;
	SET limitValue = COALESCE(resultCount, 2147483647);
    SELECT *, t.TitleId AS ReviewTitleId 
    FROM
		reviews r
    INNER JOIN titles t ON t.TitleId = r.TitleId
    ORDER BY 
		CASE WHEN orderByColumn = '' THEN ReviewId
        WHEN ReviewDate THEN ReviewDate 
        END 
        LIMIT limitValue;
END$$

DELIMITER ;