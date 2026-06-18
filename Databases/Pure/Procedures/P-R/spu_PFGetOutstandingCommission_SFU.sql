SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_PFGetOutstandingCommission_SFU'
GO

CREATE PROCEDURE spu_PFGetOutstandingCommission_SFU  
    @PremiumFinanceCnt int,  
    @PremiumFinanceVersion int,  
    @PartyCnt int = 0  
AS  
  
if @PartyCnt <> 0  
 Begin  
  SELECT td.currency_amount*-1 Commission,  
   td.outstanding_currency_amount*-1 OutstandingCommission,  
   td.Account_id AccountID,  
                        td.transdetail_id TransdetailId ,
                        td.amount as CommissionBaseAmount,
                        td.outstanding_amount as OutstandingCommissionBaseAmount
                        
                        
  FROM    Insurance_File i INNER JOIN  
          Document doc ON doc.insurance_file_cnt = i.insurance_file_cnt INNER JOIN  
          TransDetail td ON td.document_id = doc.document_id INNER JOIN  
          PFPremiumFinance pf ON pf.Insurance_File_Cnt = i.insurance_file_cnt INNER JOIN  
          PFScheme pfs ON pf.CompanyNo = pfs.CompanyNo AND pf.SchemeNo = pfs.SchemeNo AND pf.SchemeVersion = pfs.SchemeVersion  
		  INNER JOIN Transdetail_Type TDT ON TD.transdetail_type_id=TDT.transdetail_type_id
  WHERE   (pf.pfprem_finance_cnt = @PremiumFinanceCnt) AND  
    (pf.pfprem_finance_version = @PremiumFinanceVersion) AND  
    ((td.spare = 'COMM' OR td.spare = 'AGENT' OR TDT.code = 'COMSUSP') AND td.currency_amount<>0) AND  
    (td.Account_id IN (SELECT  account_id  
                                           FROM    Account  
                                           WHERE   ( account_key = @PartyCnt)))  
  Order By td.transdetail_id desc
 End  
else  
 Begin  
  SELECT td.currency_amount*-1 Commission,  
   td.outstanding_currency_amount*-1 OutstandingCommission,  
   td.Account_id AccountID,  
                        td.transdetail_id TransdetailId ,
                        td.amount as CommissionBaseAmount,
                        td.outstanding_amount as OutstandingCommissionBaseAmount
                         
  FROM    Insurance_File i INNER JOIN  
          Document doc ON doc.insurance_file_cnt = i.insurance_file_cnt INNER JOIN  
          TransDetail td ON td.document_id = doc.document_id INNER JOIN  
          PFPremiumFinance pf ON pf.Insurance_File_Cnt = i.insurance_file_cnt INNER JOIN  
          PFScheme pfs ON pf.CompanyNo = pfs.CompanyNo AND pf.SchemeNo = pfs.SchemeNo AND pf.SchemeVersion = pfs.SchemeVersion  
		  INNER JOIN Transdetail_Type TDT ON TD.transdetail_type_id=TDT.transdetail_type_id
  WHERE   (pf.pfprem_finance_cnt = @PremiumFinanceCnt) AND  
   (pf.pfprem_finance_version = @PremiumFinanceVersion) AND  
   ((td.spare = 'COMM' OR td.spare = 'AGENT' OR TDT.code = 'COMSUSP') AND td.currency_amount<>0)  
   Order By td.transdetail_id desc
 End  