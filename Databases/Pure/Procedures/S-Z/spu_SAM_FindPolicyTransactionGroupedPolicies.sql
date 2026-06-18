EXEC DDLDropProcedure 'spu_SAM_FindPolicyTransactionGroupedPolicies'
GO

CREATE PROCEDURE spu_SAM_FindPolicyTransactionGroupedPolicies 
 @SourceID INT,  
 @AgentCode VARCHAR(30)=NULL,  
 @ClientCode VARCHAR(30)=NULL,  
 @DueDate DATETIME = NULL,  
 @EffectiveFromDate DATETIME = NULL ,  
 @EffectiveToDate DATETIME = NULL ,  
 @PolicyReference VARCHAR(30)=NULL,  
 @OnlyOutstanding INT =0,
 @AgentKey INT=0
AS  
  
SELECT  MAX(PA.shortname)  'ClientCode',  
  MAX(PA.name) 'ClientName',  
  MAX(tt.code) 'Transdetail_Type_Code',  
  MAX(td.fee_type) 'Fee_Type',  
  MAX(C.code) 'PolicyCurrency',  
  IFI.insurance_folder_cnt 'PolicyFolderId' ,  
  MAX(IFI.insurance_ref) 'PolicyNumber',  
        SUM(tex.currency_amount) 'currency_amount',  
        SUM(tex.outstanding_currency_amount) 'outstanding_currency_amount'  
FROM transdetail td  
      INNER JOIN Document d ON d.document_id = td.document_id  
      INNER JOIN DocumentType dt ON dt.documenttype_id = d.documenttype_id  
      INNER JOIN TransDetailEx tex ON tex.transdetail_id = td.transdetail_id  
      INNER JOIN insurance_file ifi ON ifi.insurance_file_cnt = d.insurance_file_cnt  
      INNER JOIN Transdetail_Type tt ON tt.transdetail_type_id = td.transdetail_type_id  
      INNER JOIN Account a ON a.account_id = td.account_id  
      INNER JOIN Ledger l on l.ledger_id = a.ledger_id  
      INNER JOIN Currency C ON C.currency_id =IFI.CURRENCY_ID  
      LEFT OUTER JOIN PARTY PA ON PA.party_cnt =IFI.insured_cnt  
      LEFT OUTER JOIN PARTY PAA ON PAA.party_cnt =IFI.lead_agent_cnt  
WHERE( (ifi.insurance_ref  Like @PolicyReference AND ISNULL(@PolicyReference,'')<>'') OR  (@PolicyReference IS NULL))  
      AND dt.code IN ('SND', 'SED', 'SEC', 'SID', 'SIC', 'SRD', 'SRC')  
      AND l.ledger_short_name IN ('SA', 'AG', 'IN')  
      AND ((@OnlyOutstanding=1 AND tex.outstanding_currency_amount <> 0)  or (@OnlyOutstanding = 0))  
      AND ((@EffectiveFromDate IS NULL) OR (@EffectiveFromDate IS NOT NULL AND tex.effective_date >=@EffectiveFromDate))  
      AND ((@EffectiveToDate IS NULL) OR (@EffectiveToDate IS NOT NULL AND tex.effective_date <=@EffectiveToDate))  
      AND ( (PA.shortname Like @ClientCode AND ISNULL(@ClientCode,'')<>'') OR  (@ClientCode IS NULL))  
       AND ( (PAA.shortname Like @AgentCode AND ISNULL(@AgentCode,'')<>'') OR  (@AgentCode IS NULL)) 
	   AND (IFI.lead_agent_cnt =@AgentKey OR @AgentKey=0)
GROUP BY IFI.insurance_folder_cnt, tt.code, td.fee_type  
ORDER BY IFI.insurance_folder_cnt, td.fee_type  