SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

DDLDropProcedure 'spu_CLR_Get_Recovery_Transactions_For_Instalment'
GO

-- PBI #37524: Instalment for Claim Recovery - New Plan
-- Fetch outstanding CLR recovery transactions eligible for instalment plan creation
-- Returns transactions with outstanding > 0 that are not already linked to active plans
CREATE PROCEDURE spu_CLR_Get_Recovery_Transactions_For_Instalment
	@PartyCode      INT = 0,
	@ClaimNumber    VARCHAR(30) = NULL
AS
BEGIN
	SET NOCOUNT ON

	SELECT
		td.trans_detail_key         AS TransDetailKey,
		td.document_reference       AS DocumentReference,
		c.claim_number              AS ClaimNumber,
		p.party_key                 AS PartyKey,
		p.party_name                AS PartyName,
		td.transaction_date         AS TransactionDate,
		td.transaction_amount       AS TransactionAmount,
		td.outstanding_amount       AS OutstandingAmount,
		td.currency_code            AS CurrencyCode,
		td.insurance_file_key       AS InsuranceFileKey,
		td.clr_transaction_id       AS ClrTransactionId,
		dt.document_type_code       AS DocumentType
	FROM
		ACT_Trans_Detail td
		INNER JOIN ACT_Document_Type dt
			ON td.document_type_key = dt.document_type_key
		INNER JOIN CLM_Claim c
			ON td.claim_key = c.claim_key
		INNER JOIN PAR_Party p
			ON td.party_key = p.party_key
	WHERE
		dt.document_type_code IN ('CLR')
		AND td.outstanding_amount > 0
		AND (@PartyCode = 0 OR p.party_key = @PartyCode)
		AND (@ClaimNumber IS NULL OR @ClaimNumber = '' OR c.claim_number = @ClaimNumber)
		AND NOT EXISTS (
			SELECT 1
			FROM PFPlan_Transactions pt
				INNER JOIN PFPremiumFinance pf
					ON pt.pf_plan_key = pf.PFPrem_Finance_Cnt
			WHERE pt.trans_detail_key = td.trans_detail_key
			  AND pf.StatusInd NOT IN ('900', '999')
		)
	ORDER BY
		c.claim_number,
		td.transaction_date DESC,
		td.document_reference

END
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
