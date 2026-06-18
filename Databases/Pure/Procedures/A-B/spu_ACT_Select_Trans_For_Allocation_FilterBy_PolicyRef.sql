SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_ACT_Select_Trans_For_Allocation_FilterBy_PolicyRef'
GO

CREATE PROCEDURE spu_ACT_Select_Trans_For_Allocation_FilterBy_PolicyRef
    @lTransDetailId INT
AS
BEGIN

    -- Get the AccountId for this Transaction
    DECLARE @lAccountId INT
    SELECT
        @lAccountId = account_id
    FROM
        TransDetail
    WHERE
        transdetail_id = @lTransDetailId

    DECLARE @sMediaRef VARCHAR(30)
    DECLARE @sTheirRef VARCHAR(30)
    DECLARE @sOurRef VARCHAR(255)

    SELECT
        @sMediaRef = media_ref,
        @sTheirRef = their_ref,
        @sOurRef = our_ref
    FROM
       CashListItem
    WHERE
       transdetail_id = @lTransDetailId

    DECLARE @transdetail_id int
    CREATE TABLE #OSTransactions (
        transdetail_id int,
        amount numeric(19,4),
        currency_amount numeric(19,4),
        insurance_ref varchar(30)

    )
    INSERT INTO #OSTransactions
    SELECT t.transdetail_id,
        t.amount,
        t.currency_amount,
        inf.insurance_ref
        FROM
            transdetail t
        JOIN
            account a ON (a.account_id = t.account_id)
        JOIN
            document d ON (d.document_id = t.document_id)
        LEFT OUTER JOIN
            insurance_file inf ON (inf.insurance_file_cnt = d.insurance_file_cnt)
        WHERE
            a.account_id = @lAccountId
        AND (
                t.transdetail_id = @lTransDetailId
            OR
                inf.insurance_ref = @sMediaRef
            OR
                inf.insurance_ref = @sTheirRef
            OR
                inf.insurance_ref = @sOurRef
        )
        AND SUBSTRING(document_ref,1,3) NOT IN ('IND', 'IRD', 'IED')

    --Filter out matched and modify amounts
    DECLARE c_OS CURSOR FAST_FORWARD FOR
        SELECT transdetail_id
        FROM #OSTransactions
    OPEN c_OS
    FETCH NEXT FROM c_OS INTO @transdetail_id
    WHILE @@FETCH_STATUS = 0 BEGIN
        UPDATE #OSTransactions
            SET amount = amount - (
                SELECT ISNULL(SUM(base_match_amount), 0)
                FROM TransMatch
                WHERE transdetail_id = @transdetail_id
                AND allocationdetail_id IS NOT NULL
            ),
            currency_amount = currency_amount - (
                SELECT ISNULL(SUM(currency_match_amount), 0)
                FROM TransMatch
                WHERE transdetail_id = @transdetail_id
                AND allocationdetail_id IS NOT NULL
            )
            WHERE transdetail_id = @transdetail_id
        FETCH NEXT FROM c_OS INTO @transdetail_id
    END
    CLOSE c_OS
    DEALLOCATE c_OS

    --Filter out 0 amount
    DELETE FROM #OSTransactions
    WHERE
        amount = 0
    AND
        currency_amount = 0

    --Return data to caller
    SELECT
        transdetail_id,
        amount,
        currency_amount
    FROM
        #OSTransactions

    --Clear up the resources
    DROP TABLE #OSTransactions
END