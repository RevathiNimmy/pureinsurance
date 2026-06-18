SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO
EXECUTE DDLDropProcedure 'spu_FSA_Complaint_File_upd'
GO
CREATE PROCEDURE spu_FSA_Complaint_File_upd
    @FSA_complaint_file_cnt int,
    @FSA_complaint_folder_cnt int,
    @complaint_version int,
    @FSA_complaint_actiontype_id int,
    @comment varchar(8000),
    @user_id int,
    @date_modified datetime
AS
BEGIN
UPDATE FSA_Complaint_File
SET
    FSA_complaint_folder_cnt=@FSA_complaint_folder_cnt,
    complaint_version=@complaint_version,
    FSA_complaint_actiontype_id=@FSA_complaint_actiontype_id,
    comment=@comment,
    user_id=@user_id,
    date_modified=@date_modified
WHERE FSA_complaint_file_cnt=@FSA_complaint_file_cnt
END
GO
