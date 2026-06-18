SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Do_FindTrans'
GO


CREATE PROCEDURE spu_ACT_Do_FindTrans
    @CompanyID int = NULL,
    @AccountID int = NULL,
    @DocumentRef varchar(25) = NULL,
    @CurrencyID smallint = NULL,
    @CurrencyAmount numeric(19,4) = NULL,
    @Tolerance smallint = NULL,
    @DocTypeGroupId smallint = NULL,
    @DocumentTypeID smallint = NULL,
    @PeriodId smallint = NULL,
    @DateFrom datetime = NULL,
    @DateTo datetime = NULL,
    @InsuranceRef varchar(30) = NULL,
    @Username varchar(255) = NULL,
    @PurchaseOrderNo varchar(40) = NULL,
    @PurchaseInvoiceNo varchar(40) = NULL,
    @Department varchar(20) = NULL,
    @Spare varchar(20) = NULL,
    @Outstanding char(1),
    @sub_branch_id int = NULL
AS


SELECT DISTINCT
       d.document_ref,
       d.document_id,
       t.document_sequence,
       t.accounting_date,
       p.period_name,
       t.currency_amount,
       d.documenttype_id,
       dt.doctypegroup_id,
       t.insurance_ref,
       pmu.username,
       t.purchase_order_no,
       t.purchase_invoice_no,
       t.department,
       t.spare,
       a.short_code,
       a.account_id,
       t.currency_id,
       t.transdetail_id,
       t.amount,
       t.fully_matched
INTO   #FindTrans
FROM   TransDetail t
JOIN   Account a ON t.account_id = a.account_id
JOIN   Document d ON t.document_id = d.document_id
JOIN   Period p ON t.period_id = p.period_id
JOIN   DocumentType dt ON d.documenttype_id = dt.documenttype_id
JOIN   PMUser pmu ON t.operator_id = pmu.user_id
WHERE (d.document_ref like @DocumentRef OR @DocumentRef IS NULL)
AND   (d.documenttype_id = @DocumentTypeID OR @DocumentTypeID IS NULL)
AND   (dt.doctypegroup_id = @DocTypeGroupId OR @DocTypeGroupId IS NULL)
AND   (pmu.username like @Username OR @Username IS NULL)
AND   (t.transdetail_id IN (SELECT transdetail_id
                            FROM   Transdetail
                            WHERE  account_id = @AccountID) OR @AccountID IS NULL)
AND   (purchase_order_no like @PurchaseOrderNo OR @PurchaseOrderNo IS NULL)
AND   (purchase_invoice_no like @PurchaseInvoiceNo OR @PurchaseInvoiceNo IS NULL)
AND   (department like @Department OR @Department IS NULL)
AND   (spare like @Spare OR @Spare IS NULL)
AND   (p.period_id = @PeriodId OR @PeriodId IS NULL)
AND   (accounting_date >= @DateFrom OR @DateFrom IS NULL)
AND   (accounting_date <= @DateTo OR @DateTo IS NULL)
AND   (insurance_ref like @InsuranceRef OR @InsuranceRef IS NULL)
AND   (a.company_id = @CompanyID OR @CompanyID IS NULL)
AND   (a.sub_branch_id = @sub_branch_id OR @sub_branch_id IS NULL)
AND   (t.currency_id = @CurrencyID OR @CurrencyID IS NULL)
AND (((currency_amount >= (@CurrencyAmount - (@CurrencyAmount * @Tolerance / 100)) ) 
AND   (currency_amount <= (@CurrencyAmount + (@CurrencyAmount * @Tolerance / 100)) ))
OR     @CurrencyAmount IS NULL)

/* CF 101199 - Add Indexes */
CREATE CLUSTERED INDEX tt1 ON #FindTrans (transdetail_id)

CREATE INDEX tt2 ON #FindTrans (currency_amount)
/* CF 101199 - End */

SELECT transdetail_id,
       sum(base_match_amount) as base_match_amount,
       sum(currency_match_amount) as currency_match_amount
INTO   #MatchAmounts
FROM   TransMatch
WHERE  transdetail_id IN (SELECT transdetail_id 
                          FROM   #FindTrans)
GROUP BY transdetail_id


INSERT INTO #MatchAmounts
    SELECT transdetail_id,
            0.00 as base_match_amount,
            0.00 as currency_match_amount
    FROM #FindTrans
    WHERE transdetail_id NOT IN (SELECT transdetail_id FROM TransMatch)

/* CF 101199 - Add Index */
CREATE CLUSTERED INDEX ma1 ON #MatchAmounts (transdetail_id)
/* CF 101199 - End */

IF (@Outstanding = '0') BEGIN
SELECT ft.document_ref,
    ft.accounting_date,
    ft.period_name,

    ft.currency_amount,
    ft.fully_matched,
    ma.currency_match_amount,
    ft.documenttype_id,
    ft.doctypegroup_id,
    ft.insurance_ref,
    ft.username,
    ft.purchase_order_no,
    ft.purchase_invoice_no,
    ft.department,
    ft.spare,
    ft.short_code,
    ft.account_id,
    ft.currency_id,
    ft.transdetail_id,
    ft.amount,
    ft.document_sequence
    FROM #FindTrans ft, #MatchAmounts ma
    WHERE ft.transdetail_id = ma.transdetail_id

    ORDER BY
    ft.document_ref,
    ft.document_sequence
END
ELSE BEGIN
SELECT ft.document_ref,
    ft.accounting_date,
    ft.period_name,
    ft.currency_amount,
    ft.fully_matched,
    ma.currency_match_amount,
    ft.documenttype_id,

    ft.doctypegroup_id,

    ft.insurance_ref,
    ft.username,
    ft.purchase_order_no,
    ft.purchase_invoice_no,
    ft.department,
    ft.spare,
    ft.short_code,
    ft.account_id,
    ft.currency_id,
    ft.transdetail_id,
    ft.amount,
    ft.document_sequence

    FROM #FindTrans ft, #MatchAmounts ma
    WHERE ft.transdetail_id = ma.transdetail_id
      AND ma.currency_match_amount <> ft.currency_amount
    ORDER BY
    ft.document_ref,
    ft.document_sequence
END

SELECT sum(currency_amount) from #FindTrans
SELECT sum(base_match_amount) from #MatchAmounts


GO


