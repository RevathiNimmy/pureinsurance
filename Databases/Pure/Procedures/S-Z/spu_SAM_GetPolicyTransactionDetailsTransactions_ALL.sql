SET QUOTED_IDENTIFIER OFF 
GO

EXEC DDLDropProcedure 'spu_SAM_GetPolicyTransactionDetailsTransactions_ALL'
GO


CREATE PROCEDURE spu_SAM_GetPolicyTransactionDetailsTransactions_ALL
    @InsuranceFolderKey INT,
	@DueByDate DATETIME = NULL 
AS

SELECT  d.document_ref 'DocumentReference', 
		dt.code 'DocumentType', 
		tt.code 'Transdetail_Type_Code', 
		td.fee_type, 
		a.short_code, 
		td.transdetail_id, 
		tex.transdetailex_id, 
        ISNULL(tex.currency_amount, td.currency_amount) 'currency_amount', 
        ISNULL(tex.outstanding_currency_amount, td.outstanding_currency_amount) 'outstanding_currency_amount',
        D.document_id,
        ISNULL(tex.effective_date, ifi.cover_start_date) 'EffectiveDate',
        ifi.cover_start_date 'DocumentDate'

FROM Insurance_File ifi 
    INNER JOIN Document d ON ifi.insurance_file_cnt = d.insurance_file_cnt
    INNER JOIN TransDetail td ON d.document_id = td.document_id
    LEFT JOIN TransDetailEx tex ON tex.transdetail_id = td.transdetail_id
    INNER JOIN Account a ON a.account_id = td.account_id
    INNER JOIN Ledger l on l.ledger_id = a.ledger_id
    INNER JOIN DocumentType dt ON dt.documenttype_id = d.documenttype_id
    INNER JOIN Transdetail_Type tt ON tt.transdetail_type_id = td.transdetail_type_id
WHERE ifi.insurance_folder_cnt = @InsuranceFolderKey 
      AND dt.code IN ('SND', 'SED', 'SEC', 'SID', 'SIC', 'SRD', 'SRC')
      AND l.ledger_short_name IN ('SA', 'AG', 'CO')
      AND ISNULL(tex.effective_date, ifi.cover_start_date)
				 <= 
				 ISNULL(@DueByDate, 
			CASE ISNULL(tex.transdetailex_id, 0) 
				WHEN 0 THEN ifi.cover_start_date ELSE tex.effective_date END)
ORDER BY d.document_id, tex.effective_date, td.fee_type, tt.code
