SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_text_file_sel'
GO

CREATE PROCEDURE spe_text_file_sel
    @entity_type_id int,
    @entity_cnt int,
    @slot_number int
AS
IF @entity_type_id = 2
BEGIN

	SELECT	entity_type_id,
		@entity_cnt as entity_cnt,
		slot_number,
		max(file_number) as file_number
	FROM text_file
	WHERE entity_type_id = 2
	AND slot_number = @slot_number 
	AND entity_cnt IN
	(
		SELECT	iall.insurance_file_cnt
		FROM insurance_file iall
		JOIN insurance_file i
		ON i.insurance_folder_cnt = iall.insurance_folder_cnt
		WHERE i.insurance_file_cnt = @entity_cnt 
	)
	GROUP BY entity_type_id, slot_number

END
ELSE
BEGIN
    SELECT
        entity_type_id,
        entity_cnt,
        slot_number,
        file_number
     FROM text_file
    WHERE entity_type_id = @entity_type_id AND entity_cnt = @entity_cnt AND slot_number = @slot_number
END
GO

