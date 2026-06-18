/*
This stored procedure is used by the following reports:

Commission_Earnt_Audit_Trail_By_Effective_Date.rpt
Commission_Earnt_Audit_Trail_By_Effective_Date_Full.rpt
Commission_Earnt_by_AccountExec.rpt
Commission_Earnt_Audit_Trail_By_Business_Type.rpt
*/

SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_Report_Commission_Earnt'
GO

CREATE PROCEDURE spu_Report_Commission_Earnt

    @branch_id INT,
    @start_date DATETIME,
    @end_date DATETIME,
    @date_type VARCHAR(20)

AS

SET NOCOUNT ON

DECLARE 
    @iBranchID INT,
    @iTransdetailID1 INT,
    @sSpare VARCHAR(20),
    @iDocumentID INT,
    @sDocumentRef VARCHAR(25),
    @dtDocumentDate DATETIME,
    @sShortCode CHAR(30),
    @sAccountName VARCHAR(255),
    @iCompanyID INT,
    @nAmount MONEY,
    @sOrigShortCode CHAR(30),
    @sOrigAccountName VARCHAR(255),
    @sClientName VARCHAR(255),
    @nBroughtForward MONEY,
    @sClientCode CHAR(30), 
    @sFileCode VARCHAR(8), 
    @sAccountExecCode CHAR(20), 
    @sAccountExec VARCHAR(255), 
    @sAccountHandlerCode CHAR(20), 
    @sRisk VARCHAR(255), 
    @sInsurer VARCHAR(20), 
    @node_id INT,
    @session_id INT,
    @sInsFileAccExec CHAR(1),
    @dtEffectiveDate DATETIME,
    @sOrigDocumentRef VARCHAR(25),
    @sOrigDocumentType VARCHAR(10),
    @sPolicyRef VARCHAR(30),
    @sBusinessType VARCHAR(50),
    @sAccountHandler VARCHAR(255),
    @sBranch VARCHAR(255),
    @nInsurerOrder INT,
    @nInsurerCount INT

SELECT @iBranchID = ISNULL(@branch_id, 0)

--Create Temporary Tables
CREATE TABLE #Report_Commission_Earnt
(
    document_id INT,
    document_ref VARCHAR(25),
    document_date DATETIME,
    income_acc_code CHAR(20),
    income_acc_name VARCHAR(255),
    income_acc_amount MONEY,
    income_acc_amount_total MONEY,
    branch_id INT,
    account_name VARCHAR(255),
    account_code CHAR(30),
    file_code VARCHAR(8),
    account_exec_code CHAR(20), 
    account_exec VARCHAR(255),
    account_handler_code CHAR(20), 
    risk VARCHAR(255),
    insurer CHAR(20),
    effective_date DATETIME,
    original_document_ref VARCHAR(25),
    original_document_type VARCHAR(10),
    policy_no VARCHAR(30),
    business_type VARCHAR(50),
    account_handler VARCHAR(255),
    branch VARCHAR(255),
    insurer_order INT,
    insurer_count INT
)

CREATE TABLE #Report_Commission_Earnt_Accounts
(
    account_id INT
)

--Retrieve acc exec location from hidden options
SELECT @sInsFileAccExec = value
FROM hidden_options
WHERE option_number = 40

IF ISNULL(@sInsFileAccExec, '') = ''
BEGIN
    SELECT @sInsFileAccExec = '0'
END


INSERT INTO #Report_Commission_Earnt_Accounts
(
    account_id
)
SELECT
    AE.account_id
FROM account A
JOIN ledger L
    ON L.ledger_id = A.ledger_id
JOIN structuretree ST
    ON ST.account_id = A.account_id
JOIN elementextras EE
    ON EE.element_id = ST.element_id
JOIN account AE
    ON AE.account_id = EE.account_map_id
WHERE L.ledger_short_name = 'CO'
GROUP BY AE.account_id
    

--Search through all transactions for the comm earned accounts.
DECLARE c_cursor CURSOR FAST_FORWARD FOR

SELECT
    T.transdetail_id,
    D.document_id,
    D.document_ref,
    D.document_date,
    A.short_code,
    A.account_name,
    T.company_id,
    ROUND(ISNULL(T.amount,0),2)
FROM Transdetail T
JOIN Document D
ON D.document_id = T.document_id
JOIN Account A
ON A.account_id = T.account_id
JOIN #Report_Commission_Earnt_Accounts EA
ON EA.account_id = A.account_id
WHERE D.comment <> 'Year End Retained Profit'
AND 
(
    @date_type <> 'Transaction Date'
    OR
    (
        D.document_date BETWEEN @start_date AND @end_date
        AND
        T.spare NOT IN ('COMM ADJ', 'AGENT ADJ', 'BROK ADJ')
    )
    OR
    (
        T.ref_date BETWEEN @start_date AND @end_date
        AND 
        T.spare IN ('COMM ADJ', 'AGENT ADJ', 'BROK ADJ')
    )
)        
AND
(

    @iBranchID = 0
    OR
    (
        @iBranchID <> 0
        AND
        T.company_id = @iBranchID
    )
)

OPEN c_cursor

FETCH NEXT FROM c_cursor INTO @iTransdetailID1,
                @iDocumentID,
                @sDocumentRef,
                @dtDocumentDate,
                @sShortCode,
                @sAccountName,
                @iCompanyID,
                @nAmount

WHILE @@FETCH_STATUS = 0
BEGIN
    
    -- Get corresponding Commission account transaction
    SELECT 
        @sSpare = ISNULL(T1.spare, ''),
        @sOrigShortCode = A.short_code,
        @sOrigAccountName = A.account_name
    FROM Transdetail T1
    JOIN Transdetail T2
    ON T2.document_id = T1.document_id
    AND T2.transdetail_id <> T1.transdetail_id
    JOIN Account A
    ON A.account_id = T2.account_id
    WHERE T1.transdetail_id = @iTransdetailID1
    AND 
    (
        T2.spare = T1.spare
        OR 
        (
            SELECT SUM(1)
            FROM Transdetail 
            WHERE document_id = T1.document_id
        ) = 2 
        OR
        (
            T2.spare = 'COMM ADJ'
            AND
            T2.transdetail_id = T1.transdetail_id - 1
        )
    )

    --Set defaults
    SELECT
        @sClientName = @sOrigAccountName,
        @sClientCode = @sOrigShortCode,
        @sFileCode = '',        
        @sAccountExecCode = 'NO EXEC',
        @sAccountExec = 'NO EXEC',          
        @sAccountHandlerCode = 'NO HANDLER',
        @sRisk = '',                
        @sInsurer = '',
        @dtEffectiveDate  = NULL,
        @sOrigDocumentRef = '',
        @sOrigDocumentType = '',
        @sPolicyRef = '',
        @sBusinessType = 'NO BUSINESS TYPE',
        @sAccountHandler = 'NO HANDLER'

    --If its not an adjustment then get the original document_id.
    IF RTRIM(@sSpare) NOT IN ('AGENT ADJ', 'COMM ADJ', 'BROK ADJ')
    BEGIN
        SELECT @iDocumentID = document_id
        FROM document
        WHERE document_ref = SUBSTRING(@sSpare, 10, 11)
        AND company_id = @iCompanyID
    END

    SELECT
--        @sClientName = A.Account_Name, 
	@sClientName = P.Name, 
        @sClientCode = A.short_Code,             
        @sFileCode = P.file_Code,            
        @sAccountHandlerCode = ISNULL(PAH.shortname,'NO HANDLER'),
        @sAccountExecCode = (CASE @sInsFileAccExec when '1' then ISNULL(PAE2.shortname,'NO EXEC') else ISNULL(PAE.shortname,'NO EXEC') END), 
        @sAccountExec = (CASE @sInsFileAccExec when '1' then ISNULL(PAE2.resolved_name,'') else ISNULL(PAE.resolved_name,'') END),            
        @sRisk = R.description,              
        @sInsurer = INS.shortname,
        @dtEffectiveDate = TEF.cover_start_date,
        @sOrigDocumentRef = D.document_ref,
        @sOrigDocumentType = ISNULL(TEF.transaction_type_code, ''),
        @sPolicyRef = I.insurance_ref,
        @sBusinessType = ISNULL(BT.description, 'NO BUSINESS TYPE'),
        @sAccountHandler = ISNULL(PAH.resolved_name,'')
    FROM document D
    JOIN transdetail T
        ON T.document_id = D.document_id
    JOIN account A
        ON A.account_id = T.account_id
    LEFT OUTER JOIN transaction_export_folder TEF
        ON TEF.document_ref = D.document_ref
        AND TEF.source_id = D.company_id
    LEFT OUTER JOIN insurance_file  I
        ON I.insurance_ref = T.insurance_ref
        AND I.policy_version =  
            (
                SELECT MAX(policy_version)
                FROM Insurance_file
                WHERE insurance_ref = T.insurance_ref
                AND lead_insurer_cnt IS NOT NULL
            )       
    LEFT OUTER JOIN risk_code R
        ON R.risk_code_id = I.risk_code_id               
    LEFT OUTER JOIN party INS
        ON INS.party_cnt = I.lead_insurer_cnt
    LEFT OUTER JOIN party P
        ON P.party_cnt = I.insured_cnt
    LEFT OUTER JOIN party PAH
        ON PAH.party_cnt = I.account_handler_cnt
    LEFT OUTER JOIN party PAE
        ON PAE.party_cnt = P.consultant_cnt
    LEFT OUTER JOIN party PAE2
        ON PAE2.party_cnt = I.account_executive_cnt
    LEFT OUTER JOIN business_type BT
        ON I.business_type_id = BT.business_type_id
    WHERE D.document_id = @iDocumentID
    AND 
    (
        (
            (
                SELECT ISNULL(SUM(1),0)
                FROM transdetail TCli
                JOIN account ACli
                ON ACli.account_id = TCli.account_id
                WHERE TCli.document_id = D.document_id
                AND ACli.ledger_id in (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'SA')
            ) > 0
            AND T.transdetail_id in
            (
                SELECT MIN(transdetail_id)
                FROM transdetail TCli
                JOIN account ACli
                ON ACli.account_id = TCli.account_id
                WHERE TCli.document_id = D.document_id
                AND ACli.ledger_id in (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'SA')
            )
        )
        OR
        (
            (
                SELECT ISNULL(SUM(1),0)
                FROM transdetail TCli
                JOIN account ACli
                ON ACli.account_id = TCli.account_id
                WHERE TCli.document_id = D.document_id
                AND ACli.ledger_id in (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'SA')
            ) = 0
            AND T.transdetail_id in
            (
                SELECT MIN(transdetail_id)
                FROM transdetail TCli
                JOIN account ACli
                ON ACli.account_id = TCli.account_id
                WHERE TCli.document_id = D.document_id
                AND ACli.short_code <> @sShortCode      
            )
        )
    )
        
    SELECT @sBranch = c.description
        FROM .Company c
        WHERE c.company_id = @iCompanyId
    
    SELECT @nInsurerCount = (
                SELECT  COUNT(*) 
                FROM    transdetail tdc
                JOIN    account iac
                ON  iac.account_id = tdc.account_id
                JOIN    party pc
                ON  iac.account_key = pc.party_cnt
                JOIN    party_type ptc
                ON  ptc.party_type_id = pc.party_type_id
                WHERE   ptc.code = 'IN' 
                AND     tdc.document_id = @iDocumentId
                AND     tdc.spare = 'GROSS' 
                )

    IF @dtEffectiveDate IS NULL
        SELECT @dtEffectiveDate = @dtDocumentDate
        
    IF 
    (
        @date_type = 'Transaction Date'
        OR
        @dtEffectiveDate BETWEEN @start_date AND @end_date
    )
    BEGIN
        --Insert values into temporary table
        INSERT INTO #Report_Commission_Earnt
        VALUES
        (
            @iDocumentId,
            @sDocumentRef,
            @dtDocumentDate,
            @sShortCode,
            @sAccountName,
            CASE WHEN @nInsurerCount > 1 THEN
                0
            ELSE
                @nAmount
            END,
            @nAmount,
            @iCompanyID,
            @sClientName,       
            @sClientCode,       
            @sFileCode,         
            @sAccountExecCode,
            @sAccountExec,
            @sAccountHandlerCode,
            @sRisk,             
            CASE WHEN @nInsurerCount > 1 THEN
                'MULTI'
            ELSE
                @sInsurer
            END,
            @dtEffectiveDate,
            @sOrigDocumentRef,
            @sOrigDocumentType,
            @sPolicyRef,
            @sBusinessType,
            @sAccountHandler,
            @sBranch,
            0,
            @nInsurerCount
        )

        IF @nInsurerCount > 1
        BEGIN
    
            INSERT INTO #Report_Commission_Earnt
            SELECT  @iDocumentId,
                @sDocumentRef,
                @dtDocumentDate,
                @sShortCode,
                @sAccountName,
                (
                SELECT ISNULL(SUM(tdmi.amount),0)
                FROM transdetail tdmi
                WHERE tdmi.document_id = @iDocumentId
                AND tdmi.account_id = ia.account_id
                AND tdmi.spare IN ('COMM', 'COMM ADJ')
                ) * -1,
                0,
                @iCompanyID,
                @sClientName,       
                @sClientCode,       
                @sFileCode,         
                @sAccountExecCode,
                @sAccountExec,
                @sAccountHandlerCode,
                @sRisk,
                ip.shortname,
                @dtEffectiveDate,
                @sOrigDocumentRef,
                @sOrigDocumentType,
                @sPolicyRef,
                @sBusinessType,
                @sAccountHandler,
                @sBranch,
                1,
                -1
            FROM    transdetail tdi
            JOIN    account ia
            ON  ia.account_id = tdi.account_id
            JOIN    party ip
            ON  ia.account_key = ip.party_cnt
            JOIN    party_type pti
            ON  pti.party_type_id = ip.party_type_id
            WHERE   pti.code = 'IN' 
            AND tdi.document_id = @iDocumentId
            AND tdi.spare = 'GROSS'

        END
    END

    FETCH NEXT FROM c_cursor INTO @iTransdetailID1,
                    @iDocumentID,
                    @sDocumentRef,
                    @dtDocumentDate,
                    @sShortCode,
                    @sAccountName,
                    @iCompanyID,
                    @nAmount
END

CLOSE c_cursor
DEALLOCATE c_cursor


/*calculate brought forward figure*/
SELECT 
    @nBroughtForward = SUM(ROUND(ISNULL(T.amount,0.00),2))
FROM TransDetail T
JOIN Document D
    ON D.document_id = T.document_id 
JOIN Account A
    ON A.account_id = T.account_id
JOIN #Report_Commission_Earnt_Accounts RCEA
    ON RCEA.account_id = a.account_id
WHERE (A.company_id = @branch_id or @branch_id = 0)
AND
(
    (
        D.document_date < @start_date
        AND 
        T.spare NOT IN ('COMM ADJ', 'AGENT ADJ', 'BROK ADJ')
    )
    OR
    (
        T.ref_date < @start_date
        AND 
        T.spare IN ('COMM ADJ', 'AGENT ADJ', 'BROK ADJ')
    )
)



SET NOCOUNT OFF

-- Return data from Temporary Table
SELECT 'Brought_Forward'=@nBroughtForward, RT.*
FROM #Report_Commission_Earnt RT

DROP TABLE #Report_Commission_Earnt
DROP TABLE #Report_Commission_Earnt_Accounts

GO

