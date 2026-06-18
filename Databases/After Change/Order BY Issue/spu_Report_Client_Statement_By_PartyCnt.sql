SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Report_Client_Statement_By_PartyCnt'
GO 
CREATE PROCEDURE spu_Report_Client_Statement_By_PartyCnt  
    @party_cnt int  
AS  
  
DECLARE  
    @dEndDate       datetime,  
    @iBranchID      integer,  
    @sStatementType char(2),  
    @iLedgerID      integer,  
    @sDB            sysname,  
    @iAccountID     integer,  
    @nAccountTotal  numeric(19,4),  
    @nMatchTotal    numeric(19,4),  
    @nUnallocated   numeric(19,4),  
    @sPartyCode     varchar(20),  
    @sAccountName   varchar(100)  
  
    SET NOCOUNT ON  
  
    SELECT @dEndDate = GETDATE()  
    SELECT @iBranchID = 0  
  
    -- Empty the transaction table  
    DELETE Report_Transaction  
  
    -- Get the required transactions  
    INSERT INTO Report_Transaction  
    (  
    transdetail_id, /* TransDetail.transdetail_id */  
    account_id, /* Account.account_id */  
    account_name, /* Party.resolved_name */  
    document_ref,   /* Document.document_ref */  
    document_date,  /* Document.document_date */  
    extra_datetime1,    /* TransDetail.ref_date */  
    ledger_type,    /* Ledger.ledger_name */  
    policy_number,  /* TransDetail.insurance_ref */  
    documenttype_id,    /* Document.documenttype_id */  
    extra_char1,    /* Risk_Code.code */  
    extra_char2,    /* Risk_Code.description */  
    amount,     /* TransDetail.amount */  
    branch_id,  /* Account.company_id */  
    extra_int1, /* Match indicator */  
    extra_numeric1, /* Total match amount for transaction */  
    record_type,    /* Account.ledger_id */  
    comment  
    )  
    SELECT TransDetail.transdetail_id,  
    MAX(Account.account_id),  
    MAX(Party.resolved_name),  
    MAX(Document.document_ref),  
    MAX(Document.document_date),  
    MAX(TransDetail.ref_date),  
    MAX(Ledger.ledger_name),  
    MAX(TransDetail.insurance_ref),  
    MAX(Document.documenttype_id),  
    MAX(Risk_Code.code),  
    MAX(Risk_Code.description),  
    MAX(TransDetail.Amount),  
    MAX(Account.company_id),  
    0,  
    SUM(TransMatch.base_match_amount),  
    MAX(Account.ledger_id),  
    MAX(TransDetail.comment)  
    FROM Party  
    JOIN Account Account  
        ON Account.account_key = Party.party_cnt  
       AND party_cnt = @party_cnt  
    JOIN Ledger Ledger  
        ON Account.ledger_id = Ledger.ledger_id  
    JOIN TransDetail TransDetail  
        ON Account.account_id = TransDetail.account_id  
    JOIN Document Document  
        ON TransDetail.document_id = Document.document_id  
        AND Document.document_date <= @dEndDate  
    LEFT OUTER JOIN TransMatch TransMatch  
        INNER JOIN MatchGroup MatchGroup  
            ON TransMatch.match_id = MatchGroup.match_id  
            AND MatchGroup.match_date <= @dEndDate  
        ON TransDetail.transdetail_id = TransMatch.transdetail_id  
        AND TransMatch.base_match_amount <> 0  
    --sj 31/07/2002 - Start  
    LEFT OUTER JOIN Insurance_file  
        ON Insurance_file.insurance_file_cnt = Document.insurance_file_cnt  
    --LEFT OUTER JOIN Transaction_Export_Folder  
    --    ON Document.document_ref = Transaction_Export_Folder.document_ref  
    --    AND Document.company_id = Transaction_Export_Folder.source_id  
    --LEFT OUTER JOIN Insurance_File  
    --    ON Transaction_Export_Folder.insurance_file_cnt = Insurance_File.insurance_file_cnt  
    --    AND Transaction_Export_Folder.accounts_export_status = 'c'  
    --sj 31/07/2002 - End  
    LEFT OUTER JOIN Risk_Code  
        ON Insurance_File.risk_code_id = Risk_Code.risk_code_id  
  
    GROUP BY TransDetail.transdetail_id  
  
    -- Identify matched amounts  
    UPDATE Report_Transaction  
        SET extra_int1 = 1 where extra_numeric1 = amount  
  
    -- Add the totals  
        SELECT @iAccountID = A.account_id  
        from Account A  
        join Party P on P.party_cnt = A.account_key  
        where P.party_cnt = @party_cnt  
  
        -- Get totals  
        SELECT @nAccountTotal = SUM(RT.Amount)  
            FROM Report_Transaction RT  
            WHERE RT.account_id = @iAccountID  
  
        SELECT @nMatchTotal = SUM(TM.base_match_amount)  
            FROM Report_Transaction RT  
            JOIN TransMatch TM  
                ON RT.transdetail_id = TM.transdetail_id  
                AND TM.base_match_amount <> 0  
            JOIN MatchGroup MG  
                ON TM.match_id = MG.match_id  
                AND MG.match_date <= @dEndDate  
            WHERE RT.account_id = @iAccountID  
  
        SELECT @nUnallocated =  
            isnull((SELECT SUM(RT.Amount)  
            FROM Report_Transaction RT  
            WHERE RT.account_id = @iAccountID  
            AND (RT.documenttype_id BETWEEN 22 AND 23)), 0.0)  
            +  
            isnull((SELECT SUM(RT.Amount)  
            FROM Report_Transaction RT  
            WHERE RT.account_id = @iAccountID  
            AND (RT.documenttype_id BETWEEN 28 AND 29)), 0.0)  
            +  
            isnull((SELECT SUM(TM.base_match_amount)  
            FROM Report_Transaction RT  
            JOIN TransMatch TM  
                ON RT.transdetail_id = TM.transdetail_id  
                AND TM.base_match_amount <> 0  
            JOIN MatchGroup MG  
                ON TM.match_id = MG.match_id  
                AND MG.match_date <= @dEndDate  
            WHERE RT.account_id = @iAccountID  
            AND RT.documenttype_id NOT IN (22, 23, 28, 29)), 0.0)  
  
        UPDATE Report_Transaction  
            SET extra_numeric2 = @nUnallocated,  
            extra_numeric3 = @nAccountTotal,  
            extra_numeric4 = @nMatchTotal  
            WHERE account_id = @iAccountID  
  
    SET NOCOUNT OFF  
  
    -- Extract the data  
    SELECT RT.transdetail_id transdetail_id,  
            Account.short_code account_code,  
            RT.account_name account_name,  
            RT.document_ref document_ref,  
            RT.document_date document_date,  
            RT.extra_datetime1 Effective_Date,  
            Address.address1 account_address1,  
            Address.address2 account_address2,  
            Address.address3 account_address3,  
            Address.address4 account_address4,  
            Address.postal_code account_postal_code,  
            Account.phone_area_code phone_area_code,  
            Account.phone_number phone_number,  
            RT.ledger_type ledger_name,  
            RT.policy_number policy_number,  
            DocumentType.code transaction_type_code,  
            DocumentType.description transaction_type_description,  
            RT.extra_char1 policy_type_code,  
            RT.extra_char2 policy_type_description,  
            RT.amount gross_premium,  
            TransMatch.base_match_amount base_match_amount,  
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
            TransMatch.transmatch_id transmatch_id,  
            RT.extra_int1 matched,  
            Account.short_code client_code,  
            Account.account_name client_name,  
            RT.amount client_premium,  
            RT.extra_numeric1 client_match_amt,  
            RT.extra_int1 client_matched,  
            RT.comment,  
            RT.extra_int2 number_of_days,  
            Reminder_Type.description reminder_type  
        FROM Report_Transaction RT  
        JOIN Account Account  
            ON RT.account_id = Account.account_id  
  
        JOIN Company Company  
            ON Account.company_id = Company.company_id  
        JOIN DocumentType DocumentType  
            ON RT.documenttype_id = DocumentType.documenttype_id  
        LEFT OUTER JOIN (TransMatch TransMatch  
            INNER JOIN MatchGroup MatchGroup  
                ON TransMatch.match_id = MatchGroup.match_id  
                AND MatchGroup.match_date <=@dEndDate)  
            ON RT.transdetail_id = TransMatch.transdetail_id  
            AND TransMatch.base_match_amount <> 0  
        JOIN Party  
            ON Party.party_cnt = @party_cnt  
        LEFT OUTER JOIN Reminder_Type  
            ON Reminder_Type.reminder_type_id = Party.reminder_type_id  
        JOIN Party_Address_Usage  
            ON Party_Address_Usage.party_cnt = Party.party_cnt  
        JOIN Address  
            ON Address.address_cnt = Party_Address_Usage.address_cnt  
        JOIN Address_Usage_Type  
            ON Party_Address_Usage.address_usage_type_id = Address_Usage_Type.address_usage_type_id  
            AND Address_Usage_Type.code = '3131 XCO'  
        ORDER BY RT.account_code, 1, 32  
  
    SET NOCOUNT ON  
  
    DELETE FROM Report_Transaction  
  
    SET NOCOUNT OFF  
