SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO
EXECUTE DDLDropProcedure 'spu_FSA_Complaint_File_sel'
GO
CREATE PROCEDURE spu_FSA_Complaint_File_sel
	@FSA_complaint_folder_cnt int
AS
BEGIN
SELECT 
	f.FSA_complaint_file_cnt,
	f.FSA_complaint_folder_cnt,
	f.complaint_version,
	f.FSA_complaint_actiontype_id,
	f.comment,
	f.user_id,
	f.date_modified,
	at.description,
	0
FROM FSA_Complaint_File f,
     FSA_complaint_actiontype at
WHERE f.FSA_complaint_folder_cnt=@FSA_complaint_folder_cnt
AND   f.FSA_complaint_actiontype_id = at.FSA_complaint_actiontype_id
END
GO

