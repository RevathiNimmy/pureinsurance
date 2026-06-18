SET QUOTED_IDENTIFIER ON 
GO

EXEC DDLDropProcedure 'spu_SAM_GetPolicyTransactionDetailsTransactions'
GO

CREATE PROCEDURE [dbo].[spu_SAM_GetPolicyTransactionDetailsTransactions]
	@InsuranceFolderKey INT,
	@DueByDate DATETIME = NULL ,
	@OnlyOutstanding INT = 0
AS
/*
	Modified Date		Modified by					 Description
	----------------------------------------------------------------------------------------------------------------------------------------------
	23 May 2018			George Harris/Sahil Ansari	Removed join on select back to transdetailsex as was not efficient and moved into a temp table
													Changed the oin on the temp table to retrieve the documentid and join the main query to the temp table
													Reformatted to coding standards definition
														
	Test Code	

			exec spu_SAM_GetPolicyTransactionDetailsTransactions_Modified2 246065,NULL,1

*/
BEGIN

	IF @OnlyOutstanding = 0
	BEGIN
		EXEC spu_SAM_GetPolicyTransactionDetailsTransactions_ALL @InsuranceFolderKey, @DueByDate
	END
	ELSE
	BEGIN

		SELECT DISTINCT d.document_id 
		INTO #tmptransPortions
		FROM Insurance_File ifi
			INNER JOIN Document d ON ifi.insurance_file_cnt = d.insurance_file_cnt
			INNER JOIN TransDetail td ON d.document_id = td.document_id
			INNER JOIN TransDetailEx tex ON tex.transdetail_id = td.transdetail_id
		WHERE ifi.insurance_folder_cnt = @InsuranceFolderKey
			AND tex.outstanding_currency_amount <> 0

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
				ifi.cover_start_date 'DocumentDate',
				tex.portion_no,
				tex.transdetailex_id,
				tex.outstanding_currency_amount
		FROM Insurance_File ifi
			INNER JOIN Document d ON ifi.insurance_file_cnt = d.insurance_file_cnt
			INNER JOIN TransDetail td ON d.document_id = td.document_id
			LEFT JOIN TransDetailEx tex ON tex.transdetail_id = td.transdetail_id
			INNER JOIN Account a ON a.account_id = td.account_id
			INNER JOIN Ledger l on l.ledger_id = a.ledger_id
			INNER JOIN DocumentType dt ON dt.documenttype_id = d.documenttype_id
			INNER JOIN Transdetail_Type tt ON tt.transdetail_type_id = td.transdetail_type_id
			INNER JOIN #tmptransPortions tmp On d.document_id = tmp.document_id
		WHERE ifi.insurance_folder_cnt = @InsuranceFolderKey
			  AND dt.code IN ('SND', 'SED', 'SEC', 'SID', 'SIC', 'SRD', 'SRC')
			  AND l.ledger_short_name IN ('SA', 'AG', 'CO')
			  AND ISNULL(tex.effective_date, ifi.cover_start_date)
						 <=
						 ISNULL(@DueByDate,
					CASE ISNULL(tex.transdetailex_id, 0)
						WHEN 0 THEN ifi.cover_start_date ELSE tex.effective_date END)

		ORDER BY d.document_id, tex.effective_date, td.fee_type, tt.code
	END
END

GO
