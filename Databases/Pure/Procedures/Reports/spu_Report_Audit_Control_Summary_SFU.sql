SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  OFF
GO


--DDLDropProcedure 'spu_pmu_report_balances'
--GO

EXECUTE DDLDropProcedure 'spu_Report_Audit_Control_Summary_SFU'
GO


CREATE PROCEDURE spu_Report_Audit_Control_Summary_SFU
    @branch_id  int,
    @PeriodDate varchar (20),
    @Basis varchar(50),
    @TypeofCurrency varchar(15)

    --@period_end_date datetime
--  @start_date datetime,
--  @end_date   datetime

AS

-- $Author: Tom.brown $
-- $Revision: 3 $
-- $Modtime: 31/10/02 17:30 $
-- $Workfile: spu_PMU_Report_Balances.sql $
-- $Logfile: /Sirius For Underwriting/Crystal Reports/Stored Procedures/spu_pmu_report_balances.sql $
-- $History: spu_PMU_Report_Balances.sql $
--
-- *****************  Version 3  *****************
-- User: Tom.brown    Date: 31/10/02   Time: 17:36
-- Updated in $/Sirius For Underwriting/Crystal Reports/Stored Procedures
-- Fix End date: force to time 23:59:59
--
-- *****************  Version 2  *****************
-- User: Tom.brown    Date: 29/10/02   Time: 17:34
-- Updated in $/Sirius For Underwriting/Crystal Reports/Stored Procedures
-- Only require Period End Date, and report on all ledgers, even those
-- with no transactions.
--
-- *****************  Version 2  *****************
-- User: Tom.brown    Date: 29/10/02   Time: 16:32
-- Updated in $/Work/SWIssues/Reports/AuditControl
-- Working versions: Interim store

-- *****************  Version 1  *****************
-- User: Tom.brown    Date: 19/07/02    Time: 10:23
-- Updated in $/Sirius for Underwriting/Crystal Reports/Accounts
-- Based on sp_report_balances.sql.  Used by Crsytal Reports:
-- Audit_Control_Summary_U.rpt.  Selects balance for each ledger
-- Then selects all transactions in a date range

--***********************************************************************************************************************************
--** VER    DATE        WHO     WHAT
--** 1.04   04/Nov/02   JMK     Rename from "spu_pmu_report_balances" to match Report Name
--**                            Change Period Parameter - use string returned from Parameter lookup
--**                            Amend selection of selected period values - to be the same criteria as for balances (other than date)
--
--** 1.05   11/Nov/02   JMK     Use Period id rather than just the period dates
--**                            NB - we are aware that period_id CAN be out of date order, i.e. id 79 COULD come before id 78
--**                                 ...however, AUA and RSA periods are in sequence up to end 2005
--
--** 1.06   18/Nov/02   JMK     Add Report Basis parameter and amend date selection accordingly
--**
--***********************************************************************************************************************************
-- for testing
--declare @branch_id  int, @PeriodDate varchar (20), @Basis varchar(50)
--select @branch_id = 0, @PeriodDate = '30 Sep 2002', @Basis = 'Transaction Date'

DECLARE @dtSelectedPeriodEnd datetime, @SelectedPeriodID int

SELECT @PeriodDate = @PeriodDate + ' 23:59:59.000'
SELECT @dtSelectedPeriodEnd = CONVERT (Datetime, @PeriodDate)

-- additional date selection parameters
DECLARE @iCompanyID int, @dtPriorSelectedPeriodEnd datetime, @iBasis int
DECLARE @systemCurrencyCode	Varchar(10)
DECLARE @systemCurrencyDesc	Varchar(255)
  
-- Most sited (i.e. AUA & RSA) only have period data set up for company 1
--IF ( ISNULL(@branch_id,0) <> 0 )
--    SELECT @icompanyID = @branch_id
--ELSE
    SELECT @iCompanyID = 1

-- What is the date basis for this report?
IF @Basis = 'Transaction Date'
BEGIN
    SELECT @iBasis = 1    -- Transaction Date

    -- Get the period end date prior to the selected period
    SELECT @dtPriorSelectedPeriodEnd = max(period_end_date)
    FROM  Period
    WHERE period_end_date < @dtSelectedPeriodEnd
    AND company_id = @iCompanyid
END
ELSE
BEGIN
    SELECT @iBasis = 0    -- Transaction Period

    -- Get the selected period id
    SELECT @SelectedPeriodID = period_id
    FROM Period
    WHERE period_end_date = @dtSelectedPeriodEnd
    AND  company_id = @iCompanyid

END

/*Get System Currency Details*/
SELECT
	@SystemCurrencyCode = c.iso_code,
	@SystemCurrencyDesc = c.description
FROM PMSystem pms
JOIN currency c
	ON c.currency_id = pms.currency_id
WHERE pms.system_id = 1
/*end  Get System Currency*/


-- TEMP DEBUG
--SELECT "START DATE = ", @dtPriorSelectedPeriodEnd
--SELECT "END DATE = ", @dtSelectedPeriodEnd

-- Create the temporary tables
CREATE TABLE #TransLines
    (   dAmount       numeric(19,4)   NULL,
        lLedgerID     int             NULL,
        sLedgerName   varchar(30)     NULL,
        sCode         char(10)        NULL,
        CompanyCode	Varchar(10)	NULL, --Company for multiple currency feature
	CompanyDesc	Varchar(255)	NULL,
	CurrencyCode	Varchar(10)	NULL,	--Currency for multiple currency feature
    	CurrencyDesc	Varchar(255)	NULL
        
    )

CREATE TABLE #TransLines3
    (   dAmount         numeric(19,4) NULL,
        lLedgerID       int           NULL,
        sLedgerName     varchar(30)   NULL,
        sCode           char(10)      NULL,
        CompanyCode	Varchar(10)	NULL, --Company for multiple currency feature
	CompanyDesc	Varchar(255)	NULL,
	CurrencyCode	Varchar(10)	NULL,	--Currency for multiple currency feature
	CurrencyDesc	Varchar(255)	NULL
    )

CREATE TABLE #TransBalance
    (   dAmount        numeric(19,4) NULL,
        sLedgerName    varchar(30)   NULL,
        CompanyCode	Varchar(10)	NULL --Company for multiple currency feature

    )


-- Add a nominal Zero transaction to #Translines for each ledger
-- so that even zero ledgers will appear in the final report
INSERT INTO #Translines
    SELECT 0.0,
           L.Ledger_id,
           L.Ledger_name,
           'JN',c.code,c.description,
           --join on  company and currency for multicurrency feature on 28.06.04
           CASE @TypeOfCurrency
	   	WHEN 'Base' THEN cb.iso_code
	   	WHEN 'System' THEN @SystemCurrencyCode
	   END,
	   CASE @TypeOfCurrency
	   	WHEN 'Base' THEN cb.description
	   	WHEN 'System' THEN @SystemCurrencyDesc
	   END
     FROM Ledger L
     CROSS JOIN company		c      	   
     INNER JOIN currency cb /*Base Currency*/ON cb.currency_id = c.base_currency
     where l.company_id=1 
           AND ( ISNULL(@branch_id,0) = 0
            	OR ( ISNULL(@branch_id,0) <> 0
                          AND l.company_id = @branch_id )
               )


-- Part 1, Get all transactions for each ledger up to selected period or
-- the period start date to get the opening balance for each ledger
INSERT INTO #TransLines
     	SELECT  
     	CASE @TypeOfCurrency
		WHEN 'Base' THEN ISNULL(ROUND(T.amount,2), 0.0)
		WHEN 'System' THEN ISNULL(ROUND(T.system_amount,2), 0.0)
	END Amount,
            L.ledger_id,
            L.ledger_name,
            DT.code,
            --join on  company and currency for multicurrency feature on 28.06.04
            C.Code,C.Description,
            CASE @TypeOfCurrency
	       	WHEN 'Base' THEN cb.iso_code
	    	WHEN 'System' THEN @SystemCurrencyCode
	    END,
	    CASE @TypeOfCurrency
	    	WHEN 'Base' THEN cb.description
	    	WHEN 'System' THEN @SystemCurrencyDesc
	    END
            
       FROM Transdetail               T
            INNER JOIN Account        A            ON         A.account_id = T.account_id
            INNER JOIN Document       D            ON         D.document_id = T.document_id
            INNER JOIN DocumentType   DT           ON         DT.documenttype_id = D.documenttype_id
            LEFT OUTER JOIN Ledger    L            ON         L.ledger_id = A.ledger_id
            INNER JOIN company		c      	   ON 		c.company_id = t.company_id
	    INNER JOIN currency cb /*Base Currency*/ON cb.currency_id = c.base_currency
	
      WHERE (
            ( @iBasis = 0 AND T.period_id < @SelectedPeriodID )
            OR
            ( @iBasis = 1 AND D.document_date <= @dtPriorSelectedPeriodEnd )
            )
            AND ( ISNULL(@branch_id,0) = 0
                OR ( ISNULL(@branch_id,0) <> 0
                     AND D.company_id = @branch_id )
                )

-- Part 1A Get opening balances by ledger, by summing
-- all transactions for each ledger
INSERT INTO #TransBalance
     SELECT SUM(T.dAmount),
            T.sLedgerName,
            T.Companycode
       FROM #TransLines T
   GROUP BY sLedgerName,T.companyCode
   ORDER BY sLedgerName

-- TB seed a zero entry for each journal
INSERT INTO #Translines3
    SELECT 0.0,
           L.Ledger_id,
           L.Ledger_name,
           'JN',c.code,c.description,
           --join on  company and currency for multicurrency feature on 28.06.04
           CASE @TypeOfCurrency
	   	WHEN 'Base' THEN cb.iso_code
	   	WHEN 'System' THEN @SystemCurrencyCode
	   END,
	   CASE @TypeOfCurrency
	   	WHEN 'Base' THEN cb.description
	   	WHEN 'System' THEN @SystemCurrencyDesc
	   END
     FROM Ledger L
     --join on  company and currency for multicurrency feature on 28.06.04
     CROSS JOIN company		c      	   
     INNER JOIN currency cb /*Base Currency*/ON cb.currency_id = c.base_currency
     where l.company_id=1
     AND ( ISNULL(@branch_id,0) = 0
               	OR ( ISNULL(@branch_id,0) <> 0
     	             AND l.company_id = @branch_id )
         )

-- part 2, get all transactions for selected period or dates
-- together with document date and ledger data
INSERT INTO #TransLines3
     SELECT 
         CASE @TypeOfCurrency
     		WHEN 'Base' THEN ISNULL(ROUND(T.amount,2), 0.0)
     		WHEN 'System' THEN ISNULL(ROUND(T.system_amount,2), 0.0)
	END Amount,
            L.ledger_id,
            L.ledger_name,
            DT.code,c.code,c.description,
           --join on  company and currency for multicurrency feature on 28.06.04
           CASE @TypeOfCurrency
	   	WHEN 'Base' THEN cb.iso_code
	   	WHEN 'System' THEN @SystemCurrencyCode
	   END,
	   CASE @TypeOfCurrency
	   	WHEN 'Base' THEN cb.description
	   	WHEN 'System' THEN @SystemCurrencyDesc
	   END
       FROM Transdetail               T
            INNER JOIN Account        A            ON         A.account_id = T.account_id
            INNER JOIN Document       D            ON         D.document_id = T.document_id
            --join on  company and currency for multicurrency feature on 28.06.04
            INNER JOIN company		c      	   ON 		c.company_id = t.company_id
	    INNER JOIN currency cb /*Base Currency*/ON cb.currency_id = c.base_currency
            INNER JOIN DocumentType   DT           ON         DT.documenttype_id = D.documenttype_id
            LEFT OUTER JOIN Ledger    L            ON         L.ledger_id = A.ledger_id
      WHERE (
            ( @iBasis = 0 AND T.period_id = @SelectedPeriodID )
            OR
            ( @iBasis = 1 AND D.document_date BETWEEN dateadd(ss,1,@dtPriorSelectedPeriodEnd) AND @dtSelectedPeriodEnd)
            )
            AND ( ISNULL(@branch_id,0) = 0
                OR ( ISNULL(@branch_id,0) <> 0
                     AND D.company_id = @branch_id )
                )
				AND NOT  (DT.code =   'SED' and      D.comment='Instalment Plan' )
				
-- Final report output
  SELECT T.dAmount,
         T.sLedgerName,
         T.sCode,dOpeningBalance = B.dAmount,
         T.CompanyCode,T.CompanyDesc,
         T.CurrencyCode,T.CurrencyDesc,
         CASE @Typeofcurrency
	 	WHEN 'BASE' THEN T.CompanyCode
	 ELSE ''
	 END 'GroupByCode'
    FROM #TransLines3               T
    LEFT OUTER JOIN #TransBalance   B  ON B.sLedgerName  = T.sLedgerName 
    and B.CompanyCode= T.CompanyCode
    ORDER BY T.sLedgerName,
         T.sCode
/*
-- Debug
SELECT * from #TransLines
SELECT * from #Translines3
SELECT * from #TransBalance
*/
-- Remove Temporary tables
DROP TABLE #TransLines
DROP TABLE #TransLines3
DROP TABLE #TransBalance

GO


SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON
GO


-- End of $Workfile: spu_pmu_report_balances.sql $
