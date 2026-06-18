SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_document_spooler_saa'
GO

CREATE PROCEDURE spe_document_spooler_saa

AS
SELECT
    document_spooler_id,
    document_type_id,
    party_cnt,
    insurance_folder_cnt,
    insurance_file_cnt,
    claim_cnt,
    description,
    is_deletable,
    is_editable,
    created_by_id,
    date_created,
    modified_by_id,
    date_modified,
    times_printed,
    times_archived
 FROM document_spooler
ORDER BY document_spooler_id ASC

GO

