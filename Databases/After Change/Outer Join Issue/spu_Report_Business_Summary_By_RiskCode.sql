SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Report_Business_Summary_By_RiskCode'
GO      
-- spu_Report_Business_Summary_By_RiskCode 2,'01/01/2008','12/12/2008'    
      
------------------------------------------------      
-- Created by : Elaine Knott      
-- Date : 10/07/2002      
-- Description : Summary of Business By Risk Code      
------------------------------------------------      
CREATE procedure spu_Report_Business_Summary_By_RiskCode      
    @branch_id int,      
    @start_date datetime,      
    @end_date datetime      
AS      
      
DECLARE @iBranchID int      
      
SELECT @iBranchID = ISNULL(@branch_id, 0)      
     
SELECT RC.description Risk,      
    TEF.insurance_file_Cnt Insurance_cnt,      
    TED.transaction_amount Gross_Amount,      
    TED.taxes_total Taxes,      
    TED2.transaction_amount Commission      
      
INTO #TempTable1      
    ---Change By Kuldeep panwar 07/01/2010   
    FROM Transaction_Export_folder TEF left outer join  Transaction_Export_detail TED     
 on TEF.transaction_export_folder_cnt = TED.transaction_export_folder_cnt   AND TED.spare in ('GROSS')     
left outer join Transaction_Export_detail TED2    
on TEF.transaction_export_folder_cnt = TED2.transaction_export_folder_cnt      
    AND TED2.spare in ('COMM', 'COMM ADJ')       
        inner join  Insurance_File I  on TEF.insurance_file_Cnt = I.insurance_File_cnt      
       inner join Risk_code RC  on I.risk_Code_id = RC.risk_Code_id    
       
      
    WHERE TEF.effective_date >= @start_date      
    AND TEF.effective_date <= @end_date      
    AND (TEF.source_id = @branch_id      
         OR @branch_id = 0)      
    AND TEF.Accounts_Export_status = 'c'      
   
/*oLD QUERY bY ssp

SELECT RC.description Risk,  
    TEF.insurance_file_Cnt Insurance_cnt,  
    TED.transaction_amount Gross_Amount,  
    TED.taxes_total Taxes,  
    TED2.transaction_amount Commission  
  
INTO #TempTable1  
  
    FROM Transaction_Export_folder TEF,  
         Transaction_Export_detail TED,  
         Transaction_Export_detail TED2,  
         Insurance_File I,  
         Risk_code RC  
  
    WHERE TEF.effective_date >= @start_date  
    AND TEF.effective_date <= @end_date  
    AND (TEF.source_id = @branch_id  
         OR @branch_id = 0)  
    AND TEF.Accounts_Export_status = 'c'  
    AND TEF.transaction_export_folder_cnt *= TED.transaction_export_folder_cnt  
    AND TEF.transaction_export_folder_cnt *= TED2.transaction_export_folder_cnt  
    AND TED.spare in ('GROSS')  
    AND TED2.spare in ('COMM', 'COMM ADJ')  
    AND TEF.insurance_file_Cnt = I.insurance_File_cnt  
    AND I.risk_Code_id = RC.risk_Code_id  

*/   
SELECT Risk, count(distinct Insurance_cnt) 'Policies', (sum(gross_amount)+ sum(taxes)) * -1 'Premium', sum(commission) 'Commission'      
      
   FROM #TempTable1 group by risk      
      
DROP TABLE #TempTable1      
    
    
    
    
    