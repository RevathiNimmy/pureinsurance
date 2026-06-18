SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_wp_PFInstalment'
GO


CREATE PROCEDURE spu_wp_PFInstalment  
    @PartyCnt INT,  
    @InsuranceFileCnt INT,  
    @RiskId INT,  
    @ClaimCnt INT,  
    @DocumentRef VARCHAR(25),  
    @Instance1 INT,  
    @Instance2 INT,  
    @Instance3 INT  
AS  
  
DECLARE @nPFPremiumFinanceVersion INT  
DECLARE @nPFPremiumFinanceCnt INT  
  
SELECT  @nPFPremiumFinanceCnt = pfp.pfprem_finance_cnt,  
  @nPFPremiumFinanceVersion = MAX(pfp.pfprem_finance_version)  
  FROM PFPremiumFinance pfp  
  INNER JOIN PFInstalments pfi ON pfp.pfprem_finance_cnt =pfi.pfprem_finance_cnt  
  WHERE pfp.Insurance_File_Cnt = @InsuranceFileCnt GROUP BY pfp.pfprem_finance_cnt  
  
SELECT  
    i.DueDate,  
    i.Fee,  
    i.Amount,  
    i.TransactionCode,  
    i.BatchNumber,  
    i.BatchExportDate,  
    i.PostedDate,  
    i.PFTransaction_id,  
    i.commission,  
    i.tax,  
    i.pfinstalments_result_id,  
    i.batch_id,  
 PFS.code as InstalmentStatusCode,  
    i.InstalmentNumber  
  
FROM  
    PFPremiumFinance pf  
INNER JOIN  
    PFInstalments i  
    ON  
        pf.PFPrem_Finance_Cnt = i.PFPrem_Finance_Cnt  
    AND pf.PFPrem_Finance_Version = i.PFPrem_Finance_Version  
LEFT JOIN  
 pfinstalmentS_status AS PFS ON PFS.pfinstalmentS_status_ID = I.STATUS  
  
WHERE  
    pf.insurance_file_cnt = @InsuranceFileCnt  
AND i.InstalmentNumber = @Instance2 
AND pf.pfprem_finance_cnt = @nPFPremiumFinanceCnt
AND pf.pfprem_finance_version = @nPFPremiumFinanceVersion  

GO

SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO