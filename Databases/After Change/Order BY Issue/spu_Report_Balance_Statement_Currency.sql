SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Report_Balance_Statement_Currency'
GO 
-- DC121101 do not use party table when obtaining account total for each account,  
-- as some accounts do not have a party set up for them  
CREATE PROCEDURE spu_Report_Balance_Statement_Currency  
    @end_date datetime,  
    @branch_id integer,  
    @statement_type varchar(20),  
    @date_type varchar(20),  
    @party_code varchar(20),  
    @sIncludeInstalments varchar(20)  
AS  
  
DECLARE  
    @dEndDate datetime,  
    @iBranchID integer,  
    @sStatementType char(2),  
    @iLedgerID integer,  
    @iAccountID integer,  
    @nAccountTotal numeric(19, 4),  
    @nMatchTotal numeric(19, 4),  
    @nUnallocated numeric(19, 4),  
    @sAccountName varchar(100),  
    @sPartyCode varchar(20),  
    @CliTransDetailId integer,  
        @IncludeInstalments int  
    DECLARE @LedgerCode VARCHAR(2)  
  
    SET NOCOUNT ON  
  
    select @IncludeInstalments = 0  
    if @sIncludeInstalments = 'Including'  
    begin  
        select @IncludeInstalments = 1  
    end  
  
    -- TF0230701 - Include party_code parameter  
    IF @party_code = 'ALL' BEGIN  
        SELECT @party_code = NULL  
    END  
    SELECT @sPartyCode = ISNULL(@party_code, '')  
  
    SELECT @dEndDate = ISNULL(@end_date, GETDATE())  
    SELECT @iBranchID = ISNULL(@branch_id, 0)  
    SELECT @statement_type = LOWER(RTRIM(@statement_type))  
  
    SELECT @sStatementType = (CASE  
        WHEN @statement_type = 'client' THEN  
            'C '  
        WHEN @statement_type = 'insurer' THEN  
            'I '  
        WHEN @statement_type = 'agent' THEN  
            'A '  
        WHEN @statement_type = 'sub agent' THEN  
            'SA'  
        WHEN @statement_type = 'introducer' THEN  
            'T '  
        WHEN @statement_type = 'fees and discounts' THEN  
            'F '  
        WHEN @statement_type = 'purchase creditors' THEN  
            'PU'  
        WHEN @Statement_type = 'premium finance provider' THEN --FSA Phase IV  
     'RF'ELSE  
            'C '  
        END)  
  
    SELECT @iLedgerID = (CASE  
        WHEN @sStatementType = 'C ' THEN  
            2  
        WHEN @sStatementType = 'I ' THEN  
            4  
        WHEN @sStatementType = 'A ' THEN  
            5  
        WHEN @sStatementType = 'SA' THEN  
            10  
        WHEN @sStatementType = 'T ' THEN  
            11  
        WHEN @sStatementType = 'PU' THEN  
            3  
        WHEN @sStatementType = 'F ' THEN  
            7 -- and 8, 9  
 WHEN @sStatementType = 'RF' THEN --FSA Phase 4  
            6  
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
        WHEN @sStatementType = 'T ' THEN  
            'TR'  
        WHEN @sStatementType = 'F ' THEN  
            'FE' -- and  8, 9  
        WHEN @sStatementType = 'RF' THEN  --FSA Phase IV  
     'RF'  
 ELSE  
            'SA'  
        END)  
  
    -- Empty the transaction table  
    DELETE Report_Transaction  
  
    -- Get the required transactions  
    INSERT INTO Report_Transaction  
    (  
    transdetail_id, /* TransDetail.transdetail_id */  
    account_id, /* Account.account_id */  
    document_ref, /* Document.document_ref */  
    document_date, /* Document.document_date */  
    extra_datetime1, /* TransDetail.ref_date */  
    ledger_type, /* Ledger.ledger_name */  
    policy_number, /* TransDetail.insurance_ref */  
    documenttype_id, /* Document.documenttype_id */  
    extra_char1, /* Risk_Code.code */  
    extra_char2, /* Risk_Code.description */  
    amount, /* TransDetail.Currency_Amount */  
    branch_id, /* Document.company_id */  
    extra_int1, /* Match indicator */  
    extra_numeric1, /* Total match amount for transaction */  
    record_type, /* Account.ledger_id */  
    comment,  
    extra_int3 /*Currency ID*/  
    )  
    SELECT TransDetail.transdetail_id,  
    MAX(Account.account_id),  
    MAX(Document.document_ref),  
    MAX(Document.document_date),  
    MAX(TransDetail.ref_date),  
    MAX(Ledger.ledger_name),  
    MAX(TransDetail.insurance_ref),  
    MAX(Document.documenttype_id),  
    MAX(Risk_Code.code),  
    MAX(Risk_Code.description),  
    MAX(ROUND(TransDetail.Currency_Amount,2)),  
    MAX(Document.company_id),  
    0,  
    SUM(ROUND(TransMatch.currency_match_amount,2)),  
    MAX(Account.ledger_id),  
    MAX(TransDetail.comment),  
    MAX(TransDetail.currency_id)  
    FROM Ledger le, Account Account  
    JOIN Ledger Ledger  
        ON Account.ledger_id = Ledger.ledger_id  
        AND (@sPartyCode = '' OR (@sPartyCode <> '' AND Account.short_code = @sPartyCode))  
    JOIN TransDetail TransDetail  
        ON Account.account_id = TransDetail.account_id  
    JOIN Document Document  
        ON TransDetail.document_id = Document.document_id  
 JOIN DocumentType  
  ON Document.DocumentType_id=DocumentType.DocumentType_id  
    LEFT OUTER JOIN TransMatch TransMatch  
        INNER JOIN MatchGroup MatchGroup  
            ON TransMatch.match_id = MatchGroup.match_id  
            AND MatchGroup.match_date <= @dEndDate  
        ON TransDetail.transdetail_id = TransMatch.transdetail_id  
        AND TransMatch.allocationdetail_id IS NOT NULL  
        AND TransMatch.currency_match_amount <> 0  
    LEFT OUTER JOIN Transaction_Export_Folder  
        ON Document.document_ref = Transaction_Export_Folder.document_ref  
        AND Document.company_id = Transaction_Export_Folder.source_id  
    LEFT OUTER JOIN Insurance_File  
        ON Transaction_Export_Folder.insurance_file_cnt = Insurance_File.insurance_file_cnt  
        AND Transaction_Export_Folder.accounts_export_status = 'c'  
    LEFT OUTER JOIN Risk_Code  
        ON Insurance_File.risk_code_id = Risk_Code.risk_code_id  
  
    WHERE  
  (  
   @IncludeInstalments=1  
   OR  
   (  
    @IncludeInstalments=0  
    AND  
    DocumentType.Code NOT IN ('IDR','ICR')  
    AND NOT EXISTS  
    (  
     SELECT NULL  
     FROM transmatch origtm  
     JOIN transmatch tm  
      ON tm.match_id = origtm.match_id  
      AND tm.transmatch_id <> origtm.transmatch_id  
     JOIN transdetail td  
      ON td.transdetail_id = tm.transdetail_id  
     JOIN document d  
      ON d.document_id = td.document_id  
     JOIN documenttype dt  
      ON dt.documenttype_id = d.documenttype_id  
     WHERE origtm.transdetail_id = TransDetail.transdetail_id  
     AND dt.code IN ('IDR','ICR')  
    )  
   )  
  )  
 AND  
     ((@sStatementType = 'F ' AND le.ledger_short_name IN ('FE', 'DI', 'CO'))  
         OR  
        (@sStatementType <> 'F ' AND le.ledger_short_name = @LedgerCode))  
    AND (@iBranchID = 0 OR (@iBranchID > 0 AND Document.company_id = @iBranchID))  
 AND (  
   (  
                Document.document_date <= @dEndDate  
                AND  
                TransDetail.spare NOT IN ('COMM ADJ', 'AGENT ADJ', 'BROK ADJ')  
            )  
            OR  
            (  
                TransDetail.ref_date <= @dEndDate  
                AND  
                TransDetail.spare IN ('COMM ADJ', 'AGENT ADJ', 'BROK ADJ')  
            )  
        )  
   AND Account.Ledger_id = le.Ledger_id  
   GROUP BY TransDetail.transdetail_id  
  
    IF @sStatementType <> 'C ' BEGIN  
  
    SELECT @CliTransDetailId = Transdetail.Transdetail_Id  
    FROM Report_Transaction RT  
            JOIN Document Document  
            ON RT.document_ref = Document.document_ref  
        JOIN TransDetail TransDetail  
             ON Document.document_id = TransDetail.document_id  
        JOIN Account Account  
             ON TransDetail.account_id = Account.account_id  
             AND Account.ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'SA') --PSL 16/06/2003 Iss4707 Remove Hard coded ledger_id  
  
    IF EXISTS (  
            SELECT Transdetail_Id  
                      FROM Report_Transaction  
                            WHERE Transdetail_Id = @CliTransDetailId )  
    BEGIN  
  
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
                SELECT  
                    TransDetail.transdetail_id,  
                Account.account_id,  
                Account.short_code,  
                    Account.account_name,  
                Document.document_ref,  
                    TransDetail.Currency_Amount,  
                    0,  
                    (SELECT SUM(round(TransMatch.currency_match_amount,2))  
                    FROM TransMatch TransMatch,  
                        MatchGroup MatchGroup  
                    WHERE TransMatch.transdetail_id = Transdetail.transdetail_id  
                    AND MatchGroup.match_id = TransMatch.match_id  
                    AND MatchGroup.match_date <= @dEndDate  
        AND TransMatch.AllocationDetail_Id IS NOT NULL),  
                    2  
                FROM Report_Transaction RT  
                JOIN Document Document  
                    ON RT.document_ref = Document.document_ref  
                JOIN TransDetail TransDetail  
                    ON Document.document_id = TransDetail.document_id  
                JOIN Account Account  
                    ON TransDetail.account_id = Account.account_id  
                    AND Account.ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'SA') --PSL 16/06/2003 Iss4707 Remove Hard coded ledger_id  
  
        END  
    END  
  
    -- Identify matched amounts  
    UPDATE Report_Transaction  
        SET extra_int1 = 1 WHERE extra_numeric1 = amount  
  
 --Get totals  
 UPDATE r  
 SET  
  /*Account total by currency*/  
  extra_numeric3 =  
   (  
    SELECT SUM(amount)  
    FROM report_transaction  
    WHERE account_id = r.account_id  
    AND extra_int3 = r.extra_int3  
   ),  
  /*Unallocated total by currency*/  
  extra_numeric2 =  
   (  
    SELECT ISNULL(SUM(rt.amount - ISNULL(rt.extra_numeric1,0)),0)  
    FROM report_transaction rt  
    WHERE rt.account_id = r.account_id  
    AND rt.extra_int3 = r.extra_int3  
    AND rt.documenttype_id IN (22, 23, 28, 29)  
   ),  
  /*Account name*/  
 /* FSA Phase IV*/  
       extra_numeric7 =  
              (  
                  SELECT ISNULL(SUM(ROUND(TD.amount,2)),0) - ISNULL(SUM(ROUND(TM.base_match_amount,2)),0)  
                  FROM Account A  
    JOIN Party P on P.party_cnt =  A.account_key  
    JOIN PFPremiumFinance PFPF on PFPF.ClientId =P.party_cnt  
    JOIN Transdetail TD on TD.transdetail_id = PFPF.PlanTransaction_id  
    LEFT OUTER JOIN Transmatch TM on TM.transdetail_id = TD.transdetail_id  
    JOIN Document D on D.document_id = TD.document_id  
    WHERE A.account_id = R.account_id  
    AND D.documenttype_id = 1  
     AND ((D.document_date <= @end_date  
         AND @date_type = 'Transaction Date')  
    OR  (TD.ref_date <= @end_date  
         AND @date_type = 'Effective Date'))  
  
            ), /*unpaid finance*/  
  
  r.account_name = a.account_name  
    FROM report_transaction r  
    JOIN account a  
    ON a.account_id = r.account_id  
  
    -- Calculate the date differences  
    SELECT @date_type = LOWER(RTRIM(@date_type))  
    IF @date_type = 'effective date' BEGIN  
        UPDATE Report_Transaction  
            SET extra_int2 = DATEDIFF(day, extra_datetime1, @dEndDate)  
    END ELSE IF @date_type = 'transaction date' BEGIN  
        UPDATE Report_Transaction  
            SET extra_int2 = DATEDIFF(day, document_date, @dEndDate)  
    END  
  
  
    SET NOCOUNT OFF  
  
    -- Extract the data  
    IF @sStatementType IN ('C ') BEGIN  
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
            TransMatch.currency_match_amount currency_match_amount,  
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
            Reminder_Type.description reminder_type,  
            c.code currency_code,  
     RT.extra_numeric7 unpaid_on_finance  
        FROM Report_Transaction RT  
        JOIN Account Account  
            ON RT.account_id = Account.account_id  
        JOIN Company Company  
            ON RT.branch_id = Company.company_id  
        JOIN DocumentType DocumentType  
            ON RT.documenttype_id = DocumentType.documenttype_id  
        LEFT OUTER JOIN (TransMatch TransMatch  
            INNER JOIN MatchGroup MatchGroup  
                ON TransMatch.match_id = MatchGroup.match_id  
                AND MatchGroup.match_date <=@dEndDate)  
            ON RT.transdetail_id = TransMatch.transdetail_id  
            AND TransMatch.currency_match_amount <> 0  
    AND TransMatch.AllocationDetail_Id IS NOT NULL  
        JOIN Party  
            ON Account.account_key = Party.party_cnt  
        LEFT OUTER JOIN Reminder_Type  
            ON Reminder_Type.reminder_type_id = Party.reminder_type_id  
        JOIN Party_Address_Usage  
            ON Party_Address_Usage.party_cnt = Party.party_cnt  
        JOIN Address  
            ON Address.address_cnt = Party_Address_Usage.address_cnt  
        JOIN Address_Usage_Type  
            ON Party_Address_Usage.address_usage_type_id = Address_Usage_Type.address_usage_type_id  
            AND Address_Usage_Type.code = '3131 XCO'  
        JOIN currency c  
            ON c.currency_id = RT.extra_int3  
        ORDER BY c.currency_id, account_code, 1, 32  
    END ELSE BEGIN  
        SELECT RT.transdetail_id transdetail_id,  
            Account.short_code account_code,  
            RT.account_name account_name,  
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
            RT.policy_number policy_number,  
            DocumentType.code transaction_type_code,  
            DocumentType.description transaction_type_description,  
            RT.extra_char1 policy_type_code,  
            RT.extra_char2 policy_type_description,  
            RT.amount gross_premium,  
            TransMatch.currency_match_amount currency_match_amount,  
            RT.extra_numeric2 unallocated_amount,  
            RT.extra_numeric3 account_total,  
            RT.extra_numeric4 match_total,  
            MatchGroup.match_date date_paid,  
            Company.description Branch_Name,  
            Company.address1 Branch_address1,  
            Company.address2 Branch_address2,  
            Company.address3 Branch_address3,  
            Company.address4 Branch_address4,  
            Company.postal_code Branch_postal_code,  
            null, --ISNULL(TransMatch.transmatch_id, 0) transmatch_id,  
            RT.extra_int1 matched,  
            Client.account_code client_code,  
            Client.account_name client_name,  
            Client.amount client_premium,  
            Client.extra_numeric1 client_match_amt,  
            Client.extra_int1 client_matched,  
            RT.comment,  
            RT.extra_int2 number_of_days,  
            '' reminder_type,  
            c.code currency_code,  
     RT.extra_numeric7 unpaid_on_finance  
        FROM Report_Transaction RT  
        JOIN Account Account  
            ON RT.account_id = Account.account_id  
        JOIN Company Company  
            ON RT.branch_id = Company.company_id  
        JOIN DocumentType DocumentType  
            ON RT.documenttype_id = DocumentType.documenttype_id  
        LEFT OUTER JOIN (TransMatch TransMatch  
            INNER JOIN MatchGroup MatchGroup  
                ON TransMatch.match_id = MatchGroup.match_id  
                AND MatchGroup.match_date <=@dEndDate)  
            ON RT.transdetail_id = TransMatch.transdetail_id  
            AND TransMatch.currency_match_amount <> 0  
    AND TransMatch.AllocationDetail_id IS NOT NULL  
        LEFT OUTER JOIN Report_Transaction Client  
            ON RT.document_ref = Client.document_ref  
            AND Client.record_type = 2  
        JOIN currency c  
            ON c.currency_id = RT.extra_int3  
        ORDER BY c.currency_id, account_code, 1  
    END  
  
    SET NOCOUNT ON  
  
    DELETE FROM Report_Transaction  
  
    SET NOCOUNT OFF  
