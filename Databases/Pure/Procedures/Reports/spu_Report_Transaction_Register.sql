SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Report_Transaction_Register'
GO


CREATE PROCEDURE spu_Report_Transaction_Register
                    @PeriodDate varchar(255)
AS

/**********************************************************************************************************************************
** Created by Jude Killip
** 07/08/2000
** RSA Reports - Transaction_Register.rpt
**  Created with dummy data to build the report
**********************************************************************************************************************************
** 06/10/2000 JMK       Update to use DB
**
** 10/10/2000 JMK       Uncomment section
**                      Rename Orion DB
**
** 18/10/2000 JMK       Replace Insured with Insurance Holder
**                      Remove Risk Code
**
** 25/10/2000 JMK       - silly date condition error in last month details
**
** 15/01/2001 JMK       - filter Amount where TransDetail.document_sequence = 2   to get one element of the double entry (otherwise
**                        totals cancel each other out)
**                      - use document_date directly, use Period in future?
**
** 05/05/2001 JMK       - amend to conform to summary of premium (base dates on Current Period)
**
** 28/06/2001 JMK       - amend current period
**                      - get document ref as transaction reference
**                      - get variety of amount values
**
** 03/09/2001 JMK       - retrieve product description as well as code
**
** 17/09/2001 JMK       - split out tax and commission amounts
**                      - retrieve Document id (for sorting in report)
**
** 18/09/2001 JMK       - error: retrieved transaction id and document id wrong order
**
** 28/09/2001 JMK       - Filter out failed Export records
**                      - amend transaction_export_folder join (was excluding Orion only transactions)
**                      - amend Transmatch join to prevent duplication
**
** 01/10/2001 JMK       - Don't need cash any more! Just Sirius transactions
**
** 13/11/2001 JMK       - get Underwriting Type flag (display Insurer/Reinsurer)
**
** 19/11/2001   JMK             get UWType using spu_Report_GetUnderwritingType
**
** 05/12/2001   JMK     allow selection based on Previous or Current Period
**
** 18/12/2001   JMK     use new 'special' parameter "Period" - user's selection from list of
**                                      current and previous period_end_dates (as a string)
***********************************************************************************************************************************/
SET NOCOUNT ON

CREATE TABLE #tempRSATransReg
        (
        TransactionID int,
        DocumentID int,
        AccountDate datetime NULL,
        InsuranceRef varchar (30) NULL,
        GrossAmount decimal (19,4) NULL,                -- spare = 'GROSS'
        CommissionAndTaxAmounts decimal (19,4) NULL,    -- spare = 'COMM' OR LIKE ('TAX%'),
        Amount decimal (19,4) NULL,                     -- amount
        DocumentDate datetime NULL,
        DocumentRef varchar (25) NULL,
        DocumentType varchar (255) NULL,
        AccountCode varchar (30) NULL,
        AccountName varchar (60) NULL,
        LedgerCode varchar (2) NULL,
        LedgerName varchar (30) NULL,
        dtThisPeriodEnd datetime NULL,
        InsFileCnt int NULL,
        Product varchar (255) NULL,
        ProductCode varchar (10) NULL,
        BusType varchar (10) NULL,
        ExportStatus char (1) NULL,
        ClientName varchar (20) NULL,
        UWType char (1)
        )

-- get UWType
DECLARE @UWType char(1)
EXECUTE spu_Report_GetUnderwritingType @UWType OUTPUT

-- which period do we want?
DECLARE @SelectedPeriodID int, @dtPeriodEndDate datetime

SELECT @PeriodDate = @PeriodDate + ' 23:59:59.000'
SELECT @dtPeriodEndDate = CONVERT (Datetime, @PeriodDate)

SELECT @SelectedPeriodID = period_id
FROM ..Period
WHERE period_end_date = @dtPeriodEndDate

--print 'get Orion details'
INSERT INTO #tempRSATransReg
        SELECT  td.transdetail_id,
                td.document_id,
                td.accounting_date,
                td.insurance_ref,
                (SELECT td.amount WHERE td.spare = 'GROSS'),
                (SELECT td.amount WHERE td.spare = 'COMM' OR td.spare LIKE ('TAX%')),
                td.amount,
                d.document_date,
                d.document_ref,
                dt.description,
                a.short_code,
                a.account_name,
                l.ledger_short_name,
                l.ledger_name,
                @dtPeriodEndDate,
                NULL,
                NULL,
                NULL,
                NULL,
                NULL,
                NULL,
                @UWType
        FROM ..Account a
        JOIN ..Ledger l                   ON a.ledger_id = l.ledger_id
        JOIN ..TransDetail td             ON a.account_id = td.account_id
            AND td.period_id = @SelectedPeriodID
        JOIN ..Document d                 ON d.document_id = td.document_id
        JOIN ..DocumentType dt            ON d.documenttype_id = dt.documenttype_id
        WHERE dt.from_sirius = 1

--print 'update with export details'

--sj 31/07/2002 - start
/*
UPDATE #tempRSATransReg
    SET InsFileCnt = tef.insurance_file_cnt,
        Product = (select description from product where code = tef.product_code),
        ProductCode = tef.product_code,
        BusType = tef.business_type_code,
        ExportStatus = tef.accounts_export_status
    FROM transaction_export_folder tef
    WHERE tef.document_ref = DocumentRef
*/
UPDATE #tempRSATransReg
    SET InsFileCnt = i.insurance_file_cnt,
        Product = pr.description,
        ProductCode = pr.code,
        BusType = b.code,
        ExportStatus = 'c'
    FROM Insurance_file i,
         business_type b,
         Document d,
         product pr
    WHERE DocumentId = d.document_id
    AND i.insurance_file_cnt = d.insurance_file_cnt
    AND i.business_type_id = b.business_type_id
    AND i.product_id = pr.product_id
--sj 31/07/2002 - end 

--print 'update with Party details'
UPDATE #tempRSATransReg
    SET ClientName = p.shortname
    FROM Party p
    WHERE p.party_cnt =
        (SELECT ifo.insurance_holder_cnt
        FROM Insurance_File ifi
        JOIN Insurance_Folder ifo ON ifi.insurance_folder_cnt = ifo.insurance_folder_cnt
        WHERE ifi.insurance_file_cnt = InsFileCnt)

--print 'filter out zeros and failed exports'
SET NOCOUNT OFF
Select * FROM #tempRSATransReg
WHERE (
    Isnull(GrossAmount,0) <> 0
    OR Isnull(Amount,0) <> 0
    OR Isnull(CommissionAndTaxAmounts,0) <> 0
    )
AND ExportStatus = 'c'

DROP TABLE #tempRSATransReg
GO
