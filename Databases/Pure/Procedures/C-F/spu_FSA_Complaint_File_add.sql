SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO
EXECUTE DDLDropProcedure 'spu_FSA_Complaint_File_add'
GO
CREATE PROCEDURE spu_FSA_Complaint_File_add
    @FSA_complaint_file_cnt int OUTPUT ,
    @FSA_complaint_folder_cnt int,
    @complaint_version int,
    @FSA_complaint_actiontype_id int,
    @comment varchar(8000),
    @user_id int,
    @date_modified datetime
AS
BEGIN
INSERT INTO FSA_Complaint_File(
    FSA_complaint_folder_cnt,
    complaint_version,
    FSA_complaint_actiontype_id,
    comment,
    user_id,
    date_modified)
VALUES (
    @FSA_complaint_folder_cnt,
    @complaint_version,
    @FSA_complaint_actiontype_id,
    @comment,
    @user_id,
    @date_modified)
END
BEGIN
SELECT @FSA_complaint_file_cnt = @@IDENTITY
END
GO

