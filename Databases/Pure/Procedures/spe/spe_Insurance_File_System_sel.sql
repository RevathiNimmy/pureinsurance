SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_Insurance_File_System_sel'
GO
CREATE PROCEDURE spe_Insurance_File_System_sel
    @insurance_file_cnt int
AS
SELECT
    insurance_file_cnt,
    endorsement_count,
    created_by_id,
    date_created,
    modified_by_id,
    last_modified,
    last_trans_date,
    last_trans_type_id,
    isnull(last_trans_description, '') as last_trans_description,
    last_trans_debit_credit,
    last_trans_document_ref,
    last_trans_cover_start_date,
    last_trans_expiry_date
FROM Insurance_File_System
WHERE insurance_file_cnt = @insurance_file_cnt
GO

