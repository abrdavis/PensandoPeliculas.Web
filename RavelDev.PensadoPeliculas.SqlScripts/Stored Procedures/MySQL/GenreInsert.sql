USE `peliculas`;
DROP procedure IF EXISTS `GenreInsert`;

DELIMITER $$
USE `peliculas`$$
CREATE PROCEDURE `GenreInsert` (
   genreName VARCHAR(200)
)
BEGIN

		INSERT INTO Genres(GenreName, IsVisible) VALUES (genreName, 1);
        SELECT LAST_INSERT_ID() INTO @genreId;
        SELECT @genreId;
END$$

DELIMITER ;