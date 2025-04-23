USE `peliculas`;
DROP procedure IF EXISTS `TitleInsert`;

DELIMITER $$
USE `peliculas`$$
CREATE PROCEDURE `TitleInsert` (
   titleName VARCHAR(45),
   releaseDate DATETIME,
   posterImageUrl VARCHAR(500),
   genreId INT,
   lengthMinutes INT
)
BEGIN

		INSERT INTO Titles(TitleName, ReleaseDate, PosterUrl, LengthMinutes) VALUES (titleName, releaseDate, posterImageUrl, lengthMinutes);
        SELECT LAST_INSERT_ID() INTO @titleId;
        INSERT INTO Genres(GenreId, TItleId) VALUES (genreId, @titleId);
        SELECT * FROM Titles WHERE TitleId = @titleId;
END$$

DELIMITER ;