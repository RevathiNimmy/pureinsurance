EXECUTE DDLDropProcedure 'spu_PFGetTransactions'
GO

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

CREATE PROCEDURE spu_PFGetTransactions

    @PremiumFinanceCnt int,

    @PremiumFinanceversion int

    AS 
	
	BEGIN
    	
        SELECT
		pftransaction_id,
		insurance_ref_index,
		amount,
		PFT_ID.insurance_file_cnt,
		TD.transdetail_id,
		D.document_ref,
		IFL.alternate_reference,
		IFL.cover_start_date,
		D.document_date,
		'' as 'media_type',
		TD.outstanding_amount,
		TD.spare,
		TD.account_id,
		A.account_id,
		A.short_code,
		C.description as 'currency_description',		
		TB.code,
		TD.currency_amount,
		C.description,
		TD.company_id,
		P.period_name,
		DT.description As 'DocumentType_Descritpion',
		DTG.description As 'DocumentTypeGroup_Description',
		IFL.insurance_ref,
		D.documenttype_id,
		TD.fully_matched,
		C.code as 'currency_Code',
		PFT_ID.insurance_file_cnt

		FROM transdetail AS TD
		INNER JOIN pfTransaction_ID AS PFT_ID ON PFT_ID.pfTransaction_ID = TD.transdetail_id
		JOIN Account A on A.account_id = TD.account_id
		JOIN Currency C on C.currency_id = TD.currency_id
		LEFT JOIN  Tax_Band TB on TB.tax_band_id = TD.tax_band_id
		JOIN Period P on P.period_id =  TD.period_id
		JOIN Document D on D.document_id = TD.document_id
		JOIN DocumentType DT on DT.documenttype_id = D.documenttype_id
		LEFT JOIN  DocTypeGroup DTG on DTG.doctypegroup_id =  DT.doctypegroup_id
		JOIN Insurance_File IFL on IFL.insurance_file_cnt = D.insurance_file_cnt
		AND PFT_ID.PFPrem_Finance_Cnt = @PremiumFinanceCnt
		AND PFT_ID.PFPrem_Finance_Version = @PremiumFinanceversion
    END

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO
