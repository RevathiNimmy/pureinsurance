SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_wp_debitfee'
GO

CREATE PROCEDURE spu_wp_debitfee
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @RiskID INT,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT
AS
	--DC020703 -ISS2479 -wrong way round
	SELECT 	td.amount * -1 transaction_amount,
			td.accounting_date transaction_date,
			a.account_name fee_account,
			d.reason transaction_reason,
			ta.amount total_amount,
			ISNULL(v.amount, 0) vat_amount,
		isnull(v.taxgroup,'') taxgroup,
		isnull(v.TaxGroupCode,'') TaxGroupCode,
		isnull(v.amount,0) Taxamount
	FROM document d
	JOIN transdetail td on d.document_id = td.document_id
	JOIN account a on td.account_id = a.account_id
	JOIN ledger l on a.ledger_id = l.ledger_id
	JOIN party p on a.account_key = p.party_cnt
	JOIN party_type pt on p.party_type_id = pt.party_type_id
	JOIN accounttype at ON at.accounttype_id = a.accounttype_id
	JOIN
		-- get the total amount
		(SELECT td.amount, d.document_ref, d.company_id, p.party_cnt
			FROM transdetail td
			JOIN document d ON d.document_id = td.document_id
			JOIN account a ON td.account_id = a.account_id
			JOIN party p ON a.account_key = p.party_cnt
			JOIN ledger l ON a.ledger_id = l.ledger_id
		) AS ta ON ta.document_ref = d.document_ref
			AND ta.company_id = (SELECT source_id FROM party WHERE party_cnt = @partycnt)
			AND ta.party_cnt = @partycnt

	LEFT OUTER JOIN
		-- get the vat amount
		(SELECT td.amount * -1 amount, d.document_ref, d.company_id, tg.Code 'TaxGroupCode', tg.Description 'TaxGroup'
			FROM transdetail td
			JOIN document d on td.document_id = d.document_id
			JOIN account a on td.account_id = a.account_id
			JOIN ledger l on a.ledger_id = l.ledger_id
		LEFT JOIN Tax_Group TG on TG.Tax_Group_id = TD.Tax_Group_Id
			WHERE a.account_key = 0
		) AS v
		ON v.document_ref = d.document_ref
		AND v.company_id = (SELECT source_id FROM party WHERE party_cnt = @partycnt)

	WHERE pt.code = 'FE'
	AND d.company_id = (SELECT source_id FROM party WHERE party_cnt = @partycnt)
	AND	(d.document_ref = @DocumentRef OR ISNULL(@DocumentRef,'')='')


