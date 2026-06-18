SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

Execute DDLDropProcedure 'spu_Report_Agent_Statemt_SFU'
GO

/****** AAB - 01/21/2004 - PN 8786 -  Added bands, we will always   ******/
/*****         get a single record to use in the report header.                  ******/
/***** Alix - Removed the above, as it didn't work at all! Now if no record match criteria, ******/
/****           we don't get anything! *****/
--Modification
--10/10/04  JT  PN-16023 changed the where condition added from and to date.
--**13Jun2006	RC		Filter by Agent Group

CREATE PROCEDURE spu_Report_Agent_Statemt_SFU
        @branch_id  int,
        @AgentShortName varchar (40),
        @start_Date nvarchar(50),    
        @End_date   nvarchar(50),  
        @Basis      varchar (40),
        @Underwriting_Year varchar(40),
        @TypeOfCurrency Varchar(40),
        @GroupByCode    Varchar(40),
        @IncludeBalanceAccount as varchar(10),
    	@TransactionType   Varchar(30),
    	@AgentGroupCode Varchar(30)
AS
SET NOCOUNT ON
SELECT @start_Date = CONVERT(DATETIME, @start_date, 103),    
    @End_date = CONVERT(DATETIME, @end_date, 103)
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

DECLARE @balance    decimal (19,4)
DECLARE @ibranchid  int

--Select the Branch
IF @branch_id IS NULL
    SELECT @iBranchID = 0
ELSE
    SELECT @iBranchID = @branch_id

CREATE TABLE #AccountsWithOSBalance (AccountID int)


--Select the Agent Account ID
IF @AgentShortName = 'ALL'
    BEGIN
        INSERT INTO #AccountsWithOSBalance
            SELECT      account_id
            FROM        transdetail
            GROUP BY    account_id
            HAVING      sum(amount) <> 0
    END
ELSE
    BEGIN
        SELECT  @balance = Case @TypeOfCurrency
                            WHEN 'Base' Then  isnull(sum(amount),0)
                            WHEN 'System' Then isnull(sum(system_amount),0)
                            WHEN 'Account' THEN isnull(sum(t.account_amount),0)
                            WHEN 'Transaction' THEN isnull(sum(t.currency_amount),0)
                           END
        FROM    transdetail t
        JOIN    account a ON a.account_id = t.account_id
        WHERE   a.short_code = @AgentShortName
    END

-- If underwriting year is not passed in, set to ALL
if isnull(@Underwriting_Year, '') = ''
    set @Underwriting_Year = 'ALL'

CREATE TABLE #tempAgentStat
(
    AgentResolvedName       varchar(255)    NULL,
    LedgerID                smallint        NULL,
    TransType               varchar(30)     NULL,
    AccountKey              int             NULL,
    AccountID               int,
    DocRef                  varchar(25)     NULL,
    DocDate                 datetime        NULL,
    CreateDate              datetime        NULL,
    InsuranceRef            varchar(30)     NULL,
    EffDate                 datetime        NULL,
    FirstTransDate          datetime        NULL,
    GrossAmount             decimal(19,4)   NULL,
    DisplayGrossAmount      decimal(19,4)   NULL,
    Settled                 decimal(19,4)   NULL,
    Settled2                decimal(19,4)   NULL,
    SettledCash             decimal(19,4)   NULL,
    CommissionAmount        decimal(19,4)   NULL,
    CommissionPercent       decimal(19,4)   NULL,
    InsuredName             varchar(255)    NULL,
    FullyMatched            int             NULL,
    InsFileCnt              int             NULL,
    Spare                   varchar(100)    NULL,
    CashAllocStatus         int             NULL,
    Exclude                 int             NULL,
    Underwriting_year       char (10)       NULL,                    -- Exclude Cash records if fully allocated - NO!
    CompanyID               INT Null,
    CurrencyCode            Varchar(30)     Null,
    CurrencyDesc            Varchar(255)    NULL,
    CompanyCode             Varchar(30) Null,
    CompanyDesc             Varchar(255)    NULL,
    DueDate                 datetime        NULL
)
INSERT INTO #tempAgentStat
    SELECT  NULL,
            l.ledger_id,
            (SELECT dt.description
             FROM   documenttype dt
             WHERE  dt.documenttype_id = d.documenttype_id),
            a.account_key,
            a.account_id,
            d.document_ref,
            d.document_date,
            d.created_date,
            CASE    d.documenttype_id
                WHEN 22 THEN c.our_ref
                WHEN 23 THEN c.our_ref
            ELSE
                t.insurance_ref
            END,
            ISNULL (tef.cover_start_date,d.document_date),
            NULL,
            (SELECT
            Case @TypeOfCurrency
                When 'Base' Then t.amount
                When 'System' Then t.system_amount
                When 'Account' Then t.account_amount
                WHEN 'Transaction' THEN t.currency_amount
            END
            WHERE isnull(spare,'')  <> 'ALLOCATED'
            AND isnull(spare,'') <> 'COMM'), -- to fix PN25935

            -- Marcus think we need to include tax and comm
            --WHERE isnull(spare,'') <> 'COMM'
            --AND isnull(spare,'') <> 'TAX'
            --AND isnull(spare,'')  <> 'ALLOCATED'),

            0,
            CASE WHEN t.fully_matched = 1  AND t.outstanding_amount = 0 THEN
                Case @TypeOfCurrency
                    When 'Base' Then t.amount
                    When 'System' Then t.system_amount
                    WHEN 'Account' THEN t.account_amount
                    WHEN 'Transaction' THEN t.currency_amount
                END
            ELSE
                (SELECT SUM(ad.alloc_base_amount)
                 FROM   allocationdetail ad
                 WHERE  t.transdetail_id = ad.transdetail_id AND
--              datediff(day, ad.accounting_date, @End_date) >= 0)
                (ad.accounting_date >= @start_date AND ad.accounting_date <= @end_Date))
            END,
            (SELECT t.amount WHERE spare = 'ALLOCATED'),
            CASE WHEN t.fully_matched = 1 AND t.outstanding_amount = 0 THEN
                Case @TypeOfCurrency
                    WHEN 'Base' THEN t.amount*-1
                    WHEN 'System' THEN t.system_amount*-1
                    WHEN 'Account' THEN t.account_amount*-1
                    WHEN 'Transaction' THEN t.currency_amount*-1
                END
            ELSE
                (SELECT SUM(ad.alloc_base_amount)
                 FROM   allocationdetail ad JOIN
                        cashlistitem cl ON cl.cashlistitem_id = ad.cashlistitem_id
                 WHERE  t.transdetail_id = cl.transdetail_id AND
--                      datediff(day, ad.accounting_date, @End_Date) > = 0)
                 (ad.accounting_date >= @start_date AND ad.accounting_date <= @end_Date))
            END,
            (SELECT Case @TypeofCurrency
                    When 'Base' THEN t.amount
                    WHEN 'System' THEN T.system_amount
                    WHEN 'Account' THEN t.account_amount
                    WHEN 'Transaction' THEN t.currency_amount
                END
                WHERE spare = 'COMM'),
            NULL,
            CASE d.documenttype_id
                WHEN 22 THEN c.media_ref
                WHEN 23 THEN c.media_ref
            ELSE
                NULL
            END,
            CASE t.outstanding_amount WHEN 0 THEN 1 ELSE 0 END,
            tef.insurance_file_cnt,
            t.spare,
            c.allocationstatus_id,
            0,
            underwriting_year.code,                                                   --Exclude
            CO.Company_id,
            Case @TypeOfCurrency
                WHEN 'Base'  THEN CB.Code
                WHEN 'System' THEN @SystemcurrencyCode
                WHEN 'Account' THEN CA.Code
                WHEN 'Transaction' THEN CT.Code
            END,
            Case @TypeOfCurrency
                 WHEN 'Base'  THEN CB.Description
                 WHEN 'System' THEN @SystemcurrencyDesc
                 WHEN 'Account' THEN CA.Description
                 WHEN 'Transaction' THEN CT.Description
            END,
            CO.Code,CO.Description,
            t.due_date 
    FROM    Account a
    JOIN    Ledger l                        ON a.ledger_id = l.ledger_id
    JOIN    transdetail t                   ON a.account_id = t.account_id
    LEFT JOIN CashListItem c                ON c.transdetail_id = t.transdetail_id
    JOIN    Document d                      ON t.document_id = d.document_id
    LEFT JOIN transaction_export_folder tef ON tef.document_ref = d.document_ref
    LEFT JOIN underwriting_year             ON t.underwriting_year_id = underwriting_year.underwriting_year_id
    JOIN    Company CO                      ON CO.company_id= t.company_id
    JOIN    Currency CB                     ON CB.currency_id = CO.base_currency
    JOIN    Currency CA                     ON CA.currency_id = a.currency_id
    JOIN    Currency CT                     ON CT.Currency_id = t.currency_id
    WHERE   l.ledger_name IN ('Agent','Commission') AND
            (
				(d.document_date >= @start_date AND  d.document_date <= @End_date AND @Basis = 'Transaction Date')
              OR (isnull(tef.cover_start_date, d.document_date) >= @start_date AND
              isnull(tef.cover_start_date, d.document_date) <= @End_date AND @Basis = 'Effective Date')OR
              (t.due_date >= @start_date AND  t.due_date <= @End_date AND @Basis = 'Due Date')
             )
              AND ((a.short_code = @AgentShortName AND (@balance <> 0 OR @IncludeBalanceAccount = 'Yes'))
              OR (@AgentShortName = 'ALL' AND (a.account_id IN (SELECT accountID FROM #AccountsWithOSBalance) OR @IncludeBalanceAccount = 'Yes')))
            AND (@iBranchID= 0 OR (@iBranchID <> 0 AND d.company_id = @iBranchID ))
            AND (underwriting_year.code = @Underwriting_Year OR rtrim(@Underwriting_Year) = 'ALL')
            AND t.amount<>0
UPDATE #tempAgentStat
    SET     Exclude = 1
    WHERE   TransType in ('Receipt','Payment') AND FullyMatched = 1

UPDATE #tempAgentStat
    SET     Exclude = 1
    WHERE   InsFileCnt IN (SELECT   Insurance_File_Cnt
                           FROM     pfpremiumfinance
                           WHERE    statusIND = '040') AND
            (Spare <> 'COMM' AND FullyMatched = 1)

UPDATE #tempAgentStat
	SET DisplayGrossAmount = gross_amount
	FROM (Select document_ref, sum(amount) gross_amount From transdetail t 
	Left Join document d On d.document_id = t.document_id 
	where document_ref in (Select docref From #tempAgentStat) and 
	account_id in 
	(Select account_id From account join ledgertype ON ledger_id = ledgertype_id WHERE description='client')
	Group By document_ref, account_id) grosspremium WHERE 
	grosspremium.document_ref = #tempAgentStat.docref


UPDATE #tempAgentStat
    SET     AgentResolvedName = pAgent.resolved_name
    FROM    party pAgent
    WHERE   pAgent.party_cnt = AccountKey

UPDATE #tempAgentStat
    SET     CommissionPercent = commission_percentage
    FROM    Agent_Commission AC
    WHERE   insurance_file_cnt IN (SELECT   insurance_file_cnt
                                   FROM     transaction_export_folder
                                   WHERE    document_ref = DocRef)
		AND AC.party_cnt =  (SELECT TOP 1 INF.lead_agent_cnt 
                                   FROM     transaction_export_folder TEF JOIN Insurance_File INF On InF.insurance_file_cnt =  TEF.insurance_file_cnt
                                   WHERE    document_ref = DocRef)
		AND AC.commission_percentage <> 0
UPDATE #tempAgentStat
    SET     InsuredName =  pClient.resolved_name
    FROM    party pClient
    WHERE   pClient.party_cnt = (SELECT max(insured_cnt)
                                 FROM   Insurance_File
                                 WHERE  insurance_ref = InsuranceRef AND
                                        isnull(InsuranceRef, '') <> '') AND
                                        TransType NOT IN ('Receipt','Payment')

CREATE TABLE #FinalAgentStatT
(
    Company                 varchar(255)    NULL,
    CompanyAddress1         varchar(40)     NULL,
    CompanyAddress2         varchar(40)     NULL,
    CompanyAddress3         varchar(40)     NULL,
    CompanyAddress4         varchar(40)     NULL,
    CompanyPostCode         varchar(40)     NULL,
    PhoneAreaCode           varchar(10)     NULL,
    PhoneNumber             varchar(15)     NULL,
    PhoneExtension          varchar(6)      NULL,
    FaxAreaCode             varchar(10)     NULL,
    FaxNumber               varchar(15)     NULL,
    AgentResolvedName       varchar(255)    NULL,
    TransType               varchar(30)     NULL,
    AccountID               int             NULL,
    AccountCode             varchar(30)     NULL,
    AccountAddress1         varchar(40)     NULL,
    AccountAddress2         varchar(40)     NULL,
    AccountAddress3         varchar(40)     NULL,
    AccountAddress4         varchar(40)     NULL,
    AccountPostCode         varchar(40)     NULL,
    DocRef                  varchar(25)     NULL,
    DocDate                 datetime        NULL,
    InsuranceRef            varchar(30)     NULL,
    EffDate                 datetime        NULL,
    FirstTransDate          datetime        NULL,
    GrossAmount             decimal(19,4)   NULL,
    DisplayGrossAmount      decimal(19,4)   NULL,
    Settled                 decimal(19,4)   NULL,
    Settled2                decimal(19,4)   NULL,
    SettledCash             decimal(19,4)   NULL,
    CommissionAmount        decimal(19,4)   NULL,
    CommissionPercent       decimal(19,4)   NULL,
    InsuredName             varchar(255)    NULL,
    Band_1_Start            int             NULL,
    Band_2_Start            int             NULL,
    Band_3_Start            int             NULL,
    Band_4_Start            int             NULL,
    Band_5_Start            int             NULL,
    Band_1_End              int             NULL,
    Band_2_End              int             NULL,
    Band_3_End              int             NULL,
    Band_4_End              int             NULL,
    Underwriting_year       char (10)       NULL,
    CompanyID               Int             NULL,
    CurrencyCode            Varchar(30)     Null,
    CurrencyDesc            Varchar(255)    NULL,
    CompanyCode             Varchar(30)     Null,
    CompanyDesc             Varchar(255)    NULL,
    Reference		    Varchar(50)	    Null,
    DueDate                 datetime        NULL
)

-- Let's get the Aging bands for this report
Declare         @Band_1_Start     int,
                @Band_2_Start     int,
                @Band_3_Start     int,
                @Band_4_Start     int,
                @Band_5_Start     int,
                @Band_1_End       int,
                @Band_2_End       int,
                @Band_3_End       int,
                @Band_4_End       int,
                @Band_Branch      int

--Set the Band Branch must be = 1 at least
IF @branch_id IS NULL OR @branch_id = 0
    SELECT @Band_Branch = 1
ELSE
    SELECT @Band_Branch = @branch_id

--Get the Start
Execute spu_get_Aging_band 3001, @Band_Branch, @Band_1_Start Output
Execute spu_get_Aging_band 3003, @Band_Branch, @Band_2_Start Output
Execute spu_get_Aging_band 3005, @Band_Branch, @Band_3_Start Output
Execute spu_get_Aging_band 3007, @Band_Branch, @Band_4_Start Output
Execute spu_get_Aging_band 3009, @Band_Branch, @Band_5_Start Output

--Get the End
Execute spu_get_Aging_band 3002, @Band_Branch, @Band_1_End Output
Execute spu_get_Aging_band 3004, @Band_Branch, @Band_2_End Output
Execute spu_get_Aging_band 3006, @Band_Branch, @Band_3_End Output
Execute spu_get_Aging_band 3008, @Band_Branch, @Band_4_End Output


IF @IncludeBalanceAccount = 'Yes'
	BEGIN
		INSERT INTO #FinalAgentStatT
			SELECT      NULL, NULL, NULL, NULL, NULL, NULL,
					NULL, NULL, NULL, NULL, NULL,
					Max(AgentResolvedName),
					Max(TransType),
					AccountID,
					NULL, NULL, NULL,
					NULL, NULL, NULL,
					DocRef, Max(DocDate),
					Max(InsuranceRef), Max(EffDate), NULL,
					sum(GrossAmount) GrossAmount, avg(DisplayGrossAmount) DisplayGrossAmount,
					sum(Settled) Settled, sum(Settled2) Settled2,
					sum(SettledCash) SettledCash, sum(CommissionAmount) CommissionAmount,
					max(CommissionPercent) CommissionPercent, Max(InsuredName), 0,0,0,0,0,0,0,0,0,
					max(Underwriting_year),
					CompanyID,CurrencyCode,
					CurrencyDesc,CompanyCode,CompanyDesc,
					NULL,MAX(DueDate )
		FROM        #tempAgentStat
		WHERE       ((GrossAmount <> 0 AND TransType = 'payment') OR TransType <> 'payment') AND
					Exclude = 0
		GROUP BY    AccountID, DocRef,
					CompanyID,CurrencyCode,
					CurrencyDesc,CompanyCode,CompanyDesc
	END
    
ELSE
    BEGIN
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
					sum(GrossAmount) GrossAmount, avg(DisplayGrossAmount) DisplayGrossAmount,
					sum(Settled) Settled, sum(Settled2) Settled2,
					sum(SettledCash) SettledCash, sum(CommissionAmount) CommissionAmount,
					max(CommissionPercent) CommissionPercent, Max(InsuredName), 0,0,0,0,0,0,0,0,0,
					max(Underwriting_year),
					CompanyID,CurrencyCode,
					CurrencyDesc,CompanyCode,CompanyDesc,
					NULL,MAX(DueDate )
			FROM	#tempAgentStat
			WHERE       ((GrossAmount <> 0 AND TransType = 'payment') OR TransType <> 'payment') AND
					    Exclude = 0
			GROUP BY    AccountID, DocRef,
						CompanyID,CurrencyCode,
						CurrencyDesc,CompanyCode,CompanyDesc
			HAVING      (isnull(sum(GrossAmount), 0) +
						isnull(sum(CommissionAmount), 0) -
						((isnull(sum(Settled), isnull(sum(SettledCash)* -1, 0))) -
						isnull(sum(Settled2), 0))) <> 0
	END

CREATE TABLE #tempDocDates
(
    FirstDocDate    datetime,
    InsRef          varchar (30)
)

INSERT INTO #tempDocDates
    SELECT      min(DocDate), InsuranceRef
    FROM        #FinalAgentStatT
    GROUP BY    InsuranceRef

UPDATE #FinalAgentStatT
    SET     FirstTransDate = FirstDocDate
    FROM    #tempDocDates
    WHERE   InsuranceRef = InsRef

UPDATE #FinalAgentStatT
    SET     FirstTransDate = DocDate
    WHERE   isnull(InsuranceRef,'') = ''

UPDATE #FinalAgentStatT
    SET     AccountCode     = a.short_code,
            AccountAddress1 = a.address1,
            AccountAddress2 = a.address2,
            AccountAddress3 = a.address3,
            AccountAddress4 = a.address4,
            AccountPostCode = a.postal_code
    FROM    Account a
    WHERE   a.account_id = AccountID

UPDATE #FinalAgentStatT
    SET     Company = s.Description,
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
    FROM    Source s
    WHERE   S.Code = #FinalAgentStatT.CompanyCode

UPDATE #FinalAgentStatT
    Set     Band_1_Start = @Band_1_Start
UPDATE #FinalAgentStatT
    Set     Band_2_Start = @Band_2_Start
UPDATE #FinalAgentStatT
    Set     Band_3_Start = @Band_3_Start
UPDATE #FinalAgentStatT
    Set     Band_4_Start = @Band_4_Start
UPDATE #FinalAgentStatT
    Set     Band_5_Start = @Band_5_Start

UPDATE #FinalAgentStatT
    Set     Band_1_End = @Band_1_End
UPDATE #FinalAgentStatT
    Set     Band_2_End = @Band_2_End
UPDATE #FinalAgentStatT
    Set     Band_3_End = @Band_3_End
UPDATE #FinalAgentStatT
    Set     Band_4_End = @Band_4_End

UPDATE #FinalAgentStatT SET Reference = cp.thirdpartyreference
FROM #FinalAgentStatT INNER JOIN stats_folder SF ON #FinalAgentStatT.docref = SF.document_ref
INNER JOIN claim_payment CP ON CP.claim_payment_id = SF.payment_id 
	AND LEFT(document_ref, 3) like 'CLP'
	AND cp.thirdpartyreference IS NOT NULL

UPDATE #FinalAgentStatT
SET GrossAmount= ISNULL(GrossAmount,0),
DisplayGrossAmount= ISNULL(DisplayGrossAmount,0),
Settled= ISNULL(Settled,0),
Settled2= ISNULL(Settled2,0),
SettledCash= ISNULL(SettledCash,0),
CommissionAmount= ISNULL(CommissionAmount,0),
CommissionPercent= ISNULL(CommissionPercent,0)

SET NOCOUNT OFF

PRINT @AgentGroupCode

IF LOWER(@AgentGroupCode) = 'all'
 BEGIN

PRINT 'ENTER1'

  IF 	@TransactionType='Premium Transactions Only'
  BEGIN
  SELECT  Company,
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
          ISNULL(GrossAmount,DisplayGrossAmount) GrossAmount  , 
          DisplayGrossAmount,
          Settled,
          Settled2,
          SettledCash,
          CommissionAmount,
          CommissionPercent,
          InsuredName,
          CASE WHEN @Basis = 'Transaction Date' THEN DocDate 
					ELSE isnull(EffDate, DocDate) END
          ReportingDate,
          Band_1_Start,
          Band_2_Start,
          Band_3_Start,
          Band_4_Start,
          Band_5_Start,
          Band_1_End,
          Band_2_End,
          Band_3_End,
          Band_4_End,
          Underwriting_year,
          CurrencyCode,CurrencyDesc,CompanyCode,CompanyDesc,
	  Reference,
          Case @GroupByCode
              WHEN 'Branch' then CompanyCode
              WHEN 'Branch And Company' THEN CompanyCode
              WHEN 'Currency' THEN CurrencyCode
          ElSE    ''
          END 'GroupByCode',DueDate 
  FROM    #FinalAgentStatT where docRef not like 'c%'
  END
  
  IF 	@TransactionType='Claims Transactions Only'
  BEGIN
  SELECT  Company,
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
          DisplayGrossAmount,
          Settled,
          Settled2,
          SettledCash,
          CommissionAmount,
          CommissionPercent,
          InsuredName,
          CASE WHEN @Basis = 'Transaction Date' THEN DocDate ELSE isnull(EffDate, DocDate) END
          ReportingDate,
          Band_1_Start,
          Band_2_Start,
          Band_3_Start,
          Band_4_Start,
          Band_5_Start,
          Band_1_End,
          Band_2_End,
          Band_3_End,
          Band_4_End,
          Underwriting_year,
          CurrencyCode,CurrencyDesc,CompanyCode,CompanyDesc,
	  Reference,
          Case @GroupByCode
              WHEN 'Branch' then CompanyCode
              WHEN 'Branch And Company' THEN CompanyCode
              WHEN 'Currency' THEN CurrencyCode
          ElSE    ''
          END 'GroupByCode',DueDate 
  FROM    #FinalAgentStatT where docRef like 'c%'
  END
  
  IF 	@TransactionType='Premium & Claim Transactions'
  BEGIN
  SELECT  Company,
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
          ISNULL(GrossAmount,DisplayGrossAmount) GrossAmount  , 
          DisplayGrossAmount,
          Settled,
          Settled2,
          SettledCash,
          CommissionAmount,
          CommissionPercent,
          InsuredName,
          CASE WHEN @Basis = 'Transaction Date' THEN DocDate ELSE isnull(EffDate, DocDate) END
          ReportingDate,
          Band_1_Start,
          Band_2_Start,
          Band_3_Start,
          Band_4_Start,
          Band_5_Start,
          Band_1_End,
          Band_2_End,
          Band_3_End,
          Band_4_End,
          Underwriting_year,
          CurrencyCode,CurrencyDesc,CompanyCode,CompanyDesc,
	  Reference,
          Case @GroupByCode
              WHEN 'Branch' then CompanyCode
              WHEN 'Branch And Company' THEN CompanyCode
              WHEN 'Currency' THEN CurrencyCode
          ElSE    ''
          END 'GroupByCode',DueDate 
  FROM    #FinalAgentStatT
  END

END

IF LOWER(@AgentGroupCode) <> 'all'
 BEGIN

PRINT 'ENTER2'

  IF 	@TransactionType='Premium Transactions Only'
  BEGIN
  SELECT  Company,
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
          ISNULL(GrossAmount,DisplayGrossAmount) GrossAmount  , 
          DisplayGrossAmount,
          Settled,
          Settled2,
          SettledCash,
          CommissionAmount,
          CommissionPercent,
          InsuredName,
          CASE WHEN @Basis = 'Transaction Date' THEN DocDate ELSE isnull(EffDate, DocDate) END
          ReportingDate,
          Band_1_Start,
          Band_2_Start,
          Band_3_Start,
          Band_4_Start,
          Band_5_Start,
          Band_1_End,
          Band_2_End,
          Band_3_End,
          Band_4_End,
          Underwriting_year,
          CurrencyCode,CurrencyDesc,CompanyCode,CompanyDesc,
	  Reference,
          Case @GroupByCode
              WHEN 'Branch' then CompanyCode
              WHEN 'Branch And Company' THEN CompanyCode
              WHEN 'Currency' THEN CurrencyCode
          ElSE    ''
          END 'GroupByCode',DueDate 
  FROM    #FinalAgentStatT where docRef not like 'c%'
        --RC-- 13 Jun 2006
        AND AgentResolvedName IN(
        select trading_name from party_agent where linked_account_group = (
        select  party_cnt from party where shortname = @AgentGroupCode) )
        --RC-- 13 Jun 2006
  END

  IF 	@TransactionType='Claims Transactions Only'
  BEGIN
  SELECT  Company,
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
          DisplayGrossAmount,
          Settled,
          Settled2,
          SettledCash,
          CommissionAmount,
          CommissionPercent,
          InsuredName,
          CASE WHEN @Basis = 'Transaction Date' THEN DocDate ELSE isnull(EffDate, DocDate) END
          ReportingDate,
          Band_1_Start,
          Band_2_Start,
          Band_3_Start,
          Band_4_Start,
          Band_5_Start,
          Band_1_End,
          Band_2_End,
          Band_3_End,
          Band_4_End,
          Underwriting_year,
          CurrencyCode,CurrencyDesc,CompanyCode,CompanyDesc,
	  Reference,
          Case @GroupByCode
              WHEN 'Branch' then CompanyCode
              WHEN 'Branch And Company' THEN CompanyCode
              WHEN 'Currency' THEN CurrencyCode
          ElSE    ''
          END 'GroupByCode',DueDate 
  FROM    #FinalAgentStatT where docRef like 'c%'
        --RC-- 13 Jun 2006
        AND AgentResolvedName IN(
        select trading_name from party_agent where linked_account_group = (
        select  party_cnt from party where shortname = @AgentGroupCode) )
        --RC-- 13 Jun 2006
  END

  IF 	@TransactionType='Premium & Claim Transactions'
  BEGIN
  SELECT  Company,
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
          ISNULL(GrossAmount,DisplayGrossAmount) GrossAmount, 
          DisplayGrossAmount,
          Settled,
          Settled2,
          SettledCash,
          CommissionAmount,
          CommissionPercent,
          InsuredName,
          CASE WHEN @Basis = 'Transaction Date' THEN DocDate ELSE isnull(EffDate, DocDate) END
          ReportingDate,
          Band_1_Start,
          Band_2_Start,
          Band_3_Start,
          Band_4_Start,
          Band_5_Start,
          Band_1_End,
          Band_2_End,
          Band_3_End,
          Band_4_End,
          Underwriting_year,
          CurrencyCode,CurrencyDesc,CompanyCode,CompanyDesc,
	  Reference,
          Case @GroupByCode
              WHEN 'Branch' then CompanyCode
              WHEN 'Branch And Company' THEN CompanyCode
              WHEN 'Currency' THEN CurrencyCode
          ElSE    ''
          END 'GroupByCode',DueDate 
  FROM    #FinalAgentStatT
        --RC-- 13 Jun 2006
        WHERE AgentResolvedName IN(
        select trading_name from party_agent where linked_account_group = (
        select  party_cnt from party where shortname = @AgentGroupCode) )
        --RC-- 13 Jun 2006
  END

 END

DROP TABLE #AccountsWithOSBalance
DROP TABLE #tempDocDates
DROP TABLE #tempAgentStat
DROP TABLE #FinalAgentStatT

GO

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO