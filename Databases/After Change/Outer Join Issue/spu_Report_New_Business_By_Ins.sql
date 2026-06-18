SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Report_New_Business_By_Ins'
GO 

-----spu_Report_New_Business_By_Ins 0,'01/01/2007','01/01/2009' 

CREATE procedure spu_Report_New_Business_By_Ins  
    @branch_id int,  
    @start_date datetime,  
    @end_date datetime  
AS  
  
DECLARE @iBranchID  int  
  
SELECT  @iBranchID = ISNULL(@branch_id, 0)  
  
IF  @iBranchID = 0  
BEGIN  

----Begin new Code By Kuldeep panwar NIIT
 SELECT  Ins.shortname           Insurer,  
  
        ISNULL(Acc.resolved_name, '')   Account_Handler,  
        R.description           Risk,  
        D.document_date         Transaction_Date,  
                D.document_ref              Document_ref,  
        TD.accounting_date      Effective_Date,  
        Cli.name            Client,  
        I.insurance_ref         Policy_No,  
  
        I.this_premium          Gross_Premium,  
        I.commission_amount     Commission  
    FROM Party Ins inner join Insurance_File I on Ins.party_cnt = I.lead_insurer_cnt
        left outer join Party Acc on Acc.party_cnt = I.account_handler_cnt
		inner join Insurance_Folder F on F.insurance_folder_cnt = I.insurance_folder_cnt
		inner join Party Cli on Cli.party_cnt = F.insurance_holder_cnt
		inner join Risk_Code R on  R.risk_code_id = I.risk_code_id
		inner join Document D on D.Insurance_file_cnt = I.insurance_file_cnt
		inner join Transdetail   TD on  TD.document_id = D.document_id
    
    
    WHERE    
   
     D.documenttype_id IN (4,5)  
    AND (  
        D.document_date >= @start_date  
        AND D.document_date <= @end_date  
        )  
  
    
    AND TD.document_sequence = 1  

----End new Code By Kuldeep


  /* Old Code By SSP Before NIIT
  
    SELECT  Ins.shortname           Insurer,  
  
        ISNULL(Acc.resolved_name, '')   Account_Handler,  
        R.description           Risk,  
        D.document_date         Transaction_Date,  
                D.document_ref              Document_ref,  
        TD.accounting_date      Effective_Date,  
        Cli.name            Client,  
        I.insurance_ref         Policy_No,  
  
        I.this_premium          Gross_Premium,  
        I.commission_amount     Commission  
    FROM    Insurance_File              I,  
        Insurance_Folder            F,  
        Risk_Code               R,  
        Party                   Ins,  
        Party                   Acc,  
        Party                   Cli,  
        --sj 31/07/2002 - Start  
        --Transaction_Export_Folder       EF,  
        --sj 31/07/2002 - End  
        Document      D,  
        Transdetail   TD  
    WHERE   Ins.party_cnt = I.lead_insurer_cnt  
    AND Acc.party_cnt =* I.account_handler_cnt  
    AND F.insurance_folder_cnt = I.insurance_folder_cnt  
    AND Cli.party_cnt = F.insurance_holder_cnt  
    AND R.risk_code_id = I.risk_code_id  
    --sj 31/07/2002 - Start  
    AND D.Insurance_file_cnt = I.insurance_file_cnt  
    --AND EF.insurance_file_cnt = I.insurance_file_cnt  
    --AND EF.accounts_export_status = 'c'  
    --AND D.document_ref = EF.document_ref  
    --sj 31/07/2002 - End  
    AND D.documenttype_id IN (4,5)  
    AND (  
        D.document_date >= @start_date  
        AND D.document_date <= @end_date  
        )  
  
    AND     TD.document_id = D.document_id  
    AND TD.document_sequence = 1
    */
    
      
END  
ELSE  
BEGIN  
   
   ----Begin New Code BY KUldeep panwar NIIT
    SELECT  Ins.shortname           Insurer,  
        ISNULL(Acc.resolved_name, '')   Account_Handler,  
        R.description           Risk,  
        D.document_date         Transaction_Date,  
                D.document_ref              Document_ref,  
        TD.accounting_date      Effective_Date,  
        Cli.name            Client,  
        I.insurance_ref         Policy_No,  
        I.this_premium          Gross_Premium,  
        I.commission_amount     Commission  
    FROM    Party Ins inner join Insurance_File I on Ins.party_cnt = I.lead_insurer_cnt
			left outer Join Party  Acc on Acc.party_cnt = I.account_handler_cnt 
			inner join Insurance_Folder F on F.insurance_folder_cnt = I.insurance_folder_cnt
			inner join Party Cli on Cli.party_cnt = F.insurance_holder_cnt
			inner join Risk_Code R on R.risk_code_id = I.risk_code_id  
			inner join Document D on D.Insurance_file_cnt = I.insurance_file_cnt 
			inner join  Transdetail   TD on  TD.document_id = D.document_id  
       
    WHERE  D.documenttype_id IN (4,5)  
    AND (  
        D.document_date >= @start_date  
        AND D.document_date <= @end_date  
        )  
   
    AND TD.document_sequence = 1  
    AND I.source_id = @iBranchID  
   
   
   ----End New Code By kuldeep 
   
   /*Old Code By SSP Before NIIT
   
    SELECT  Ins.shortname           Insurer,  
        ISNULL(Acc.resolved_name, '')   Account_Handler,  
        R.description           Risk,  
        D.document_date         Transaction_Date,  
                D.document_ref              Document_ref,  
        TD.accounting_date      Effective_Date,  
        Cli.name            Client,  
        I.insurance_ref         Policy_No,  
        I.this_premium          Gross_Premium,  
        I.commission_amount     Commission  
    FROM    Insurance_File              I,  
        Insurance_Folder            F,  
        Risk_Code               R,  
        Party                   Ins,  
        Party                   Acc,  
        Party                   Cli,  
         --sj 31/07/2002 - Start  
        --Transaction_Export_Folder       EF,  
        --sj 31/07/2002 - End  
        Document      D,  
        Transdetail   TD  
    WHERE   Ins.party_cnt = I.lead_insurer_cnt  
    AND Acc.party_cnt =* I.account_handler_cnt  
    AND F.insurance_folder_cnt = I.insurance_folder_cnt  
    AND Cli.party_cnt = F.insurance_holder_cnt  
    AND R.risk_code_id = I.risk_code_id  
    --sj 31/07/2002 - Start  
    AND D.Insurance_file_cnt = I.insurance_file_cnt  
    --AND EF.insurance_file_cnt = I.insurance_file_cnt  
    --AND EF.accounts_export_status = 'c'  
    --AND D.document_ref = EF.document_ref  
    --sj 31/07/2002 - End  
    AND D.documenttype_id IN (4,5)  
    AND (  
        D.document_date >= @start_date  
        AND D.document_date <= @end_date  
        )  
    AND     TD.document_id = D.document_id  
    AND TD.document_sequence = 1  
    AND I.source_id = @iBranchID  
    */
    
END  
  