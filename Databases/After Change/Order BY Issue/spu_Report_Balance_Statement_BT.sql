SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Report_Balance_Statement_BT'
GO 
  
--DC270303 added breakdown of total account into debits and credits  
--DC121101 do not use party table when obtaining account total for each account,  
--  as some accounts do not have a party set up for them  
-- MJ 17052002 changed so Client account amounts are not doubled.  
-- eck080702 added business_type option  
-- eck201202 modified the business_type option  
-- eck290102 modified to exclude fully matched transactions from the report  
		
CREATE procedure spu_Report_Balance_Statement_BT(  
    @end_date       datetime,  
    @branch_id      integer,  
    @statement_type varchar(20),  
    @date_type      varchar(20),  
    @party_code     varchar(20),  
    @business_type  varchar(10))  
AS  
  
DECLARE  
    @dEndDate       datetime,  
    @iBranchID      integer,  
    @sStatementType char(2),  
    @iLedgerID      integer,  
    @sDB            sysname,  
    @iAccountID     integer,  
    @nAccountTotal  numeric(19,4),  
    @nAccountTotalCR numeric(19,4),  --DC270303  
    @nAccountTotalDB numeric(19,4),  --DC270303  
    @nMatchTotal    numeric(19,4),  
    @nUnallocated   numeric(19,4),  
    @sAccountName   varchar(100),  
    @sPartyCode     varchar(20),  
    @CliTransDetailId integer,  
    @iBusinessTypeId integer,       --eck140602  
    @sbusiness_type  varchar(10)     --eck080702  
  
    --PSL 17/06/2003 4707  
    DECLARE @LedgerCode varchar(2)  
  
    SET NOCOUNT ON  
  
    -- TF0230701 - Include party_code parameter  
    IF @party_code = 'ALL' BEGIN  
        SELECT @party_code = NULL  
    END  
    SELECT @sPartyCode = ISNULL(@party_code, '')  
  
   -- eck080702 - Include business_type parameter  
    IF @business_type = 'ALL' BEGIN  
        SELECT @business_type  = NULL  
    END  
  
    SELECT @sbusiness_type = ISNULL(@business_type, '')  
  
  
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
        WHEN @statement_type = 'fees and discounts' THEN  
            'F '  
        WHEN @statement_type = 'purchase creditors' THEN  
            'PU'  
        ELSE  
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
        WHEN @sStatementType = 'PU' THEN  
            3  
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
        WHEN @sStatementType = 'PU' THEN  
            'PU'  
        WHEN @sStatementType = 'F ' THEN  
            'FE'  
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
    document_ref,   /* Document.document_ref */  
    document_date,  /* Document.document_date */  
    extra_datetime1,    /* TransDetail.ref_date */  
    ledger_type,    /* Ledger.ledger_name */  
    policy_number,  /* TransDetail.insurance_ref */  
    documenttype_id,    /* Document.documenttype_id */  
    extra_char1,    /* Risk_Code.code */  
    extra_char2,    /* Risk_Code.description */  
--eck140602  
    extra_int3,    /* Business_Type.business_type_id */   
    extra_char3,    /* Business_Type.description */  
--eck140602end  
    amount,     /* TransDetail.amount */  
    branch_id,  /* Document.company_id */  
    extra_int1, /* Match indicator */  
    extra_numeric1, /* Total match amount for transaction */  
    record_type,    /* Account.ledger_id */  
    comment  
    )  
    SELECT TransDetail.transdetail_id,  
    MAX(Account.account_id),  
    MAX(Document.document_ref),  
    MAX(Document.document_date),  
    MAX(TransDetail.ref_date),  
    MAX(ISNULL(Ledger.ledger_name,'')),  
    MAX(ISNULL(TransDetail.insurance_ref, '')),  
    MAX(Document.documenttype_id),  
    MAX(Risk_Code.code),  
    MAX(Risk_Code.description),  
--eck140602  
    MAX(ISNULL(Business_Type.Business_Type_Id,0)),  
    MAX(ISNULL(Business_Type.description,'No Business Type')),  
--eck140602End  
    MAX(TransDetail.Amount),  
    MAX(Document.company_id),  
    0,  
    SUM(round(ISNULL(TransMatch.base_match_amount, 0.0),2)),  
    MAX(ISNULL(Account.ledger_id, 0)),  
    MAX(ISNULL(TransDetail.comment, ''))  
    FROM Ledger le, Account Account  
    JOIN Ledger Ledger  
        ON Account.ledger_id = Ledger.ledger_id  
        AND (@sPartyCode = '' OR (@sPartyCode <> '' AND Account.short_code = @sPartyCode))  
    JOIN TransDetail TransDetail  
        ON Account.account_id = TransDetail.account_id  
    JOIN Document Document  
        ON TransDetail.document_id = Document.document_id  
    LEFT OUTER JOIN TransMatch TransMatch  
        INNER JOIN MatchGroup MatchGroup  
            ON TransMatch.match_id = MatchGroup.match_id  
            AND MatchGroup.match_date <= @dEndDate  
        ON TransDetail.transdetail_id = TransMatch.transdetail_id  
        AND TransMatch.base_match_amount <> 0  
    LEFT OUTER JOIN Transaction_Export_Folder  
        ON Document.document_ref = Transaction_Export_Folder.document_ref  
        AND Document.company_id = Transaction_Export_Folder.source_id  
    LEFT OUTER JOIN Insurance_File  
        ON Transaction_Export_Folder.insurance_file_cnt = Insurance_File.insurance_file_cnt  
        AND Transaction_Export_Folder.accounts_export_status = 'c'  
    LEFT OUTER JOIN Risk_Code  
        ON Insurance_File.risk_code_id = Risk_Code.risk_code_id  
--eck140602  
    LEFT OUTER JOIN Business_Type  
        ON Insurance_File.business_type_id = Business_Type.business_type_id  
--eck080702  
 AND (@sBusiness_Type = '' OR (@sBusiness_Type <> '' AND Business_Type.code = @sBusiness_Type))  
  
    WHERE ((@sStatementType = 'F ' AND le.ledger_short_name IN ('FE','DI', 'CO'))  
        OR  
        (@sStatementType <> 'F ' AND le.ledger_short_name = @LedgerCode))  
    AND (@iBranchID = 0 OR (@iBranchID > 0 AND Document.company_id = @iBranchID))  
 AND (  
   (  
   (Document.document_date <= @dEndDate)  
   AND (  
    ISNULL(TransDetail.spare, '') NOT IN ('COMM ADJ', 'AGENT ADJ')  
    AND     TransDetail.Document_Sequence NOT IN  
     (  
     SELECT Document_Sequence + 1  
     FROM TransDetail  
     WHERE document_id = Document.document_id  
     AND spare = 'COMM ADJ'  
     )  
    )  
   )  
   OR  
   (  
   (TransDetail.ref_date <= @dEndDate)  
   AND (  
    ISNULL(TransDetail.spare, '') IN ('COMM ADJ', 'AGENT ADJ')  
    OR     TransDetail.Document_Sequence IN  
     (  
     SELECT Document_Sequence + 1  
     FROM TransDetail  
     WHERE document_id = Document.document_id  
     AND spare = 'COMM ADJ'  
     )  
    )  
   )  
  )  
    AND le.Ledger_id = account.ledger_id  
    GROUP BY TransDetail.transdetail_id  
  
    IF @sStatementType <> 'C ' BEGIN  
  
 SELECT @CliTransDetailId = Transdetail.Transdetail_Id  
 FROM Report_Transaction RT  
 JOIN Document Document  
  ON RT.document_ref = Document.document_ref  
  AND RT.branch_id = Document.company_id  
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
        SELECT TransDetail.transdetail_id,  
  
            Account.account_id,  
            Account.short_code,  
             ISNULL(Account.account_name, ''),  
            Document.document_ref,  
             Transdetail.amount,  
             0,  
             SUM(round(ISNULL(TransMatch.base_match_amount, 0.0),2)),  
             2  
         FROM Report_Transaction   RT  
         JOIN Document    Document  
              ON RT.document_ref = Document.document_ref  
   AND RT.branch_id = Document.company_id  
         JOIN TransDetail    TransDetail  
              ON Document.document_id = TransDetail.document_id  
         JOIN Account    Account  
              ON TransDetail.account_id = Account.account_id  
  JOIN  TransMatch   TransMatch  
   ON TransMatch.transdetail_id = Transdetail.transdetail_id  
                JOIN MatchGroup   MatchGroup  
                 ON MatchGroup.match_id = TransMatch.match_id  
             WHERE Account.ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'SA') --PSL 16/06/2003 Iss4707 Remove Hard coded ledger_id  
                AND MatchGroup.match_date <= @dEndDate  
  AND TransMatch.AllocationDetail_Id IS NOT NULL  
  group by  TransDetail.transdetail_id,  
              Account.account_id,  
              Account.short_code,  
               Account.account_name,  
              Document.document_ref,  
               Transdetail.amount  
     END  
    END  
  
    -- Identify matched amounts  
    UPDATE Report_Transaction  
        SET extra_int1 = 1  
 WHERE extra_numeric1 = amount  
  
    DECLARE cAccount CURSOR FAST_FORWARD FOR  
        SELECT DISTINCT A.account_id, A.Account_Name, RT.extra_int3  
        FROM Account A,  
      Report_Transaction RT  
       WHERE A.account_id = RT.account_id  
  
  
    OPEN cAccount  
  
--eck140602 include business type in criteria  
    FETCH NEXT FROM cAccount INTO @iAccountID, @sAccountName, @iBusinessTypeId  
  
    WHILE @@FETCH_STATUS = 0 BEGIN  
        -- Get totals  
--eck140602 include business type in criteria  
        SELECT @nAccountTotal = SUM(round(RT.Amount,2))  
            FROM Report_Transaction RT  
            WHERE RT.account_id = @iAccountID  
--eck201202 AND RT.extra_int3 = @ibusinessTypeId  
  
--DC270303 -start -total Debits & Credits  
        SELECT @nAccountTotalCR = SUM(round(RT.Amount,2))  
            FROM Report_Transaction RT  
            WHERE RT.account_id = @iAccountID  
     AND RT.amount < 0  
  
        SELECT @nAccountTotalDB = SUM(round(RT.Amount,2))  
            FROM Report_Transaction RT  
            WHERE RT.account_id = @iAccountID  
     AND RT.amount > 0  
--DC270303 -end  
  
        SELECT @nMatchTotal = SUM(round(TM.base_match_amount,2))  
            FROM Report_Transaction RT  
            JOIN TransMatch TM  
                ON RT.transdetail_id = TM.transdetail_id  
                AND TM.base_match_amount <> 0  
  AND TM.allocationdetail_id IS NOT NULL  
            JOIN MatchGroup MG  
                ON TM.match_id = MG.match_id  
                AND MG.match_date <= @dEndDate  
            WHERE RT.account_id = @iAccountID  
--eck201202 AND RT.extra_int3 = @ibusinessTypeId  
  
        SELECT @nUnallocated =  
            ISNULL((SELECT SUM(round(RT.Amount,2))  
            FROM Report_Transaction RT  
            WHERE RT.account_id = @iAccountID  
--eck201202 AND RT.extra_int3 = @ibusinessTypeId  
            AND (RT.documenttype_id BETWEEN 22 AND 23)), 0.0)  
            +  
     ISNULL((SELECT SUM(round(RT.Amount,2))  
            FROM Report_Transaction RT  
           WHERE RT.account_id = @iAccountID  
--eck201202 AND RT.extra_int3 = @ibusinessTypeId  
            AND (RT.documenttype_id BETWEEN 28 AND 29)), 0.0)  
            +  
            ISNULL((SELECT SUM(round(TM.base_match_amount,2))  
            FROM Report_Transaction RT  
            JOIN TransMatch TM  
                ON RT.transdetail_id = TM.transdetail_id  
                AND TM.base_match_amount <> 0  
            JOIN MatchGroup MG  
                ON TM.match_id = MG.match_id  
                AND MG.match_date <= @dEndDate  
            WHERE RT.account_id = @iAccountID  
--eck201202 AND RT.extra_int3 = @ibusinessTypeId  
            AND RT.documenttype_id NOT IN (22, 23, 28, 29)), 0.0)  
  
        UPDATE Report_Transaction  
            SET extra_numeric2 = @nUnallocated,  
            extra_numeric3 = @nAccountTotal,  
            extra_numeric4 = @nMatchTotal,  
            account_name = @sAccountName,  
            extra_numeric5 = @nAccountTotalCR, --DC270303  
            extra_numeric6 = @nAccountTotalDB --DC270303  
  
            WHERE account_id = @iAccountID  
--eck201202 AND extra_int3 = @ibusinessTypeId  
  
        FETCH NEXT FROM cAccount INTO @iAccountID, @sAccountName, @iBusinessTypeID  
    END  
  
    CLOSE cAccount  
    DEALLOCATE cAccount  
--eck201202  
   DELETE Report_Transaction  
   WHERE @sBusiness_Type <> ''  
 AND extra_int3 = 0  
  
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
            ISNULL(TransMatch.base_match_amount,0) base_match_amount, --eck290103  
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
            --TransMatch.base_match_amount base_match_amount, --mkw260603 PN5026  
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
--eck140602  
  RT.extra_int3 Business_Type_Id,  
  RT.extra_char3 Business_Type_Description,  
--eck140602End  
--DC270303 -start  
 RT.extra_numeric5 Account_Total_CR,  
 RT.extra_numeric6 Account_Total_DB  
--DC270303 -end  
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
            AND TransMatch.base_match_amount <> 0  
 AND TransMatch.AllocationDetail_Id IS NOT NULL  
        JOIN Party  
            --ON Account.account_key = ((Party.source_id - 1) * 268435456) + Party.party_id --MKW190603 PN4707 Link directly to party_cnt.  
            ON Account.account_key = party.party_cnt  
        LEFT OUTER JOIN Reminder_Type  
            ON Reminder_Type.reminder_type_id = Party.reminder_type_id  
        JOIN Party_Address_Usage  
            ON Party_Address_Usage.party_cnt = Party.party_cnt  
        JOIN Address  
            ON Address.address_cnt = Party_Address_Usage.address_cnt  
        JOIN Address_Usage_Type  
            ON Party_Address_Usage.address_usage_type_id = Address_Usage_Type.address_usage_type_id  
            AND Address_Usage_Type.code = '3131 XCO'  
 WHERE ISNULL(TransMatch.base_match_amount,0) <> RT.amount --eck290103  
        ORDER BY RT.extra_int3,account_code, 1, 32  
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
            TransMatch.base_match_amount base_match_amount,  
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
            null transmatch_id, --isnull(TransMatch.transmatch_id,0) transmatch_id,  
            RT.extra_int1 matched,  
            Client.account_code client_code,  
            Client.account_name client_name,  
            Client.amount client_premium,  
            Client.extra_numeric1 client_match_amt,  
            Client.extra_int1 client_matched,  
            RT.comment,  
            RT.extra_int2 number_of_days,  
            '' reminder_type,  
--eck140602  
     RT.extra_int3 Business_Type_Id,  
     RT.extra_char3 Business_Type_Description,  
--eck140602End  
--DC270303 -start  
 RT.extra_numeric5 Account_Total_CR,  
 RT.extra_numeric6 Account_Total_DB  
--DC270303 -end  
        FROM Report_Transaction    RT  
        JOIN Account     Account  
            ON RT.account_id = Account.account_id  
        JOIN Company     Company  
            ON RT.branch_id = Company.company_id  
        JOIN DocumentType    DocumentType  
            ON RT.documenttype_id = DocumentType.documenttype_id  
        LEFT OUTER JOIN (TransMatch   TransMatch  
            INNER JOIN MatchGroup   MatchGroup  
                ON TransMatch.match_id = MatchGroup.match_id  
                AND MatchGroup.match_date <=@dEndDate)  
            ON RT.transdetail_id = TransMatch.transdetail_id  
            AND TransMatch.base_match_amount <> 0  
 AND TransMatch.AllocationDetail_id IS NOT NULL  
        LEFT OUTER JOIN Report_Transaction  Client  
            ON RT.document_ref = Client.document_ref  
     AND RT.branch_id = Client.branch_id  
            AND Client.record_type = 2  
 WHERE ISNULL(TransMatch.base_match_amount,0) <> RT.amount --eck290103  
        ORDER BY RT.extra_int3,account_code, 1
    END  
  
    SET NOCOUNT ON  
  
    DELETE FROM Report_Transaction  
  
    SET NOCOUNT OFF  
  