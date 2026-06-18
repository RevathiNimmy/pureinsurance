SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Ins_File_Private_Text_add'
GO

CREATE PROCEDURE spe_Ins_File_Private_Text_add
    @insurance_file_cnt int,
    @ins_file_private_text_id int,
    @text_line varchar(255)
AS
BEGIN

--MKW050603 PN3372 1.6.9 to 1.8.6 catchup START
SELECT @ins_file_private_text_id = isnull(max(ins_file_private_text_id),0) + 1
FROM Ins_File_Private_Text
WHERE insurance_file_cnt = @insurance_file_cnt
--MKW050603 PN3372 1.6.9 to 1.8.6 catchup END

INSERT INTO Ins_File_Private_Text (
    insurance_file_cnt ,
    ins_file_private_text_id ,
    text_line )
VALUES (
    @insurance_file_cnt,
    @ins_file_private_text_id,
    @text_line)
END

GO

