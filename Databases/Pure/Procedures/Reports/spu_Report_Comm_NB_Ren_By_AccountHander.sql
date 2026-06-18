SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure spu_Report_Comm_NB_Ren_By_AccountHander
GO

CREATE PROCEDURE spu_Report_Comm_NB_Ren_By_AccountHander
    @branch_id INT,  
    @start_date DATETIME,  
    @end_date DATETIME,  
    @account_type VARCHAR(10)  
AS  
BEGIN
  
    DECLARE
        @iBranchID INT,  
        @document_id INT,  
        @commission NUMERIC(19, 4)  
      
    SELECT @iBranchID = ISNULL(@branch_id, 0)  
      
    --mkw130603 START  
    DECLARE @InsuranceFileAccountExec VARCHAR(1)  
    SELECT @InsuranceFileAccountExec = value  
        FROM Hidden_Options  
        WHERE option_number = 40  
    IF ISNULL(@InsuranceFileAccountExec, '') = '' BEGIN  
        SELECT @InsuranceFileAccountExec = '0'  
    END  
    --mkw130603 END  
      
    CREATE TABLE #Report_Comm_NB_Ren_By_AccountHander  
        (  
        branch VARCHAR(255) NULL,  
        account_handler VARCHAR(255) NULL,  
        client VARCHAR(255) NULL,  
        client_premium NUMERIC(19, 4) NULL,  
        commission NUMERIC(19, 4) NULL,  
        insurer VARCHAR(255) NULL,  
        effective_date DATETIME NULL,  
        document_id INT NULL,  
        document_ref VARCHAR(30)  
        )  
    -- Select all Documents which have commission  
    DECLARE c_cursor CURSOR FAST_FORWARD FOR  
        SELECT DISTINCT(D.document_id),  
            Sum(T.amount)  
        FROM Transdetail T
            INNER JOIN Document D  
                ON D.document_id = T.document_id  
        WHERE (T.spare = 'COMM'  
            OR  
            T.spare = 'COMM ADJ'  
            )  
        AND (SELECT Sum(amount) FROM Transdetail  
                WHERE ( spare = 'COMM'  
                OR  
                     spare = 'COMM ADJ'  
                    )  
                and document_Id = D.document_id  
            ) <> 0  
        AND D.documenttype_id IN(4, 5, 15, 16)  
        AND (  
            D.document_date >= @start_date  
            AND  
            D.document_date <= @end_date  
            )  
        AND (  
                @iBranchID = 0  
                OR  
                (  
                    @iBranchID <> 0  
                    AND  
                    D.company_id = @iBranchID  
                )  
            )  
        Group by D.document_id  
      
    OPEN c_cursor  
      
    FETCH NEXT FROM c_cursor INTO @document_id,  
                    @commission  
      
    WHILE @@FETCH_STATUS = 0  
    BEGIN  
    /* Set up report data in temporary table */  
      
        INSERT INTO #Report_Comm_NB_Ren_By_AccountHander  
      
        (  
        branch,  
        account_handler,  
        client,  
        client_premium,  
        commission,  
        insurer,  
        effective_date,  
        document_id,  
        document_ref  
        )  
        SELECT  
            ISNULL(  
                (  
                SELECT C.description  
                FROM document D
                    INNER JOIN Company C  
                        ON D.company_id = C.company_id  
                WHERE D.document_id = @document_id  
                )  
            , 'No Branch'),  
            '',
            ISNULL(  
                (  
                SELECT P.resolved_name  
                FROM Transdetail T
                    INNER JOIN Account A
                        ON T.account_id = A.account_id  
                    INNER JOIN Party P  
                        ON A.Account_Key = P.party_cnt  
                WHERE  
                    @document_Id = T.document_id  
                    AND T.document_sequence = 1  
                    --AND A.ledger_id = 2  
                    AND A.ledger_id IN (
                                        SELECT
                                        ledger_id 
                                        FROM ledger WHERE
                                        ledger_short_name = 'SA'
                                       )  
                )  
            , ''),  
            ISNULL(  
                (  
                SELECT SUM(T.amount)  
                FROM Transdetail T
                    INNER JOIN Account A  
                        ON T.account_id = A.account_id  
                WHERE  
                    @document_Id = T.document_id  
                    AND (T.spare = '' OR T.spare is null)  
                    --AND A.ledger_id = 2  
                    AND A.ledger_id IN (
                                        SELECT
                                        ledger_id 
                                        FROM ledger WHERE
                                        ledger_short_name = 'SA'
                                       )   
                )  
            , 0),  
            @commission,  
            ISNULL(  
                (  
                SELECT P.resolved_name  
                FROM Transdetail T    
                    INNER JOIN Account A
                        ON T.account_id = A.account_id  
                    INNER JOIN Party P  
                        ON A.Account_Key = P.party_cnt  
                WHERE  
                    @document_Id = T.document_id  
                    AND T.transdetail_id =(
                                           SELECT MIN(transdetail_id) 
                                           FROM Transdetail  
                                           WHERE spare = 'GROSS'  
                                               AND document_id = @document_id
                                          )  
                    --AND A.ledger_id = 4  
                    AND A.ledger_id IN (
                                        SELECT
                                        ledger_id 
                                        FROM ledger WHERE
                                        ledger_short_name = 'in'
                                       )   
                )  
            , ''),  
            ISNULL(  
                (  
                SELECT T.accounting_date  
                FROM Transdetail T  
                WHERE  
                    @document_Id = T.document_id  
                    AND T.document_sequence = 1  
                )  
            , ''),  
            @document_id,  
            ISNULL(  
                (  
                SELECT D.document_ref  
                FROM Document D  
                WHERE  
                    @document_Id = D.document_id  
                )  
            , '')  
      
        FETCH NEXT FROM c_cursor INTO @document_id,  
                        @commission  
    END  
      
    CLOSE c_cursor  
    DEALLOCATE c_cursor  
      
    UPDATE #Report_Comm_NB_Ren_By_AccountHander  
      
        SET account_handler = ISNULL(  
                (  
                SELECT P.resolved_name  
                FROM Transdetail T
                    INNER JOIN Insurance_File I
                        ON T.insurance_ref = I.insurance_ref 
                    LEFT OUTER JOIN Party P  
                        ON I.account_handler_cnt = P.party_cnt  
                WHERE  
                    #Report_Comm_NB_Ren_By_AccountHander.document_id = T.document_id  
                    AND T.document_sequence = 1  
                    AND I.insurance_file_cnt = (
                                                SELECT MAX(insurance_file_cnt)  
                                                FROM insurance_file  
                                                WHERE insurance_ref = I.insurance_ref  
                                               )  
                )  
                , 'No Handler')  
        WHERE @account_type = 'Handler'  
      
    UPDATE #Report_Comm_NB_Ren_By_AccountHander  
      
        SET account_handler = ISNULL(  
                (  
                SELECT (
                        CASE @InsuranceFileAccountExec 
                            WHEN '1' 
                                THEN ISNULL(P3.resolved_name, '''') 
                            ELSE ISNULL(P.resolved_name, '''') 
                        END
                       )  --mkw130603  
                       --P.resolved_name --mkw130603  
                FROM 
                    Transdetail T  
                    INNER JOIN Insurance_File I
                        ON T.insurance_ref = I.insurance_ref  
                    INNER JOIN Party P2
                        ON I.insured_cnt = P2.party_cnt  
                    LEFT OUTER JOIN Party P
                        ON P2.consultant_cnt = P.party_cnt    
                    LEFT OUTER JOIN Party P3  --mkw130603  
                        ON I.account_executive_cnt = P3.party_cnt  --mkw130603  
                WHERE  
                    #Report_Comm_NB_Ren_By_AccountHander.document_id = T.document_id  
                    AND T.document_sequence = 1  
                    AND I.insurance_file_cnt = (
                                                SELECT MAX(insurance_file_cnt)  
                                                FROM insurance_file  
                                                WHERE insurance_ref = I.insurance_ref  
                                               )  
                )  
                , 'No Executive')  
        WHERE @account_type = 'Executive'  
      
    --Return required data  
    SELECT  
        branch,  
        account_handler,  
        client,  
        client_premium,  
        commission,  
        insurer,  
        effective_date,  
        document_id,  
        document_ref  
      
    FROM #Report_Comm_NB_Ren_By_AccountHander  
      
    DROP TABLE #Report_Comm_NB_Ren_By_AccountHander  
      
    SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON  

END

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

