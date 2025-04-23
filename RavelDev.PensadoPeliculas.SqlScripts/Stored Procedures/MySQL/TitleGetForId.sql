USE `peliculas`;
DROP procedure IF EXISTS `TitleGetForId`;

DELIMITER $$
USE `peliculas`$$
CREATE PROCEDURE `TitleGetForId` (
   titleId INT
)
BEGIN
		SELECT
	*
	FROM Titles t 
	WHERE
		t.TitleId = titleId
	LIMIT 1;
END$$

DELIMITER ;