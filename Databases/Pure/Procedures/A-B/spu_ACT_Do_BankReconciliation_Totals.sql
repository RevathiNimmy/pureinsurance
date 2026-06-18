EXECUTE DDLDropProcedure 'spu_ACT_Do_BankReconciliation_Totals'
GO

CREATE PROCEDURE spu_ACT_Do_BankReconciliation_Totals
    @account_id int,
    @date_to datetime = NULL,
    @month int = NULL
AS
BEGIN
    DECLARE @settlement_period smallint
    DECLARE @document_ref varchar(25)
    DECLARE @short_code char(20)
    DECLARE @gross_transdetail_id int
    DECLARE @banktranstemp_id int
    DECLARE @company_id int	
    SELECT @settlement_period = settlement_period
        FROM Account
        WHERE account_id = @account_id
    CREATE TABLE #BankTransTemp (
        banktranstemp_id int IDENTITY PRIMARY KEY,
        source_id int,
        period varchar(15) NULL,
        short_code char(20),
        cheque_no varchar(30),
        document_ref varchar(25) NULL,
        accounting_date datetime NULL,
        gross_transdetail_id int NULL,
        gross_amount numeric(19, 4) NULL,
        currency_id int,
        currency varchar(4) NULL,
        marked_status tinyint,
        month smallint NULL,
        spare char(20)
    )
    INSERT INTO #BankTransTemp (
        source_id,
        period,
        short_code,
        cheque_no,
        document_ref,
        accounting_date,
        gross_transdetail_id,
        gross_amount,
        currency_id,
        currency,
        marked_status,
        month,
        spare
    ) SELECT
        d.company_id,
        p.period_name,
        '***Multiple***',
        'Not Available',
        d.document_ref,
        t.accounting_date,
        t.transdetail_id,
        t.currency_amount,
        t.currency_id,
        c.iso_code,
        0,
        DatePart(mm, DateAdd(dd, @settlement_period, t.accounting_date)),
        t.spare
        FROM Account a, Transdetail t, Document d , period P , currency C
        WHERE (a.account_id = @account_id)
        AND (t.account_id =a.account_id)
        AND (d.document_id = t.document_id)
        AND (t.accounting_date <= @date_to OR @date_to IS NULL)
        AND (p.period_id = t.period_id)
        AND (t.currency_id=c.currency_id)
    IF @month IS NOT NULL BEGIN
        DELETE FROM #BankTransTemp
            WHERE month <> @month
    END
    UPDATE #BankTransTemp
        SET marked_status = 2
        WHERE gross_transdetail_id IN (
            SELECT transdetail_id
            FROM transmatch
            WHERE allocationdetail_id IS NULL
        )
    UPDATE #BankTransTemp
        SET marked_status = 1
        WHERE spare like '%RECONCILED%'
        
    SELECT source_id,
        period,
        short_code,
        cheque_no,
        document_ref,
        accounting_date,
        gross_transdetail_id,
        gross_amount,
        currency_id,
        currency,
        marked_status,
        month,
        spare
        FROM #BankTransTemp
        ORDER BY source_id, document_ref
    DROP TABLE #BankTransTemp
END

