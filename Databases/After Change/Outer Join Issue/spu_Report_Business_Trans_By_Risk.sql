SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Report_Business_Trans_By_Risk'
GO 

  --spu_Report_Business_Trans_By_Risk 2,'01/01/2007','01/01/2009'
  
CREATE procedure spu_Report_Business_Trans_By_Risk  
    @branch_id int,  
    @start_date datetime,  
    @end_date datetime  
AS  
  
DECLARE @iBranchID  int  
  
  
  
  

SELECT  @iBranchID = ISNULL(@branch_id, 0)  
  
IF  @iBranchID = 0  

BEGIN  
  
  -----Begin  New COde by Kuldeep Panwar NIIT
  
  SELECT  Ins.shortname           Insurer,  
        ISNULL(Acc.resolved_name, '')   Account_Handler,  
        R.description           Risk,  
        D.document_date         Transaction_Date,  
        D.document_ref              Document_ref,  
        TD.accounting_date      Effective_Date,  
        Cli.shortname           Client_Code,  
        Cli.resolved_name       Client_Name,  
        I.insurance_ref         Policy_No,  
        TD.amount           Gross_Premium,  
        (SELECT SUM(amount)  
        FROM    Transdetail  
        WHERE   document_id = D.document_id  
        AND spare = 'COMM')     Commission,  
        T.description           Document_Type,  
        T.code              Trans_Code,  
        (SELECT AE.resolved_name  
         FROM   Party   AE  
         WHERE  Cli.Consultant_cnt = AE.Party_cnt) Account_Executive  
    FROM   Party Ins inner join Insurance_File I on Ins.party_cnt = I.lead_insurer_cnt
		   left outer join Party Acc on I.account_handler_cnt=Acc.party_cnt
		   inner join Insurance_Folder F on F.insurance_folder_cnt = I.insurance_folder_cnt
		   inner join Party Cli on Cli.party_cnt = F.insurance_holder_cnt
		   inner join Risk_Code R on R.risk_code_id = I.risk_code_id
		   inner join Document D on D.insurance_file_cnt = I.insurance_file_cnt
		   inner join DocumentType  T on T.documenttype_id = D.documenttype_id
		   inner join Transdetail TD on TD.document_id = D.document_id
		   inner join Account A	 on A.account_id = TD.account_id  and A.account_key = Cli.party_cnt 
    
    WHERE   (  
        D.document_date >= @start_date  
        AND D.document_date <= @end_date  
        )  
     
    AND A.ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'SA') --PSL 16/06/2003 Iss4707 Remove Hard coded ledger_id  
    AND     D.documenttype_id IN(4, 5, 15, 16, 17, 18, 31, 32, 35, 36)  
  
  ----End New Code By Kuldeep Panwar
    
    /*
    Old Code By SSP before NIIT
    SELECT  Ins.shortname           Insurer,  
        ISNULL(Acc.resolved_name, '')   Account_Handler,  
        R.description           Risk,  
        D.document_date         Transaction_Date,  
        D.document_ref              Document_ref,  
        TD.accounting_date      Effective_Date,  
        Cli.shortname           Client_Code,  
        Cli.resolved_name       Client_Name,  
        I.insurance_ref         Policy_No,  
        TD.amount           Gross_Premium,  
        (SELECT SUM(amount)  
        FROM    Transdetail  
        WHERE   document_id = D.document_id  
        AND spare = 'COMM')     Commission,  
        T.description           Document_Type,  
        T.code              Trans_Code,  
        (SELECT AE.resolved_name  
         FROM   Party   AE  
         WHERE  Cli.Consultant_cnt = AE.Party_cnt) Account_Executive  
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
        Transdetail   TD,  
        DocumentType  T,  
        Account       A  
    WHERE   Ins.party_cnt = I.lead_insurer_cnt  
    AND Acc.party_cnt =* I.account_handler_cnt  
    AND F.insurance_folder_cnt = I.insurance_folder_cnt  
    AND Cli.party_cnt = F.insurance_holder_cnt  
    AND R.risk_code_id = I.risk_code_id  
    --sj 31/07/2002 - Start  
    AND D.insurance_file_cnt = I.insurance_file_cnt  
    --AND EF.insurance_file_cnt = I.insurance_file_cnt  
    --AND EF.accounts_export_status = 'c'  
    --AND D.document_ref = EF.document_ref  
    --sj 31/07/2002 - end  
    AND T.documenttype_id = D.documenttype_id  
    AND (  
        D.document_date >= @start_date  
        AND D.document_date <= @end_date  
        )  
    AND     TD.document_id = D.document_id  
--  AND TD.document_sequence = 1  
    AND A.account_id = TD.account_id  
    AND A.account_key = Cli.party_cnt  
    AND A.ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'SA') --PSL 16/06/2003 Iss4707 Remove Hard coded ledger_id  
    AND     D.documenttype_id IN(4, 5, 15, 16, 17, 18, 31, 32, 35, 36)  
   */
END  
ELSE  
BEGIN  

 SELECT  Ins.shortname           Insurer,  
        ISNULL(Acc.resolved_name, '')   Account_Handler,  
        R.description           Risk,  
        D.document_date         Transaction_Date,  
        D.document_ref              Document_ref,  
        TD.accounting_date      Effective_Date,  
        Cli.shortname           Client_Code,  
        Cli.resolved_name       Client_Name,  
        I.insurance_ref         Policy_No,  
        (SELECT SUM(amount)  
        FROM    Transdetail  
        WHERE   document_id = D.document_id  
        AND spare = 'GROSS')    Gross_Premium,  
        (SELECT SUM(amount)  
        FROM    Transdetail  
        WHERE   document_id = D.document_id  
        AND spare = 'COMM')     Commission,  
        T.description           Document_Type,  
        T.code              Trans_Code,  
        (SELECT AE.resolved_name  
         FROM   Party   AE  
         WHERE  Cli.Consultant_cnt = AE.Party_cnt) Account_Executive  
    FROM    
		Party Ins inner join Insurance_File I on Ins.party_cnt = I.lead_insurer_cnt
		left outer join Party Acc on Acc.party_cnt = I.account_handler_cnt
		inner join Insurance_Folder F on F.insurance_folder_cnt = I.insurance_folder_cnt
		inner join Party Cli on Cli.party_cnt = F.insurance_holder_cnt
		inner join Risk_Code R on R.risk_code_id = I.risk_code_id
		inner join Document D on D.insurance_file_cnt = I.insurance_file_cnt
		inner join DocumentType  T on T.documenttype_id = D.documenttype_id
		inner join Transdetail TD on TD.document_id = D.document_id
		inner join Account A on A.account_id = TD.account_id  
							AND A.account_key = Cli.party_cnt 
    
    
    WHERE    (  
        D.document_date >= @start_date  
        AND D.document_date <= @end_date  
        )  
    
    AND A.ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'SA') --PSL 16/06/2003 Iss4707 Remove Hard coded ledger_id  
    AND     D.documenttype_id IN(4, 5, 15, 16, 17, 18, 31, 32, 35, 36)  
    AND I.source_id = @iBranchID  


/* Old Code by SSP before Niit
    SELECT  Ins.shortname           Insurer,  
        ISNULL(Acc.resolved_name, '')   Account_Handler,  
        R.description           Risk,  
        D.document_date         Transaction_Date,  
        D.document_ref              Document_ref,  
        TD.accounting_date      Effective_Date,  
        Cli.shortname           Client_Code,  
        Cli.resolved_name       Client_Name,  
        I.insurance_ref         Policy_No,  
        (SELECT SUM(amount)  
        FROM    Transdetail  
        WHERE   document_id = D.document_id  
        AND spare = 'GROSS')    Gross_Premium,  
        (SELECT SUM(amount)  
        FROM    Transdetail  
        WHERE   document_id = D.document_id  
        AND spare = 'COMM')     Commission,  
        T.description           Document_Type,  
        T.code              Trans_Code,  
        (SELECT AE.resolved_name  
         FROM   Party   AE  
         WHERE  Cli.Consultant_cnt = AE.Party_cnt) Account_Executive  
    FROM    Insurance_File              I,  
        Insurance_Folder            F,  
        Risk_Code               R,  
        Party                   Ins,  
        Party                   Acc,  
        Party                   Cli,  
        --sj 31/07/2002 - Start  
        --Transaction_Export_Folder       EF,  
        --sj 31/07/2002 - end  
        Document      D,  
        Transdetail   TD,  
        DocumentType  T,  
        Account       A  
    WHERE   Ins.party_cnt = I.lead_insurer_cnt  
    AND Acc.party_cnt =* I.account_handler_cnt  
    AND F.insurance_folder_cnt = I.insurance_folder_cnt  
    AND Cli.party_cnt = F.insurance_holder_cnt  
    AND R.risk_code_id = I.risk_code_id  
    --sj 31/07/2002 - Start  
    AND D.insurance_file_cnt = I.insurance_file_cnt  
    --AND EF.insurance_file_cnt = I.insurance_file_cnt  
    --AND EF.accounts_export_status = 'c'  
    --AND D.document_ref = EF.document_ref  
    --sj 31/07/2002 - End  
    AND T.documenttype_id = D.documenttype_id  
    AND (  
        D.document_date >= @start_date  
        AND D.document_date <= @end_date  
        )  
    AND     TD.document_id = D.document_id  
--  AND TD.document_sequence = 1  
    AND A.account_id = TD.account_id  
    AND A.account_key = Cli.party_cnt  
    AND A.ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'SA') --PSL 16/06/2003 Iss4707 Remove Hard coded ledger_id  
    AND     D.documenttype_id IN(4, 5, 15, 16, 17, 18, 31, 32, 35, 36)  
    AND I.source_id = @iBranchID  
    
    */
END  
  