SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_PFGetTransactionsFromInsuranceFile'
GO

CREATE PROCEDURE spu_PFGetTransactionsFromInsuranceFile 
    @InsuranceFileCnt INT    
AS    
    
DECLARE @IsBrokerBusiness int    
DECLARE @insurance_folder_cnt int    
DECLARE @PFRF_ID INT
DECLARE @Finance_Net_Commission TINYINT
    
-- get the insurance folder cnt    
SELECT @insurance_Folder_cnt = insurance_folder_cnt from insurance_file where insurance_file_cnt = @InsuranceFileCnt    


SELECT @PFRF_ID = PFRF_ID FROM PFPremiumFinance WHERE Insurance_file_cnt = @InsuranceFileCnt
SELECT @Finance_Net_Commission = finance_net_commission FROM PFRF WHERE PFRF_ID = @PFRF_ID
			

SELECT  @IsBrokerBusiness = Count(*)    
FROM    Insurance_file i    
 JOIN    Business_Type bt ON bt.business_type_id = i.business_type_id    
 JOIN    Party_Agent pa ON pa.party_cnt = i.lead_agent_cnt    
WHERE   pa.party_agent_type_id IN (1,5) -- Broker Agent    
AND     bt.business_type_id <> 1 -- Not Direct    
AND     i.insurance_file_cnt = @InsuranceFileCnt    
    
-- If we have a count we have a broker    
IF @IsBrokerBusiness > 0    
BEGIN    
SELECT * FROM    
 (SELECT    
        td.transdetail_id,    
        i.insurance_ref,    
        CASE WHEN spare='TAX' THEN td.amount-(Select isnull(SUM(value),0) FROM tax_calculation    
         WHERE insurance_file_cnt=@InsuranceFileCnt    
         AND include_tax_in_instalments=0
		 AND tax_group_id = TD.tax_group_id
		 AND tax_band_id = TD.tax_band_id
		 AND transtype not in ('TTRITP','TTRITC')
		 AND pfprem_finance_cnt IS  NULL)    
      WHEN spare='FEE' THEN td.amount-(Select isnull(SUM(base_fee_amount),0) FROM policy_fee_u    
         WHERE insurance_file_cnt=@InsuranceFileCnt    
         AND include_fee_in_instalments=0)    
          ELSE td.amount END camount,    
   td.account_id,    
 doc.insurance_file_cnt    
    FROM    
        Insurance_file i    
    INNER JOIN    
        Account Ac WITH(NOLOCK) ON ( Ac.account_key = i.lead_agent_cnt or Ac.account_key=i.insured_cnt) -- Link to lead_agent_cnt    
    INNER JOIN    
        Document doc ON doc.insurance_file_cnt = i.insurance_file_cnt    
    INNER JOIN    
        TransDetail td ON td.document_id = doc.document_id    
                      AND td.account_id = ac.account_id    
    WHERE    
        i.insurance_file_cnt = @InsuranceFileCnt    
    AND (td.spare NOT LIKE 'COM%' OR @Finance_Net_Commission=1) -- No commission transactions please (hmm)    
    
   UNION    
    
   SELECT td.transdetail_id,    
 i.insurance_ref,    
 td.amount,    
 td.account_id,    
 doc.insurance_file_cnt    
    FROM    
        Insurance_file i    
    INNER JOIN    
        Account Ac ON Ac.account_key = i.lead_agent_cnt -- Link to lead_agent_cnt    
    INNER JOIN    
        Document doc ON doc.insurance_file_cnt = i.insurance_file_cnt    
    INNER JOIN    
        TransDetail td ON td.document_id = doc.document_id    
                      AND td.account_id = ac.account_id    
    LEFT JOIN pftransaction_id pft ON    
 pft.pftransaction_id = td.transdetail_id    
    
    WHERE i.insurance_folder_cnt = @insurance_folder_cnt    
    AND i.put_on_next_instalment_renewal = 1    
    AND pftransaction_id IS NULL ) TRANS    
    
END    
    
ELSE    
    
BEGIN    
    
SELECT * FROM (SELECT    
        td.transdetail_id,    
        i.insurance_ref,    
        CASE WHEN spare='TAX' THEN td.amount-(Select isnull(SUM(value),0) FROM tax_calculation    
         WHERE insurance_file_cnt=@InsuranceFileCnt    
         AND include_tax_in_instalments=0
		 AND tax_group_id = TD.tax_group_id
		 AND tax_band_id = TD.tax_band_id
		 AND transtype not in ('TTRITP','TTRITC')
		 AND pfprem_finance_cnt IS  NULL)    
      WHEN spare='FEE' THEN td.amount-(Select isnull(SUM(base_fee_amount),0) FROM policy_fee_u    
         WHERE insurance_file_cnt=@InsuranceFileCnt    
         AND include_fee_in_instalments=0)    
          ELSE td.amount END camount,    
   td.account_id,    
 doc.insurance_file_cnt    
    FROM    
        Insurance_file i    
    INNER JOIN    
        Account Ac ON Ac.account_key = i.insured_cnt -- Link to insured_cnt    
    INNER JOIN    
        Document doc ON doc.insurance_file_cnt = i.insurance_file_cnt    
    INNER JOIN    
        TransDetail td ON td.document_id = doc.document_id    
                      AND td.account_id = ac.account_id    
    WHERE    
        i.insurance_file_cnt = @InsuranceFileCnt    
    
   UNION    
    
   SELECT td.transdetail_id,    
 i.insurance_ref,    
 td.amount,    
 td.account_id,    
 doc.insurance_file_cnt    
    FROM    
        Insurance_file i    
    INNER JOIN    
        Account Ac ON Ac.account_key = i.insured_cnt -- Link to insured_cnt    
    INNER JOIN    
        Document doc ON doc.insurance_file_cnt = i.insurance_file_cnt    
    INNER JOIN    
        TransDetail td ON td.document_id = doc.document_id    
                      AND td.account_id = ac.account_id    
    LEFT JOIN pftransaction_id pft ON    
pft.pftransaction_id = td.transdetail_id    
    
    WHERE i.insurance_folder_cnt = @insurance_folder_cnt    
    AND i.put_on_next_instalment_renewal = 1    
    AND pftransaction_id IS NULL) TRANS    
    
    --Tracy Richards - removed reliance on Spare column, using Account ID instead    
    --AND (td.spare = 'GROSS' OR td.spare = 'TAX' or (bt.code = 'DIRECT' and td.document_sequence in (1,2)))    
END    
    


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
