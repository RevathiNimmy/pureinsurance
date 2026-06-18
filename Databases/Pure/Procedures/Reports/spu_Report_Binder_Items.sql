SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


EXECUTE DDLDropProcedure 'spu_Report_Binder_Items'
GO

CREATE  PROCEDURE spu_Report_Binder_Items
    @account_type varchar(255),
    @short_code varchar(255)
AS
DECLARE @iLedgerID int, @UWType char(1)
SELECT @iLedgerID = CASE @account_type
        WHEN  'REINSURER' THEN 4
        WHEN  'AGENT' THEN 5
        WHEN  'SUBAGENT' THEN 10
END
EXECUTE spu_Report_GetUnderwritingType @UWType OUTPUT
CREATE TABLE #tempRSABinderItems
(
    AccountID int NULL,
    AccountCode varchar (30),
    AccountName varchar (60),
    RecordType int NULL,
    DocID int NULL,
    DocRef varchar (25) NULL,
    DocDate datetime NULL,
    DocTypeID int NULL,
    LedgerName varchar (30) NULL,
    TransDetailID int NULL,
    InsuranceRef varchar (30) NULL,
    AccountingDate datetime NULL,
    FullyMatched int NULL,
    Amount numeric (19,4) NULL,
    CommissionAndTax numeric (19,4) NULL,
    GrossAmount numeric (19,4) NULL,
    IsMatched int NULL,
    MatchAmount numeric (19,4) NULL,
    ProductCode varchar (10) NULL,
    EffectiveDate datetime NULL,
    InsuranceHolder varchar (20) NULL,
    AccountExportStatus char (1) NULL,
    FromSirius tinyint NULL,
    UWType char(1) NULL
)
IF IsNull(@short_code,'ALL') <> 'ALL'
BEGIN
    INSERT INTO #tempRSABinderItems
            SELECT  a.account_id,
            a.short_code,
            a.account_name,
            a.ledger_id,
            d.document_id,
            d.document_ref,
            d.document_date,
            d.documenttype_id,
            l.ledger_name,
            td.transdetail_id,
            td.insurance_ref,
            td.accounting_date,
            td.fully_matched,
            td.Amount,
            (SELECT td.amount WHERE td.spare = 'COMM' OR td.spare LIKE ('TAX%')),
            (SELECT td.amount WHERE td.spare = 'GROSS'),
            0,
            (SELECT SUM(AllocationDetail.alloc_base_amount)
                FROM AllocationDetail AllocationDetail
                WHERE td.transdetail_id = AllocationDetail.transdetail_id),
            NULL,
            NULL,
            NULL,
            NULL,
            (SELECT dt.from_sirius
                FROM documenttype dt
                WHERE d.documenttype_id = dt.documenttype_id),
            @UWType
        FROM Account a
        JOIN  Ledger l          ON a.ledger_id = l.ledger_id
        JOIN  TransDetail td    ON a.account_id = td.account_id
            AND td.fully_matched <> 1
        JOIN  Document d        ON td.document_id = d.document_id
        WHERE a.short_code LIKE  @short_code + '%'
        AND a.ledger_id IN (4, 5, 10)
END
ELSE
IF @account_type = 'ALL'
BEGIN
    INSERT INTO #tempRSABinderItems
            SELECT  a.account_id,
            a.short_code,
            a.account_name,
            a.ledger_id,
            d.document_id,
            d.document_ref,
            d.document_date,
            d.documenttype_id,
            l.ledger_name,
            td.transdetail_id,
            td.insurance_ref,
            td.accounting_date,
            td.fully_matched,
            td.Amount,
            (SELECT td.amount WHERE td.spare = 'COMM' OR td.spare LIKE ('TAX%')),
            (SELECT td.amount WHERE td.spare = 'GROSS'),
            0,
            (SELECT SUM(AllocationDetail.alloc_base_amount)
                FROM AllocationDetail AllocationDetail
                WHERE td.transdetail_id = AllocationDetail.transdetail_id),
            NULL,
            NULL,
            NULL,
            NULL,
            (SELECT dt.from_sirius
                FROM documenttype dt
                WHERE d.documenttype_id = dt.documenttype_id),
            @UWType
        FROM Account a
        JOIN  Ledger l          ON a.ledger_id = l.ledger_id
        JOIN  TransDetail td    ON a.account_id = td.account_id
            AND td.fully_matched <> 1
        JOIN  Document d        ON td.document_id = d.document_id
        WHERE a.ledger_id IN (4, 5, 10)
END
ELSE
BEGIN
    INSERT INTO #tempRSABinderItems
            SELECT  a.account_id,
            a.short_code,
            a.account_name,
            a.ledger_id,
            d.document_id,
            d.document_ref,
            d.document_date,
            d.documenttype_id,
            l.ledger_name,
            td.transdetail_id,
            td.insurance_ref,
            td.accounting_date,
            td.fully_matched,
            td.Amount,
            (SELECT td.amount WHERE td.spare = 'COMM' OR td.spare LIKE ('TAX%')),
            (SELECT td.amount WHERE td.spare = 'GROSS'),
            0,
            (SELECT SUM(AllocationDetail.alloc_base_amount)
                FROM AllocationDetail AllocationDetail
                WHERE td.transdetail_id = AllocationDetail.transdetail_id),
            NULL,
            NULL,
            NULL,
            NULL,
            (SELECT dt.from_sirius
                FROM documenttype dt
                WHERE d.documenttype_id = dt.documenttype_id),
            @UWType
        FROM Account a
        JOIN  Ledger l          ON a.ledger_id = l.ledger_id
        JOIN  TransDetail td    ON a.account_id = td.account_id
            AND td.fully_matched <> 1
        JOIN  Document d        ON td.document_id = d.document_id
        WHERE a.ledger_id = @iLedgerID
END
UPDATE #tempRSABinderItems
SET IsMatched = 1 WHERE MatchAmount = Amount
UPDATE #tempRSABinderItems
    SET ProductCode = pr.code,
        EffectiveDate = i.cover_start_date,
        InsuranceHolder = pa.shortname,
        AccountExportStatus = 'c'
    FROM Insurance_file i,
         Party pa,
         Document d,
         product pr
    WHERE DocId = d.document_id
    AND i.insurance_file_cnt = d.insurance_file_cnt
    AND i.insured_cnt = pa.party_cnt
    AND i.product_id = pr.product_id
SELECT * FROM #tempRSABinderItems
WHERE IsMatched <> 1
AND (FromSirius = 1 and AccountExportStatus = 'c'
    OR FromSirius = 0)
DROP TABLE #tempRSABinderItems

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

