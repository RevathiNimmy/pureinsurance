SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON
GO

EXECUTE DDLDropProcedure 'spu_ACT_Do_AgentFinance'
GO

CREATE PROCEDURE spu_ACT_Do_AgentFinance
    @account_id int,
    @date_to datetime = NULL,
    @marked_status int = NULL,
    @month int = NULL
AS
/******************************************************************************************/
/* Procedure: sp_ACT_Do_AgentPayments                         */
/* Description: Used by AgentPayments                         */
/* Edit History: ECK 130300 - Created.                            */
/******************************************************************************************/
BEGIN

    DECLARE @settlement_period  smallint
    DECLARE @amt_settled        numeric(19, 4)
    DECLARE @transdetail_id     int
    DECLARE @document_id        int
    DECLARE @currency_amount    numeric(19, 4)
    DECLARE @commadj_amount     numeric(19, 4)
    DECLARE @document_id_copy   int

    /* Get the settlement period */
    SELECT  @settlement_period = settlement_period
    FROM    Account
    WHERE   account_id = @account_id

    CREATE TABLE #InsurerTemp
    (
        account_name    char(60),
        insurer_ref varchar(30),
        document_ref    varchar(25) NULL,
        gross_transdetail_id    int NULL,
        gross_amount    numeric(19, 4) NULL,
        primary_transdetail_id  int NULL,
        primary_amount  numeric(19, 4) NULL,
        adj_transdetail_id int NULL,
        adj_amount numeric(19,4) NULL,
        amt_settled numeric(19, 4) NULL,
        document_id int NULL,
        accounting_date datetime NULL,
        currency_id smallint NULL,
        marked_status   tinyint,
        month       smallint NULL,
        spare       char(8),
        payment     numeric(19, 4) NULL,
        source_id   int,
        short_code  char(20))

    INSERT INTO #InsurerTemp

    SELECT  a2.account_name,
        t.insurance_ref,
        d.document_ref,
        t.transdetail_id,
        t.currency_amount,
        0,
        t2.currency_amount,
        0,
        0,
        0,
        d.document_id,      t.accounting_date,      t.currency_id,
        0,
        DatePart(mm, DateAdd(dd, @settlement_period, t.accounting_date)),
        t.spare,
        0,
        t.company_id,
        a2.short_code

    FROM    Account a, Account a2, Transdetail t, Transdetail t2, Document d

    WHERE   (a.account_id = @account_id)
    AND (t.account_id = @account_id)
    AND (a2.account_id = t2.account_id)
    AND (d.document_id = t.document_id) AND (t.accounting_date <= @date_to OR @date_to IS NULL)
    AND     (t2.document_sequence = 1)
    AND     (t.document_id = t2.document_id)
    AND (t.spare= 'AGENT')

    /* Update the Amounts with any adjustments. They should all be 0 at the moment. */
    /*eck there could be more than one
    UPDATE #InsurerTemp
        SET adj_transdetail_id = t.transdetail_id,
        adj_amount = t.currency_amount
        FROM transdetail t,
        #insurertemp it
        WHERE it.document_id = t.document_id
        AND t.spare = 'AGENT ADJ'
    */

    /*Commission Adjustments */
    SELECT @commadj_amount = 0

    /* Declare the Temp Cursor */
    DECLARE it_adjtemp CURSOR FAST_FORWARD FOR
        SELECT t.currency_amount,
        it.document_id
        FROM transdetail t,
        #InsurerTemp it
        WHERE it.document_id = t.document_id
        AND t.account_id = @account_id
        AND t.spare = 'AGENT ADJ'
        ORDER BY it.document_id /* Open the temp Cursor */

    OPEN it_adjtemp

    FETCH NEXT FROM it_adjtemp INTO @currency_amount, @document_id

    --BSJ
    SELECT @document_id_copy = @document_id

    WHILE @@FETCH_STATUS = 0 BEGIN
        -- Are we still on the same doc_id?
        IF @document_id_copy = @document_id BEGIN
            -- Add the curr amnts together for the same doc id
            SELECT @commadj_amount = @commadj_amount + @currency_amount
        END

        /* Fetch Next */
        FETCH NEXT FROM it_adjtemp INTO @currency_amount, @document_id

        IF @document_id_copy <> @document_id BEGIN
            UPDATE #InsurerTemp
                SET adj_amount = @commadj_amount
                WHERE document_id = @document_id_copy

            SELECT @commadj_amount = 0
            SELECT @document_id_copy = @document_id
        END
    END

    -- Any left over?
    IF @commadj_amount <> 0 BEGIN
        UPDATE #InsurerTemp
            SET adj_amount = @commadj_amount
            WHERE document_id = @document_id
    END

    /* Close and Deallocate Cursor */
    CLOSE it_adjtemp
    DEALLOCATE it_adjtemp

    /* Declare the Temp Cursor */
    DECLARE it_instemp CURSOR FAST_FORWARD FOR
        SELECT transdetail_id = it.gross_transdetail_id
        FROM #InsurerTemp it

    /* Open the temp Cursor */
    OPEN it_instemp
    FETCH NEXT FROM it_instemp INTO @transdetail_id

    WHILE @@FETCH_STATUS = 0 BEGIN
        SELECT @amt_settled = 0

        SELECT @amt_settled = sum(a.alloc_base_amount)
            FROM transdetail t,
            allocationdetail a
            WHERE t.transdetail_id = @transdetail_id
            AND a.transdetail_id = t.transdetail_id

        UPDATE #InsurerTemp
            SET amt_settled = @amt_settled
            WHERE gross_transdetail_id =  @transdetail_id
            AND @amt_settled IS NOT NULL

        /* Fetch Next */
        FETCH NEXT FROM it_instemp INTO @transdetail_id
    END

    /* Close and Deallocate Cursor */
    CLOSE it_instemp
    DEALLOCATE it_instemp

    /* Remove Fully Settled Ones */
    DELETE FROM #InsurerTemp
        WHERE amt_settled = gross_amount

    /* Remove ones that dont comply with the month, if needed */
    IF @month IS NOT NULL BEGIN
        DELETE FROM #InsurerTemp
            WHERE month <> @month
    END

    /* Update the marked status. They should all be 0 at the moment. */
    UPDATE #InsurerTemp
        SET marked_status = 1
        WHERE gross_transdetail_id IN (
            SELECT transdetail_id AS base_match_amount
            FROM transmatch
            WHERE allocationdetail_id IS NULL
        )

    /* Remove the marked ones now if needed */
    IF @marked_status IS NOT NULL BEGIN
        DELETE FROM #InsurerTemp
            WHERE marked_status = (1 - (@marked_status))
    END

    /* Set all the amount settled nulls to 0 */
    UPDATE #InsurerTemp
        SET amt_settled = 0
        WHERE amt_settled IS NULL

    /* Select it all. We know the column order so an asterisk should suffice. */
    SELECT *
        FROM #InsurerTemp
        ORDER BY source_id, insurer_ref, document_ref

    /* Remove the temp table */
    DROP TABLE #InsurerTemp
END
GO

