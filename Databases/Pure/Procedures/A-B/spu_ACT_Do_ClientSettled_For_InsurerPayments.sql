SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_ACT_Do_ClientSettled_For_InsurerPayments'
GO

CREATE PROCEDURE spu_ACT_Do_ClientSettled_For_InsurerPayments
    @ClientTransdetailID INT,
    @ClientAmount MONEY OUTPUT,
    @ClientSettled MONEY OUTPUT,
    @EndDate DATETIME = NULL
AS

 
DECLARE @IsPremiumFinance BIT
DECLARE @IsThirdParty BIT
DECLARE @instalment_transdetail_id INT
DECLARE @deposit1_transdetail_id INT
DECLARE @deposit2_transdetail_id INT
DECLARE @document_id INT
 
SELECT
    @IsPremiumFinance = 1,
    @IsThirdParty = 
        (
            CASE d2.documenttype_id
                WHEN 1 THEN 1
                ELSE 0
            END
        )
FROM transdetail td
JOIN transmatch tm
    ON tm.transdetail_id = td.transdetail_id
    AND tm.is_reversed IS NULL
JOIN matchgroup mg
    ON mg.match_id = tm.match_id
    AND mg.match_date <= ISNULL(@EndDate, mg.match_date)
JOIN transmatch tm2
    ON tm2.match_id = tm.match_id
    AND tm2.is_reversed IS NULL
JOIN transdetail td2
    ON td2.transdetail_id = tm2.transdetail_id
JOIN document d2
    ON d2.document_id = td2.document_id
WHERE td.transdetail_id = @ClientTransdetailID
AND (
        (
            /*Third Party Instalments*/
            d2.documenttype_id = 1
            AND 
            RTRIM(d2.comment) = 'Premium Finance Transfer'
        )
        OR
        (
            /*In-House Instalments*/
            d2.documenttype_id = 43
        )
    )  

IF @IsPremiumFinance IS NOT NULL
BEGIN
 
    IF @IsThirdParty = 0
    BEGIN
        /*Get transactions that need to be paid off for in-house instalments*/
        SELECT
            @instalment_transdetail_id = td5.transdetail_id,
            @deposit1_transdetail_id = td6.transdetail_id,
            @deposit2_transdetail_id = td7.transdetail_id
        FROM transdetail td
        JOIN transmatch tm
            ON tm.transdetail_id = td.transdetail_id
            AND tm.is_reversed IS NULL
        JOIN transmatch tm2
            ON tm2.match_id = tm.match_id
            AND tm2.transmatch_id <> tm.transmatch_id
            AND tm2.is_reversed IS NULL
        JOIN transdetail td2
            ON td2.transdetail_id = tm2.transdetail_id
            AND td2.document_sequence = 1
        JOIN document d2
            ON d2.document_id = td2.document_id
            AND d2.documenttype_id = 43
        JOIN transdetail td3
            ON td3.document_id = d2.document_id
            AND td3.document_sequence = 2
        JOIN transmatch tm3
            ON tm3.transdetail_id = td3.transdetail_id
            AND tm3.is_reversed IS NULL
        /*Get the IND transaction*/
        JOIN transmatch tm4
            ON tm4.match_id = tm3.match_id
            AND tm4.transmatch_id <> tm3.transmatch_id
            AND tm4.is_reversed IS NULL
        JOIN transdetail td4
            ON td4.transdetail_id = tm4.transdetail_id
            AND td4.document_sequence = 1
        JOIN document d4
            ON d4.document_id = td4.document_id
            AND d4.documenttype_id = 37
        JOIN transdetail td5
            ON td5.document_id = d4.document_id
            AND td5.document_sequence = 2
        JOIN account a5
            ON a5.account_id = td5.account_id
        JOIN ledger l5
            ON l5.ledger_id = a5.ledger_id
            AND l5.ledger_short_name = 'SA'
        /*Get the deposit JN transaction, if there is one*/
        LEFT JOIN transmatch tm6
                JOIN transdetail td6
                    ON td6.transdetail_id = tm6.transdetail_id
                    AND td6.document_sequence = 1
                JOIN document d6
                    ON d6.document_id = td6.document_id
                    AND d6.documenttype_id = 1
                JOIN transdetail td7
                    ON td7.document_id = d6.document_id
                    AND td7.document_sequence = 2
            ON tm6.match_id = tm.match_id
            AND tm6.transmatch_id <> tm.transmatch_id
            AND tm6.transmatch_id <> tm2.transmatch_id
            AND tm6.is_reversed IS NULL
        WHERE td.transdetail_id = @ClientTransdetailID
        
    END
    ELSE
    BEGIN
    
        /*Get transactions that need to be paid off for third party instalments*/
        SELECT
            @instalment_transdetail_id = td5.transdetail_id,
            /*@deposit1_transdetail_id = td6.transdetail_id,*/
            @deposit2_transdetail_id = td7.transdetail_id
        FROM transdetail td
        JOIN transmatch tm
            ON tm.transdetail_id = td.transdetail_id
            AND tm.is_reversed IS NULL
        JOIN transmatch tm2
            ON tm2.match_id = tm.match_id
            AND tm2.transmatch_id <> tm.transmatch_id
            AND tm2.is_reversed IS NULL
        JOIN transdetail td2
            ON td2.transdetail_id = tm2.transdetail_id
            AND td2.document_sequence = 1
        JOIN document d2
            ON d2.document_id = td2.document_id
            AND d2.documenttype_id = 1
        JOIN transdetail td3
            ON td3.document_id = d2.document_id
            AND td3.document_sequence = 2
        JOIN transmatch tm3
            ON tm3.transdetail_id = td3.transdetail_id
            AND tm3.is_reversed IS NULL
        /*Get the main JN transaction*/
        JOIN transmatch tm4
            ON tm4.match_id = tm3.match_id
            AND tm4.transmatch_id <> tm3.transmatch_id
            AND tm4.is_reversed IS NULL
            AND tm4.transmatch_id IN
                (
                    SELECT
                        MAX(transmatch_id)
                    FROM transmatch
                    WHERE match_id = tm3.match_id
                    AND transmatch_id <> tm3.transmatch_id
                )
        JOIN transdetail td4
            ON td4.transdetail_id = tm4.transdetail_id
            AND td4.document_sequence = 1
        JOIN document d4
            ON d4.document_id = td4.document_id
            AND d4.documenttype_id = 1
        JOIN transdetail td5
            ON td5.document_id = d4.document_id
            AND td5.document_sequence = 2
        /*Get the deposit JN transaction*/
        LEFT JOIN transmatch tm6
                JOIN transdetail td6
                    ON td6.transdetail_id = tm6.transdetail_id
                    AND td6.document_sequence = 1
                JOIN document d6
                    ON d6.document_id = td6.document_id
                    AND d6.documenttype_id = 1
                JOIN transdetail td7
                    ON td7.document_id = d6.document_id
                    AND td7.document_sequence = 1
            ON tm6.match_id = tm3.match_id
            AND tm6.transmatch_id <> tm3.transmatch_id
            AND tm6.transmatch_id <> tm4.transmatch_id
            AND tm6.is_reversed IS NULL
        WHERE td.transdetail_id = @ClientTransdetailID

    END

    SELECT    
        @ClientAmount = 
            (
                SELECT 
                    SUM(ROUND(td.amount,2))
                FROM transdetail td
                WHERE 
                (
                    td.transdetail_id = @deposit1_transdetail_id
                    OR
                    td.transdetail_id = @deposit2_transdetail_id
                    OR
                    td.transdetail_id = @instalment_transdetail_id
                )
            ),
        @ClientSettled = 
            (
                SELECT 
                    ISNULL(SUM(ROUND(tm.base_match_amount,2)),0)
                FROM transdetail td
                JOIN transmatch tm
                    ON tm.transdetail_id = td.transdetail_id
                    AND tm.is_reversed IS NULL
                JOIN matchgroup mg
                    ON mg.match_id = tm.match_id
                    AND mg.match_date <= ISNULL(@EndDate, mg.match_date)
                WHERE 
                (
                    td.transdetail_id = @deposit1_transdetail_id
                    OR
                    td.transdetail_id = @deposit2_transdetail_id
                    OR
                    td.transdetail_id = @instalment_transdetail_id
                )
            )

END
 

IF @IsPremiumFinance IS NULL OR (@instalment_transdetail_id IS NULL AND @deposit1_transdetail_id IS NULL AND @deposit2_transdetail_id IS NULL)
BEGIN

    IF EXISTS
        (
            SELECT 
                NULL
            FROM transdetail td
            JOIN account a
                ON a.account_id = td.account_id
            JOIN ledger l
                ON l.ledger_id = a.ledger_id
            WHERE td.transdetail_id = @ClientTransdetailID 
            AND l.ledger_short_name = 'SA'
         )
    BEGIN
        SELECT 
            @document_id = document_id 
        FROM transdetail 
        WHERE transdetail_id = @ClientTransdetailID

        SELECT 
            @ClientAmount = 
                (
                    SELECT
                        SUM(td.currency_amount)
                    FROM transdetail td
                    JOIN account a
                        ON a.account_id = td.account_id
                    JOIN ledger l
                        ON l.ledger_id = a.ledger_id
                    WHERE td.document_id = @document_id 
                    AND l.ledger_short_name = 'SA'
                ),
            @ClientSettled = 
                (
                    SELECT
                        ISNULL(SUM(tm.currency_match_amount), 0)
                    FROM transdetail td
                    JOIN account a
                        ON a.account_id = td.account_id
                    JOIN ledger l
                        ON l.ledger_id = a.ledger_id
                    JOIN transmatch tm
                        ON tm.transdetail_id = td.transdetail_id
                        AND tm.is_reversed IS NULL
                    JOIN matchgroup mg
                        ON mg.match_id = tm.match_id
                        AND mg.match_date <= ISNULL(@EndDate, mg.match_date)
                    WHERE td.document_id = @document_id 
                    AND l.ledger_short_name = 'SA'
                )

    END
    ELSE
    BEGIN

        SELECT   
            @ClientAmount = td.currency_amount,
            @ClientSettled = 
                (
                    SELECT
                        ISNULL(SUM(tm.currency_match_amount), 0)
                    FROM transmatch tm
                    JOIN matchgroup mg
                        ON mg.match_id = tm.match_id
                        AND mg.match_date <= ISNULL(@EndDate, mg.match_date)
                    WHERE tm.transdetail_id = td.transdetail_id
                    AND tm.is_reversed IS NULL
                )
        FROM transdetail td
        WHERE td.transdetail_id = @ClientTransdetailID

    END

END
GO 


 