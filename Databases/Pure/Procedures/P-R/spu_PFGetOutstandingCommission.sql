SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_PFGetOutstandingCommission'
GO

CREATE PROCEDURE spu_PFGetOutstandingCommission
    @PremiumFinanceCnt int,
    @PremiumFinanceVersion int 
AS
DECLARE @TransactionID 	int
DECLARE @Amount 	numeric(19, 4)
DECLARE @AmountPaid 	numeric(19, 4)
DECLARE @AccountID 	int

 
	SELECT	@TransactionID = td.transdetail_id,
   		@Amount = currency_amount*-1,
		@accountID = ex.account_map_id
	FROM    Insurance_File i 
		INNER JOIN Document doc ON doc.insurance_file_cnt = i.insurance_file_cnt 
                INNER JOIN TransDetail td ON td.document_id = doc.document_id 
                INNER JOIN Business_Type bt ON bt.business_type_id = i.business_type_id 
                INNER JOIN PFPremiumFinance pf ON pf.Insurance_File_Cnt = i.insurance_file_cnt
		INNER JOIN PFScheme pfs ON pf.CompanyNo = pfs.CompanyNo 
			     AND pf.SchemeNo = pfs.SchemeNo 
                             AND pf.SchemeVersion = pfs.SchemeVersion
		INNER JOIN account a ON td.account_id = a.account_id
		INNER JOIN Element e on a.short_code = e.element_name
		INNER JOIN Elementextras ex on e.element_id = ex.element_id
	WHERE   (pf.pfprem_finance_cnt = @PremiumFinanceCnt) AND 
		(pf.pfprem_finance_version = @PremiumFinanceVersion) AND
		(td.spare = 'BROK' )
	 
	SELECT @AmountPaid = sum(m.currency_match_amount)
	FROM   TransMatch m RIGHT OUTER JOIN
		TransDetail t INNER JOIN
		Document d ON t.document_id = d.document_id ON m.transdetail_id = t.transdetail_id
	WHERE  (t.transdetail_id = @transactionID)
	GROUP BY d.document_ref, t.transdetail_id, t.amount,t.currency_amount


	SELECT  Commission = ISNULL(@amount,0.0) ,
		OutstandingCommission = ISNULL(@amount,0.0) + ISNULL(@amountPaid, 0.0),	
		AccountID = ISNULL(@AccountID,0) ,
		TransactionID = ISNULL(@TransactionID,0)  

GO
