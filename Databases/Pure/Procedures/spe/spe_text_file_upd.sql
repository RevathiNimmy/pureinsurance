SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_text_file_upd'
GO

CREATE PROCEDURE spe_text_file_upd
    @entity_type_id int,
    @entity_cnt int,
    @slot_number int,
    @file_number int
AS

IF @entity_type_id = 2
BEGIN
	UPDATE text_file
	SET	file_number = @file_number,
		entity_cnt = @entity_cnt
	WHERE entity_type_id = 2
	AND slot_number = @slot_number 
	AND entity_cnt IN
	(
		SELECT	max(iall.insurance_file_cnt)
		FROM insurance_file iall
		JOIN insurance_file i
		ON i.insurance_folder_cnt = iall.insurance_folder_cnt
		WHERE i.insurance_file_cnt = @entity_cnt 
		AND EXISTS 
			(
				SELECT * 
				FROM text_file 
				WHERE entity_type_id = 2
				AND slot_number = @slot_number 
				AND entity_cnt = iall.insurance_file_cnt
			)
	)
END
ELSE
BEGIN
    UPDATE text_file
        SET
        file_number=@file_number
    WHERE entity_type_id = @entity_type_id AND entity_cnt = @entity_cnt AND slot_number = @slot_number
END

GO

