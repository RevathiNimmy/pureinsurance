SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Report_Business_Summary_By_Transaction_Type'
GO 
---spu_Report_Business_Summary_By_Transaction_Type '2','01/01/2008','12/12/2009'

------------------------------------------------  
-- Edit History : 1  
-- Created by : Elaine Knott  
-- Date : 14/05/2002  
-- Description : Summary of Business By Transaction Type  
------------------------------------------------  
CREATE PROCEDURE spu_Report_Business_Summary_By_Transaction_Type  
    @branch_id int,  
    @start_date datetime,  
    @end_date datetime  
AS  
  
DECLARE @iBranchID int  
  
SELECT @iBranchID = ISNULL(@branch_id, 0)  
  
CREATE TABLE #Business_Temp  
    (  
        transaction_type varchar(255),  
        gross_amount numeric(19, 4) NULL,  
        comm_amount numeric(19, 4) NULL  
    )  
  
INSERT INTO #Business_Temp 

---New Query By Kuldeep Panwar on 11-01-2010 -NIIT
SELECT 'New Business'--,t.amount  
    ,ISNULL(SUM(ISNULL(t.amount, 0)), 0)  
  ,ISNULL(SUM(ISNULL(t2.amount, 0)) + SUM(ISNULL(t3.amount, 0)), 0)  
  
    FROM  
    Transdetail t inner join Document d on t.document_id = d.document_id    
    AND (t.spare = '' OR t.spare is null) 
    left outer join Transdetail t2 on t2.document_id = t.document_id and t2.document_id = d.document_id  
    AND t2.spare = 'COMM' 
    left outer join Transdetail t3 on  t3.document_id = t.document_id and t3.document_id = d.document_id  
    AND t3.spare = 'COMM ADJ'      
    inner join Account a on  t.account_id = a.account_id   
 
    WHERE  
    a.ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'SA') --PSL 16/06/2003 Iss4707 Remove Hard coded ledger_id  
    
    AND d.documenttype_id IN(4, 5)  
    AND (@iBranchID = 0  
            OR  
            ( @iBranchID <> 0  
                AND  
                D.company_id = @iBranchID  
            )  
        )  
    AND (D.document_date >= @start_date  
        AND  
        D.document_date <= @end_date 
        )  
   
    /* Old query By SSP
    SELECT 'New Business',  
    ISNULL(SUM(ISNULL(t.amount, 0)), 0),  
    ISNULL(SUM(ISNULL(t2.amount, 0)) + SUM(ISNULL(t3.amount, 0)), 0)  
  
    FROM  
    Transdetail t,  
    Transdetail t2,  
    Transdetail t3,  
    Document d,  
    Account a  
  
    WHERE t.document_id = d.document_id  
    AND t2.document_id =* t.document_id  
    AND t2.document_id =* d.document_id  
    AND t3.document_id =* t.document_id  
    AND t3.document_id =* d.document_id  
    AND (t.spare = '' OR t.spare is null)  
    AND t.account_id = a.account_id  
    AND a.ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'SA') --PSL 16/06/2003 Iss4707 Remove Hard coded ledger_id  
    AND t2.spare = 'COMM'  
    AND t3.spare = 'COMM ADJ'  
    AND d.documenttype_id IN(4, 5)  
    AND (@iBranchID = 0  
            OR  
            ( @iBranchID <> 0  
                AND  
                D.company_id = @iBranchID  
            )  
        )  
    AND (D.document_date >= @start_date  
        AND  
        D.document_date <= @end_date  
        )  
  */
  
INSERT INTO #Business_Temp  
--New Query By Kuldeep Panwar NIIT
SELECT 'Renewals',  
    ISNULL(SUM(ISNULL(t.amount, 0)), 0),  
    ISNULL(SUM(ISNULL(t2.amount, 0)) + SUM(ISNULL(t3.amount, 0)), 0)  
  
    FROM  
     Transdetail t inner join Document d on t.document_id = d.document_id    
    AND (t.spare = '' OR t.spare is null) 
    left outer join Transdetail t2 on t2.document_id = t.document_id and t2.document_id = d.document_id  
    AND t2.spare = 'COMM' 
    left outer join Transdetail t3 on  t3.document_id = t.document_id and t3.document_id = d.document_id  
    AND t3.spare = 'COMM ADJ'      
    inner join Account a on  t.account_id = a.account_id     
  
    WHERE  
     a.ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'SA') --PSL 16/06/2003 Iss4707 Remove Hard coded ledger_id  
     AND d.documenttype_id IN(15, 16)  
    AND (@iBranchID = 0  
            OR  
            ( @iBranchID <> 0  
                AND  
                D.company_id = @iBranchID  
            )  
        )  
    AND (D.document_date >= @start_date  
        AND  
        D.document_date <= @end_date  
        )  

    ---Old Query From SSP
    /*SELECT 'Renewals',  
    ISNULL(SUM(ISNULL(t.amount, 0)), 0),  
    ISNULL(SUM(ISNULL(t2.amount, 0)) + SUM(ISNULL(t3.amount, 0)), 0)  
  
    FROM  
    Transdetail t,  
    Transdetail t2,  
    Transdetail t3,  
    Document d,  
    Account a  
  
    WHERE t.document_id = d.document_id  
    AND t2.document_id =* t.document_id  
    AND t2.document_id =* d.document_id  
    AND t3.document_id =* t.document_id  
    AND t3.document_id =* d.document_id  
    AND (t.spare = '' OR t.spare is null)  
    AND t.account_id = a.account_id  
    AND a.ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'SA') --PSL 16/06/2003 Iss4707 Remove Hard coded ledger_id  
    AND t2.spare = 'COMM'  
    AND t3.spare = 'COMM ADJ'  
    AND d.documenttype_id IN(15, 16)  
    AND (@iBranchID = 0  
            OR  
            ( @iBranchID <> 0  
                AND  
                D.company_id = @iBranchID  
            )  
        )  
    AND (D.document_date >= @start_date  
        AND  
        D.document_date <= @end_date  
        )  
  */
  
INSERT INTO #Business_Temp  
   --New Query By Kuldeep Panwar NIIT
    SELECT 'Adjustments',  
    ISNULL(SUM(ISNULL(t.amount, 0)), 0),  
    ISNULL(SUM(ISNULL(t2.amount, 0)) + SUM(ISNULL(t3.amount, 0)), 0)  
  
    FROM  
    Transdetail t inner join Document d on t.document_id = d.document_id    
    AND (t.spare = '' OR t.spare is null) 
    left outer join Transdetail t2 on t2.document_id = t.document_id and t2.document_id = d.document_id  
    AND t2.spare = 'COMM' 
    left outer join Transdetail t3 on  t3.document_id = t.document_id and t3.document_id = d.document_id  
    AND t3.spare = 'COMM ADJ'      
    inner join Account a on  t.account_id = a.account_id   
  
    WHERE  
     a.ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'SA') --PSL 16/06/2003 Iss4707 Remove Hard coded ledger_id  
    AND d.documenttype_id IN(17, 18)  
    AND (@iBranchID = 0  
            OR  
            ( @iBranchID <> 0  
                AND  
                D.company_id = @iBranchID  
            )  
        )  
    AND (D.document_date >= @start_date  
        AND  
        D.document_date <= @end_date  
        )  
   
   /*Old Query By SSP
    SELECT 'Adjustments',  
    ISNULL(SUM(ISNULL(t.amount, 0)), 0),  
    ISNULL(SUM(ISNULL(t2.amount, 0)) + SUM(ISNULL(t3.amount, 0)), 0)  
  
    FROM  
    Transdetail t,  
    Transdetail t2,  
    Transdetail t3,  
    Document d,  
    Account a  
  
    WHERE t.document_id = d.document_id  
    AND t2.document_id =* t.document_id  
    AND t2.document_id =* d.document_id  
    AND t3.document_id =* t.document_id  
    AND t3.document_id =* d.document_id  
    AND (t.spare = '' OR t.spare is null)  
    AND t.account_id = a.account_id  
    AND a.ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'SA') --PSL 16/06/2003 Iss4707 Remove Hard coded ledger_id  
    AND t2.spare = 'COMM'  
    AND t3.spare = 'COMM ADJ'  
    AND d.documenttype_id IN(17, 18)  
    AND (@iBranchID = 0  
            OR  
            ( @iBranchID <> 0  
                AND  
                D.company_id = @iBranchID  
            )  
        )  
    AND (D.document_date >= @start_date  
        AND  
        D.document_date <= @end_date  
        )  
  */
INSERT INTO #Business_Temp  
    
    ----New Query By Kuldeep Panwar NIIT
    SELECT 'Short Term',  
    ISNULL(SUM(ISNULL(t.amount, 0)), 0),  
    ISNULL(SUM(ISNULL(t2.amount, 0)) + SUM(ISNULL(t3.amount, 0)), 0)  
  
    FROM  
    Transdetail t inner join Document d on t.document_id = d.document_id    
    AND (t.spare = '' OR t.spare is null) 
    left outer join Transdetail t2 on t2.document_id = t.document_id and t2.document_id = d.document_id  
    AND t2.spare = 'COMM' 
    left outer join Transdetail t3 on  t3.document_id = t.document_id and t3.document_id = d.document_id  
    AND t3.spare = 'COMM ADJ'      
    inner join Account a on  t.account_id = a.account_id  
  
    WHERE  
     a.ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'SA') --PSL 16/06/2003 Iss4707 Remove Hard coded ledger_id  
    AND d.documenttype_id IN(31, 32)  
    AND (@iBranchID = 0  
            OR  
            ( @iBranchID <> 0  
                AND  
                D.company_id = @iBranchID  
            )  
        )  
    AND (D.document_date >= @start_date  
        AND  
        D.document_date <= @end_date  
        ) 
    
    
    
    /*Old Query By SSP
    SELECT 'Short Term',  
    ISNULL(SUM(ISNULL(t.amount, 0)), 0),  
    ISNULL(SUM(ISNULL(t2.amount, 0)) + SUM(ISNULL(t3.amount, 0)), 0)  
  
    FROM  
    Transdetail t,  
    Transdetail t2,  
    Transdetail t3,  
    Document d,  
    Account a  
  
    WHERE t.document_id = d.document_id  
    AND t2.document_id =* t.document_id  
    AND t2.document_id =* d.document_id  
    AND t3.document_id =* t.document_id  
    AND t3.document_id =* d.document_id  
    AND (t.spare = '' OR t.spare is null)  
    AND t.account_id = a.account_id  
    AND a.ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'SA') --PSL 16/06/2003 Iss4707 Remove Hard coded ledger_id  
    AND t2.spare = 'COMM'  
    AND t3.spare = 'COMM ADJ'  
    AND d.documenttype_id IN(31, 32)  
    AND (@iBranchID = 0  
            OR  
            ( @iBranchID <> 0  
                AND  
                D.company_id = @iBranchID  
            )  
        )  
    AND (D.document_date >= @start_date  
        AND  
        D.document_date <= @end_date  
        )  
  */
INSERT INTO #Business_Temp  
SELECT 'Transfers',  
    ISNULL(SUM(ISNULL(t.amount, 0)), 0),  
    ISNULL(SUM(ISNULL(t2.amount, 0)) + SUM(ISNULL(t3.amount, 0)), 0)  
  
    FROM  
    Transdetail t inner join Document d on t.document_id = d.document_id    
    AND (t.spare = '' OR t.spare is null) 
    left outer join Transdetail t2 on t2.document_id = t.document_id and t2.document_id = d.document_id  
    AND t2.spare = 'COMM' 
    left outer join Transdetail t3 on  t3.document_id = t.document_id and t3.document_id = d.document_id  
    AND t3.spare = 'COMM ADJ'      
    inner join Account a on  t.account_id = a.account_id  
  
    WHERE 
     a.ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'SA') --PSL 16/06/2003 Iss4707 Remove Hard coded ledger_id  
    AND d.documenttype_id IN(35, 36)  
    AND (@iBranchID = 0  
            OR  
            ( @iBranchID <> 0  
                AND  
                D.company_id = @iBranchID  
            )  
        )  
    AND (D.document_date >= @start_date  
        AND  
        D.document_date <= @end_date  
        )  

   /* Old Query By SSP
    SELECT 'Transfers',  
    ISNULL(SUM(ISNULL(t.amount, 0)), 0),  
    ISNULL(SUM(ISNULL(t2.amount, 0)) + SUM(ISNULL(t3.amount, 0)), 0)  
  
    FROM  
    Transdetail t,  
    Transdetail t2,  
    Transdetail t3,  
    Document d,  
    Account a  
  
    WHERE t.document_id = d.document_id  
    AND t2.document_id =* t.document_id  
    AND t2.document_id =* d.document_id  
    AND t3.document_id =* t.document_id  
    AND t3.document_id =* d.document_id  
    AND (t.spare = '' OR t.spare is null)  
    AND t.account_id = a.account_id  
    AND a.ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'SA') --PSL 16/06/2003 Iss4707 Remove Hard coded ledger_id  
    AND t2.spare = 'COMM'  
    AND t3.spare = 'COMM ADJ'  
    AND d.documenttype_id IN(35, 36)  
    AND (@iBranchID = 0  
            OR  
            ( @iBranchID <> 0  
                AND  
                D.company_id = @iBranchID  
            )  
        )  
    AND (D.document_date >= @start_date  
        AND  
        D.document_date <= @end_date  
        )  
        */
--DC020603 -ISS3990 -added extra check to only get data if there is values anywhere  
SELECT * FROM #Business_Temp WHERE EXISTS (SELECT * FROM #Business_Temp WHERE gross_amount <> 0 OR comm_amount <> 0)  
  
DROP TABLE #Business_Temp  
