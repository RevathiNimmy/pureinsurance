SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Select_TransDetail_OS'
GO


CREATE PROCEDURE spu_ACT_Select_TransDetail_OS
    @transdetail_id int = NULL,
    @account_id int = NULL
AS


IF (@account_id IS NOT NULL AND @transdetail_id IS NOT NULL)
    SELECT d.document_ref,
        t.transdetail_id,
        sum(m.base_match_amount) AS matched_base_amount,
        sum(m.currency_match_amount) AS matched_currency_amount
    FROM Document d, TransDetail t LEFT JOIN TransMatch m
    ON t.transdetail_id = m.transdetail_id
    WHERE (t.transdetail_id = @transdetail_id)
    AND (t.account_id = @account_id)
    AND (t.document_id = d.document_id)
    AND (m.allocationdetail_id IS NOT NULL)
    GROUP BY d.document_ref,
        t.transdetail_id,
        t.amount,
        t.currency_amount
ELSE IF (@account_id IS NULL AND @transdetail_id IS NOT NULL)
    SELECT d.document_ref,
        t.transdetail_id,
        sum(m.base_match_amount) AS matched_base_amount,
        sum(m.currency_match_amount) AS matched_currency_amount
    FROM Document d, TransDetail t LEFT JOIN TransMatch m
    ON t.transdetail_id = m.transdetail_id
    WHERE (t.transdetail_id = @transdetail_id)
    AND (t.document_id = d.document_id)
    AND (m.allocationdetail_id IS NOT NULL)
    GROUP BY d.document_ref,
        t.transdetail_id,
        t.amount,
        t.currency_amount
ELSE IF (@account_id IS NOT NULL AND @transdetail_id IS NULL)
    SELECT d.document_ref,
        t.transdetail_id,
        sum(m.base_match_amount) AS matched_base_amount,
        sum(m.currency_match_amount) AS matched_currency_amount
    FROM Document d, TransDetail t LEFT JOIN TransMatch m
    ON t.transdetail_id = m.transdetail_id
    WHERE (t.account_id = @account_id)
    AND (t.document_id = d.document_id)
    AND (m.allocationdetail_id IS NOT NULL)
    GROUP BY d.document_ref,
        t.transdetail_id,
        t.amount,
        t.currency_amount
ELSE
    SELECT d.document_ref,
        t.transdetail_id,
        sum(m.base_match_amount) AS matched_base_amount,
        sum(m.currency_match_amount) AS matched_currency_amount
    FROM Document d, TransDetail t LEFT JOIN TransMatch m
    ON t.transdetail_id = m.transdetail_id
    WHERE (t.document_id = d.document_id)
    AND (m.allocationdetail_id IS NOT NULL)
    GROUP BY d.document_ref,
        t.transdetail_id,
        t.amount,
        t.currency_amount
GO


