USE `peliculas`;
DROP procedure IF EXISTS `TitlesGetForFilterName`;

DELIMITER $$
USE `peliculas`$$
CREATE PROCEDURE `TitlesGetForFilterName` (
   filterText LONGTEXT
)
BEGIN
	SELECT
	*
	FROM Titles t
	WHERE
		t.TitleName LIKE CONCAT('%',  filterText, '%')
	ORDER BY
	TitleName;
END$$

DELIMITER ;