SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Report_Balance_Statement2'
GO

CREATE PROCEDURE spu_Report_Balance_Statement2  
    @end_date datetime,  
    @branch_id integer,  
    @statement_type varchar(10)  
AS  
  
    DECLARE  
        @dEndDate       datetime,  
        @iBranchID      integer,  
        @sStatementType char(2),  
        @iLedgerID      integer,  
        @iAccountID     integer,  
        @nAccountTotal  numeric(19,4),  
        @nMatchTotal    numeric(19,4),  
        @nUnallTD       numeric(19,4),  
        @nUnallTM       numeric(19,4),  
        @nUnallocated   numeric(19,4),  
        @LedgerCode VARCHAR(2)  
  
    SET NOCOUNT ON  
  
    SELECT @dEndDate = ISNULL(@end_date, GETDATE())  
    SELECT @iBranchID = ISNULL(@branch_id, 0)  
  
    SELECT @statement_type = LOWER(RTRIM(@sStatementType) )  
    SELECT @sStatementType = CASE  
        WHEN @statement_type = 'client' THEN  
            'C '  
        WHEN @statement_type = 'insurer' THEN  
            'I '  
        WHEN @statement_type = 'agent' THEN  
            'A '  
        WHEN @statement_type IN ('subagent', 'sub-agent', 'sub agent') THEN  
            'SA'  
        WHEN @statement_type IN ('fee', 'fees') THEN  
            'F '  
        ELSE 'C '  
        END  
  
    SELECT @iLedgerID = (CASE  
        WHEN @sStatementType = 'C ' THEN  
            2  
        WHEN @sStatementType = 'I ' THEN  
            4  
        WHEN @sStatementType = 'A ' THEN  
            5  
        WHEN @sStatementType = 'SA' THEN  
            10  
        WHEN @sStatementType = 'F ' THEN  
            7 -- and  8, 9  
        ELSE  
            2  
        END)  
  
    SELECT @LedgerCode = (CASE  
        WHEN @sStatementType = 'C ' THEN  
            'SA'  
        WHEN @sStatementType = 'I ' THEN  
            'IN'  
        WHEN @sStatementType = 'A ' THEN  
            'AG'  
        WHEN @sStatementType = 'SA' THEN  
            'UB'  
        WHEN @sStatementType = 'F ' THEN  
            'FE' -- and  8, 9  
        ELSE  
            'SA'  
        END)  
  
    -- Empty the transaction table  
    DELETE FROM Report_Transaction  
  
    -- Get the required transactions  
    INSERT INTO Report_Transaction(  
        transdetail_id,     /* TransDetail.transdetail_id */  
        account_id,         /* Account.account_id */  
        document_ref,       /* Document.document_ref */  
        document_date,      /* Document.document_date */  
        extra_datetime1,    /* TransDetail.accounting_date */  
        ledger_type,        /* Ledger.ledger_name */  
        policy_number,      /* TransDetail.insurance_ref */  
        documenttype_id,    /* Document.documenttype_id */  
        extra_char1,        /* Risk_Code.code */  
        extra_char2,        /* Risk_Code.description */  
        amount,             /* TransDetail.amount */  
        branch_id,          /* Account.company_id */  
        extra_int1,         /* Match indicator */  
        extra_numeric1,     /* Total match amount for transaction */  
        record_type,        /* Account.ledger_id */  
        comment)  
    SELECT TransDetail.transdetail_id,  
        MAX(Account.account_id),  
        MAX(Document.document_ref),  
        MAX(Document.document_date),  
        MAX(TransDetail.accounting_date),  
        MAX(ISNULL(Ledger.ledger_name, '')),  
        MAX(ISNULL(TransDetail.insurance_ref, '')),  
        MAX(ISNULL(Document.documenttype_id, 0)),  
        MAX(ISNULL(Risk_Code.code, '')),  
        MAX(ISNULL(Risk_Code.description, '')),  
        MAX(TransDetail.Amount),  
        MAX(Account.company_id),  
        0,  
        SUM(round(ISNULL(TransMatch.base_match_amount, 0.0),2)),  
        MAX(Account.ledger_id),  
        MAX(ISNULL(TransDetail.comment,''))  
        FROM Ledger le, Account Account  
        JOIN Ledger Ledger  
        ON Account.ledger_id = Ledger.ledger_id  
        JOIN TransDetail TransDetail  
        ON Account.account_id = TransDetail.account_id  
        JOIN Document Document  
        ON TransDetail.document_id = Document.document_id  
        AND Document.document_date <= @dEndDate  
        LEFT OUTER JOIN TransMatch TransMatch  
        ON TransDetail.transdetail_id = TransMatch.transdetail_id  
        AND TransMatch.base_match_amount <> 0  
        LEFT OUTER JOIN MatchGroup MatchGroup  
        ON TransMatch.match_id = MatchGroup.match_id  
        AND MatchGroup.match_date <= @dEndDate  
        --sj 31/07/2002 - start  
        LEFT OUTER JOIN Insurance_file  
        ON Insurance_file.insurance_file_cnt = document.insurance_file_cnt  
        --LEFT OUTER JOIN Transaction_Export_Folder  
        --ON Document.document_ref = Transaction_Export_Folder.document_ref  
        --LEFT OUTER JOIN Insurance_File  
        --ON Transaction_Export_Folder.insurance_file_cnt = Insurance_File.insurance_file_cnt  
        --AND Transaction_Export_Folder.accounts_export_status = 'c'  
        --sj 31/07/2002 - end  
        LEFT OUTER JOIN Risk_Code  
        ON Insurance_File.risk_code_id = Risk_Code.risk_code_id  
  
        WHERE (  
            (@sStatementType = 'F ' AND le.ledger_short_name IN ('FE', 'DI', 'CO'))  
            OR  
            (@sStatementType <> 'F ' AND le.ledger_short_name = @LedgerCode)  
        )  
        AND (@iBranchID = 0 OR (@iBranchID > 0 AND Account.company_id = @iBranchID))  
        AND Account.Ledger_id = le.Ledger_id  
        GROUP BY TransDetail.transdetail_id  
  
    IF @sStatementType <> 'C ' BEGIN  
        -- Get client transactions  
        INSERT INTO Report_Transaction(  
                transdetail_id,  
                account_id,  
                account_code,  
                account_name,  
                document_ref,  
                amount,  
                extra_int1,  
                extra_numeric1,  
                record_type)  
        SELECT DISTINCT TransDetail.transdetail_id,  
                MAX(Account.account_id),  
                MAX(Account.short_code),  
                MAX(Account.account_name),  
                MAX(Document.document_ref),  
                MAX(Transdetail.amount),  
                0,  
                SUM(round(ISNULL(TransMatch.base_match_amount, 0.0),2)),  
                2  
        FROM    Report_Transaction RT  
            JOIN Document Document  
            ON RT.document_ref = Document.document_ref  
            JOIN TransDetail TransDetail  
            ON Document.document_id = TransDetail.document_id  
            JOIN Account Account  
            ON TransDetail.account_id = Account.account_id  
            AND Account.ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'SA') --PSL 16/06/2003 Iss4707 Remove Hard coded ledger_id  
            LEFT OUTER JOIN TransMatch TransMatch         ON TransDetail.transdetail_id = TransMatch.transmatch_id  
            AND TransMatch.base_match_amount <> 0  
            LEFT OUTER JOIN MatchGroup MatchGroup  
            ON TransMatch.match_id = MatchGroup.match_id  
            AND MatchGroup.match_date <=  @dEndDate  
        GROUP BY TransDetail.transdetail_id  
  
    END  
  
    -- Identify matched amounts  
    UPDATE Report_Transaction  
    SET extra_int1 = 1 where extra_numeric1 = amount  
  
    -- Add the totals  
    DECLARE cAccount CURSOR FAST_FORWARD FOR  
        SELECT  DISTINCT account_id  
        FROM    Report_Transaction  
  
    OPEN cAccount  
    FETCH NEXT FROM cAccount INTO @iAccountID  
  
    WHILE @@FETCH_STATUS = 0 BEGIN  
        -- Get totals  
        SELECT @nAccountTotal = SUM(round(RT.Amount,2))  
                    FROM    Report_Transaction RT  
                    WHERE   RT.account_id = @iAccountID  
  
        SELECT @nMatchTotal =   SUM(round(ISNULL(TM.base_match_amount, 0.0),2))  
                    FROM    Report_Transaction RT  
                         JOIN TransMatch TM  
                        ON RT.transdetail_id = TM.transdetail_id  
                        AND TM.base_match_amount <> 0  
                        -- LEFT OUTER JOIN AllocationDetail AD  
                        -- ON TM.allocationdetail_id = AD.allocationdetail_id  
                        -- AND AD.accounting_date <= @dEndDate  
          JOIN MatchGroup MG  
                        ON TM.match_id = MG.match_id  
                        AND MG.match_date <= @dEndDate  
                    WHERE   RT.account_id = @iAccountID  
  
        SELECT @nUnallTD =  SUM(round(ISNULL(RT.Amount, 0.0),2))  
                    FROM    Report_Transaction RT  
                    WHERE   RT.account_id = @iAccountID  
                    AND RT.documenttype_id IN (22, 23)  
        /*  
        SELECT @nUnallTM =  SUM(round(ISNULL(TM.base_match_amount, 0.0),2))  
                    FROM    Report_Transaction RT  
                        JOIN TransMatch TM  
                        ON RT.transdetail_id = TM.transdetail_id  
                        AND TM.base_match_amount <> 0  
                        JOIN MatchGroup MG  
                        ON TM.match_id = MG.match_id  
                        AND MG.match_date <= @dEndDate  
                    WHERE   RT.account_id = @iAccountID  
                    AND RT.documenttype_id IN (22, 23)  
        */  
        SELECT @nUnallTM  = ISNULL(SUM(round(TM.base_match_amount,2)), 0.0)  
                    FROM    Report_Transaction RT  
                         JOIN TransMatch TM  
                        ON RT.transdetail_id = TM.transdetail_id  
                        AND TM.base_match_amount <> 0  
                        -- LEFT OUTER JOIN AllocationDetail AD  
                        -- ON TM.allocationdetail_id = AD.allocationdetail_id  
                        -- AND AD.accounting_date <= @dEndDate  
                        JOIN MatchGroup MG  
                        ON TM.match_id = MG.match_id  
                        AND MG.match_date <= @dEndDate  
                    WHERE   RT.account_id = @iAccountID  
                    AND RT.documenttype_id NOT IN (22, 23)  
  
        SELECT @nUnallocated =  ISNULL(@nUnallTD, 0.0) + ISNULL( @nUnallTM, 0.0)  
  
        UPDATE  Report_Transaction  
        SET extra_numeric2 = ISNULL(@nUnallocated, 0.0),  
            extra_numeric3 = ISNULL(@nAccountTotal, 0.0),  
            extra_numeric4 = ISNULL(@nMatchTotal, 0.0)  
        WHERE   account_id = @iAccountID  
  
        FETCH NEXT FROM cAccount INTO @iAccountID  
    END  
  
    CLOSE cAccount  
    DEALLOCATE cAccount  
  
    SET NOCOUNT OFF  
  
    -- Extract the data  
    IF @sStatementType = 'C ' BEGIN  
        SELECT  RT.transdetail_id transdetail_id,  
            Account.short_code account_code,  
            Account.account_name account_name,  
            RT.document_ref document_ref,  
            RT.document_date document_date,  
            RT.extra_datetime1 Effective_Date,  
            Account.address1 account_address1,  
            Account.address2 account_address2,  
            Account.address3 account_address3,  
            Account.address4 account_address4,  
            Account.postal_code account_postal_code,  
            Account.phone_area_code phone_area_code,  
            Account.phone_number phone_number,  
            RT.ledger_type ledger_name,  
            ISNULL(RT.policy_number, '') policy_number,  
            DocumentType.code transaction_type_code,  
            DocumentType.description transaction_type_description,  
            RT.extra_char1 policy_type_code,  
            RT.extra_char2 policy_type_description,  
            ISNULL(RT.amount, 0.0) gross_premium,  
            ISNULL(TransMatch.base_match_amount, 0.0) base_match_amount,  
            RT.extra_numeric2 unallocated_amount,  
            RT.extra_numeric3 account_total,  
            RT.extra_numeric4 match_total,  
            -- AllocationDetail.accounting_date date_paid,  
            MatchGroup.match_date date_paid,  
            Company.description Branch_Name,  
            Company.address1 Branch_address1,  
            Company.address2 Branch_address2,  
            Company.address3 Branch_address3,  
            Company.address4 Branch_address4,  
            Company.postal_code Branch_postal_code,  
            ISNULL(TransMatch.transmatch_id, 0) transmatch_id,  
 RT.extra_int1 matched,  
            ISNULL(Account.short_code, '') client_code,  
            ISNULL(Account.account_name, '') client_name,  
            ISNULL(RT.amount, 0.0) client_premium,  
            ISNULL(RT.extra_numeric1, 0.0) client_match_amt,  
            RT.extra_int1 client_matched,  
            RT.comment  
        FROM Report_Transaction RT  
        JOIN Account Account  
        ON RT.account_id = Account.account_id  
        JOIN Company Company  
        ON Account.company_id = Company.company_id  
        JOIN DocumentType DocumentType  
        ON RT.documenttype_id = DocumentType.documenttype_id  
        LEFT OUTER JOIN TransMatch TransMatch  
        ON RT.transdetail_id = TransMatch.transdetail_id  
        AND TransMatch.base_match_amount <> 0  
        LEFT OUTER JOIN MatchGroup MatchGroup  
        ON TransMatch.match_id = MatchGroup.match_id  
        AND MatchGroup.match_date <=@dEndDate  
        -- LEFT OUTER JOIN AllocationDetail AllocationDetail  
        -- ON TransMatch.allocationdetail_id = AllocationDetail.allocationdetail_id  
        -- AND AllocationDetail.accounting_date < @dEndDate  
        ORDER BY RT.account_code, 1, 32  
    END ELSE BEGIN  
        SELECT  RT.transdetail_id transdetail_id,  
            Account.short_code account_code,  
            Account.account_name account_name,  
            RT.document_ref document_ref,  
            RT.document_date document_date,  
            RT.extra_datetime1 Effective_Date,  
            Account.address1 account_address1,  
            Account.address2 account_address2,  
            Account.address3 account_address3,  
            Account.address4 account_address4,  
            Account.postal_code account_postal_code,            Account.phone_area_code phone_area_code,  
            Account.phone_number phone_number,  
            RT.ledger_type ledger_name,  
            ISNULL(RT.policy_number, '') policy_number,  
            DocumentType.code transaction_type_code,  
            DocumentType.description transaction_type_description,  
            RT.extra_char1 policy_type_code,  
            RT.extra_char2 policy_type_description,  
            ISNULL(RT.amount, 0.0) gross_premium,  
            ISNULL(TransMatch.base_match_amount, 0.0) base_match_amount,  
            ISNULL(RT.extra_numeric2, 0.0) unallocated_amount,  
            ISNULL(RT.extra_numeric3, 0.0) account_total,  
            ISNULL(RT.extra_numeric4, 0.0) match_total,  
            -- AllocationDetail.accounting_date date_paid,  
            MatchGroup.match_date date_paid,  
            Company.description Branch_Name,  
            Company.address1 Branch_address1,  
            Company.address2 Branch_address2,  
            Company.address3 Branch_address3,  
            Company.address4 Branch_address4,  
            Company.postal_code Branch_postal_code,  
            null, --ISNULL(TransMatch.transmatch_id, 0) transmatch_id,  
            RT.extra_int1 matched,  
            ISNULL(Client.account_code, '') client_code,  
            ISNULL(Client.account_name, '') client_name,            ISNULL(Client.amount, 0.0) client_premium,  
            ISNULL(Client.extra_numeric1, 0.0) client_match_amt,  
            ISNULL(Client.extra_int1, 0) client_matched,  
            RT.comment  
            FROM Report_Transaction RT  
            JOIN Account Account  
            ON RT.account_id = Account.account_id  
            JOIN Company Company  
            ON Account.company_id = Company.company_id  
            JOIN DocumentType DocumentType  
            ON RT.documenttype_id = DocumentType.documenttype_id  
            LEFT OUTER JOIN TransMatch TransMatch  
            ON RT.transdetail_id = TransMatch.transdetail_id  
            AND TransMatch.base_match_amount <> 0  
            LEFT OUTER JOIN MatchGroup MatchGroup  
            ON TransMatch.match_id = MatchGroup.match_id  
            AND MatchGroup.match_date <=@dEndDate  
            -- LEFT OUTER JOIN AllocationDetail AllocationDetail  
            -- ON TransMatch.allocationdetail_id = AllocationDetail.allocationdetail_id  
            -- AND AllocationDetail.accounting_date < @dEndDate  
            LEFT OUTER JOIN Report_Transaction Client  
            ON RT.document_ref = Client.document_ref  
            AND Client.record_type = 2  
        ORDER BY RT.account_code, 1 --, TransMatch.transmatch_id  
    END  
  
    SET NOCOUNT ON  
    DELETE FROM Report_Transaction  
    SET NOCOUNT OFF  
