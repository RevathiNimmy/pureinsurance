SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

EXECUTE DDLDropProcedure 'spu_Report_Agent_Statemt_Tax_SFU'
GO

/**********************************************************************************************************************************
**Modified JT*****09-08-2004
Multicurrency Feature.Changed the Update stmt. Which earlier was only for HO
*************************************
** Created by Jude Killip
** 12/03/2002
** Same as sp_Report_Agent_Statemt but with Tax split out - additional report required by AUA
**********************************************************************************************************************************
** WHEN		WHO		WHAT
** 23/Oct/03	JMK		PN 7726 - Change Update with Agent Resolved Name (Party/account link has changed)
** 27/Oct/03	JMK		PN 6904 - Merge Agent Statement changes, parameters etc.
** 06/10/04	JT		PN-15345-Added Transaction Currency as per latest Multicurrency Spec
**07/07/05	JT		PN-22149	Insured name is Varchar(255)  now
**13Jun2006	RC		Filter by Agent Group
***********************************************************************************************************************************/
CREATE PROCEDURE spu_Report_Agent_Statemt_Tax_SFU
		@branch_id int,
        @AgentShortName varchar (20),
        @End_date datetime,
        @Basis varchar (20),
        @TypeOfCurrency	Varchar(30),
        @GroupByCode	Varchar(30),
	@TransactionType Varchar(30),
	@AgentGroupCode Varchar(30)
   AS

SET NOCOUNT ON

/*
declare @AgentShortName varchar (20), @End_date datetime, @branch_id int, @Basis varchar(20)
select @End_date = CONVERT(datetime,'2003-06-30 23:59:59') ,
 @Basis =
'Transaction Date',
--'Effective Date',
@AgentShortName =
--'testagent'
--'jsjohnson'
'abacoins'
*/
 /*Get System Currency Details*/
	declare @SystemCurrencyCode varchar(10)
	declare @SystemCurrencyDesc varchar(255)
    SELECT
    	@SystemCurrencyCode = c.iso_code,
    	@SystemCurrencyDesc = c.description
    FROM PMSystem pms
    JOIN currency c
    	ON c.currency_id = pms.currency_id
    WHERE pms.system_id = 1
/*end  Get System Currency*/

-- get a list of accounts with outstanding balance, or check the balance of the selected account
DECLARE @balance decimal (19,4)
declare @ibranchid int
IF @branch_id IS NULL
		SELECT @iBranchID = 0
	ELSE
		SELECT @iBranchID = @branch_id

CREATE TABLE #AccountsWithOSBalance
(AccountID int)
IF @AgentShortName = 'ALL'
BEGIN
    INSERT INTO #AccountsWithOSBalance
        SELECT account_id
        FROM transdetail
        GROUP BY account_id
        HAVING sum(amount) <> 0
    --select * from  #AccountsWithOSBalance

END
ELSE
BEGIN
    SELECT @balance = Case @TypeOfCurrency 
    				When 'Base' Then sum(t.amount) 
				When 'system' Then sum(t.system_amount)
				WHEN 'Transaction' THEN sum(t.Currency_amount)
        			WHEN 'Account' THEN sum(t.account_amount)
    			END
    FROM transdetail t
    JOIN account a ON a.account_id = t.account_id
    WHERE a.short_code = @AgentShortName
    --select @balance
END

-- get initial values together
CREATE TABLE #tempAgentStatTax
(
    AgentResolvedName       varchar (387) NULL,
    LedgerID                smallint NULL,
    TransType               varchar (10) NULL,
    AccountKey              int NULL,
    AccountID               int,
    DocRef                  varchar (25) NULL,
    DocDate                 datetime NULL,
    CreateDate              datetime NULL,
    InsuranceRef            varchar (30) NULL,
    EffDate                 datetime NULL,
    FirstTransDate          datetime NULL,
    GrossAmount             decimal (19,4) NULL,
    Settled                 decimal (19,4) NULL,
    Settled2                decimal (19,4) NULL,
    SettledCash             decimal (19,4) NULL,
    CommissionAmount        decimal (19,4) NULL,
    CommissionPercent       decimal (19,4) NULL,
    TaxAmount               decimal (19,4) NULL,
    InsuredName             varchar (255) NULL,
    FullyMatched            int NULL,                   -- can't trust it *...but should use it anyway!
    InsFileCnt              int NULL,
    Spare                   varchar(100) NULL,
    CashAllocStatus         int NULL,                   -- 1 = Unallocated; 2 = Posted; 3 = Allocated; 4 = Part (Under)
    Exclude                 int NULL,                    -- Exclude Cash records if fully allocated - NO!
    CompanyID				INT Null,
    CurrencyCode			Varchar(30) Null,
    CurrencyDesc			Varchar(255)	NULL,
	CompanyCode				Varchar(30) Null,
    CompanyDesc				Varchar(255)	NULL,
    DueDate                 datetime NULL
)                                                       -- Exclude fully allocated Data Transfer items (AllocatedX) - YES!

                                                        -- Exclude DD items which are not Commission -- YES!


INSERT INTO #tempAgentStatTax
    SELECT NULL,                                            --AgentResolvedName
        l.ledger_id,                                        --LedgerID
        (SELECT CASE dt.code
            WHEN 'JN' THEN 'JRN'
            WHEN 'SND' THEN 'NBD'
            WHEN 'SNC' THEN 'NBC'
            WHEN 'SWD' THEN 'WO'
            WHEN 'SRD' THEN 'RND'
            WHEN 'SRC' THEN 'RNC'
            WHEN 'SED' THEN 'END'
            WHEN 'SEC' THEN 'END'
            WHEN 'SRP' THEN 'REC'
            WHEN 'SPY' THEN 'PAY'
            WHEN 'CLP' THEN 'CLMP'
            WHEN 'CLR' THEN 'CLMR'
            ELSE dt.code
            END
        FROM documenttype dt
        WHERE dt.documenttype_id = d.documenttype_id
        ),                                                  --TransType
        a.account_key,                                      --AccountKey
        a.account_id,
        d.document_ref,                                     --DocRef
        d.document_date,                                    --DocDate
        d.created_date,                                     --CreateDate
        CASE d.documenttype_id
            WHEN 22 THEN c.media_ref
            WHEN 23 THEN c.media_ref
            ELSE t.insurance_ref
        END,                                                --InsuranceRef
        isnull (tef.cover_start_date,
                d.document_date),                           --EffDate
        NULL,                                               --FirstTransDate
        (SELECT 
        	Case @TypeOfCurrency
        		When 'Base' Then t.amount
        		When 'System' Then t.system_amount
        		WHEN 'Transaction' THEN t.Currency_amount
        		WHEN 'Account' THEN t.account_amount
        	END
            WHERE isnull(spare,'') <> 'COMM'
            AND isnull(spare,'') <> 'TAX'
            AND isnull(spare,'')  <> 'ALLOCATED'),                      --GrossAmount
        CASE WHEN t.fully_matched = 1 and t.outstanding_amount = 0
        	THEN
				Case @TypeOfCurrency
					When 'Base' Then t.amount
					When 'System' Then t.system_amount
					WHEN 'Transaction' THEN t.Currency_amount
					WHEN 'Account' THEN t.account_amount
        		END
			ELSE
        (SELECT SUM(ad.alloc_base_amount)
            FROM allocationdetail ad
            WHERE t.transdetail_id = ad.transdetail_id
				AND datediff(day, ad.accounting_date, @End_date) >= 0)
			END,           														--Settled
        (SELECT t.amount WHERE spare = 'ALLOCATED'),        					--Settled2 data transfer adjustment
		CASE WHEN t.fully_matched = 1 and t.outstanding_amount = 0
			THEN
				Case @TypeOfCurrency
					WHEN 'Base' THEN t.amount*-1
					WHEN 'System' THEN t.system_amount*-1
					WHEN 'Transaction' THEN t.Currency_amount*-1
					WHEN 'Account' THEN t.account_amount*-1
				END
			ELSE
				(SELECT SUM(ad.alloc_base_amount)
            FROM allocationdetail ad
            JOIN cashlistitem cl ON cl.cashlistitem_id = ad.cashlistitem_id
            WHERE t.transdetail_id = cl.transdetail_id
				AND datediff(day, ad.accounting_date, @End_Date) > = 0)
            END,                                            -- SettledCash
        (SELECT Case @TypeofCurrency 
	        		When 'Base' THEN t.amount 
        			WHEN 'System' THEN T.system_amount
        			WHEN 'Transaction' THEN t.Currency_amount
        			WHEN 'Account' THEN t.account_amount
        		END
        		WHERE spare = 'COMM'),             --CommissionAmount
        NULL,                                               --CommissionPercent
        (SELECT Case @TypeOfCurrency 
        			When 'Base' Then t.amount 
        			When 'system' Then t.system_amount
        			WHEN 'Transaction' THEN t.Currency_amount
        			WHEN 'Account' THEN t.account_amount
        		END
        			WHERE spare = 'TAX'),              --TaxAmount
        CASE d.documenttype_id
            WHEN 22 THEN c.our_ref
            WHEN 23 THEN c.our_ref
            ELSE NULL
        END,                                                --InsuredName
        t.fully_matched,                                    --FullyMatched
        tef.insurance_file_cnt,       						--InsFileCnt
        t.spare,                                            --Spare
        c.allocationstatus_id,                              --CashAllocStatus
        0,                                                   --Exclude
        CO.Company_id,
        Case @TypeOfCurrency 
        	WHEN 'Base'  THEN CB.Code
        	WHEN 'System' THEN @SystemcurrencyCode
        	WHEN 'Transaction' THEN CT.Code
        	WHEN 'Account' THEN CA.Code
        END,
        Case @TypeOfCurrency 
		     WHEN 'Base'  THEN CB.Description
		     WHEN 'System' THEN @SystemcurrencyDesc
		     WHEN 'Transaction' THEN CT.Description
		     WHEN 'Account' THEN CA.Description
        END,
        CO.Code,CO.Description,t.due_date 
    FROM Account a
    JOIN Ledger l                    ON a.ledger_id = l.ledger_id
    JOIN transdetail t               ON a.account_id = t.account_id
    JOIN Company CO					 ON CO.company_id= t.company_id
    Join Currency CB				 ON CB.currency_id = CO.base_currency /*Base Currency*/
    JOIN Currency CT				 ON CT.Currency_id= t.Currency_id /*Transaction Currency*/
    JOIN Currency CA				 ON CA.Currency_id = a.currency_id /*Account Currency*/
    LEFT JOIN CashListItem c   			 ON c.transdetail_id = t.transdetail_id
    JOIN  Document d                 ON t.document_id = d.document_id
    LEFT JOIN transaction_export_folder tef ON tef.document_ref = d.document_ref
    WHERE l.ledger_name IN ('Agent','Commission')
    AND (
    	(datediff(day, d.document_date, @End_date) >= 0 AND @Basis = 'Transaction Date')
    	OR
    	(datediff(day, isnull(tef.cover_start_date, d.document_date),  @End_date) >= 0 AND @Basis = 'Effective Date')
    	OR
    	(datediff(day, t.due_date , @End_date) >= 0 AND @Basis = 'Due Date')
    	)
    AND ((a.short_code = @AgentShortName        -- specific Agent
        AND @balance <> 0)
        OR
        (@AgentShortName = 'ALL'                -- all Agents
        AND a.account_id IN (SELECT accountID FROM #AccountsWithOSBalance)
        ))
	AND ( @iBranchID= 0
              or    (   @iBranchID <> 0 and a.company_id = @iBranchID ))

-- Decide which receipts and payments to exclude from the report
UPDATE #tempAgentStatTax
    SET Exclude = 1
    WHERE TransType in ('REC','PAY')
    AND FullyMatched = 1


-- Decide which Direct Debit items to exclude
-- (Commission amounts should be displayed on the report)

UPDATE #tempAgentStatTax
    SET Exclude = 1
    WHERE InsFileCnt IN (SELECT Insurance_File_Cnt FROM pfpremiumfinance WHERE statusIND = '040')
    AND (Spare <> 'COMM'
        AND FullyMatched = 1)

-- Use SettledCash IF Settled has no value, Payments and Receipts only
UPDATE #tempAgentStatTax
    SET Settled = -SettledCash
    WHERE TransType in ('REC','PAY')
    AND Isnull(Settled,0) = 0

--Print 'Update with Agent Resolved Name, and Insured Resolved Name'
    UPDATE #tempAgentStatTax
    SET AgentResolvedName = pAgent.resolved_name
    FROM party pAgent
    WHERE pAgent.party_cnt = AccountKey

/*  UPDATE #tempAgentStatTax
    SET CommissionPercent = commission_percentage
    FROM Agent_Commission
    WHERE insurance_file_cnt IN
   		(SELECT insurance_file_cnt 
   		FROM  transaction_export_folder 
   		WHERE document_ref = DocRef)   */
    
    UPDATE #tempAgentStatTax
    SET InsuredName =  pClient.resolved_name
    FROM party pClient
    WHERE pClient.party_cnt =
        (SELECT max(insured_cnt)
        FROM Insurance_File WHERE insurance_ref = InsuranceRef
        AND isnull(InsuranceRef, '') <> ''
        )
    AND TransType NOT IN ('REC', 'PAY')

-- Need to sort out the first trans date with the final set of data
-- new temp table

CREATE TABLE #FinalAgentStatT
(
    Company                 varchar (255) NULL,
    CompanyAddress1         varchar (40) NULL,
    CompanyAddress2         varchar (40) NULL,
    CompanyAddress3         varchar (40) NULL,
    CompanyAddress4         varchar (40) NULL,
    CompanyPostCode         varchar (40) NULL,
    PhoneAreaCode           varchar (10) NULL,
    PhoneNumber             varchar (15) NULL,
    PhoneExtension          varchar (6) NULL,
    FaxAreaCode             varchar (10) NULL,
    FaxNumber               varchar (15) NULL,
    AgentResolvedName       varchar (255) NULL,
    TransType               varchar (10) NULL,
    AccountID 		    	int NULL,
    AccountCode             varchar (30) NULL,
    AccountAddress1         varchar (40) NULL,
    AccountAddress2         varchar (40) NULL,
    AccountAddress3         varchar (40) NULL,
    AccountAddress4         varchar (40) NULL,
    AccountPostCode         varchar (40) NULL,
    DocRef                  varchar (25) NULL,
    DocDate                 datetime NULL,
    InsuranceRef            varchar (30) NULL,
    EffDate                 datetime NULL,
    FirstTransDate          datetime NULL,
    GrossAmount             decimal (19,4) NULL,
    Settled                 decimal (19,4) NULL,
    Settled2                decimal (19,4) NULL,
    SettledCash             decimal (19,4) NULL,
    CommissionAmount        decimal (19,4) NULL,
    CommissionPercent       decimal (19,4) NULL,
    TaxAmount               decimal (19,4) NULL,
    InsuredName             varchar (255) NULL,
    CompanyID			Int NULL,
    CurrencyCode		Varchar(30) Null,
    CurrencyDesc		Varchar(255) NULL,
    CompanyCode			Varchar(30) Null,
    CompanyDesc			Varchar(255) NULL,
    Reference			Varchar(50) NULL,
    DueDate datetime NULL
)

INSERT INTO #FinalAgentStatT
    SELECT  NULL, NULL, NULL, NULL, NULL, NULL,
            NULL, NULL, NULL, NULL, NULL,
            Max(AgentResolvedName),
            Max(TransType), 
	    	AccountID,
            NULL, NULL, NULL,
            NULL, NULL, NULL,
            DocRef, Max(DocDate),
            Max(InsuranceRef), Max(EffDate), NULL,
            sum(GrossAmount),
            sum(Settled), sum(Settled2),
            sum(SettledCash), sum(CommissionAmount),
            max(CommissionPercent),sum(TaxAmount),
	    Max(InsuredName),CompanyID,CurrencyCode,CurrencyDesc,CompanyCode,CompanyDesc,
	    NULL,MAX(DueDate )
    FROM    #tempAgentStatTax
    WHERE  ((GrossAmount <> 0 AND TransType = 'payment') OR TransType <> 'payment')
    AND    Exclude = 0
    GROUP BY AccountID, DocRef,CompanyID,CurrencyCode,CurrencyDesc,CompanyCode,CompanyDesc
    HAVING (isnull(sum(GrossAmount),0)
            + isnull(sum(CommissionAmount),0)
            + isnull(sum(TaxAmount),0)
            - (
             (isnull(sum(Settled),isnull(sum(SettledCash)*-1,0)))
            - isnull(sum(Settled2),0)
            ))
            <> 0            -- balance per Document <> zero


-- Get together policy numbers and their earliest doc date
CREATE TABLE #tempDocDates
(
    FirstDocDate    datetime,
    InsRef          varchar (30)
)

INSERT INTO #tempDocDates
    SELECT min(DocDate), InsuranceRef
    FROM #FinalAgentStatT
    GROUP BY InsuranceRef


--Print 'Update with earliest DocDate for each InsuranceRef'
UPDATE #FinalAgentStatT
    SET FirstTransDate = FirstDocDate
    FROM #tempDocDates
    WHERE InsuranceRef = InsRef

--Print 'Use DocDate for Transactions with no InsuranceRef'
UPDATE #FinalAgentStatT
    SET FirstTransDate = DocDate
    WHERE isnull(InsuranceRef,'') = ''

--Print 'Update with Agent details'
UPDATE #FinalAgentStatT
    SET AccountCode = a.short_code,
        AccountAddress1 = a.address1,
        AccountAddress2 = a.address2,
        AccountAddress3 = a.address3,
        AccountAddress4 = a.address4,
        AccountPostCode = a.postal_code
    FROM Account a
    WHERE a.account_id = AccountID

--Print 'Update with Company details'
UPDATE #FinalAgentStatT
    SET Company = s.Description,
        CompanyAddress1 = s.Address1,
        CompanyAddress2 = s.Address2,
        CompanyAddress3 = s.Address3,
        CompanyAddress4 = s.Address4,
        CompanyPostCode = s.postal_code,
        PhoneAreaCode = s.Phone_Area_Code,
        PhoneNumber = s.Phone_Number,
        PhoneExtension = s.Phone_Extension,
        FaxAreaCode = s.Fax_Area_Code,
        FaxNumber = s.Fax_Number
    FROM Source s WHERE S.Code = #FinalAgentStatT.CompanyCode
    --WHERE s.Source_Id = 1   -- 1 = Head Office*******changed on 10-08-2004 by jitendra


-- TB30/01/03 - set the commission percent field
    UPDATE #FinalAgentStatT
        SET CommissionPercent = ABS( ROUND( (CommissionAmount * 100 / GrossAmount ), 2))
    WHERE ISNULL(GrossAmount,0) <> 0

UPDATE #FinalAgentStatT SET Reference = cp.thirdpartyreference 
FROM #FinalAgentStatT INNER JOIN stats_folder SF ON #FinalAgentStatT.docref = SF.document_ref
INNER JOIN claim_payment CP ON CP.claim_payment_id = SF.payment_id 
	AND LEFT(SF.document_ref, 3) like 'CLP' 
	AND cp.thirdpartyreference IS NOT NULL

SET NOCOUNT OFF
-- OUTPUT

PRINT @AgentGroupCode

IF LOWER(@AgentGroupCode) = 'all'
 BEGIN

PRINT 'ENTER1'

  IF 	@TransactionType='Premium Transactions Only'
  BEGIN
  SELECT Company,
  		CompanyAddress1,
  		CompanyAddress2,
  		CompanyAddress3,
  		CompanyAddress4,
  		CompanyPostCode,
  		PhoneAreaCode,
  		PhoneNumber,
  		PhoneExtension,
  		FaxAreaCode,
  		FaxNumber,
  		AgentResolvedName,
  		TransType,
  		AccountCode,
  		AccountAddress1,
  		AccountAddress2,
  		AccountAddress3,
  		AccountAddress4,
  		AccountPostCode,
  		DocRef,
  		DocDate,
  		InsuranceRef,
  		EffDate,
  		FirstTransDate,
  		GrossAmount,
  		Settled,
  		Settled2,
  		SettledCash,
  		CommissionAmount,
  		CommissionPercent,
  		TaxAmount,
  		InsuredName,
  		CASE WHEN @Basis = 'Transaction Date'
  			THEN
  				DocDate
  			ELSE
  				isnull(EffDate, DocDate)
  			END ReportingDate,
  		CurrencyCode,CurrencyDesc,CompanyCode,CompanyDesc,
		Reference,
  		Case @GroupByCode
  			WHEN 'Branch' then CompanyCode
  			WHEN 'Branch And Company' THEN CompanyCode
  			WHEN 'Currency' THEN CurrencyCode
  		ElSE	''
  		END 'GroupByCode',DueDate 
  FROM  #FinalAgentStatT WHERE docRef not like 'c%'
  END

  IF 	@TransactionType='Claims Transactions Only'
  BEGIN
  SELECT Company,
  		CompanyAddress1,
  		CompanyAddress2,
  		CompanyAddress3,
  		CompanyAddress4,
  		CompanyPostCode,
  		PhoneAreaCode,
  		PhoneNumber,
  		PhoneExtension,
  		FaxAreaCode,
  		FaxNumber,
  		AgentResolvedName,
  		TransType,
  		AccountCode,
  		AccountAddress1,
  		AccountAddress2,
  		AccountAddress3,
  		AccountAddress4,
  		AccountPostCode,
  		DocRef,
  		DocDate,
  		InsuranceRef,
  		EffDate,
  		FirstTransDate,
  		GrossAmount,
  		Settled,
  		Settled2,
  		SettledCash,
  		CommissionAmount,
  		CommissionPercent,
  		TaxAmount,
  		InsuredName,
  		CASE WHEN @Basis = 'Transaction Date'
  			THEN
  				DocDate
  			ELSE
  				isnull(EffDate, DocDate)
  			END ReportingDate,
  		CurrencyCode,CurrencyDesc,CompanyCode,CompanyDesc,
		Reference,
  		Case @GroupByCode
  			WHEN 'Branch' then CompanyCode
  			WHEN 'Branch And Company' THEN CompanyCode
  			WHEN 'Currency' THEN CurrencyCode
  		ElSE	''
  		END 'GroupByCode',DueDate 
  FROM  #FinalAgentStatT WHERE docRef like 'c%'
  END

  IF 	@TransactionType='Premium & Claim Transactions'
  BEGIN
  SELECT Company,
  		CompanyAddress1,
  		CompanyAddress2,
  		CompanyAddress3,
  		CompanyAddress4,
  		CompanyPostCode,
  		PhoneAreaCode,
  		PhoneNumber,
  		PhoneExtension,
  		FaxAreaCode,
  		FaxNumber,
  		AgentResolvedName,
  		TransType,
  		AccountCode,
  		AccountAddress1,
  		AccountAddress2,
  		AccountAddress3,
  		AccountAddress4,
  		AccountPostCode,
  		DocRef,
  		DocDate,
  		InsuranceRef,
  		EffDate,
  		FirstTransDate,
  		GrossAmount,
  		Settled,
  		Settled2,
  		SettledCash,
  		CommissionAmount,
  		CommissionPercent,
  		TaxAmount,
  		InsuredName,
  		CASE WHEN @Basis = 'Transaction Date'
  			THEN
  				DocDate
  			ELSE
  				isnull(EffDate, DocDate)
  			END ReportingDate,
  		CurrencyCode,CurrencyDesc,CompanyCode,CompanyDesc,
		Reference,
  		Case @GroupByCode
  			WHEN 'Branch' then CompanyCode
  			WHEN 'Branch And Company' THEN CompanyCode
  			WHEN 'Currency' THEN CurrencyCode
  		ElSE	''
  		END 'GroupByCode',DueDate 
  FROM  #FinalAgentStatT
  END

END


IF LOWER(@AgentGroupCode) <> 'all'
 BEGIN

PRINT 'ENTER2'

  IF 	@TransactionType='Premium Transactions Only'
  BEGIN

PRINT 'ENTER2.1'

  SELECT Company,
  		CompanyAddress1,
  		CompanyAddress2,
  		CompanyAddress3,
  		CompanyAddress4,
  		CompanyPostCode,
  		PhoneAreaCode,
  		PhoneNumber,
  		PhoneExtension,
  		FaxAreaCode,
  		FaxNumber,
  		AgentResolvedName,
  		TransType,
  		AccountCode,
  		AccountAddress1,
  		AccountAddress2,
  		AccountAddress3,
  		AccountAddress4,
  		AccountPostCode,
  		DocRef,
  		DocDate,
  		InsuranceRef,
  		EffDate,
  		FirstTransDate,
  		GrossAmount,
  		Settled,
  		Settled2,
  		SettledCash,
  		CommissionAmount,
  		CommissionPercent,
  		TaxAmount,
  		InsuredName,
  		CASE WHEN @Basis = 'Transaction Date'
  			THEN
  				DocDate
  			ELSE
  				isnull(EffDate, DocDate)
  			END ReportingDate,
  		CurrencyCode,CurrencyDesc,CompanyCode,CompanyDesc,
		Reference,
  		Case @GroupByCode
  			WHEN 'Branch' then CompanyCode
  			WHEN 'Branch And Company' THEN CompanyCode
  			WHEN 'Currency' THEN CurrencyCode
  		ElSE	''
  		END 'GroupByCode',DueDate 
  FROM  #FinalAgentStatT WHERE docRef not like 'c%'
        --RC-- 13 Jun 2006
        and AgentResolvedName IN(
        select trading_name from party_agent where linked_account_group = (
        select  party_cnt from party where shortname = @AgentGroupCode) )
        --RC-- 13 Jun 2006
  END

  IF 	@TransactionType='Claims Transactions Only'
  BEGIN

PRINT 'ENTER2.2'

  SELECT Company,
  		CompanyAddress1,
  		CompanyAddress2,
  		CompanyAddress3,
  		CompanyAddress4,
  		CompanyPostCode,
  		PhoneAreaCode,
  		PhoneNumber,
  		PhoneExtension,
  		FaxAreaCode,
  		FaxNumber,
  		AgentResolvedName,
  		TransType,
  		AccountCode,
  		AccountAddress1,
  		AccountAddress2,
  		AccountAddress3,
  		AccountAddress4,
  		AccountPostCode,
  		DocRef,
  		DocDate,
  		InsuranceRef,
  		EffDate,
  		FirstTransDate,
  		GrossAmount,
  		Settled,
  		Settled2,
  		SettledCash,
  		CommissionAmount,
  		CommissionPercent,
  		TaxAmount,
  		InsuredName,
  		CASE WHEN @Basis = 'Transaction Date'
  			THEN
  				DocDate
  			ELSE
  				isnull(EffDate, DocDate)
  			END ReportingDate,
  		CurrencyCode,CurrencyDesc,CompanyCode,CompanyDesc,
		Reference,
  		Case @GroupByCode
  			WHEN 'Branch' then CompanyCode
  			WHEN 'Branch And Company' THEN CompanyCode
  			WHEN 'Currency' THEN CurrencyCode
  		ElSE	''
  		END 'GroupByCode',DueDate 
  FROM  #FinalAgentStatT WHERE docRef like 'c%'
        --RC-- 13 Jun 2006
        and AgentResolvedName IN(
        select trading_name from party_agent where linked_account_group = (
        select  party_cnt from party where shortname = @AgentGroupCode) )
        --RC-- 13 Jun 2006
  END

  IF 	@TransactionType='Premium & Claim Transactions'
  BEGIN

PRINT 'ENTER2.3'

  SELECT Company,
  		CompanyAddress1,
  		CompanyAddress2,
  		CompanyAddress3,
  		CompanyAddress4,
  		CompanyPostCode,
  		PhoneAreaCode,
  		PhoneNumber,
  		PhoneExtension,
  		FaxAreaCode,
  		FaxNumber,
  		AgentResolvedName,
  		TransType,
  		AccountCode,
  		AccountAddress1,
  		AccountAddress2,
  		AccountAddress3,
  		AccountAddress4,
  		AccountPostCode,
  		DocRef,
  		DocDate,
  		InsuranceRef,
  		EffDate,
  		FirstTransDate,
  		GrossAmount,
  		Settled,
  		Settled2,
  		SettledCash,
  		CommissionAmount,
  		CommissionPercent,
  		TaxAmount,
  		InsuredName,
  		CASE WHEN @Basis = 'Transaction Date'
  			THEN
  				DocDate
  			ELSE
  				isnull(EffDate, DocDate)
  			END ReportingDate,
  		CurrencyCode,CurrencyDesc,CompanyCode,CompanyDesc,
		Reference,
  		Case @GroupByCode
  			WHEN 'Branch' then CompanyCode
  			WHEN 'Branch And Company' THEN CompanyCode
  			WHEN 'Currency' THEN CurrencyCode
  		ElSE	''
  		END 'GroupByCode',DueDate 
  FROM  #FinalAgentStatT
        --RC-- 13 Jun 2006
        WHERE AgentResolvedName IN(
        select trading_name from party_agent where linked_account_group = (
        select  party_cnt from party where shortname = @AgentGroupCode) )
        --RC-- 13 Jun 2006
  END

END

DROP TABLE #AccountsWithOSBalance
DROP TABLE #tempDocDates
DROP TABLE #tempAgentStatTax
DROP TABLE #FinalAgentStatT

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
