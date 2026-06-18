SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_update_text_file_insfilecnt'
GO

CREATE PROCEDURE spe_update_text_file_insfilecnt
    @old_insurance_file_cnt int,
    @new_insurance_file_cnt int
AS
BEGIN

UPDATE  text_file
SET     entity_cnt = @new_insurance_file_cnt
WHERE   entity_cnt = @old_insurance_file_cnt

END
GO
