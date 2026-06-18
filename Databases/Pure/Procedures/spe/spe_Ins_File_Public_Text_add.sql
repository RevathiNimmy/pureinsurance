SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Ins_File_Public_Text_add'
GO

CREATE PROCEDURE spe_Ins_File_Public_Text_add
    @insurance_file_cnt int,
    @ins_file_public_text_id int,
    @text_line varchar(255)
AS
BEGIN

--MKW050603 PN3372 1.6.9 to 1.8.6 catchup START
SELECT @ins_file_public_text_id = isnull(max(ins_file_public_text_id),0) + 1
FROM Ins_File_Public_Text
WHERE insurance_file_cnt = @insurance_file_cnt
--MKW050603 PN3372 1.6.9 to 1.8.6 catchup END

--AR20050223 PN16395 Populate datestamp field with curent date/time
INSERT INTO Ins_File_Public_Text (
    insurance_file_cnt ,
    ins_file_public_text_id ,
    text_line,
    created_datestamp)
VALUES (
    @insurance_file_cnt,
    @ins_file_public_text_id,
    @text_line,
    getdate())
END

GO

