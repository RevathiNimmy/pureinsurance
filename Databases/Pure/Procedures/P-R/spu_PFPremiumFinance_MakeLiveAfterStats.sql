EXECUTE DDLDropProcedure 'spu_PFPremiumFinance_MakeLiveAfterStats'
GO

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE spu_PFPremiumFinance_MakeLiveAfterStats
(
    @InsuranceFileCnt INT,
    @DepositExportFolderCnt INT,
    @AccountID INT OUTPUT,
    @PlanTransDetailID INT OUTPUT,
    @DepositCreditTransDetailID INT OUTPUT,
    @Amount NUMERIC(19,4) OUTPUT,
    @CurrencyAmount NUMERIC(19,4) OUTPUT
)
AS

DECLARE @CommissionTransDetailID INT
DECLARE @PFPremFinanceCnt INT
DECLARE @PFPremFinanceVersion INT

-- Get the Plan Debt entry in the Document
--SMJB 25/09/03 Changes to ensure we get the latest Transdetail_ID (Could be more than
--one finance plan on an insurance file), also check document type
SELECT
    @AccountID=a.account_id,
    @PlanTransDetailID=Max(t.transdetail_id),
    @PFPremFinanceCnt=p.pfprem_finance_cnt,
    @PFPremFinanceVersion=p.pfprem_finance_version
FROM
    TransDetail t
INNER JOIN
    Document d ON d.document_id=t.document_id
INNER JOIN
    PFPremiumFinance p ON p.insurance_file_cnt=d.insurance_file_cnt
INNER JOIN
    Account a ON a.account_key=p.clientid
WHERE
    d.insurance_file_cnt=@InsuranceFileCnt
	AND d.documenttype_id IN (42,44,46)
    AND a.account_id=t.account_id
    AND (t.spare='' or t.spare = 'GROSS')

GROUP BY
	a.account_id,
	p.pfprem_finance_cnt,
	p.pfprem_finance_version

-- Get out if Policy is not on Instalments
IF @PFPremFinanceCnt is NULL
BEGIN
    -- Let caller know that
    SELECT @PlanTransDetailID=-1
    RETURN
END

-- Get the Suspended Commission entry in the Document (this will be null for Direct Business)
SELECT
    @CommissionTransDetailID=t.transdetail_id
FROM
    TransDetail t
INNER JOIN
    Document d ON d.document_id=t.document_id
INNER JOIN
    PFPremiumFinance p ON p.insurance_file_cnt=d.insurance_file_cnt
INNER JOIN
    PFScheme pfs ON pfs.companyno=p.companyno AND pfs.schemeno=p.schemeno AND pfs.schemeversion=p.schemeversion
WHERE
    d.insurance_file_cnt=@InsuranceFileCnt
    AND pfs.commission_suspense_account_id=t.account_id

IF @DepositExportFolderCnt > 0
BEGIN
    -- Get the Deposit Credit entry from TransDetail
    SELECT
        @DepositCreditTransDetailID=t.transdetail_id,
        @Amount=t.amount,
        @CurrencyAmount=t.currency_amount
    FROM
        TransDetail t
    INNER JOIN
        Document d ON d.document_id=t.document_id
    INNER JOIN
        Transaction_Export_Folder tef ON tef.insurance_file_cnt=d.insurance_file_cnt
    INNER JOIN
        PFPremiumFinance p ON p.insurance_file_cnt=d.insurance_file_cnt
    INNER JOIN
        Account a ON a.account_key=p.clientid
    WHERE
        tef.transaction_export_folder_cnt=@DepositExportFolderCnt
    AND     d.documenttype_id=1
    AND     t.amount<0
END

-- Update the Plan to Live
UPDATE
    PFPremiumFinance
SET
    PlanTransaction_id=@PlanTransDetailID,
    commission_transdetail_id=@CommissionTransDetailID,
    StatusInd='040' -- Live
WHERE
    pfprem_finance_cnt=@PFPremFinanceCnt
AND pfprem_finance_version=@PFPremFinanceVersion

-- Update the settings on the Insurance File
UPDATE
    Insurance_File
SET
    Insurance_File.payment_method = 'Instalments'
FROM
    Insurance_File
INNER JOIN
        PFPremiumFinance ON Insurance_File.insurance_file_cnt = PFPremiumFinance.Insurance_File_Cnt
WHERE
    PFPremiumFinance.pfprem_finance_cnt = @PFPremFinanceCnt
    AND
        pfprem_finance_version = @PFPremFinanceVersion

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

