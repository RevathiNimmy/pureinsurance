SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Ins_File_Public_Text_saa'
GO

CREATE PROCEDURE spe_Ins_File_Public_Text_saa
    @insurance_file_cnt int
AS
SELECT
    insurance_file_cnt,
    ins_file_public_text_id,
    text_line
 FROM Ins_File_Public_Text
--MKW050603 PN3372 1.6.9 to 1.8.6 catchup START
WHERE insurance_file_cnt IN
(
	SELECT	iall.insurance_file_cnt
	FROM insurance_file iall
	JOIN insurance_file i
	ON i.insurance_folder_cnt = iall.insurance_folder_cnt
	WHERE i.insurance_file_cnt = @insurance_file_cnt 
)
--MKW050603 PN3372 1.6.9 to 1.8.6 catchup END
--AR20050223 PN16395 Add sort order of 'by date created'
ORDER BY created_datestamp, insurance_file_cnt, ins_file_public_text_id ASC

GO

