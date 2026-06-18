SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Report_WeekMonth_Audit7'
GO


CREATE PROCEDURE spu_Report_WeekMonth_Audit7
    -- TF030500

    @branch_id int,
    @start_date datetime,
    @end_date datetime
AS

--**************************************************************************
-- Created by Jude Killip       20/07/2001
--
-- Same as original Broking sp_Report_Daily_Audit7
--**************************************************************************
-- TF030500
DECLARE @iBranchID  int

SELECT @iBranchID = ISNULL(@branch_id, 0)

IF @iBranchID = 0
BEGIN
    SELECT
        D.document_ref,
        D.document_date,
        A.short_code        acc_code,
        A.ledger_id         ledger,
        T.transdetail_id        transdetail_id,
        T.amount        amount,
        T.company_id        branch_id,
                C.description       branch,
        ISNULL(
        (
            SELECT
                short_code
            FROM    Account       A1,
                BankAccount   B
            WHERE
                B.account_id = A1.account_id
            AND A1.short_code = A.short_code
        ), '') bank_account
    FROM    TransDetail   T,
        Document      D,
        Account       A,
        Company       C
    WHERE
        T.document_id = D.document_id
    AND     T.Account_id = A.Account_id
    AND D.documenttype_id IN (1)
    AND
    (
        D.document_date >= @start_date
        AND
        D.document_date <= @end_date
    )
        AND C.company_id = T.company_id
    AND T.amount <> 0
    ORDER BY document_ref, acc_code
END
ELSE
BEGIN
    SELECT
        D.document_ref,
        D.document_date,
        A.short_code        acc_code,
        A.ledger_id         ledger,
        T.transdetail_id        transdetail_id,
        T.amount        amount,
        T.company_id        branch_id,
                C.description       branch,
        ISNULL(
        (
            SELECT
                short_code
            FROM    Account       A1,
                BankAccount   B
            WHERE
                B.account_id = A1.account_id
            AND A1.short_code = A.short_code
        ), '') bank_account
    FROM    TransDetail   T,
        Document      D,
        Account       A,
        Company       C
    WHERE
        T.document_id = D.document_id
    AND     T.Account_id = A.Account_id
    AND D.documenttype_id IN (1)
    AND
    (
        D.document_date >= @start_date
        AND
        D.document_date <= @end_date
    )
        AND C.company_id = T.company_id
    AND T.amount <> 0
    AND C.company_id = @iBranchID
    ORDER BY document_ref, acc_code
END
GO


