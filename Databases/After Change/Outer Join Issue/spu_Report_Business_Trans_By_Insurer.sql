SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Report_Business_Trans_By_Insurer'
GO 

-----spu_Report_Business_Trans_By_Insurer 2,'01/01/2007','01/01/2009'
---No record return by this proc to test before alter and after alter


CREATE PROCEDURE spu_Report_Business_Trans_By_Insurer  
    @branch_id int,  
    @start_date datetime,  
    @end_date datetime  
AS  
  
------------------------------------------------  
-- Edit History : 1  
-- Modified BY  : Ram Chandrabose  
-- Date     : 12/01/2001  
-- Description  : Added code to get the Account Executive  
------------------------------------------------  
DECLARE @iBranchID  int  

----Begin New Code By Kuldeep panwar NIIT
  
SELECT  Ins.shortname           Insurer,  
    ISNULL(Acc.resolved_name, '')   Account_Handler,  
    R.description           Risk,  
    D.document_date         Transaction_Date,  
    D.document_ref              Document_ref,  
    TD.accounting_date      Effective_Date,  
    Cli.name            Client,  
    I.insurance_ref         Policy_No,  
    ISNULL  
    (  
        (  
        SELECT  SUM(amount)  
        FROM    Transdetail  
        WHERE   document_id = D.document_id  
        AND account_id = A.account_id  
        AND (  
            spare = 'GROSS'  
            OR  
            spare = 'Reversed GROSS'  
            )  
        )  
    , 0)                Gross_Premium,  
    ISNULL  
    (  
        (  
        SELECT  SUM(amount)  
        FROM    Transdetail  
        WHERE   document_id = D.document_id  
        AND account_id = A.account_id  
        AND (  
            spare = 'COMM'  
            OR  
            spare = 'COMM ADJ'  
            OR  
            spare = 'Reversed COMM'  
            )  
        )  
    , 0)                Commission,  
    T.description           Document_Type,  
    T.code              Trans_Code,  
    ISNULL  
    (  
        (  
        SELECT AE.resolved_name  
        FROM   Party    AE  
        WHERE  Cli.Consultant_cnt = AE.Party_cnt  
        )  
    , '')               Account_Executive  

FROM    Insurance_File I inner join  Party Ins on Ins.party_cnt = I.lead_insurer_cnt  
		inner join Account A on A.account_key = Ins.party_cnt 
		left outer join  Party Acc on Acc.party_cnt = I.account_handler_cnt
		inner join  Insurance_Folder F on F.insurance_folder_cnt = I.insurance_folder_cnt
		inner join  Party Cli on Cli.party_cnt = F.insurance_holder_cnt
		inner join  Risk_Code R on R.risk_code_id = I.risk_code_id
		inner join  Document  D on D.insurance_file_cnt = i.insurance_file_cnt
		inner join DocumentType T on T.documenttype_id = D.documenttype_id
		inner join Transdetail  TD on TD.document_id = D.document_id
		
          
    
WHERE   (  
    D.document_date >= '01/01/2007'  
    AND D.document_date <= '01/01/2009'
    )  
AND TD.document_sequence = 1  
AND     D.documenttype_id IN(4, 5, 15, 16, 17, 18, 31, 32, 35, 36)  
AND (  
        2 = 0  
        OR  
        (  
            2 <> 0  
            AND  
            I.source_id = 2  
        )  
    )  
    
    
    UNION 
    
     /* Include Reversals */ 
SELECT  Ins.shortname           Insurer,  
    ISNULL(Acc.resolved_name, '')   Account_Handler,  
    R.description           Risk,  
    D.document_date         Transaction_Date,  
    D.document_ref              Document_ref,  
    TD.accounting_date      Effective_Date,  
    Cli.name            Client,  
    I.insurance_ref         Policy_No,  
    ISNULL  
    (  
        (  
        SELECT  SUM(amount)  
        FROM    Transdetail  
        WHERE   document_id = D1.document_id  
        AND account_id = A.account_id  
        AND (  
            spare = 'GROSS'  
            OR  
            spare = 'Reversed GROSS'  
            )  
        )  
    , 0)                Gross_Premium,  
    ISNULL  
    (  
        (  
        SELECT  SUM(amount)  
        FROM    Transdetail  
        WHERE   document_id = D1.document_id  
        AND account_id = A.account_id  
        AND (  
            spare = 'COMM'  
            OR  
            spare = 'COMM ADJ'  
            OR  
            spare = 'Reversed COMM'  
            )  
        )  
    , 0)                Commission,  
    T.description           Document_Type,  
    T.code              Trans_Code,  
    ISNULL  
    (  
        (  
        SELECT AE.resolved_name  
        FROM   Party    AE  
        WHERE  Cli.Consultant_cnt = AE.Party_cnt  
        )  
    , '')               Account_Executive  
FROM   
Party Ins inner join Insurance_File I on Ins.party_cnt = I.lead_insurer_cnt
		  inner join  Account A on A.account_key = Ins.party_cnt
		  LEFT outer join Party Acc on  Acc.party_cnt = I.account_handler_cnt 
		  inner join Insurance_Folder F on F.insurance_folder_cnt = I.insurance_folder_cnt
		  inner join  Party Cli on Cli.party_cnt = F.insurance_holder_cnt
		  inner join Risk_Code R on R.risk_code_id = I.risk_code_id
		  inner join Document D1 on D1.insurance_file_cnt = I.insurance_file_cnt
		  inner join Document D on  D.company_id = i.source_id  and D.comment LIKE 'Reversal%'
		  inner join Transdetail   TD on TD.document_id = D.document_id  
		  inner join DocumentType T on T.documenttype_id = D.documenttype_id

    
WHERE   
 (  
    D.document_date >= '01/01/2007'  
    AND D.document_date <= '01/01/2009'  
    )  

AND TD.document_sequence = 1  
AND     D.documenttype_id IN(4, 5, 15, 16, 17, 18, 31, 32, 35, 36)  
AND (  
        2 = 0  
        OR  
        (  
            2 <> 0  
            AND  
            I.source_id = 2  
        )  
    )  
  
/* Include Extras */  
UNION 
 
SELECT  Ins.shortname           Insurer,  
    ISNULL(Acc.resolved_name, '')   Account_Handler,  
    R.description           Risk,  
    D.document_date         Transaction_Date,  
    D.document_ref              Document_ref,  
    TD.accounting_date      Effective_Date,  
    Cli.name            Client,  
    I.insurance_ref         Policy_No,  
    ISNULL  
    (  
        (  
        SELECT  SUM(amount)  
        FROM    Transdetail  
        WHERE   document_id = D.document_id  
        AND account_id = A.account_id  
        AND (  
            spare = 'GROSS'  
            OR  
            spare = 'Reversed GROSS'  
            )  
        )  
    , 0)                Gross_Premium,  
    ISNULL  
    (  
        (  
        SELECT  SUM(amount)  
        FROM    Transdetail  
        WHERE   document_id = D.document_id  
        AND account_id = A.account_id  
        AND (  
            spare = 'COMM'  
            OR  
            spare = 'COMM ADJ'  
            OR  
            spare = 'Reversed COMM'  
            )  
        )  
    , 0)                Commission,  
    T.description           Document_Type,  
    T.code              Trans_Code,  
    ISNULL  
    (  
        (  
        SELECT AE.resolved_name  
        FROM   Party    AE  
        WHERE  Cli.Consultant_cnt = AE.Party_cnt  
        )  
    , '') 
                  Account_Executive  
FROM   Account A inner join Party Ins on A.account_key = Ins.party_cnt
	inner join Transdetail   TD on TD.account_id = A.account_id 
	, Party Acc right outer join Insurance_File I on  Acc.party_cnt = I.account_handler_cnt 
	inner join Insurance_Folder F on F.insurance_folder_cnt = I.insurance_folder_cnt
	inner join Party Cli on Cli.party_cnt = F.insurance_holder_cnt
	inner join Risk_Code R on R.risk_code_id = I.risk_code_id
	inner join Document  D on D.insurance_file_cnt = I.insurance_file_cnt
	inner join DocumentType T on T.documenttype_id = D.documenttype_id
	--inner join Transdetail TD1 on TD1.document_id = D.document_id 

WHERE   Ins.party_type_id = 10  
 
AND (  
   D.document_date >= '01/01/2007'  
    AND D.document_date <= '01/01/2009'  
    )  

AND     D.documenttype_id IN(4, 5, 15, 16, 17, 18, 31, 32, 35, 36)  
AND (  
        2 = 0  
        OR  
        (  
            2 <> 0  
            AND  
            I.source_id = 2  
        )  
    )  



----End New Code by KUldeep Panwar NIIT
 -- /*Old Code By SSP Before NIIT
/*SELECT  @iBranchID = ISNULL(@branch_id, 0)  
  
SELECT  Ins.shortname           Insurer,  
    ISNULL(Acc.resolved_name, '')   Account_Handler,  
    R.description           Risk,  
    D.document_date         Transaction_Date,  
    D.document_ref              Document_ref,  
    TD.accounting_date      Effective_Date,  
    Cli.name            Client,  
    I.insurance_ref         Policy_No,  
    ISNULL  
    (  
        (  
        SELECT  SUM(amount)  
        FROM    Transdetail  
        WHERE   document_id = D.document_id  
        AND account_id = A.account_id  
        AND (  
            spare = 'GROSS'  
            OR  
            spare = 'Reversed GROSS'  
            )  
        )  
    , 0)                Gross_Premium,  
    ISNULL  
    (  
        (  
        SELECT  SUM(amount)  
        FROM    Transdetail  
        WHERE   document_id = D.document_id  
        AND account_id = A.account_id  
        AND (  
            spare = 'COMM'  
            OR  
            spare = 'COMM ADJ'  
            OR  
            spare = 'Reversed COMM'  
            )  
        )  
    , 0)                Commission,  
    T.description           Document_Type,  
    T.code              Trans_Code,  
    ISNULL  
    (  
        (  
        SELECT AE.resolved_name  
        FROM   Party    AE  
        WHERE  Cli.Consultant_cnt = AE.Party_cnt  
        )  
    , '')               Account_Executive  
FROM    Insurance_File              I,  
    Insurance_Folder            F,  
    Risk_Code               R,  
    Party                   Ins,  
    Party                   Acc,  
    Party                   Cli,  
    --sj 31/07/2002 - start  
    --Transaction_Export_Folder       EF,  
    --sj 31/07/2002 - end  
    Document      D,  
    Transdetail   TD,  
    DocumentType  T,  
    Account       A  
WHERE   Ins.party_cnt = I.lead_insurer_cnt  
AND A.account_key = Ins.party_cnt  
AND Acc.party_cnt =* I.account_handler_cnt  
AND F.insurance_folder_cnt = I.insurance_folder_cnt  
AND Cli.party_cnt = F.insurance_holder_cnt  
AND R.risk_code_id = I.risk_code_id  
--sj 31/07/2002 - start  
AND D.insurance_file_cnt = i.insurance_file_cnt  
--AND EF.insurance_file_cnt = I.insurance_file_cnt  
--AND EF.accounts_export_status = 'c'  
--AND D.document_ref = EF.document_ref  
--AND D.company_id = EF.source_id  
--sj 31/07/2002 - end  
AND T.documenttype_id = D.documenttype_id	
AND (  
    D.document_date >= @start_date  
    AND D.document_date <= @end_date  
    )  
AND     TD.document_id = D.document_id  
AND TD.document_sequence = 1  
AND     D.documenttype_id IN(4, 5, 15, 16, 17, 18, 31, 32, 35, 36)  
AND (  
        @iBranchID = 0  
        OR  
        (  
            @iBranchID <> 0  
            AND  
            I.source_id = @iBranchID  
        )  
    )  
  
/* Include Reversals */  
UNION  
SELECT  Ins.shortname           Insurer,  
    ISNULL(Acc.resolved_name, '')   Account_Handler,  
    R.description           Risk,  
    D.document_date         Transaction_Date,  
    D.document_ref              Document_ref,  
    TD.accounting_date      Effective_Date,  
    Cli.name            Client,  
    I.insurance_ref         Policy_No,  
    ISNULL  
    (  
        (  
        SELECT  SUM(amount)  
        FROM    Transdetail  
        WHERE   document_id = D1.document_id  
        AND account_id = A.account_id  
        AND (  
            spare = 'GROSS'  
            OR  
            spare = 'Reversed GROSS'  
            )  
        )  
    , 0)                Gross_Premium,  
    ISNULL  
    (  
        (  
        SELECT  SUM(amount)  
        FROM    Transdetail  
        WHERE   document_id = D1.document_id  
        AND account_id = A.account_id  
        AND (  
            spare = 'COMM'  
            OR  
            spare = 'COMM ADJ'  
            OR  
            spare = 'Reversed COMM'  
            )  
        )  
    , 0)                Commission,  
    T.description           Document_Type,  
    T.code              Trans_Code,  
    ISNULL  
    (  
        (  
        SELECT AE.resolved_name  
        FROM   Party    AE  
        WHERE  Cli.Consultant_cnt = AE.Party_cnt  
        )  
    , '')               Account_Executive  
FROM    Insurance_File              I,  
    Insurance_Folder            F,  
    Risk_Code               R,  
    Party                   Ins,  
    Party                   Acc,  
    Party                   Cli,  
    --sj 31/07/2002 - start  
    --Transaction_Export_Folder       EF,  
    --sj 31/07/2002 - end  
    Document      D,  
    Transdetail   TD,  
    DocumentType  T,  
    Account       A,  
    Document      D1  
WHERE   Ins.party_cnt = I.lead_insurer_cnt  
  
AND A.account_key = Ins.party_cnt  
AND Acc.party_cnt =* I.account_handler_cnt  
AND F.insurance_folder_cnt = I.insurance_folder_cnt  
AND Cli.party_cnt = F.insurance_holder_cnt  
AND R.risk_code_id = I.risk_code_id  
--sj 31/07/2002 - start  
--AND EF.insurance_file_cnt = I.insurance_file_cnt  
--AND EF.accounts_export_status = 'c'  
--AND D1.document_ref = EF.document_ref  
--AND D1.company_id = EF.source_id  
AND D1.insurance_file_cnt = I.insurance_file_cnt  
--sj 31/07/2002 - end  
AND D.comment LIKE 'Reversal%'  
--sj 31/07/2002 - start  
--AND D.company_id = EF.source_id  
AND D.company_id = i.source_id  
--sj 31/07/2002 - start  
AND T.documenttype_id = D.documenttype_id  
AND (  
    D.document_date >= @start_date  
    AND D.document_date <= @end_date  
    )  
AND     TD.document_id = D.document_id  
AND TD.document_sequence = 1  
AND     D.documenttype_id IN(4, 5, 15, 16, 17, 18, 31, 32, 35, 36)  
AND (  
        @iBranchID = 0  
        OR  
        (  
            @iBranchID <> 0  
            AND  
            I.source_id = @iBranchID  
        )  
    )  
  
/* Include Extras */  
UNION  
SELECT  Ins.shortname           Insurer,  
    ISNULL(Acc.resolved_name, '')   Account_Handler,  
    R.description           Risk,  
    D.document_date         Transaction_Date,  
    D.document_ref              Document_ref,  
    TD.accounting_date      Effective_Date,  
    Cli.name            Client,  
    I.insurance_ref         Policy_No,  
    ISNULL  
    (  
        (  
        SELECT  SUM(amount)  
        FROM    Transdetail  
        WHERE   document_id = D.document_id  
        AND account_id = A.account_id  
        AND (  
            spare = 'GROSS'  
            OR  
            spare = 'Reversed GROSS'  
            )  
        )  
    , 0)                Gross_Premium,  
    ISNULL  
    (  
        (  
        SELECT  SUM(amount)  
        FROM    Transdetail  
        WHERE   document_id = D.document_id  
        AND account_id = A.account_id  
        AND (  
            spare = 'COMM'  
            OR  
            spare = 'COMM ADJ'  
            OR  
            spare = 'Reversed COMM'  
            )  
        )  
    , 0)                Commission,  
    T.description           Document_Type,  
    T.code              Trans_Code,  
    ISNULL  
    (  
        (  
        SELECT AE.resolved_name  
        FROM   Party    AE  
        WHERE  Cli.Consultant_cnt = AE.Party_cnt  
        )  
    , '')               Account_Executive  
FROM    Insurance_File              I,  
    Insurance_Folder            F,  
    Risk_Code               R,  
    Party                   Ins,  
    Party                   Acc,  
    Party                   Cli,  
    --sj 31/07/2002 - Start  
    --Transaction_Export_Folder       EF,  
    ----sj 31/07/2002 - End  
    Document      D,  
    Transdetail   TD,  
    DocumentType  T,  
    Account       A  
WHERE   Ins.party_type_id = 10  
AND A.account_key = Ins.party_cnt  
AND TD.account_id = A.account_id  
AND Acc.party_cnt =* I.account_handler_cnt  
AND F.insurance_folder_cnt = I.insurance_folder_cnt  
AND Cli.party_cnt = F.insurance_holder_cnt  
AND R.risk_code_id = I.risk_code_id  
--sj 31/07/2002 - Start  
AND D.insurance_file_cnt = I.insurance_file_cnt  
--AND EF.insurance_file_cnt = I.insurance_file_cnt  
--AND EF.accounts_export_status = 'c'  
--AND D.document_ref = EF.document_ref  
--AND D.company_id = EF.source_id  
--sj 31/07/2002 - End  
AND T.documenttype_id = D.documenttype_id  
AND (  
    D.document_date >= @start_date  
    AND D.document_date <= @end_date  
    )  
AND     TD.document_id = D.document_id  
AND     D.documenttype_id IN(4, 5, 15, 16, 17, 18, 31, 32, 35, 36)  
AND (  
        @iBranchID = 0  
        OR  
        (  
            @iBranchID <> 0  
            AND  
            I.source_id = @iBranchID  
        )  
    )  
*/