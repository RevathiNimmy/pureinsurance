SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 

EXECUTE DDLDropProcedure 'spu_Report_Balance_Sheet_Agency_SFU'
GO

/**********************************************************************************************************************************
** Created by Gaurav Arora
** Created On 16 Feb 2006 At 4:30 PM
** Created For S4I Reports - Balance_Sheet_U_Agency.rpt**
**********************************************************************************************************************************
** Based on Back Office Balance Sheet Report
** Uses existing Orion SPs to get Tree Structure information
** NB - Make sure Orion referred to as "Orion_For_Broking"
**********************************************************************************************************************************
***********************************************************************************************************************************/

CREATE	PROCEDURE spu_Report_Balance_Sheet_Agency_SFU
	@branch_id          INT,
	@period_end_date    VARCHAR(50) ,
	@TypeOfCurrency		VARCHAR(50),
	@Basis VARCHAR(50),
	@IncClaim	VARCHAR(30)
AS


DECLARE @dPeriodEndDateIn       datetime    
DECLARE @dNextMonth             datetime    
DECLARE @sYear                  char(4)    
DECLARE @sMonth                 char(2)    
DECLARE @dFirstDay              datetime    
DECLARE @dLastDay               datetime    
DECLARE @dPeriodEndDate         datetime    
DECLARE @d1stPeriodStartDate    datetime    
DECLARE @d1stPeriodEndDate      datetime    
DECLARE @dPrevPeriodEndDate     datetime    
    
DECLARE @sYearName          varchar(20)    
DECLARE @iBranchID              int    
DECLARE @iCompanyID             smallint    
DECLARE @iSubBranchID           integer    
DECLARE @RootnodeID             integer    
DECLARE @iso_code VARCHAR(4)    
DECLARE @description VARCHAR(255)    
    
    -- Empty temporary tables    
 SET NOCOUNT ON    
 SET DATEFORMAT DMY    
 
 CREATE TABLE #Report_Transaction(
	transdetail_id int NULL,
	amount numeric(19, 4) NULL,
	document_sequence smallint NULL,
	policy_number varchar(30) NULL,
	branch_id int NULL,
	comment varchar(60) NULL,
	document_ref varchar(25) NULL,
	document_date datetime NULL,
	documenttype_id int NULL,
	account_id int NULL,
	account_code char(30) NULL,
	account_name varchar(100) NULL,
	account_type varchar(100) NULL,
	ledger_type varchar(100) NULL,
	branch_name varchar(100) NULL,
	period_id int NULL,
	record_type smallint NULL,
	transdetail_id2 int NULL,
	amount2 numeric(19, 4) NULL,
	document_sequence2 smallint NULL,
	policy_number2 varchar(30) NULL,
	branch_id2 int NULL,
	comment2 varchar(60) NULL,
	account_id2 int NULL,
	account_code2 char(20) NULL,
	account_name2 varchar(100) NULL,
	account_type2 varchar(100) NULL,
	ledger_type2 varchar(100) NULL,
	branch_name2 varchar(100) NULL,
	period_id2 int NULL,
	record_type2 smallint NULL,
	extra_char1 varchar(100) NULL,
	extra_char2 varchar(255) NULL,
	extra_char3 varchar(100) NULL,
	extra_char4 varchar(255) NULL,
	extra_char5 varchar(100) NULL,
	extra_char6 varchar(100) NULL,
	extra_char7 varchar(100) NULL,
	extra_int1 int NULL,
	extra_int2 int NULL,
	extra_int3 int NULL,
	extra_int4 int NULL,
	extra_int5 int NULL,
	extra_int6 int NULL,
	extar_int7 int NULL,
	extra_datetime1 datetime NULL,
	extra_datetime2 datetime NULL,
	extra_datetime3 datetime NULL,
	extra_datetime4 datetime NULL,
	extra_datetime5 datetime NULL,
	extra_datetime6 datetime NULL,
	extra_datetime7 datetime NULL,
	extra_numeric1 numeric(19, 4) NULL,
	extra_numeric2 numeric(19, 4) NULL,
	extra_numeric3 numeric(19, 4) NULL,
	extra_numeric4 numeric(19, 4) NULL,
	extra_numeric5 numeric(19, 4) NULL,
	extra_numeric6 numeric(19, 4) NULL,
	extra_numeric7 numeric(19, 4) NULL
) 

BEGIN TRAN

	TRUNCATE TABLE Report_TreePathNames        
    SELECT @iBranchID = ISNULL(@branch_id, 0)    
    
    SELECT @RootNodeID = (SELECT node_id from structuretree    
    WHERE (core_node is NULL OR core_node = 0)    
    AND mapping_id is NULL    
    AND account_id IS NULL    
    AND parent_node_id = 0    
    AND company_id = @iBranchID)    
    
    SELECT @RootNodeID = ISNULL(@RootNodeid, 0)    
    
    EXECUTE spu_Report_BS_TreePathNames @RootNodeID    
    
    SELECT * INTO #Report_TreePathNames FROM Report_TreePathNames
	TRUNCATE TABLE Report_TreePathNames
	COMMIT TRAN
	
    
    SELECT @dPeriodEndDateIn = ISNULL(CONVERT(DATETIME,@period_end_Date,103), GETDATE())    
    
    CREATE          TABLE #DocumentIds    
    (Document_id int)    
      
    EXEC Ddladdindex    
    @sTableName = #DocumentIds,    
    @sColumnName1 = Document_id    
    
    IF LOWER(@IncClaim) = 'no'    
        BEGIN    
     DELETE FROM #DocumentIds    
     INSERT INTO #DocumentIds    
     SELECT DISTINCT(document_id) from TransDetail TransDetail    
      INNER JOIN Account Account    
     ON TransDetail.account_id = Account.account_id    
     WHERE Account.account_name LIKE 'CLM%'    
 END    
    
   CREATE TABLE #TempDataTable    
    (    
       Company_id INT    
    )    
    
    IF @iBranchID = 0 BEGIN    
       INSERT INTO #TempDataTable    
        SELECT company_id    
        FROM   Company    
    END ELSE BEGIN    
        INSERT INTO #TempDataTable    
        SELECT company_id    
        FROM   Company    
        WHERE  company_id = @iBranchID    
    END    
    
    
    
    
    ---------------------------------    
    DECLARE @dtSelectedPeriodEnd datetime, @SelectedPeriodID int    
    --SELECT @Period_end_date = CONVERT(datetime,@period_end_date)    
    SELECT @dtSelectedPeriodEnd = CONVERT (DATETIME, @Period_end_Date,103)    
    DECLARE @iBasis int    
    DECLARE @dtPriorSelectedPeriodEnd datetime    
    SELECT @iBasis = 1    -- Transaction Date    
    
    IF @Basis = 'Transaction Date'    
    BEGIN    
     
        -- Get the period end date prior to the selected period     
            
        SELECT @dtPriorSelectedPeriodEnd = max(period_end_date)    
        FROM  Period    
        WHERE period_end_date < @dtSelectedPeriodEnd    
    
    END    
    ELSE    
        
    IF @Basis = 'Transaction Period'    
    BEGIN    
        SELECT @iBasis = 0    -- Transaction Period    
    
 SELECT @dtPriorSelectedPeriodEnd = max(period_end_date)    
        FROM  Period    
        WHERE period_end_date < @dtSelectedPeriodEnd    
    
        -- Get the selected period id    
        SELECT @SelectedPeriodID = period_id    
        FROM Period    
        WHERE period_end_date = @dtPriorSelectedPeriodEnd    
     
    END    
    ---------------------------------    
    
    
 --OPEN cCompany    
    
 --   -- Get the transactions for each company    
 --FETCH NEXT FROM cCompany INTO @iCompanyID    
    
 --WHILE @@FETCH_STATUS = 0    
 --BEGIN    
    
  --EXEC spu_sub_branch_default @source_id=@iCompanyID, @sub_branch_id=@iSubBranchID OUTPUT    
    
        -- If Period end date is not found, default to end of input month    
  SELECT @dNextMonth = DATEADD(month, 1, @dPeriodEndDateIn)    
  SELECT @sYear = CONVERT(char(4), DATEPART(year, @dNextMonth))    
  SELECT @sMonth = CONVERT(char(2), DATEPART(month, @dNextMonth))    
  SELECT @dFirstDay = CONVERT(datetime, @sYear + '-' + @sMonth + '-01 23:59:59')    
  SELECT @dLastDay = DATEADD(day, -1, @dFirstDay)    
    
    
  SELECT @dPeriodEndDate = (SELECT ISNULL(MIN(period_end_date), @dLastDay)    
       FROM  Period    
       WHERE  company_id = @iCompanyID    
       AND  CONVERT(DATE,period_end_date) >= CONVERT(DATE,@dPeriodEndDateIn))    
    
  --SELECT @sYearName = (SELECT ISNULL(year_name, '')    
  --       FROM Period    
  --       WHERE company_id = @iCompanyID    
  --                   AND    period_end_date = @dPeriodEndDate    
  --                   AND    sub_branch_id = @iSubBranchID)    
    
        -- If 1st Period End Date not found set it to the current period end date    
  --SELECT @d1stPeriodEndDate = (SELECT ISNULL(MIN(period_end_date), @dPeriodEndDate)    
  --        FROM Period    
  --        WHERE company_id = @iCompanyID    
  --        AND year_name = @sYearName)    
    
        ---- If previous period end date not found set it to end of previous month    
  --SELECT @sYear = CONVERT(char(4), DATEPART(year, @dPeriodEndDate))    
  --SELECT @sMonth = CONVERT(char(2), DATEPART(month, @dPeriodEndDate))    
    
  --SELECT @dFirstDay = CONVERT(datetime, @sYear + '-' + @sMonth + '-01 23:59:59')    
  --SELECT @dLastDay = DATEADD(day, -1, @dFirstDay)    
    
  --SELECT @dPrevPeriodEndDate = (SELECT ISNULL(MAX(period_end_date), @dLastDay)    
  --         FROM Period    
  --         WHERE company_id = @iCompanyID    
    
  --         AND period_end_date < @d1stPeriodEndDate)    
    
    
    
    
  INSERT INTO #Report_Transaction    
  (    
   transdetail_id,    
   amount,    
   account_id,    
   account_name,    
   account_code,    
   extra_char1,    
   extra_datetime1,    
   document_date,    
   documenttype_id,    
   ledger_type,    
   account_type    
  )    
        -- Balance Sheet details    
  SELECT TransDetail.transdetail_id,    
            ROUND(TransDetail.amount,2),    
   Account.account_id,    
   Account.account_name,    
   Account.short_code,    
   Period.period_name,    
   Period.period_end_date,    
   Document.document_date,    
   Document.documenttype_id,    
   ISNULL(LedgerType.[description], ''),    
   ISNULL(AccountType.[description], '')    
  FROM #Report_TreePathNames AS RTPN    
   JOIN Account AS Account    
   ON RTPN.account_id = Account.account_id    
   JOIN TransDetail TransDetail    
   ON Account.account_id = TransDetail.account_id    
   JOIN #TempDataTable TDT    
   ON transdetail.company_id = TDT.Company_id   
   --AND Transdetail.company_id = @iCompanyID    
   JOIN Period Period    
   ON TransDetail.period_id = Period.period_id    
   JOIN Document  Document    
   ON TransDetail.document_id = Document.document_id    
   LEFT OUTER JOIN AccountType AccountType    
   ON Account.accounttype_id = AccountType.accounttype_id    
   LEFT OUTER JOIN Ledger Ledger    
   ON Account.ledger_id = Ledger.ledger_id    
   LEFT OUTER JOIN LedgerType LedgerType    
   ON Ledger.ledgertype_id = LedgerType.ledgertype_id    
   JOIN ElementExtras EE    
   ON EE.report_map_id = RTPN.report_map_id2    
  WHERE     
  (    
                        ( @iBasis = 0 AND TransDetail.period_id < @SelectedPeriodID )    
                    OR    
                        ( @iBasis = 1 AND Document.document_date <= @dtPriorSelectedPeriodEnd )    
      
                )    
  AND    
  TransDetail.document_id NOT IN (SELECT Document_id FROM #DocumentIds)    
  AND EE.element_id in (SELECT element_id from element where element_name = 'Balance Sheet')    
  AND    
  --start of check on COMM ADJ and AGENT ADJ    
  ( ( Document.document_date <= @dPeriodEndDate    
    AND ( TransDetail.Document_Sequence NOT IN    
       ( SELECT Document_Sequence + 1    
        FROM TransDetail    
        WHERE document_id = Document.document_id    
        AND spare = 'COMM ADJ'    
       )    
     --spare NULL and not part of AGENT ADJ COMM ADJ transaction    
     AND ISNULL(TransDetail.spare, '') NOT IN ('AGENT ADJ', 'COMM ADJ')    
     )    
   )    
    
   OR    
   ( TransDetail.ref_date <= @dPeriodEndDate    
    AND ( TransDetail.Document_Sequence IN    
       ( SELECT Document_Sequence + 1    
        FROM TransDetail    
        WHERE document_id = Document.document_id    
        AND spare = 'COMM ADJ'    
       )    
      OR ISNULL(TransDetail.spare, '') IN ('AGENT ADJ', 'COMM ADJ')     )    
   )    
  )    
  --end of check of COMM ADJ and AGENT ADJ    
    UNION    
        -- Profit & Loss Details    
  SELECT TransDetail.transdetail_id,    
            ROUND(TransDetail.amount,2),    
   Account.account_id,    
   Account.account_name,    
   Account.short_code,    
   Period.period_name,    
   Period.period_end_date,    
   Document.document_date,    
   Document.documenttype_id,    
   ISNULL(LedgerType.[description], ''),    
   ISNULL(AccountType.[description], '')    
  FROM #Report_TreePathNames AS RTPN    
   JOIN Account AS Account    
   ON RTPN.account_id = Account.account_id    
            -- eck030902  AND Account.company_id = @iCompanyID    
   JOIN TransDetail TransDetail    
   ON Account.account_id = TransDetail.account_id    
   JOIN #TempDataTable TDT    
   ON transdetail.company_id = tdt.Company_id  
   --AND Transdetail.company_id = @iCompanyID  -- eck030902    
   JOIN Period Period    
   ON TransDetail.period_id = Period.period_id    
   JOIN Document Document    
   ON TransDetail.document_id = Document.document_id    
   LEFT OUTER JOIN AccountType AccountType    
   ON Account.accounttype_id = AccountType.accounttype_id    
   LEFT OUTER JOIN Ledger Ledger    
   ON Account.ledger_id = Ledger.ledger_id    
   LEFT OUTER JOIN LedgerType LedgerType    
   ON Ledger.ledgertype_id = LedgerType.ledgertype_id    
   JOIN ElementExtras EE    
   ON EE.report_map_id = RTPN.report_map_id2    
  WHERE     
  (    
                        ( @iBasis = 0 AND TransDetail.period_id < @SelectedPeriodID )    
                    OR    
                        ( @iBasis = 1 AND Document.document_date <= @dtPriorSelectedPeriodEnd )    
  )    
         AND    
  TransDetail.document_id NOT IN (SELECT Document_id FROM #DocumentIds)     
  AND    
                --EE.element_id = 7    
  EE.element_id in (SELECT element_id from element where element_name = 'Profit and Loss')    
  AND    
  --start of check on COMM ADJ and AGENT ADJ    
  ( ( Document.document_date <= @dPeriodEndDate    
    AND ( TransDetail.Document_Sequence NOT IN    
       ( SELECT Document_Sequence + 1    
        FROM TransDetail    
        WHERE document_id = Document.document_id    
        AND spare = 'COMM ADJ'    
       )    
     --spare NULL and not part of AGENT ADJ COMM ADJ transaction    
     AND ISNULL(TransDetail.spare, '') NOT IN ('AGENT ADJ', 'COMM ADJ')    
    )    
   )    
    
   OR    
   ( TransDetail.ref_date <= @dPeriodEndDate    
    AND ( TransDetail.Document_Sequence IN    
       ( SELECT Document_Sequence + 1    
  FROM TransDetail    
    
        WHERE document_id = Document.document_id    
        AND spare = 'COMM ADJ'    
       )    
      OR ISNULL(TransDetail.spare, '') IN ('AGENT ADJ', 'COMM ADJ')    
     )    
   )    
       
  )    
  --end of check of COMM ADJ and AGENT ADJ    
    
  INSERT INTO #Report_Transaction    
  (    
   transdetail_id,    
   amount,    
   account_id,    
   account_name,    
   account_code,    
   extra_char1,    
   extra_datetime1,    
   document_date,    
   documenttype_id,    
   ledger_type,    
   account_type    
  )    
  SELECT    
   0,    
   0.0,    
   Account.account_id,    
   Account.account_name,    
   Account.short_code,    
   '',    
   @dPeriodEndDate,    
   null,    
   0,    
   ISNULL(LedgerType.[description], ''),    
   ISNULL(AccountType.[description], '')    
  FROM #Report_TreePathNames AS RTPN    
  JOIN Account AS Account    
   ON RTPN.account_id = Account.account_id    
  --Join #TempDataTable tdt  
  --On account.company_id = tdt.Company_id   
  LEFT OUTER JOIN Ledger Ledger    
   ON Account.ledger_id = Ledger.ledger_id    
  LEFT OUTER JOIN LedgerType LedgerType    
   ON Ledger.ledgertype_id = LedgerType.ledgertype_id    
  LEFT OUTER JOIN AccountType AccountType    
   ON Account.accounttype_id = AccountType.accounttype_id    
  WHERE     
  NOT EXISTS    
   (    
    SELECT NULL    
    FROM #Report_Transaction rt    
    WHERE rt.account_id = RTPN.account_id    
   )    
    
 -- FETCH NEXT FROM cCompany INTO @iCompanyID    
    
 --END    
    
 --CLOSE cCompany    
 --DEALLOCATE cCompany    
    
 IF @TypeOfCurrency = 'Base'    
 BEGIN    
    
  SELECT    
   @iso_code = c.iso_code,    
   @description = c.description    
  FROM currency c    
  JOIN source s    
   ON s.base_currency_id = c.currency_id    
  WHERE s.source_id = @branch_id    
    
  UPDATE rt    
  SET Amount = ROUND(td.amount,2),    
   extra_char2 = @iso_code,    
   extra_char3 = @description    
  FROM #Report_Transaction rt  JOIN transdetail td    
   ON td.transdetail_id = rt.transdetail_id    
  WHERE rt.transdetail_id <> 0    
    
  UPDATE rt    
  SET extra_char2 = @iso_code,    
   extra_char3 = @description    
  FROM #Report_Transaction rt    
  WHERE rt.transdetail_id = 0    
 END    
    
 IF @TypeOfCurrency = 'System'    
 BEGIN    
  UPDATE rt    
  SET Amount = ROUND(td.system_amount,2),    
   extra_char2 =    
    (    
     SELECT iso_code    
     FROM currency    
     WHERE currency_id IN    
      (    
       SELECT currency_id    
       FROM pmsystem    
       WHERE system_id = 1    
      )    
    ),    
   extra_char3 =    
    (    
     SELECT description    
     FROM currency    
     WHERE currency_id IN    
      (    
       SELECT currency_id    
       FROM pmsystem    
       WHERE system_id = 1    
      )    
    )    
  FROM #Report_Transaction rt    
  JOIN transdetail td    
   ON td.transdetail_id = rt.transdetail_id    
  WHERE rt.transdetail_id <> 0    
    
  UPDATE rt    
  SET extra_char2 =    
    (    
     SELECT iso_code    
     FROM currency    
     WHERE currency_id IN    
      (    
       SELECT currency_id    
       FROM pmsystem    
       WHERE system_id = 1    
      )    
    
    ),    
   extra_char3 =    
    (    
     SELECT description    
     FROM currency    
    
     WHERE currency_id IN    
      (    
       SELECT currency_id    
       FROM pmsystem    
       WHERE system_id = 1    
      )    
    )    
  FROM #Report_Transaction rt    
  WHERE rt.transdetail_id = 0    
 END    
    
    -- Extract data    
 SET NOCOUNT OFF     
    
    
    
    -- Balance Sheet items    
 SELECT RTPN.element_name1,    
  RTPN.Report_Map_Id1,    
  RTPN.element_name2,    
    
  RTPN.Report_Map_Id2,    
    
  RTPN.element_name3,    
  RTPN.Report_Map_Id3,    
  RTPN.element_name4,    
  RTPN.Report_Map_Id4,    
  RTPN.element_name5,    
  RTPN.Report_Map_Id5,    
  RTPN.element_name6,    
  RTPN.Report_Map_Id6,    
  RTPN.element_name7,    
  RTPN.Report_Map_Id7,    
  RTPN.element_name8,    
  RTPN.Report_Map_Id8,    
  RTPN.element_name9,    
  RTPN.Report_Map_Id9,    
  RTPN.element_name10,    
    
  RTPN.Report_Map_Id10,    
        ROUND(RT.Amount,2) Amount,    
  RT.account_name,    
  RT.account_code short_code,    
  RT.extra_char1 period_name,    
  RT.extra_datetime1 period_end_date,    
  RT.document_date,    
  RT.documenttype_id,    
  RT.transdetail_id,    
  RT.ledger_type,    
  RT.account_type,    
  RT.extra_char2 currency_code,    
  RT.extra_char3 currency_desc    
 FROM #Report_TreePathNames AS RTPN    
  JOIN #Report_Transaction RT    
  ON RTPN.account_id = RT.account_id    
  JOIN ElementExtras EE    
  ON EE.report_map_id = RTPN.report_map_id2    
 WHERE     
        EE.element_id IN (SELECT element_id FROM element WHERE element_name = 'Balance Sheet')    
    
 UNION    
    
    -- Profit & Loss Items    
 SELECT RTPN.element_name1,    
  RTPN.Report_Map_Id1,    
  RTPN.element_name2,    
  RTPN.Report_Map_Id2,    
  RTPN.element_name3,    
  RTPN.Report_Map_Id3,    
  '',    
  0,    
  '',    
  0,    
  '',    
  0,    
  '',    
  0,    
  '',    
  0,    
  '',    
  0,    
  '',    
  0,    
        ISNULL(ROUND(RT.Amount,2), 0.0) Amount,    
  RT.account_name,    
  RT.account_code short_code,    
  RT.extra_char1 period_name,    
  RT.extra_datetime1 period_end_date,    
  RT.document_date,    
  RT.documenttype_id,    
  RT.transdetail_id,    
  RT.ledger_type,    
  RT.account_type,    
  RT.extra_char2 currency_code,    
  RT.extra_char3 currency_desc    
 FROM #Report_TreePathNames AS RTPN    
  JOIN #Report_Transaction RT    
  ON RTPN.account_id = RT.account_id    
  JOIN ElementExtras EE    
  ON EE.report_map_id = RTPN.report_map_id2    
 WHERE     
  EE.element_id IN (SELECT element_id FROM element WHERE element_name = 'Profit And Loss')    
 ORDER BY Report_Map_Id1, Report_Map_Id2, Report_Map_Id3, Report_Map_Id4, Report_Map_Id5, Report_Map_Id6, Report_Map_Id7, Report_Map_Id8, Report_Map_Id9, Report_Map_Id10, short_code    
 SET NOCOUNT ON    
    
   DROP TABLE #DocumentIds    
   DROP TABLE #Report_Transaction    
   DROP TABLE #Report_TreePathNames    
 SET NOCOUNT OFF    

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

