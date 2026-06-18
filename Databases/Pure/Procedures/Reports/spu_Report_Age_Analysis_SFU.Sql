SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_Report_Age_Analysis_SFU'
GO

/**********************************************************************************************************************************          
** Created by Jude Killip          
** 14/11/2000          
** Age_Analysis.rpt          
**      Age Analysis reports for        71 - Client          
**                                      72 - Reinsurer          
**                                      74 - Agent          
**                                      76 - SubAgent          
**********************************************************************************************************************************          
** DATE         WHO DESCRIPTION          
** 01/10/2003    JMK    PN 4588 - Take unallocated cash value out of aged amounts          
**                            - Aged amounts + Anallocated Cash = Account Balance          
**                    PN 6904 - Add @Basis parameter to allow choice of cover_start_date for aging (Effective Date)          
** 02/09/2004      Added Custome bands for this report.  We MUST return one record of data all the time for the bands to work          
**          
** 10/08/2005  Puneet Kukreti PN:22879 - Due to some Missing code the report was showing Zeros when run using statment type Agent.It's working fine now.          
** 22-11-2005 Mark Evans : PN25815 - Updated to remove Currency Rate from Grouping as this was splitting the results for an account over multiple lines          
***********************************************************************************************************************************/          
CREATE PROCEDURE spu_Report_Age_Analysis_SFU          
    @branch_id      int,          
    @statement_type varchar (20),          
    @end_date        NVARCHAR(50),          
    @Basis            varchar (20),          
    @TypeOfCurrency VARCHAR(15),          
    @GroupBy VARCHAR(20),          
    @TransactionType VARCHAR(30),          
    @transactions VARCHAR(100)          
          
AS          
          
SET NOCOUNT ON     
SELECT @end_date= CONVERT(DATETIME,@end_date,103)              
DECLARE @dEndDate       datetime,          
        @iBranchID      int,          
        @iAccountID     int,          
        @UWType         char(1),          
        @InsOrReins     varchar(10),          
        @LedgerCode VARCHAR(2),          
        @SystemCurrencyCode VARCHAR(10),          
        @SystemCurrencyDesc VARCHAR(255)          
          
/*Get System Currency Details*/          
SELECT          
    @SystemCurrencyCode = c.iso_code,          
    @SystemCurrencyDesc = c.description          
FROM PMSystem pms          
JOIN currency c          
    ON c.currency_id = pms.currency_id          
WHERE pms.system_id = 1          
          
IF @end_date IS NULL OR @end_date = ''          
        SELECT @dEndDate = GETDATE()          
ELSE          
        SELECT @dEndDate = @end_date          
          
IF @branch_id IS NULL          
        SELECT @iBranchID = 0          
ELSE          
        SELECT @iBranchID = @branch_id          
          
SELECT @LedgerCode =          
    CASE UPPER(LTRIM(@statement_type))          
        WHEN  'CLIENT' THEN 'SA'          
        WHEN  'REINSURER' THEN 'IN'          
        WHEN  'AGENT' THEN 'AG'          
        WHEN  'SUBAGENT' THEN 'UB'          
        WHEN  'PURCHASE' THEN 'PU'          
    END   
                
-- get UWType          
EXECUTE spu_Report_GetUnderwritingType_SFU @UWType OUTPUT          
IF @UWType = 'A'          
    SELECT @InsOrReins = 'Insurer'          
ELSE          
    SELECT @InsOrReins = 'Reinsurer'          
          
-- Let's get the Aging bands for this report          
DECLARE         @Band_1_Start     int,          
                @Band_2_Start     int,          
                @Band_3_Start     int,          
                @Band_4_Start     int,          
                @Band_5_Start     int,          
                @Band_6_Start     int,          
                @Band_7_Start     int,          
                @Band_1_End       int,          
                @Band_2_End       int,          
                @Band_3_End       int,          
                @Band_4_End       int,          
                @Band_5_End       int,          
      @Band_6_End       int,          
                @Band_Branch      int          
          
--Set the Band Branch must be = 1 at least          
IF @branch_id IS NULL OR @branch_id = 0          
    SELECT @Band_Branch = 1          
ELSE          
    SELECT @Band_Branch = @branch_id          
          
--Get the Start          
Execute spu_get_Aging_band 3010, @Band_Branch, @Band_1_Start Output -- Equivalent 0          
Execute spu_get_Aging_band 3012, @Band_Branch, @Band_2_Start Output -- Equivalent 31          
Execute spu_get_Aging_band 3014, @Band_Branch, @Band_3_Start Output -- Equivalent 61          
Execute spu_get_Aging_band 3016, @Band_Branch, @Band_4_Start Output -- Equivalent 91          
Execute spu_get_Aging_band 3018, @Band_Branch, @Band_5_Start Output -- Equivalent 121          
Execute spu_get_Aging_band 3020, @Band_Branch, @Band_6_Start Output -- Equivalent 181          
Execute spu_get_Aging_band 3022, @Band_Branch, @Band_7_Start Output -- Equivalent 0          
          
--Get the End          
Execute spu_get_Aging_band 3011, @Band_Branch, @Band_1_End Output -- Equivalent 30          
Execute spu_get_Aging_band 3013, @Band_Branch, @Band_2_End Output -- Equivalent 60          
Execute spu_get_Aging_band 3015, @Band_Branch, @Band_3_End Output -- Equivalent 90          
Execute spu_get_Aging_band 3017, @Band_Branch, @Band_4_End Output -- Equivalent 120          
Execute spu_get_Aging_band 3019, @Band_Branch, @Band_5_End Output -- Equivalent 180          
Execute spu_get_Aging_band 3021, @Band_Branch, @Band_6_End Output -- Equivalent 365          
          
CREATE TABLE #tmpReportAgeAnal          
(          
    TransdetailID            int,          
    AccountID                int           NULL,    /* Account.account_id - */          
    AccountCode                varchar(30),           /* Account.short_code - */          
    AccountName                varchar(60),           /* Account.account_name */          
    ResolvedName            varchar(100)  NULL,    /* Party.resolved_name */          
    DocDate                    datetime      NULL,    /* Document.document_date */          
    ReportingDate            datetime      NULL,       /* Either document_date or cover_start_date */          
    LedgerName                varchar(30)   NULL,    /* Ledger.ledger_name */          
    InsuranceRef            varchar(30)   NULL,    /* TransDetail.insurance_ref */          
    DocTypeID                int           NULL,    /* Document.documenttype_id */          
    DocCashType                int           NULL,    /* Indicator, based on documenttype_id */          
                                                   /* 1= payment/receipt, 0 = not payment/receipt */          
    Amount                    numeric(19,4) NULL,    /* TransDetail.amount */          
    CompanyID                int           NULL,    /* Account.company_id */          
    BranchCode                varchar(10)   NULL,          
    AllocAmount                numeric(19,4) NULL,    /* Total allocated amount for transaction */          
    CashAllocAmount            numeric(19,4) NULL,    /* Total allocated cash amount for transaction */          
    TransferAllocAmount        numeric(19,4) NULL,       /* Data Transfer Allocated amounts */          
    UnallocatedCash            numeric(19,4) NULL,          
    DocRef                    varchar(50)   NULL,    /* For debugging the report */          
    NotYetDue                numeric(19,4) NULL,          
    Days30                    numeric(19,4) NULL,          
    Days60                    numeric(19,4) NULL,          
    Days90                    numeric(19,4) NULL,          
    Days120                    numeric(19,4) NULL,          
    Days180                    numeric(19,4) NULL,          
    Days365                    numeric(19,4) NULL,          
    Over1Year                numeric(19,4) NULL,          
    Band_1_Start            int           NULL,          
    Band_2_Start            int           NULL,          
    Band_3_Start            int           NULL,          
    Band_4_Start            int           NULL,          
    Band_5_Start            int           NULL,          
    Band_6_Start            int           NULL,          
    Band_7_Start            int           NULL,          
    Band_1_End                int           NULL,          
    Band_2_End                int           NULL,          
    Band_3_End                int           NULL,          
    Band_4_End                int           NULL,          
    Band_5_End                int           NULL,          
    Band_6_End                int           NULL,          
    CompanyCode                VARCHAR(10),          
    CompanyDesc                VARCHAR(255),          
    CurrencyCode            VARCHAR(10),          
    CurrencyDesc            VARCHAR(255),          
    GroupByCode                VARCHAR(10),          
    CurrencyRate            FLOAT,          
    Code                          VARCHAR(10),          
    DateBasis  varchar(50)          
)          
          
INSERT INTO #tmpReportAgeAnal          
  SELECT
  td.transdetail_id,
  a.account_id, 
  a.short_code,
  a.account_name, 
  NULL,              -- ResolvedName 
  d.document_date,   
  CASE 
	WHEN @Basis = 'Transaction Date' THEN d.document_date        
	WHEN @Basis = 'Due Date' THEN isnull(td.due_date,td.accounting_date)     
  ELSE isnull(tef.cover_start_date, d.document_date)  
  END,            -- ReportingDate   
  l.ledger_name, 
  td.insurance_ref,    
  d.documenttype_id,   
  CASE    
	WHEN d.documenttype_id in (22, 23,28,29) THEN 1       
	ELSE 0    
  END,                -- DocCashType 
  CASE @TypeOfCurrency      
	WHEN 'Account' THEN td.account_amount        
	WHEN 'Base' THEN td.amount        
	WHEN 'System' THEN td.system_amount     
	WHEN 'Transaction' THEN td.currency_amount   
  END,  
  td.company_id,   
  NULL,               -- BranchCode    
  CASE @TypeOfCurrency    
	WHEN 'Account' THEN td.account_amount - td.outstanding_account_amount  
	WHEN 'Base' THEN td.amount - td.outstanding_amount      
	WHEN 'System' THEN td.system_amount - td.outstanding_system_amount    
	WHEN 'Transaction' THEN td.currency_amount - td.outstanding_currency_amount  
	END,                -- AllocAmount  
	
  CASE   
	WHEN td.fully_matched = 1 THEN       
		(            
			SELECT             
				CASE @TypeOfCurrency  
					WHEN 'Account' THEN td.account_amount * -1   
					WHEN 'Base' THEN td.amount * -1      
					WHEN 'System' THEN td.system_amount * -1               
					WHEN 'Transaction' THEN td.currency_amount * -1             
				END        
		)       
	ELSE   
		(       
			SELECT    
				CASE @TypeOfCurrency  
					WHEN 'Account' THEN SUM(ad.alloc_account_amount)    
					WHEN 'Base' THEN SUM(ad.alloc_base_amount)         
					WHEN 'System' THEN SUM(ad.alloc_system_amount)       
					WHEN 'Transaction' THEN SUM(ad.alloc_ccy_amount)            
				END      
           FROM allocationdetail ad 
           JOIN cashlistitem cl        
		   ON cl.cashlistitem_id = ad.cashlistitem_id      
		   WHERE cl.transdetail_id = td.transdetail_id     
		   AND DATEDIFF(DAY, ad.accounting_date, @dEndDate) > = 0      
		 )  
		 END,                -- CashAllocAmount  

		 CASE @TypeOfCurrency     
			WHEN 'Account' THEN (SELECT td.account_amount WHERE spare = 'ALLOCATED')     
			WHEN 'Base' THEN (SELECT td.amount WHERE spare = 'ALLOCATED')    
			WHEN 'System' THEN (SELECT td.system_amount WHERE spare = 'ALLOCATED')   
			WHEN 'Transaction' THEN (SELECT td.currency_amount WHERE spare = 'ALLOCATED')    
		END,                   -- TransferAllocAmount   
		0,                    -- UnallocatedCash 
		d.document_ref,  
        0,0,0,0,0,0,0,0, -- The new added columns for instalments  
		0,0,0,0,0,0,0,0,0,0,0,0,0,  -- For the bands 
		s.code,  
		s.description, 
		CASE @TypeOfCurrency   
			WHEN 'Account' THEN ca.iso_code      
			WHEN 'Base' THEN cb.iso_code      
			WHEN 'System' THEN @SystemCurrencyCode    
			WHEN 'Transaction' THEN ct.iso_code    
		END,
		CASE @TypeOfCurrency   
			WHEN 'Account' THEN ca.description       
			WHEN 'Base' THEN cb.description     
			WHEN 'System' THEN @SystemCurrencyDesc   
			WHEN 'Transaction' THEN ct.description   
		END, 
		NULL,
        CASE d.documenttype_id WHEN 37 THEN td.currency_base_xrate ELSE 
		CASE @TypeOfCurrency  
			WHEN 'Account' THEN td.account_base_xrate     
			WHEN 'Base' THEN 1       
			WHEN 'System' THEN td.system_base_xrate   
			WHEN 'Transaction' THEN td.currency_base_xrate   
		END END,
(SELECT dt.code from documenttype dt 
            Where dt.documenttype_id=d.documenttype_id), 
@Basis

FROM Account a 
JOIN Ledger l   
	ON a.ledger_id = l.ledger_id 
JOIN TransDetail td  
	ON a.account_id = td.account_id
JOIN Document d   
	ON td.document_id = d.document_id 
LEFT OUTER JOIN transaction_export_folder tef   
	ON tef.document_ref = d.document_ref 
JOIN Source s  
	ON s.source_id = td.company_id 
JOIN Currency cb /*Base Currency*/   
	ON cb.currency_id = s.base_currency_id 
JOIN Currency ct /*Transaction Currency*/    
	ON ct.currency_id = td.currency_id 
JOIN Currency ca /*Account Currency*/  
	ON ca.currency_id = a.currency_id 
LEFT JOIN tax_band tb     
	On tb.tax_band_id = td.tax_band_id 
LEFT JOIN tax_type tt    
	On tt.tax_type_id = tb.tax_type_id 
WHERE l.ledger_short_name = @LedgerCode 
AND (td.company_id = @iBranchID OR @iBranchID = 0)
AND    ((DATEDIFF(DAY, d.document_date, @End_date) >= 0 AND @Basis = 'Transaction Date') OR 
    (DATEDIFF(DAY, ISNULL(tef.cover_start_date, d.document_date),  @End_date) >= 0 AND @Basis = 'Due Date') OR
	 (DATEDIFF(DAY, ISNULL(tef.cover_start_date, d.document_date),  @End_date) >= 0 AND @Basis = 'Effective Date')) 
AND IsNull(tt.is_excluded_from_age_analysis, 0) = 0   

--Update group by column          
UPDATE #tmpReportAgeAnal          
SET GroupByCode =          
    CASE @GroupBy          
        WHEN 'Branch' THEN CompanyCode          
        WHEN 'Branch and Currency' THEN CompanyCode          
        WHEN 'Currency' THEN CurrencyCode          
        ELSE ''          
    END,          
  CompanyCode =          
    CASE @GroupBy          
        WHEN 'Branch' THEN CompanyCode          
        WHEN 'Branch and Currency' THEN CompanyCode          
        WHEN 'Currency' THEN CurrencyCode          
        ELSE ''          
    END,          
 CompanyDesc = CASE @GroupBy          
        WHEN 'Branch' THEN CompanyDesc          
        WHEN 'Branch and Currency' THEN CompanyDesc          
        WHEN 'Currency' THEN CompanyDesc          
        ELSE ''          
    END          
          
-- calculate unallocated balance          
UPDATE #tmpReportAgeAnal          
SET UnallocatedCash =          
    CASE          
        WHEN ISNULL(AllocAmount,0) = 0 THEN ISNULL(Amount,0) + ISNULL(CashAllocAmount,0) + ISNULL(TransferAllocAmount,0)          
        ELSE ISNULL(Amount,0) - ISNULL(AllocAmount,0) + ISNULL(TransferAllocAmount,0)          
    END          
FROM #tmpReportAgeAnal tmp          
WHERE tmp.TransdetailID = TransdetailID          
AND    DocCashType = 1          
          
UPDATE #tmpReportAgeAnal          
SET AllocAmount = 0          
WHERE AllocAmount IS NULL          
          
/*Installment transaction should be splot out into it's component installments*/          
/*And converted into correct report currency*/          
UPDATE #tmpReportAgeAnal          
SET NotYetDue =          
        (          
            SELECT SUM(i.amount) * tmp.CurrencyRate          
            FROM pfInstalments i          
            WHERE i.pfprem_finance_cnt = p.pfprem_finance_cnt          
            AND i.pfprem_finance_version = p.pfprem_finance_version          
            AND i.DueDate > @dEndDate          
     AND i.status <> 3          
        ),          
    Days30 =          
        (          
            SELECT SUM(i.amount) * tmp.CurrencyRate          
            FROM pfInstalments i          
            WHERE i.pfprem_finance_cnt = p.pfprem_finance_cnt          
            AND i.pfprem_finance_version = p.pfprem_finance_version          
            AND DATEDIFF(DAY,i.DueDate,@dEndDate) >= @Band_1_Start          
            AND DATEDIFF(DAY,i.DueDate,@dEndDate) <= @Band_1_End          
            AND i.status <> 3          
        ),          
    Days60 =          
        (          
            SELECT SUM(i.amount) * tmp.CurrencyRate        
            FROM pfInstalments i          
            WHERE i.pfprem_finance_cnt = p.pfprem_finance_cnt          
            AND i.pfprem_finance_version = p.pfprem_finance_version          
            AND DATEDIFF(DAY,i.DueDate,@dEndDate) >= @Band_2_Start          
            AND DATEDIFF(DAY,i.DueDate,@dEndDate) <= @Band_2_End          
            AND i.status <> 3          
        ),          
    Days90 =          
        (          
            SELECT SUM(i.amount) * tmp.CurrencyRate          
            FROM pfInstalments i          
            WHERE i.pfprem_finance_cnt = p.pfprem_finance_cnt          
            AND i.pfprem_finance_version = p.pfprem_finance_version          
            AND DATEDIFF(DAY,i.DueDate,@dEndDate) >= @Band_3_Start          
       AND DATEDIFF(DAY,i.DueDate,@dEndDate) <= @Band_3_End          
            AND i.status <> 3          
        ),          
    Days120 =          
        (          
            SELECT SUM(i.amount) * tmp.CurrencyRate          
            FROM pfInstalments i          
            WHERE i.pfprem_finance_cnt = p.pfprem_finance_cnt          
            AND i.pfprem_finance_version = p.pfprem_finance_version          
            AND DATEDIFF(DAY,i.DueDate,@dEndDate) >= @Band_4_Start          
            AND DATEDIFF(DAY,i.DueDate,@dEndDate) <= @Band_4_End          
            AND i.status <> 3          
        ),          
    Days180 =          
        (          
            SELECT SUM(i.amount) * tmp.CurrencyRate          
            FROM pfInstalments i          
            WHERE i.pfprem_finance_cnt = p.pfprem_finance_cnt          
            AND i.pfprem_finance_version = p.pfprem_finance_version          
            AND DATEDIFF(DAY,i.DueDate,@dEndDate) >= @Band_5_Start          
            AND DATEDIFF(DAY,i.DueDate,@dEndDate) <= @Band_5_End          
            AND i.status <> 3          
        ),          
    Days365 =          
        (          
            SELECT SUM(i.amount) * tmp.CurrencyRate          
            FROM pfInstalments i          
            WHERE i.pfprem_finance_cnt = p.pfprem_finance_cnt          
            AND i.pfprem_finance_version = p.pfprem_finance_version          
            AND DATEDIFF(DAY,i.DueDate,@dEndDate) >= @Band_6_Start          
            AND DATEDIFF(DAY,i.DueDate,@dEndDate) <= @Band_6_End          
            AND i.status <> 3          
        ),          
    Over1Year =          
        (          
            SELECT SUM(i.amount) * tmp.CurrencyRate          
            FROM pfInstalments i          
            WHERE i.pfprem_finance_cnt = p.pfprem_finance_cnt          
            AND i.pfprem_finance_version = p.pfprem_finance_version          
            AND DATEDIFF(DAY,i.DueDate,@dEndDate) >= @Band_7_Start          
            AND i.status <> 3          
        )          
FROM pfPremiumFinance p          
JOIN TransDetail t          
    ON t.TransDetail_id = p.PlanTransaction_id          
JOIN Document d          
    ON d.document_id = t.document_id          
JOIN #tmpReportAgeAnal tmp          
    ON tmp.DocRef = d.document_ref          
    AND tmp.CompanyID = d.company_id          
WHERE P.statusIND IN ('040', '011', '900')          
          
 --print 'Update with Branch Name'          
UPDATE #tmpReportAgeAnal          
SET BranchCode = c.code          
FROM Company c          
WHERE CompanyID = c.company_id          
          
UPDATE #tmpReportAgeAnal          
SET BranchCode =          
    CASE @GroupBy          
        WHEN 'Branch' THEN BranchCode          
        WHEN 'Branch and Currency' THEN BranchCode          
        WHEN 'Currency' THEN BranchCode          
        ELSE ''          
    END          
          
--Copy non instalment amount into new cols          
--NotYetDue Band          
UPDATE tmp          
SET tmp.NotYetDue = tmp.Amount - tmp.AllocAmount - tmp.UnallocatedCash          
FROM #tmpReportAgeAnal tmp          
WHERE DATEDIFF(DAY,ReportingDate,@dEndDate) < 0          
AND NOT EXISTS          
    (          
        SELECT NULL          
        FROM Document d          
        JOIN TransDetail t          
            ON t.document_id = d.document_id          
        JOIN pfPremiumFinance p          
            ON p.PlanTransaction_id = t.TransDetail_id          
        WHERE d.document_ref = tmp.DocRef          
        AND d.company_id = tmp.CompanyID          
    )          
          
--First Band         
UPDATE tmp          
SET tmp.Days30 = tmp.Amount - tmp.AllocAmount - tmp.UnallocatedCash          
FROM #tmpReportAgeAnal tmp          
WHERE DATEDIFF(DAY,ReportingDate,@dEndDate) >= @Band_1_Start          
AND DATEDIFF(DAY,ReportingDate,@dEndDate) <= @Band_1_End          
AND NOT EXISTS          
    (          
        SELECT NULL          
        FROM Document d          
        JOIN TransDetail t          
            ON t.document_id = d.document_id          
        JOIN pfPremiumFinance p          
            ON p.PlanTransaction_id = t.TransDetail_id          
        WHERE d.document_ref = tmp.DocRef          
        AND d.company_id = tmp.CompanyID          
    )          
          
--Second Band          
UPDATE tmp          
SET tmp.Days60 = tmp.Amount - tmp.AllocAmount - tmp.UnallocatedCash          
FROM #tmpReportAgeAnal tmp          
WHERE DATEDIFF(DAY,ReportingDate,@dEndDate) >= @Band_2_Start          
AND DATEDIFF(DAY,ReportingDate,@dEndDate) <= @Band_2_End          
AND NOT EXISTS          
    (          
        SELECT NULL          
        FROM Document d          
        JOIN TransDetail t          
            ON t.document_id = d.document_id          
        JOIN pfPremiumFinance p          
            ON p.PlanTransaction_id = t.TransDetail_id          
        WHERE d.document_ref = tmp.DocRef          
        AND d.company_id = tmp.CompanyID          
    )          
          
--Third Band          
UPDATE tmp          
SET tmp.Days90 = tmp.Amount - tmp.AllocAmount - tmp.UnallocatedCash          
FROM #tmpReportAgeAnal tmp          
WHERE DATEDIFF(DAY,ReportingDate,@dEndDate) >= @Band_3_Start          
AND DATEDIFF(DAY,ReportingDate,@dEndDate) <= @Band_3_End          
AND NOT EXISTS          
    (          
        SELECT NULL          
        FROM Document d          
        JOIN TransDetail t          
            ON t.document_id = d.document_id          
        JOIN pfPremiumFinance p          
            ON p.PlanTransaction_id = t.TransDetail_id          
        WHERE d.document_ref = tmp.DocRef          
        AND d.company_id = tmp.CompanyID          
    )          
          
--Fourth Band          
UPDATE tmp          
SET tmp.Days120 = tmp.Amount - tmp.AllocAmount - tmp.UnallocatedCash          
FROM #tmpReportAgeAnal tmp          
WHERE DATEDIFF(DAY,ReportingDate,@dEndDate) >= @Band_4_Start          
AND DATEDIFF(DAY,ReportingDate,@dEndDate) <= @Band_4_End          
AND NOT EXISTS          
    (          
        SELECT NULL          
        FROM Document d          
        JOIN TransDetail t          
            ON t.document_id = d.document_id          
        JOIN pfPremiumFinance p          
            ON p.PlanTransaction_id = t.TransDetail_id          
        WHERE d.document_ref = tmp.DocRef          
        AND d.company_id = tmp.CompanyID          
    )          
          
--Fifth Band          
UPDATE tmp          
SET tmp.Days180 = tmp.Amount - tmp.AllocAmount - tmp.UnallocatedCash          
FROM #tmpReportAgeAnal tmp          
WHERE DATEDIFF(DAY,ReportingDate,@dEndDate) >= @Band_5_Start          
AND DATEDIFF(DAY,ReportingDate,@dEndDate) <= @Band_5_End          
AND NOT EXISTS          
    (          
        SELECT NULL          
        FROM Document d          
        JOIN TransDetail t          
            ON t.document_id = d.document_id          
        JOIN pfPremiumFinance p          
            ON p.PlanTransaction_id = t.TransDetail_id          
        WHERE d.document_ref = tmp.DocRef          
        AND d.company_id = tmp.CompanyID          
    )          
          
--Sixth Band          
UPDATE tmp          
SET tmp.Days365 = tmp.Amount - tmp.AllocAmount - tmp.UnallocatedCash          
FROM #tmpReportAgeAnal tmp          
WHERE DATEDIFF(DAY,ReportingDate,@dEndDate) >= @Band_6_Start          
AND DATEDIFF(DAY,ReportingDate,@dEndDate) <= @Band_6_End          
AND NOT EXISTS          
    (          
        SELECT NULL          
        FROM Document d          
        JOIN TransDetail t          
            ON t.document_id = d.document_id          
        JOIN pfPremiumFinance p          
            ON p.PlanTransaction_id = t.TransDetail_id          
        WHERE d.document_ref = tmp.DocRef          
        AND d.company_id = tmp.CompanyID          
    )          
          
--Seventh Band, Where are using Band_6_End          
UPDATE tmp          
SET tmp.Over1Year = tmp.Amount - tmp.AllocAmount - tmp.UnallocatedCash          
FROM #tmpReportAgeAnal tmp          
WHERE DATEDIFF(DAY,ReportingDate,@dEndDate) > @Band_6_End          
AND NOT EXISTS          
    (          
        SELECT NULL          
        FROM Document d          
        JOIN TransDetail t          
            ON t.document_id = d.document_id          
        JOIN pfPremiumFinance p          
            ON p.PlanTransaction_id = t.TransDetail_id          
        WHERE d.document_ref = tmp.DocRef          
        AND d.company_id = tmp.CompanyID          
    )          
          
--Assign the Bands to the TempTable          
UPDATE #tmpReportAgeAnal          
SET    Band_1_Start = @Band_1_Start,          
    Band_2_Start = @Band_2_Start,          
    Band_3_Start = @Band_3_Start,          
    Band_4_Start = @Band_4_Start,          
    Band_5_Start = @Band_5_Start,          
    Band_6_Start = @Band_6_Start,          
    Band_7_Start = @Band_7_Start,          
    Band_1_End = @Band_1_End,          
    Band_2_End = @Band_2_End,          
    Band_3_End = @Band_3_End,          
    Band_4_End = @Band_4_End,          
    Band_5_End = @Band_5_End,          
    Band_6_End = @Band_6_End          
          
SET NOCOUNT OFF

          
CREATE TABLE #tmpReportAgeAnalSummary          
(   AccountCode       varchar(30),          
    AccountName            varchar(60),          
    LedgerName            varchar(30)   NULL,          
    ResolvedName        varchar(100)  NULL,          
    Amount                numeric(19,4) NULL,          
    BranchCode            varchar(10)   NULL,          
    InsurerReinsurer    varchar(10)   NULL,          
    UnallocatedCash        numeric(19,4) NULL,          
    NotYetDue            numeric(19,4) NULL,          
    Days30                numeric(19,4) NULL,          
    Days60                numeric(19,4) NULL,          
    Days90                numeric(19,4) NULL,          
    Days120                numeric(19,4) NULL,          
    Days180                numeric(19,4) NULL,          
    Days365                numeric(19,4) NULL,          
    Over1Year            numeric(19,4) NULL,          
    Band_1_Start        int,          
    Band_2_Start        int,          
    Band_3_Start        int,          
    Band_4_Start        int,          
    Band_5_Start        int,          
    Band_6_Start        int,          
    Band_7_Start        int,          
    Band_1_End            int,          
    Band_2_End            int,          
    Band_3_End            int,          
    Band_4_End            int,          
    Band_5_End            int,          
    Band_6_End            int,          
    CurrencyRate            FLOAT,          
    CompanyCode                VARCHAR(10),          
    CompanyDesc                VARCHAR(255),          
    CurrencyCode            VARCHAR(10),          
    CurrencyDesc            VARCHAR(255),          
    GroupByCode VARCHAR(10),          
    Code                          VARCHAR(10),          
    DateBasis  varchar(50)          
          
)          
INSERT INTO  #tmpReportAgeAnalSummary         
    SELECT  AccountCode,          
            CASE @transactions WHEN 'Summary' THEN AccountName ELSE DocRef END,          
            LedgerName,          
            ResolvedName,          
            --Amount - AllocAmount,--Commented by Nitesh Dwivedi as on 25-Mar-2010(For PN-64153 1.11 SR6)          
            Amount,          
            BranchCode,          
            @InsOrReins,          
            CASE DocCashType          
                WHEN 1 THEN          
                    UnallocatedCash          
                ELSE          
                    0          
                END,          
            NotYetDue,          
            Days30,          
            Days60,          
            Days90,          
            Days120,          
            Days180,          
            Days365,          
            Over1Year,          
            Band_1_Start,          
            Band_2_Start,          
            Band_3_Start,          
            Band_4_Start,          
            Band_5_Start,          
            Band_6_Start,          
            Band_7_Start,          
            Band_1_End,          
            Band_2_End,          
            Band_3_End,          
            Band_4_End,          
            Band_5_End,          
            Band_6_End,          
            CurrencyRate,          
            CompanyCode,          
            CompanyDesc,          
            CurrencyCode,          
            CurrencyDesc,          
            GroupByCode,          
            Code,          
    DateBasis          
          
    FROM    #tmpReportAgeAnal   

          
IF @TransactionType='Premium Transactions Only'          
BEGIN          
         
        SELECT      AccountCode,          
                    AccountName,          
                    LedgerName,          
                    ResolvedName,          
                    BranchCode,          
                    InsurerReinsurer,          
                    SUM(Amount) Amount,          
                    SUM(UnallocatedCash) UnallocatedCash,          
                    SUM(NotYetDue) NotYetDue,          
                    SUM(Days30) Days30,          
                    SUM(Days60) Days60,          
                    SUM(Days90) Days90,          
                    SUM(Days120) Days120,          
                    SUM(Days180) Days180,          
                    SUM(Days365) Days365,          
                    SUM(Over1Year) Over1Year,          
                    Band_1_Start,          
                    Band_2_Start,          
                    Band_3_Start,          
                    Band_4_Start,          
                    Band_5_Start,          
                    Band_6_Start,          
                    Band_7_Start,          
                    Band_1_End,          
                    Band_2_End,          
                    Band_3_End,          
                    Band_4_End,          
                    Band_5_End,          
                    Band_6_End,          
                    MIN(CurrencyRate) CurrencyRate,          
                    CompanyCode,          
                    CompanyDesc,          
                    CurrencyCode,          
                    CurrencyDesc,          
                    GroupByCode,          
    DateBasis          
FROM        #tmpReportAgeAnalSummary  Where code not in('CLA','CLP','CLO','CLR') 
    GROUP BY    AccountCode,          
                    AccountName,          
    LedgerName,          
                    ResolvedName,          
                    BranchCode,          
                    InsurerReinsurer,          
                    Band_1_Start,          
                    Band_2_Start,          
                    Band_3_Start,          
                    Band_4_Start,          
                    Band_5_Start,          
                    Band_6_Start,          
                    Band_7_Start,          
                    Band_1_End,          
                    Band_2_End,          
                    Band_3_End,          
                    Band_4_End,          
                    Band_5_End,          
                    Band_6_End,          
            --CurrencyRate,          
                    CompanyCode,          
                    CompanyDesc,          
                    CurrencyCode,          
                    CurrencyDesc,          
                    GroupByCode,          
                    DateBasis          
          
         HAVING        SUM(isnull(Amount,0)) <> 0 OR SUM(isnull(UnallocatedCash,0)) <> 0
               
          
END          
          
IF @TransactionType='Claims Transactions Only'          
BEGIN          
          
        SELECT      AccountCode,          
           AccountName,          
                    LedgerName,          
                    ResolvedName,          
                    BranchCode,          
                    InsurerReinsurer,          
                    SUM(Amount) Amount,          
                    SUM(UnallocatedCash) UnallocatedCash,          
                    SUM(NotYetDue) NotYetDue,          
                    SUM(Days30) Days30,          
                    SUM(Days60) Days60,          
                    SUM(Days90) Days90,          
                    SUM(Days120) Days120,          
                    SUM(Days180) Days180,          
                    SUM(Days365) Days365,          
                    SUM(Over1Year) Over1Year,          
                    Band_1_Start,          
                    Band_2_Start,          
                    Band_3_Start,          
                    Band_4_Start,          
                    Band_5_Start,          
                    Band_6_Start,          
                    Band_7_Start,          
                    Band_1_End,          
                    Band_2_End,          
                    Band_3_End,          
                    Band_4_End,          
                    Band_5_End,          
                    Band_6_End,          
                    MIN(CurrencyRate) CurrencyRate,          
                    CompanyCode,          
                    CompanyDesc,          
                    CurrencyCode,          
                    CurrencyDesc,          
                    GroupByCode,          
    DateBasis          
FROM        #tmpReportAgeAnalSummary  Where code in('CLA','CLP','CLO','CLR') 
        GROUP BY    AccountCode,          
                    AccountName,          
                    LedgerName,          
                    ResolvedName,          
                    BranchCode,          
                    InsurerReinsurer,          
                    Band_1_Start,          
                    Band_2_Start,          
                    Band_3_Start,          
                    Band_4_Start,          
                    Band_5_Start,          
                    Band_6_Start,          
                    Band_7_Start,          
                    Band_1_End,          
                    Band_2_End,          
                    Band_3_End,          
                    Band_4_End,          
                    Band_5_End,          
                    Band_6_End,          
                    --CurrencyRate,          
                    CompanyCode,          
                    CompanyDesc,          
                    CurrencyCode,          
                    CurrencyDesc,          
                    GroupByCode,          
    DateBasis          
          
       HAVING        SUM(isnull(Amount,0)) <> 0 OR SUM(isnull(UnallocatedCash,0)) <> 0   
          

END          
          

 IF @TransactionType='Premium & Claim Transactions'          
BEGIN          
          
        SELECT      AccountCode,          
                    AccountName,          
                    LedgerName,          
                    ResolvedName,          
                    BranchCode,          
                    InsurerReinsurer,          
                    SUM(Amount) Amount,          
                    SUM(UnallocatedCash) UnallocatedCash,          
                    SUM(NotYetDue) NotYetDue,          
                    SUM(Days30) Days30,          
                    SUM(Days60) Days60,          
                    SUM(Days90) Days90,          
                    SUM(Days120) Days120,          
                    SUM(Days180) Days180,          
                    SUM(Days365) Days365,          
                    SUM(Over1Year) Over1Year,          
                    Band_1_Start,          
                    Band_2_Start,          
                    Band_3_Start,          
                    Band_4_Start,          
                    Band_5_Start,          
                    Band_6_Start,          
                    Band_7_Start,          
                    Band_1_End,          
                    Band_2_End,          
                    Band_3_End,          
                    Band_4_End,          
                    Band_5_End,          
                    Band_6_End,          
                    MIN(CurrencyRate) CurrencyRate,          
                    CompanyCode,          
                    CompanyDesc,          
                    CurrencyCode,          
                    CurrencyDesc,          
                    GroupByCode,          
    DateBasis          
FROM        #tmpReportAgeAnalSummary
        GROUP BY    AccountCode,          
                    AccountName,          
                    LedgerName,          
                    ResolvedName,          
                    BranchCode,          
                    InsurerReinsurer,          
            Band_1_Start,          
                    Band_2_Start,          
                    Band_3_Start,          
                    Band_4_Start,          
                    Band_5_Start,          
  Band_6_Start,          
                    Band_7_Start,          
                    Band_1_End,          
                    Band_2_End,          
                    Band_3_End,          
                    Band_4_End,          
                    Band_5_End,          
                    Band_6_End,          
                    --CurrencyRate,          
                    CompanyCode,          
  CompanyDesc,          
                    CurrencyCode,          
                    CurrencyDesc,          
                    GroupByCode,          
    DateBasis          
          
       HAVING        SUM(isnull(Amount,0)) <> 0 OR SUM(isnull(UnallocatedCash,0)) <> 0	          
          
END          
          
DROP TABLE #tmpReportAgeAnalSummary          
DROP TABLE #tmpReportAgeAnal 

GO
