USE `peliculas`;
DROP procedure IF EXISTS `GenreGetAll`;

DELIMITER $$
USE `peliculas`$$
CREATE PROCEDURE `GenreGetAll` (
)
BEGIN
		SELECT
	*
	FROM Genres g 
	WHERE
		g.IsVisible = 1;
END$$

DELIMITER ;