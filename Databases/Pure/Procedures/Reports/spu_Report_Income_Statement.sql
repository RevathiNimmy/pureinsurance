SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Report_Income_Statement'
GO
CREATE PROCEDURE spu_Report_Income_Statement
    @company_id int,
    @sub_branch_id int = NULL --AMJ
AS
/**********************************************************************************************************************************
** Created by Jude Killip
** 21/11/2000
** RSA Reports - Income_Statement.rpt
**********************************************************************************************************************************
** Based on Back Office Profit&Loss report
** Uses existing Orion SPs to get Tree Structure information
** Use node_id = 3 because existing Back Office report does! But Why?
**                                                   Because:   - 1 = Balance Sheet & Profit and Loss
**                                                              - 2 = Balance Sheet
**                                                              - 3 = Profit and Loss
**********************************************************************************************************************************
** 12/12/2000   Jude Killip     improve date criteria
**                              td.document_sequence = 2         -- get one element of the double entry
**
** 03/10/2001   Jude Killip     use spu_Report_FullTreePathNames
**                              ditch old date criteria, use Period
**
** 04/10/2001   Jude Killip     DON'T limit by element!
**
** 01/08/2002   AMJ - branch specific change
***********************************************************************************************************************************/
SET NOCOUNT ON
/************************ Populate the Orion Tables ************************************/
/* spu_Report_ShortTreePathNames calls spu_Report_Treepath.
   Both SPs use the Orion tables: Report_Nodes, Report_Treepath, Report_TreepathNames
   ... which are cleared down and re-populated */

IF @sub_branch_id IS NULL
    EXECUTE spu_sub_branch_default @source_id=@company_id, @sub_branch_id=@sub_branch_id OUTPUT

declare @node_id int

select @node_id = node_id
from structuretree  st
join element        e
on e.element_id = st.element_id
where element_name = 'Profit and Loss'
and company_id = @company_id

EXECUTE spu_Report_ShortTreePathNames @node_id

-- get current period values
DECLARE @CurrentPeriodID int, @dtCurrentPeriodEnd datetime
EXECUTE spu_Report_GetCurrentPeriod @sub_branch_id, @CurrentPeriodID OUTPUT,  @dtCurrentPeriodEnd OUTPUT

-- get current year values
DECLARE @CurrentYearStartPeriodID int, @dtLastYearPeriodEndDate datetime
EXECUTE spu_Report_GetCurrentYear @sub_branch_id, @CurrentYearStartPeriodID OUTPUT, @dtLastYearPeriodEndDate OUTPUT

-- get current 12 month period values
DECLARE  @12MonthPeriodID int, @dt12MonthPeriodEnd datetime
EXECUTE spu_Report_GetCurrent12MonthPeriod @sub_branch_id, @12MonthPeriodID OUTPUT, @dt12MonthPeriodEnd OUTPUT

-- calc Last Year YTD range
DECLARE @YTDRange int
SELECT @YTDRange = DATEDIFF(mm, @dtCurrentPeriodEnd, @dtLastYearPeriodEndDate)

/************************ Get Account details based on Orion.Report_TreePathNames ******/
CREATE TABLE #tmpRSAIncomeStatement
(
        dtCurrentPeriodEnd datetime NULL,
        ReportMapID1 int NULL,
        ElementName1 varchar (30) NULL,
        ReportMapID2 int NULL,
        ElementName2 varchar (30) NULL,
        ReportMapID3 int NULL,
        ElementName3 varchar (30) NULL,
        ReportMapID4 int NULL,
        ElementName4 varchar (30) NULL,
        ReportMapID5 int NULL,
        ElementName5 varchar (30) NULL,
        ReportMapID6 int NULL,
        ElementName6 varchar (30) NULL,
        ReportMapID7 int NULL,
        ElementName7 varchar (30) NULL,
        ReportMapID8 int NULL,
        ElementName8 varchar (30) NULL,
        ReportMapID9 int NULL,
        ElementName9 varchar (30) NULL,
        ReportMapID10 int NULL,
        ElementName10 varchar (30) NULL,
        AccountID int,
        Account varchar (60) NULL,
        AccountCode varchar (30) NULL,
        amountTMTY money NULL,             /* Current Period */
        amountTMLY money NULL,             /* Equivalent Period Last Year */
        amountYTDTY money NULL,            /* Current Year To Date */
        amountYTDLY money NULL,            /* Same Period Last Year */
        LedgerType varchar (255) NULL,
        AccountType varchar (255) NULL
)

INSERT INTO #tmpRSAIncomeStatement
        SELECT @dtCurrentPeriodEnd,
                RTPN.Report_Map_Id1,
                RTPN.element_name1,
                RTPN.Report_Map_Id2,
                RTPN.element_name2,
                RTPN.Report_Map_Id3,
                RTPN.element_name3,
                RTPN.Report_Map_Id4,
                RTPN.element_name4,
                RTPN.Report_Map_Id5,
                RTPN.element_name5,
                RTPN.Report_Map_Id6,
                RTPN.element_name6,
                RTPN.Report_Map_Id7,
                RTPN.element_name7,
                RTPN.Report_Map_Id8,
                RTPN.element_name8,
                RTPN.Report_Map_Id9,
                RTPN.element_name9,
                RTPN.Report_Map_Id10,
                RTPN.element_name10,
                a.account_id,
                a.account_name,
                a.short_code,
        -- Current Period
        (SELECT sum(td.amount)
                FROM TransDetail td
                JOIN Document d
                ON td.document_id = d.document_id
                WHERE td.period_id = @CurrentPeriodID
                AND td.account_id = a.account_id
                ),
        -- Current Period last year
        (SELECT sum(td.amount)
                FROM TransDetail td
                JOIN Document d
                ON td.document_id = d.document_id
                WHERE td.period_id = @12MonthPeriodID
                AND td.account_id = a.account_id
                ),
        -- this year Year To Date
        (SELECT sum(td.amount)
                FROM TransDetail td
                JOIN Document d
                ON td.document_id = d.document_id
                WHERE td.period_id IN
            (   SELECT period_id
                FROM Period
                WHERE period_end_date >= @dtLastYearPeriodEndDate
                and period_end_date <= @dtCurrentPeriodEnd
                AND   sub_branch_id = @sub_branch_id
            )
-- AMJ  BETWEEN @CurrentYearStartPeriodID AND @CurrentPeriodID
                AND td.account_id = a.account_id
                ),
        -- last year Year To Date
        (SELECT sum(td.amount)
                FROM TransDetail td
                JOIN Document d
                ON td.document_id = d.document_id
                AND td.document_sequence = 2         -- get one element of the double entry
                AND td.account_id = a.account_id
                ),
        lt.description,
        act.description
FROM    Report_TreePathNames RTPN
        JOIN Account a                      ON RTPN.account_id = a.account_id
        LEFT OUTER JOIN Ledger l            ON a.ledger_id = l.ledger_id
        LEFT OUTER JOIN LedgerType lt       ON l.ledgertype_id = lt.ledgertype_id
        LEFT OUTER JOIN AccountType act     ON a.accounttype_id = act.accounttype_id


SET NOCOUNT OFF
-- get data back from temporary table
SELECT * FROM #tmpRSAIncomeStatement

-- delete temporary table
DROP TABLE #tmpRSAIncomeStatement
-- clear down Orion Table
DELETE FROM Report_TreePathNames

GO

