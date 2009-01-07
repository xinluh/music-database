CREATE TABLE 'tblAlbum' (
    "id" INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
    "name" TEXT UNIQUE NOT NULL,
    "location" TEXT NOT NULL,
    "type" INTEGER NOT NULL,
    "total_disc" INTEGER DEFAULT '1' NOT NULL,
    "total_track" TEXT NOT NULL,
    "complete" INTEGER DEFAULT '0' NOT NULL,
    "label_id" INTEGER,
    "in_ipod" INTEGER NOT NULL,
    "comment" TEXT,
    "albumartist_id" INTEGER NOT NULL,
    "need_update" INTEGER NOT NULL DEFAULT '0',
    "created_date" TEXT NULL,
    "modified_date" TEXT
);
CREATE TABLE "tblArtist" (
    "id" INTEGER  PRIMARY KEY AUTOINCREMENT NOT NULL,
    "name_id" INTEGER  NOT NULL,
    "type_id" INTEGER  NOT NULL,
    "created_date" TEXT  NULL,
    "modified_date" TEXT  NULL,
    "temp" TEXT
);
CREATE TABLE "tblArtistName" (
    "id" INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
    "last_name" TEXT NOT NULL,
    "first_name" TEXT,
    "alternate_last" TEXT,
    "alternate_first" TEXT,
    "created_date" TEXT,
    "modified_date" TEXT
);
CREATE TABLE "tblArtistType" (
    "id" INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
    "name" TEXT NOT NULL,
    "created_date" TEXT,
    "modified_date" TEXT
);
CREATE TABLE "tblGenre" (
    "id" INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
    "name" TEXT NOT NULL
);
CREATE TABLE "tblLabel" (
    "id" INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
    "name" TEXT NOT NULL,
    "created_date" TEXT,
    "modified_date" TEXT
);
CREATE TABLE "tblPiece" (
    "id" INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
    "parent_piece_id" TEXT,
    "name" TEXT NOT NULL,
    "old_name" TEXT,
    "connector" TEXT,
    "composer_id" INTEGER NOT NULL,
    "genre_id" INTEGER NOT NULL,
    "detail" TEXT,
    "extra" TEXT,
    "text" TEXT,
    "created_date" TEXT,
    "modified_date" TEXT,
    "temp" TEXT);
CREATE TABLE "tblTrack" (
    "id" INTEGER  PRIMARY KEY AUTOINCREMENT NOT NULL,
    "album_id" INTEGER  NOT NULL,
    "piece_id" INTEGER  NOT NULL,
    "disc_num" INTEGER  NOT NULL,
    "name" TEXT  NOT NULL,
    "track_num" TEXT  NOT NULL,
    "filename" TEXT  NOT NULL,
    "length" INTEGER  NOT NULL,
    "size" INTEGER  NOT NULL,
    "performer" TEXT  NOT NULL,
    "year" INTEGER,
    "comment" TEXT  NULL,
    "need_update" INTEGER DEFAULT '0' NOT NULL,
    "created_date" TEXT  NULL,
    "modified_date" TEXT  NULL
);
CREATE VIEW "vwArtist" AS

SELECT tblArtist.id AS id, 

	tblArtistName.last_name AS last_name,

	tblArtistName.first_name AS first_name,

        tblArtistType.id AS type_id,

	tblArtistType.name AS type,

	tblArtistName.last_name || 

	(CASE

	WHEN (tblArtistName.first_name IS NOT NULL) THEN

	 ', ' || tblArtistName.first_name

	 ELSE

	 ''

	END) AS fullname,

	tblArtistName.last_name || 

	(CASE

	WHEN (tblArtistName.first_name IS NOT NULL) THEN

	 ', ' || tblArtistName.first_name

	ELSE

	''

	END)

	|| ' (' || tblArtistType.name || ')' AS fullname_type

FROM tblArtist, tblArtistName, tblArtistType

WHERE tblArtist.name_id = tblArtistName.id 

and tblArtist.type_id = tblArtistType.id;
CREATE VIEW "vwGenreComposer" AS SELECT DISTINCT 
	tblPiece.genre_id AS id, 
	tblPiece.composer_id AS composer_id, 
	tblGenre.name AS name 
FROM tblGenre, tblPiece 
WHERE tblPiece.genre_id = tblGenre.id;
CREATE TRIGGER "tr_archive_old_name"
   AFTER UPDATE OF name
   ON main.tblPiece
BEGIN

    UPDATE tblPiece SET old_name =  

    CASE

    WHEN (old_name IS NULL) THEN
    old.name || '|'

    ELSE

    old_name || old.name || '|' 

    END  WHERE id = old.id;
END;
CREATE TRIGGER "tr_created_date_album"
   AFTER   INSERT
   ON main.tblAlbum
BEGIN
    UPDATE tblAlbum 
    SET created_date = strftime('%m/%d/%Y %H:%M','now', 'localtime')
    WHERE id = new.id;
END;
CREATE TRIGGER "tr_created_date_artist"
   AFTER   INSERT
   ON main.tblArtist
BEGIN
    UPDATE tblArtist
    SET created_date = strftime('%m/%d/%Y %H:%M','now', 'localtime')
    WHERE id = new.id;
END;
CREATE TRIGGER "tr_created_date_artistname"
   AFTER   INSERT
   ON main.tblArtistName
BEGIN
    UPDATE tblArtistName

    SET created_date = strftime('%m/%d/%Y %H:%M','now', 'localtime')

    WHERE id = new.id;
END;
CREATE TRIGGER "tr_created_date_artisttype"
   AFTER   INSERT
   ON main.tblArtistType
BEGIN
    UPDATE tblArtistType

    SET created_date = strftime('%m/%d/%Y %H:%M','now', 'localtime')

    WHERE id = new.id;
END;
CREATE TRIGGER "tr_created_date_label"
   AFTER   INSERT
   ON main.'tblLabel'
BEGIN
    UPDATE tblLabel

    SET created_date = strftime('%m/%d/%Y %H:%M','now', 'localtime')

    WHERE id = new.id;
END;
CREATE TRIGGER "tr_created_date_piece"
   AFTER   INSERT
   ON main.'tblPiece'
BEGIN
    UPDATE tblPiece

    SET created_date = strftime('%m/%d/%Y %H:%M','now', 'localtime')

    WHERE id = new.id;
END;
CREATE TRIGGER "tr_created_date_track"
   AFTER   INSERT
   ON main.tblTrack
BEGIN
    UPDATE tblTrack
    SET created_date = strftime('%m/%d/%Y %H:%M','now', 'localtime')
    WHERE id = new.id;
END;
CREATE TRIGGER "tr_set_changed_album"
   AFTER UPDATE OF name,type,totaldisc,totaltrack,complete,
   label_id,in_ipod,albumartist_id ON main.tblAlbum
BEGIN
    UPDATE tblAlbum
    SET need_update = 1
    WHERE id = old.id;
END;
CREATE TRIGGER "tr_set_changed_artist"
   AFTER UPDATE OF name_id, type_id, modified_date ON main.tblArtist
BEGIN
    UPDATE tblPiece 
    SET temp = 1 --will trigger update
    WHERE composer_id = old.id;
    
    UPDATE tblTrack
    SET need_update = 1
    WHERE performer LIKE '%' || old.id || ',%';
END;
CREATE TRIGGER "tr_set_changed_artistname"
   AFTER UPDATE OF last_name, first_name ON tblArtistName
   WHEN (old.last_name != new.last_name OR old.first_name != new.first_name)
BEGIN
    UPDATE tblArtist 
    SET temp = 1 --will trigger update on tblArtist
    WHERE tblArtist.name_id = old.id;
    
    UPDATE tblAlbum
    SET need_update = 1
    WHERE tblAlbum.albumartist_id = old.id;
END;
CREATE TRIGGER "tr_set_changed_artisttype"
   AFTER UPDATE OF name ON main.tblArtistType
   WHEN (old.name != new.name)
BEGIN
    UPDATE tblArtist 
    SET temp = 1 --will trigger update on tblArtist
    WHERE tblArtist.name_id = old.id;
END;
CREATE TRIGGER "tr_set_changed_label"
   AFTER UPDATE OF name ON main.tblLabel
   WHEN (old.name != new.name)
BEGIN
    UPDATE tblAlbum 
    SET need_update = 1 
    WHERE label_id = old.id;
END;
CREATE TRIGGER "tr_set_changed_piece"
   AFTER UPDATE OF name,connector,composer_id,genre_id,text,temp ON main.tblPiece
   WHEN (old.name != new.name OR
         old.connector != new.connector OR
         old.composer_id != new.composer_id OR
         old.genre_id != new.genre_id OR 
         old.text != new.text OR
         new.temp = 1)
BEGIN
    UPDATE tblTrack
    SET need_update = 1
    WHERE piece_id = old.id;
END;
CREATE TRIGGER "tr_set_changed_track"
   AFTER UPDATE OF album_id,piece_id,disc_num,name,
   track_num,performer,year ON main.tblTrack
BEGIN
    UPDATE tblTrack
    SET need_update = 1
    WHERE id = old.id;
END;
CREATE TRIGGER "tr_set_mod_date_album"
   AFTER  UPDATE ON main.tblAlbum
BEGIN
    UPDATE tblAlbum 
    SET modified_date = 

    strftime('%m/%d/%Y %H:%M','now', 'localtime')
    WHERE id = new.id;
END;
CREATE TRIGGER "tr_set_mod_date_artist"
   AFTER  UPDATE ON main.tblArtist
BEGIN
    UPDATE tblArtist
    SET modified_date =
    strftime('%m/%d/%Y %H:%M','now', 'localtime')
    WHERE id = new.id;
END;
CREATE TRIGGER "tr_set_mod_date_artistname"
   AFTER  UPDATE ON main.tblArtistName
BEGIN
    UPDATE tblArtistName
    SET modified_date = 

    strftime('%m/%d/%Y %H:%M','now', 'localtime')
    WHERE id = new.id;
END;
CREATE TRIGGER "tr_set_mod_date_artisttype"
   AFTER  UPDATE ON main.tblArtistType
BEGIN
    UPDATE tblArtistType
    SET modified_date = 

    strftime('%m/%d/%Y %H:%M','now', 'localtime')
    WHERE id = new.id;
END;
CREATE TRIGGER "tr_set_mod_date_label"
   AFTER  UPDATE ON main.tblLabel
BEGIN
    UPDATE tblLabel
    SET modified_date = 

    strftime('%m/%d/%Y %H:%M','now', 'localtime')
    WHERE id = new.id;
END;
CREATE TRIGGER "tr_set_mod_date_piece"
   AFTER  UPDATE ON main.tblpiece
BEGIN
    UPDATE tblpiece
    SET modified_date = 

    strftime('%m/%d/%Y %H:%M','now', 'localtime')
    WHERE id = new.id;
END;
CREATE TRIGGER "tr_set_mod_date_track"
   AFTER  UPDATE ON main.tblTrack
BEGIN
    UPDATE tblTrack
    SET modified_date =
    strftime('%m/%d/%Y %H:%M','now', 'localtime')
    WHERE id = new.id;
END;
