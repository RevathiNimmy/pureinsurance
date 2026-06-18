
SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_get_Failed_transaction'
GO

-- get all transaction from stats_folder with status <> 'c' in transaction_export_folder or does not exist in transaction_export_folder
CREATE PROCEDURE spu_get_Failed_transaction
	@ExcludeOtherDoc int
AS

BEGIN

	SELECT  IsNull(tef.transaction_export_folder_cnt,0),
			sf.insurance_file_cnt,
			sf.document_ref,
			sf.insurance_ref,
			sf.cover_start_date,
			sf.expiry_date,
			sf.insurance_holder_shortname,
			IsNull(tef.accounts_export_status,'No Export')
	FROM	Stats_Folder sf 
    LEFT JOIN Transaction_Export_Folder tef ON sf.insurance_file_cnt = tef.insurance_file_cnt AND sf.document_ref = tef.document_ref
	WHERE	tef.accounts_export_status <> 'c' 
	AND     (LEFT(tef.document_ref,3) IN ('SID','SIC','SDD','SPD','SRD','SRC','SED','SEC','SND','SNC') OR @ExcludeOtherDoc=0)
    OR		(tef.accounts_export_status IS NULL AND (LEFT(tef.document_ref,3) IN ('SID','SIC','SDD','SPD','SRD','SRC','SED','SEC','SND','SNC') OR @ExcludeOtherDoc=0))
	
	
END