CREATE TABLE 'tblAlbum' (
    "id" INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
    "name" TEXT UNIQUE NOT NULL,
    "location" TEXT NOT NULL,
    "type" INTEGER NOT NULL,
    "total_disc" INTEGER DEFAULT '1' NOT NULL,
    "total_track" TEXT NOT NULL,
    "complete" INTEGER DEFAULT '0' NOT NULL,
    "label_id" INTEGER
			   CONSTRAINT fk_label_id REFERENCES tblLabel(id),
    "in_ipod" INTEGER NOT NULL,
    "comment" TEXT,
    "albumartist_id" INTEGER NOT NULL
			   CONSTRAINT fk_albumartist_id  REFERENCES tblArtist(id),
    "need_update" INTEGER NOT NULL DEFAULT '0',
    "created_date" TEXT NULL,
    "modified_date" TEXT
);
CREATE TABLE "tblArtist" (
    "id" INTEGER  PRIMARY KEY AUTOINCREMENT NOT NULL,
    "name_id" INTEGER  NOT NULL
			   CONSTRAINT fk_name_id  REFERENCES tblArtistName(id),
    "type_id" INTEGER  NOT NULL
			   CONSTRAINT fk_type_id  REFERENCES tblArtistType(id),
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
    "name" TEXT UNIQUE NOT NULL,
    "created_date" TEXT,
    "modified_date" TEXT
);
CREATE TABLE "tblGenre" (
    "id" INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
    "name" TEXT UNIQUE NOT NULL
);
CREATE TABLE "tblLabel" (
    "id" INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
    "name" TEXT UNIQUE NOT NULL,
    "created_date" TEXT,
    "modified_date" TEXT
);
CREATE TABLE "tblPiece" (
    "id" INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
    "parent_piece_id" TEXT
			   CONSTRAINT fk_parent_piece_id  REFERENCES tblPiece(id),
    "name" TEXT NOT NULL,
    "old_name" TEXT,
    "connector" TEXT,
    "composer_id" INTEGER NOT NULL
			   CONSTRAINT fk_composer_id  REFERENCES tblArtist(id),
    "genre_id" INTEGER NOT NULL
			   CONSTRAINT fk_genre_id  REFERENCES tblGenre(id),
    "detail" TEXT,
    "extra" TEXT,
    "text" TEXT,
    "created_date" TEXT,
    "modified_date" TEXT,
    "temp" TEXT);
CREATE TABLE "tblTrack" (
    "id" INTEGER  PRIMARY KEY AUTOINCREMENT NOT NULL,
    "album_id" INTEGER  NOT NULL
			   CONSTRAINT fk_album_id  REFERENCES tblAlbum(id),
    "piece_id" INTEGER  NOT NULL
			   CONSTRAINT fk_piece_id  REFERENCES tblPiece(id),
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
CREATE VIEW "vwGenreComposer" AS SELECT DISTINCT 
	tblPiece.genre_id AS id, 
	tblPiece.composer_id AS composer_id, 
	tblGenre.name AS name 
FROM tblGenre, tblPiece 
WHERE tblPiece.genre_id = tblGenre.id;
CREATE VIEW "vwartist" AS
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
	tblArtistName.alternate_last AS alternate_last,
	tblArtistName.alternate_first AS alternate_first
FROM tblArtist, tblArtistName, tblArtistType
WHERE tblArtist.name_id = tblArtistName.id 
and tblArtist.type_id = tblArtistType.id;
CREATE TRIGGER fkd_tblAlbum_albumartist_id_tblArtist_id
BEFORE DELETE ON tblArtist
FOR EACH ROW BEGIN
  SELECT RAISE(ROLLBACK, 'delete on table tblArtist violates foreign key constraint "fkd_tblAlbum_albumartist_id_tblArtist_id"')
  WHERE (SELECT albumartist_id FROM tblAlbum WHERE albumartist_id = OLD.id) IS NOT NULL;
END;
CREATE TRIGGER fkd_tblAlbum_label_id_tblLabel_id
BEFORE DELETE ON tblLabel
FOR EACH ROW BEGIN
  SELECT RAISE(ROLLBACK, 'delete on table tblLabel violates foreign key constraint "fkd_tblAlbum_label_id_tblLabel_id"')
  WHERE (SELECT label_id FROM tblAlbum WHERE label_id = OLD.id) IS NOT NULL;
END;
CREATE TRIGGER fkd_tblArtist_name_id_tblArtistName_id
BEFORE DELETE ON tblArtistName
FOR EACH ROW BEGIN
  SELECT RAISE(ROLLBACK, 'delete on table tblArtistName violates foreign key constraint "fkd_tblArtist_name_id_tblArtistName_id"')
  WHERE (SELECT name_id FROM tblArtist WHERE name_id = OLD.id) IS NOT NULL;
END;
CREATE TRIGGER fkd_tblArtist_type_id_tblArtistType_id
BEFORE DELETE ON tblArtistType
FOR EACH ROW BEGIN
  SELECT RAISE(ROLLBACK, 'delete on table tblArtistType violates foreign key constraint "fkd_tblArtist_type_id_tblArtistType_id"')
  WHERE (SELECT type_id FROM tblArtist WHERE type_id = OLD.id) IS NOT NULL;
END;
CREATE TRIGGER fkd_tblPiece_composer_id_tblArtist_id
BEFORE DELETE ON tblArtist
FOR EACH ROW BEGIN
  SELECT RAISE(ROLLBACK, 'delete on table tblArtist violates foreign key constraint "fkd_tblPiece_composer_id_tblArtist_id"')
  WHERE (SELECT composer_id FROM tblPiece WHERE composer_id = OLD.id) IS NOT NULL;
END;
CREATE TRIGGER fkd_tblPiece_genre_id_tblGenre_id
BEFORE DELETE ON tblGenre
FOR EACH ROW BEGIN
  SELECT RAISE(ROLLBACK, 'delete on table tblGenre violates foreign key constraint "fkd_tblPiece_genre_id_tblGenre_id"')
  WHERE (SELECT genre_id FROM tblPiece WHERE genre_id = OLD.id) IS NOT NULL;
END;
CREATE TRIGGER fkd_tblPiece_parent_piece_id_tblPiece_id
BEFORE DELETE ON tblPiece
FOR EACH ROW BEGIN
  SELECT RAISE(ROLLBACK, 'delete on table tblPiece violates foreign key constraint "fkd_tblPiece_parent_piece_id_tblPiece_id"')
  WHERE (SELECT parent_piece_id FROM tblPiece WHERE parent_piece_id = OLD.id) IS NOT NULL;
END;
CREATE TRIGGER fkd_tblTrack_album_id_tblAlbum_id
BEFORE DELETE ON tblAlbum
FOR EACH ROW BEGIN
  SELECT RAISE(ROLLBACK, 'delete on table tblAlbum violates foreign key constraint "fkd_tblTrack_album_id_tblAlbum_id"')
  WHERE (SELECT album_id FROM tblTrack WHERE album_id = OLD.id) IS NOT NULL;
END;
CREATE TRIGGER fkd_tblTrack_piece_id_tblPiece_id
BEFORE DELETE ON tblPiece
FOR EACH ROW BEGIN
  SELECT RAISE(ROLLBACK, 'delete on table tblPiece violates foreign key constraint "fkd_tblTrack_piece_id_tblPiece_id"')
  WHERE (SELECT piece_id FROM tblTrack WHERE piece_id = OLD.id) IS NOT NULL;
END;
CREATE TRIGGER fki_tblAlbum_albumartist_id_tblArtist_id
BEFORE INSERT ON [tblAlbum]
FOR EACH ROW BEGIN
  SELECT RAISE(ROLLBACK, 'insert on table tblAlbum violates foreign key constraint "fki_tblAlbum_albumartist_id_tblArtist_id"')
  WHERE (SELECT id FROM tblArtist WHERE id = NEW.albumartist_id) IS NULL;
END;
CREATE TRIGGER fki_tblAlbum_label_id_tblLabel_id
BEFORE INSERT ON [tblAlbum]
FOR EACH ROW BEGIN
  SELECT RAISE(ROLLBACK, 'insert on table tblAlbum violates foreign key constraint "fki_tblAlbum_label_id_tblLabel_id"')
  WHERE NEW.label_id IS NOT NULL AND (SELECT id FROM tblLabel WHERE id = NEW.label_id) IS NULL;
END;
CREATE TRIGGER fki_tblArtist_name_id_tblArtistName_id
BEFORE INSERT ON [tblArtist]
FOR EACH ROW BEGIN
  SELECT RAISE(ROLLBACK, 'insert on table "tblArtist" violates foreign key constraint "fki_tblArtist_name_id_tblArtistName_id"')
  WHERE (SELECT id FROM tblArtistName WHERE id = NEW.name_id) IS NULL;
END;
CREATE TRIGGER fki_tblArtist_type_id_tblArtistType_id
BEFORE INSERT ON [tblArtist]
FOR EACH ROW BEGIN
  SELECT RAISE(ROLLBACK, 'insert on table tblArtist violates foreign key constraint "fki_tblArtist_type_id_tblArtistType_id"')
  WHERE (SELECT id FROM tblArtistType WHERE id = NEW.type_id) IS NULL;
END;
CREATE TRIGGER fki_tblPiece_composer_id_tblArtist_id
BEFORE INSERT ON [tblPiece]
FOR EACH ROW BEGIN
  SELECT RAISE(ROLLBACK, 'insert on table tblPiece violates foreign key constraint "fki_tblPiece_composer_id_tblArtist_id"')
  WHERE (SELECT id FROM tblArtist WHERE id = NEW.composer_id) IS NULL;
END;
CREATE TRIGGER fki_tblPiece_genre_id_tblGenre_id
BEFORE INSERT ON [tblPiece]
FOR EACH ROW BEGIN
  SELECT RAISE(ROLLBACK, 'insert on table tblPiece violates foreign key constraint "fki_tblPiece_genre_id_tblGenre_id"')
  WHERE (SELECT id FROM tblGenre WHERE id = NEW.genre_id) IS NULL;
END;
CREATE TRIGGER fki_tblPiece_parent_piece_id_tblPiece_id
BEFORE INSERT ON [tblPiece]
FOR EACH ROW BEGIN
  SELECT RAISE(ROLLBACK, 'insert on table tblPiece violates foreign key constraint "fki_tblPiece_parent_piece_id_tblPiece_id"')
  WHERE NEW.parent_piece_id IS NOT NULL AND (SELECT id FROM tblPiece WHERE id = NEW.parent_piece_id) IS NULL;
END;
CREATE TRIGGER fki_tblTrack_album_id_tblAlbum_id
BEFORE INSERT ON [tblTrack]
FOR EACH ROW BEGIN
  SELECT RAISE(ROLLBACK, 'insert on table tblTrack violates foreign key constraint "fki_tblTrack_album_id_tblAlbum_id"')
  WHERE (SELECT id FROM tblAlbum WHERE id = NEW.album_id) IS NULL;
END;
CREATE TRIGGER fki_tblTrack_piece_id_tblPiece_id
BEFORE INSERT ON [tblTrack]
FOR EACH ROW BEGIN
  SELECT RAISE(ROLLBACK, 'insert on table tblTrack violates foreign key constraint "fki_tblTrack_piece_id_tblPiece_id"')
  WHERE (SELECT id FROM tblPiece WHERE id = NEW.piece_id) IS NULL;
END;
CREATE TRIGGER fku_tblAlbum_albumartist_id_tblArtist_id
BEFORE UPDATE ON [tblAlbum]
FOR EACH ROW BEGIN
    SELECT RAISE(ROLLBACK, 'update on table tblAlbum violates foreign key constraint "fku_tblAlbum_albumartist_id_tblArtist_id"')
      WHERE (SELECT id FROM tblArtist WHERE id = NEW.albumartist_id) IS NULL;
END;
CREATE TRIGGER fku_tblAlbum_label_id_tblLabel_id
BEFORE UPDATE ON [tblAlbum]
FOR EACH ROW BEGIN
    SELECT RAISE(ROLLBACK, 'update on table tblAlbum violates foreign key constraint "fku_tblAlbum_label_id_tblLabel_id"')
      WHERE NEW.label_id IS NOT NULL AND (SELECT id FROM tblLabel WHERE id = NEW.label_id) IS NULL;
END;
CREATE TRIGGER fku_tblArtist_name_id_tblArtistName_id
BEFORE UPDATE ON [tblArtist]
FOR EACH ROW BEGIN
    SELECT RAISE(ROLLBACK, 'update on table tblArtist violates foreign key constraint "fku_tblArtist_name_id_tblArtistName_id"')
      WHERE (SELECT id FROM tblArtistName WHERE id = NEW.name_id) IS NULL;
END;
CREATE TRIGGER fku_tblArtist_type_id_tblArtistType_id
BEFORE UPDATE ON [tblArtist]
FOR EACH ROW BEGIN
    SELECT RAISE(ROLLBACK, 'update on table tblArtist violates foreign key constraint "fku_tblArtist_type_id_tblArtistType_id"')
      WHERE (SELECT id FROM tblArtistType WHERE id = NEW.type_id) IS NULL;
END;
CREATE TRIGGER fku_tblPiece_composer_id_tblArtist_id
BEFORE UPDATE ON [tblPiece]
FOR EACH ROW BEGIN
    SELECT RAISE(ROLLBACK, 'update on table tblPiece violates foreign key constraint "fku_tblPiece_composer_id_tblArtist_id"')
      WHERE (SELECT id FROM tblArtist WHERE id = NEW.composer_id) IS NULL;
END;
CREATE TRIGGER fku_tblPiece_genre_id_tblGenre_id
BEFORE UPDATE ON [tblPiece]
FOR EACH ROW BEGIN
    SELECT RAISE(ROLLBACK, 'update on table tblPiece violates foreign key constraint "fku_tblPiece_genre_id_tblGenre_id"')
      WHERE (SELECT id FROM tblGenre WHERE id = NEW.genre_id) IS NULL;
END;
CREATE TRIGGER fku_tblPiece_parent_piece_id_tblPiece_id
BEFORE UPDATE ON [tblPiece]
FOR EACH ROW BEGIN
    SELECT RAISE(ROLLBACK, 'update on table tblPiece violates foreign key constraint "fku_tblPiece_parent_piece_id_tblPiece_id"')
      WHERE NEW.parent_piece_id IS NOT NULL AND (SELECT id FROM tblPiece WHERE id = NEW.parent_piece_id) IS NULL;
END;
CREATE TRIGGER fku_tblTrack_album_id_tblAlbum_id
BEFORE UPDATE ON [tblTrack]
FOR EACH ROW BEGIN
    SELECT RAISE(ROLLBACK, 'update on table tblTrack violates foreign key constraint "fku_tblTrack_album_id_tblAlbum_id"')
      WHERE (SELECT id FROM tblAlbum WHERE id = NEW.album_id) IS NULL;
END;
CREATE TRIGGER fku_tblTrack_piece_id_tblPiece_id
BEFORE UPDATE ON [tblTrack]
FOR EACH ROW BEGIN
    SELECT RAISE(ROLLBACK, 'update on table tblTrack violates foreign key constraint "fku_tblTrack_piece_id_tblPiece_id"')
      WHERE (SELECT id FROM tblPiece WHERE id = NEW.piece_id) IS NULL;
END;
CREATE TRIGGER "tr_archive_old_name"
   AFTER UPDATE OF name ON main.tblPiece
   WHEN (old.name != new.name AND 
               (new.old_name IS NULL OR new.old_name 
	       NOT LIKE '%' || old.name || '|%'))
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
    SET created_date = strftime('%Y-%m-%dT%H:%M','now', 'localtime')
    WHERE id = new.id;
END;
CREATE TRIGGER "tr_created_date_artist"
   AFTER   INSERT
   ON main.tblArtist
BEGIN
    UPDATE tblArtist
    SET created_date = strftime('%Y-%m-%dT%H:%M','now', 'localtime')
    WHERE id = new.id;
END;
CREATE TRIGGER "tr_created_date_artistname"
   AFTER   INSERT
   ON main.tblArtistName
BEGIN
    UPDATE tblArtistName
    SET created_date = strftime('%Y-%m-%dT%H:%M','now', 'localtime')
    WHERE id = new.id;
END;
CREATE TRIGGER "tr_created_date_artisttype"
   AFTER   INSERT
   ON main.tblArtistType
BEGIN
    UPDATE tblArtistType
    SET created_date = strftime('%Y-%m-%dT%H:%M','now', 'localtime')
    WHERE id = new.id;
END;
CREATE TRIGGER "tr_created_date_label"
   AFTER   INSERT
   ON main.'tblLabel'
BEGIN
    UPDATE tblLabel
    SET created_date = strftime('%Y-%m-%dT%H:%M','now', 'localtime')
    WHERE id = new.id;
END;
CREATE TRIGGER "tr_created_date_piece"
   AFTER   INSERT
   ON main.'tblPiece'
BEGIN
    UPDATE tblPiece
    SET created_date = strftime('%Y-%m-%dT%H:%M','now', 'localtime')
    WHERE id = new.id;
END;
CREATE TRIGGER "tr_created_date_track"
   AFTER   INSERT
   ON main.tblTrack
BEGIN
    UPDATE tblTrack
    SET created_date = strftime('%Y-%m-%dT%H:%M','now', 'localtime')
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
        strftime('%Y-%m-%dT%H:%M','now', 'localtime')
    WHERE id = new.id;
END;
CREATE TRIGGER "tr_set_mod_date_artist"
   AFTER  UPDATE ON main.tblArtist
BEGIN
    UPDATE tblArtist
    SET modified_date =  strftime('%Y-%m-%dT%H:%M','now', 'localtime')
    WHERE id = new.id;
END;
CREATE TRIGGER "tr_set_mod_date_artistname"
   AFTER  UPDATE ON main.tblArtistName
BEGIN
    UPDATE tblArtistName
    SET modified_date = strftime('%Y-%m-%dT%H:%M','now', 'localtime')
    WHERE id = new.id;
END;
CREATE TRIGGER "tr_set_mod_date_artisttype"
   AFTER  UPDATE ON main.tblArtistType
BEGIN
    UPDATE tblArtistType
    SET modified_date = strftime('%Y-%m-%dT%H:%M','now', 'localtime')
    WHERE id = new.id;
END;
CREATE TRIGGER "tr_set_mod_date_label"
   AFTER  UPDATE ON main.tblLabel
BEGIN
    UPDATE tblLabel
    SET modified_date = strftime('%Y-%m-%dT%H:%M','now', 'localtime')
    WHERE id = new.id;
END;
CREATE TRIGGER "tr_set_mod_date_piece"
   AFTER  UPDATE ON main.tblpiece
BEGIN
    UPDATE tblpiece
    SET modified_date = strftime('%Y-%m-%dT%H:%M','now', 'localtime')
    WHERE id = new.id;
END;
CREATE TRIGGER "tr_set_mod_date_track"
   AFTER  UPDATE ON main.tblTrack
BEGIN
    UPDATE tblTrack
    SET modified_date = strftime('%Y-%m-%dT%H:%M','now', 'localtime')
    WHERE id = new.id;
END;
