SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Report_Comm_Outstanding'
GO

CREATE PROCEDURE spu_Report_Comm_Outstanding 

    @end_date DATETIME,
    @branch_id INT,
    @TypeOfCurrency VARCHAR(15)

AS

DECLARE 
    @account_id INT,
    @document_id INT,
    
    @total_gross_commission MONEY,
    @total_fees MONEY,
    @total_agent MONEY,
    @total_subagent MONEY,
    @total_net_commission MONEY,
    @total_client_amount MONEY,
    @total_os_commission MONEY,

    @rounding_gross_commission MONEY,
    @rounding_fees MONEY,
    @rounding_agent MONEY,
    @rounding_subagent MONEY,
    @rounding_net_commission MONEY,
    @rounding_client_amount MONEY,
    @rounding_os_commission MONEY,
    
    @share_percentage FLOAT,
    @share_gross_commission MONEY,
    @share_fees MONEY,
    @share_agent MONEY,
    @share_subagent MONEY,
    @share_net_commission MONEY,
    @share_client_amount MONEY,
    @share_os_commission MONEY

SET NOCOUNT ON

SELECT @end_date = ISNULL(@end_date, GETDATE())

IF @branch_id = 0
BEGIN
    SELECT @branch_id = NULL
END

CREATE TABLE #Instalments
(
    document_id INT,
    third_party BIT,
    instalment_transdetail_id INT,
    deposit_transdetail_id INT
)
CREATE INDEX I__instalments__document_id ON #instalments (document_id)

INSERT INTO #Instalments
SELECT
    d.document_id,
    CASE d2.documenttype_id
        WHEN 1 THEN 1
        ELSE 0
    END,
    0,
    0
FROM document d
JOIN transdetail td
    ON td.document_id = d.document_id
    AND td.document_sequence = 1
JOIN transmatch tm
    ON tm.transdetail_id = td.transdetail_id
    AND tm.is_reversed IS NULL
JOIN matchgroup mg
    ON mg.match_id = tm.match_id
    AND mg.match_date <= @end_date
JOIN transmatch tm2
    ON tm2.match_id = mg.match_id
    AND tm2.is_reversed IS NULL
JOIN transdetail td2
    ON td2.transdetail_id = tm2.transdetail_id
JOIN document d2
    ON d2.document_id = td2.document_id
WHERE d.document_date <= @end_date
AND d.company_id = ISNULL(@branch_id, d.company_id)
AND (
        --(
        --    /*Third Party Instalments*/
        --    d2.documenttype_id = 1
        --    AND 
        --    RTRIM(d2.comment) = 'Premium Finance Transfer'
        --)
       -- OR
        (
            /*In-House Instalments*/
            d2.documenttype_id = 43
        )
    )

/*Get transactions that need to be paid off for in-house instalments*/
UPDATE i
SET i.instalment_transdetail_id = td5.transdetail_id,
    i.deposit_transdetail_id = td7.transdetail_id
FROM #Instalments i
JOIN transdetail td
    ON td.document_id = i.document_id
    AND td.document_sequence = 1
JOIN transmatch tm
    ON tm.transdetail_id = td.transdetail_id
    AND tm.is_reversed IS NULL
JOIN matchgroup mg
    ON mg.match_id = tm.match_id
    AND mg.match_date <= @end_date
JOIN transmatch tm2
    ON tm2.match_id = mg.match_id
    AND tm2.transmatch_id <> tm.transmatch_id
    AND tm2.is_reversed IS NULL
JOIN transdetail td2
    ON td2.transdetail_id = tm2.transdetail_id
JOIN document d2
    ON d2.document_id = td2.document_id
    AND d2.documenttype_id = 43
JOIN transdetail td3
    ON td3.document_id = d2.document_id
    AND td3.transdetail_id <> td2.transdetail_id
JOIN transmatch tm3
    ON tm3.transdetail_id = td3.transdetail_id
    AND tm3.is_reversed IS NULL
JOIN matchgroup mg3
    ON mg3.match_id = tm3.match_id
    AND mg3.match_date <= @end_date
/*Get the IND transaction*/
JOIN transmatch tm4
    ON tm4.match_id = mg3.match_id
    AND tm4.transmatch_id <> tm3.transmatch_id
    AND tm4.is_reversed IS NULL
JOIN transdetail td4
    ON td4.transdetail_id = tm4.transdetail_id
JOIN document d4
    ON d4.document_id = td4.document_id
    AND d4.documenttype_id = 37
JOIN transdetail td5
    ON td5.document_id = d4.document_id
    AND td5.transdetail_id <> td4.transdetail_id
JOIN account a5
    ON a5.account_id = td5.account_id
JOIN ledger l5
    ON l5.ledger_id = a5.ledger_id
    AND l5.ledger_short_name = 'SA'
/*Get the deposit JN transaction, if there is one*/
LEFT JOIN transmatch tm6
        JOIN transdetail td6
            ON td6.transdetail_id = tm6.transdetail_id
        JOIN document d6
            ON d6.document_id = td6.document_id
            AND d6.documenttype_id = 1
        JOIN transdetail td7
            ON td7.document_id = d6.document_id
            AND td7.transdetail_id <> td6.transdetail_id
    ON tm6.match_id = mg.match_id
    AND tm6.transmatch_id <> tm.transmatch_id
    AND tm6.transmatch_id <> tm2.transmatch_id
    AND tm6.is_reversed IS NULL
WHERE i.third_party = 0

/*Get transactions that need to be paid off for third party instalments*/
UPDATE i
SET i.instalment_transdetail_id = td5.transdetail_id,
    i.deposit_transdetail_id = td7.transdetail_id
FROM #Instalments i
JOIN transdetail td
    ON td.document_id = i.document_id
    AND td.document_sequence = 1
JOIN transmatch tm
    ON tm.transdetail_id = td.transdetail_id
    AND tm.is_reversed IS NULL
JOIN matchgroup mg
    ON mg.match_id = tm.match_id
    AND mg.match_date <= @end_date
JOIN transmatch tm2
    ON tm2.match_id = mg.match_id
    AND tm2.transmatch_id <> tm.transmatch_id
    AND tm2.is_reversed IS NULL
JOIN transdetail td2
    ON td2.transdetail_id = tm2.transdetail_id
JOIN document d2
    ON d2.document_id = td2.document_id
    AND d2.documenttype_id = 1
JOIN transdetail td3
    ON td3.document_id = d2.document_id
    AND td3.transdetail_id <> td2.transdetail_id
JOIN transmatch tm3
    ON tm3.transdetail_id = td3.transdetail_id
    AND tm3.is_reversed IS NULL
JOIN matchgroup mg3
    ON mg3.match_id = tm3.match_id
    AND mg3.match_date <= @end_date
/*Get the main JN transaction*/
JOIN transmatch tm4
    ON tm4.match_id = mg3.match_id
    AND tm4.transmatch_id <> tm3.transmatch_id
    AND tm4.is_reversed IS NULL
    AND tm4.transmatch_id IN
        (
            SELECT
                MAX(transmatch_id)
            FROM transmatch
            WHERE match_id = mg3.match_id
            AND transmatch_id <> tm3.transmatch_id
        )
JOIN transdetail td4
    ON td4.transdetail_id = tm4.transdetail_id
JOIN document d4
    ON d4.document_id = td4.document_id
    AND d4.documenttype_id = 1
JOIN transdetail td5
    ON td5.document_id = d4.document_id
    AND td5.transdetail_id <> td4.transdetail_id
/*Get the deposit JN transaction*/
LEFT JOIN transmatch tm6
        JOIN transdetail td6
            ON td6.transdetail_id = tm6.transdetail_id
        JOIN document d6
            ON d6.document_id = td6.document_id
            AND d6.documenttype_id = 1
        JOIN transdetail td7
            ON td7.document_id = d6.document_id
            AND td7.transdetail_id <> td6.transdetail_id
    ON tm6.match_id = mg3.match_id
    AND tm6.transmatch_id <> tm3.transmatch_id
    AND tm6.transmatch_id <> tm4.transmatch_id
    AND tm6.is_reversed IS NULL
WHERE i.third_party = 1

/*Remove instalment transactions that have already been fully paid*/
DELETE FROM #Instalments
WHERE
    (
        SELECT ISNULL(SUM(ROUND(tm.base_match_amount,2)),0)
        FROM transdetail td
        JOIN transmatch tm
            ON tm.transdetail_id = td.transdetail_id
            AND tm.is_reversed IS NULL
        JOIN matchgroup mg
            ON mg.match_id = tm.match_id
            AND mg.match_date <= @end_date            
        WHERE 
        (
            td.transdetail_id = deposit_transdetail_id
            OR
            td.transdetail_id = instalment_transdetail_id
        )
    )
    =
    (
        SELECT SUM(ROUND(td.amount,2))
        FROM transdetail td
        WHERE 
        (
            td.transdetail_id = deposit_transdetail_id
            OR
            td.transdetail_id = instalment_transdetail_id
        )
    )

CREATE TABLE #TransLines
(
    /*Account Details*/
    account_id INT,
    account_code VARCHAR(30),
    account_name VARCHAR(60),
    ledger_short_name VARCHAR(2),
    balance MONEY,

    /*Transaction Details*/    
    document_id INT,
    document_ref VARCHAR(25),
    policy_share VARCHAR(1),

    /*Commission Details*/    
    agent MONEY,
    subagent MONEY,
    net_commission MONEY,
    fees MONEY,
    client_amount MONEY,
    gross_commission MONEY,
    total_premium MONEY,
    total_premium_paid MONEY,
    percentage_paid MONEY,
    os_commission MONEY,
    comm_adj MONEY,

    currency_id INT,
    currency_code VARCHAR(10),
    currency_description VARCHAR(255)
   
)
CREATE INDEX I__TransLines__document_id ON #TransLines (document_id)
CREATE INDEX I__TransLines__currency_id__account_id ON #TransLines (currency_id, account_id)

INSERT INTO #TransLines
SELECT  
    a1.account_id,
    a1.short_code,
    a1.account_name,
    l1.ledger_short_name,
    NULL,  
    d1.document_id,
    d1.document_ref,
    CASE
        (
            SELECT ISNULL(SUM(1),1)
            FROM transdetail td
            JOIN account a
                ON a.account_id = td.account_id
            JOIN ledger l
                ON l.ledger_id = a.ledger_id
            WHERE td.document_id = d1.document_id
            AND l.ledger_short_name = 'SA'
            AND td.document_sequence = 
                (
                    SELECT MIN(document_sequence)
                    FROM transdetail
                    WHERE document_id = td.document_id
                    AND account_id = td.account_id
                )
        )
        WHEN 1 THEN ''
        ELSE 'Y'
    END,
    /*Agent amount*/    
    ISNULL(SUM(CASE l2.ledger_short_name WHEN 'AG' THEN 
        CASE @TypeOfCurrency
            WHEN 'Base' THEN td2.amount
            WHEN 'Transaction' THEN td2.currency_amount
        END
    ELSE 0 END),0),
    /*SubAgent amount*/    
    ISNULL(SUM(CASE l2.ledger_short_name WHEN 'UB' THEN 
        CASE @TypeOfCurrency
            WHEN 'Base' THEN td2.amount
            WHEN 'Transaction' THEN td2.currency_amount
        END
    ELSE 0 END),0), /*needs client amount subtracted from it*/
    /*Net Commission*/
    ISNULL(SUM(CASE l2.ledger_short_name WHEN 'CO' THEN 
        CASE @TypeOfCurrency
            WHEN 'Base' THEN td2.amount
            WHEN 'Transaction' THEN td2.currency_amount
        END
    ELSE 0 END),0),
    /*Fees*/
    ISNULL(SUM(CASE l2.ledger_short_name WHEN 'FE' THEN 
        CASE @TypeOfCurrency
            WHEN 'Base' THEN td2.amount
            WHEN 'Transaction' THEN td2.currency_amount
        END
    ELSE 0 END),0),
    /*Client amount*/
    ISNULL(SUM(CASE l2.ledger_short_name WHEN 'SA' THEN 
        CASE @TypeOfCurrency
            WHEN 'Base' THEN td2.amount
            WHEN 'Transaction' THEN td2.currency_amount
        END
    ELSE 0 END),0), /*needs subtrating from subagent amount*/
    0,
    (
        SELECT 
            CASE @TypeOfCurrency
                WHEN 'Base' THEN ISNULL(SUM(ROUND(tdy.amount,2)),0)
                WHEN 'Transaction' THEN ISNULL(SUM(ROUND(tdy.currency_amount,2)),0)
            END
        FROM transdetail tdy
        JOIN account ay
            ON ay.account_id = tdy.account_id
        JOIN ledger ly
            ON ly.ledger_id = ay.ledger_id
        WHERE tdy.document_id = d1.document_id
        AND ly.ledger_short_name IN ('SA','UB')
        AND NOT EXISTS
            (
                SELECT
                    NULL
                FROM #Instalments
                WHERE document_id = d1.document_id
            )
    )
    ,
    (
        SELECT 
            CASE @TypeOfCurrency
                WHEN 'Base' THEN ISNULL(SUM(ROUND(tmx.base_match_amount,2)),0)
                WHEN 'Transaction' THEN ISNULL(SUM(ROUND(tmx.currency_match_amount,2)),0)
            END
        FROM transdetail tdx
        JOIN account ax
            ON ax.account_id = tdx.account_id
        JOIN ledger lx
            ON lx.ledger_id = ax.ledger_id
        JOIN transmatch tmx
            ON tmx.transdetail_id = tdx.transdetail_id
            AND tmx.is_reversed IS NULL
        JOIN matchgroup mgx
            ON mgx.match_id = tmx.match_id
            AND mgx.match_date <= @end_date            
        WHERE tdx.document_id = d1.document_id
        AND lx.ledger_short_name IN ('SA','UB')
        AND NOT EXISTS  
            (   
                SELECT
                    NULL
                FROM #Instalments
                WHERE document_id = d1.document_id
            )
    ),
    0,
    0,
    0,
    (
        CASE @TypeOfCurrency
            WHEN 'Base' THEN td2.amount_currency_id
            WHEN 'Transaction' THEN td2.currency_id
        END
    ),
    '',
    ''
FROM account a1
JOIN ledger l1
    ON l1.ledger_id = a1.ledger_id
JOIN transdetail td1
    ON td1.account_id = a1.account_id
JOIN document d1
    ON d1.document_id = td1.document_id
JOIN transdetail td2
    ON td2.document_id = d1.document_id
JOIN account a2
    ON a2.account_id = td2.account_id
JOIN ledger l2
    ON l2.ledger_id = a2.ledger_id
    
/*See if this transaction has been allocated by a direct debit.*/
LEFT JOIN transdetail td3
    JOIN transmatch tm3
        ON tm3.transdetail_id = td3.transdetail_id
        AND tm3.is_reversed IS NULL
    JOIN matchgroup mg3
        ON mg3.match_id = tm3.match_id
        AND mg3.match_date <= @end_date
    JOIN transmatch tm4
        ON tm4.match_id = mg3.match_id
        AND tm4.is_reversed IS NULL
    JOIN transdetail td4
        ON td4.transdetail_id = tm4.transdetail_id
    JOIN document d4
        ON d4.document_id = td4.document_id
        AND d4.documenttype_id IN (33,34)
    ON td3.document_id = d1.document_id
    AND td3.document_sequence = 1
    AND td3.amount = 0
WHERE  
(
    /*Add this transactions commission/fee to the client, if there's no subagent, or the subagent, if their is one.*/
    (
        l1.ledger_short_name = 'SA' 
        AND
        NOT EXISTS
            (
                SELECT NULL
                FROM transdetail td
                JOIN account a
                    ON a.account_id = td.account_id
                JOIN ledger l
                    ON l.ledger_id = a.ledger_id
                WHERE td.document_id = d1.document_id
                AND l.ledger_short_name = 'UB'
            )
    )
    OR
    l1.ledger_short_name = 'UB' 
)
AND td1.document_sequence IN
    (   
        SELECT MIN(document_sequence)
        FROM transdetail
        WHERE document_id = d1.document_id
        AND account_id = a1.account_id
    )
AND d1.document_date <= @end_date
AND d1.company_id = ISNULL(@branch_id, d1.company_id)
AND (
        (
            /*We haven't received all of the money.*/
            (
                SELECT SUM(ROUND(td.amount,2))
                FROM transdetail td
                JOIN account a
                    ON a.account_id = td.account_id
                JOIN ledger l
                    ON l.ledger_id = a.ledger_id
                WHERE td.document_id = td1.document_id
                AND l.ledger_short_name IN ('SA','UB')
            )
            <>
            (
                SELECT ISNULL(SUM(ROUND(tm.base_match_amount,2)),0)
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
                    AND mg.match_date <= @end_date            
                WHERE td.document_id = td1.document_id
                AND l.ledger_short_name IN ('SA','UB')
            )
        )
        OR EXISTS
        (   
            SELECT
                NULL
            FROM #Instalments
            WHERE document_id = td1.document_id
        )
    )
AND 
(
    (
        /*Include fee amounts even if transaction is direct debit.*/
        l2.ledger_short_name = 'FE' 
    )
    OR
    (
        (  
            (
                l2.ledger_short_name = 'SA'
                AND 
                td2.Document_Sequence = 1
                AND NOT EXISTS
                (   
                    SELECT
                        NULL
                    FROM #Instalments
                    WHERE document_id = td1.document_id
                )
            )
            OR
            (
                l2.ledger_short_name IN ('AG', 'CO', 'UB')
            )
        )
        /*Not a direct debit.*/
        AND td3.transdetail_id IS NULL
    )
)
GROUP BY 
    l1.ledger_short_name, 
    a1.short_code, 
    a1.account_name, 
    a1.account_id,     
    d1.document_id,
    d1.document_ref,
    td2.amount_currency_id,
    td2.currency_id
ORDER BY 
    l1.ledger_short_name, 
    a1.short_code

IF @TypeOfCurrency = 'Base'
BEGIN
    UPDATE TL
    SET balance = 
        (  
            SELECT  
                ISNULL(SUM(td.amount),0)  
            FROM transdetail td  
            JOIN document d  
                ON d.document_id = td.document_id  
                AND d.document_date <= @end_date 
            WHERE td.account_id = TL.account_id  
            AND td.company_id = ISNULL(@branch_id, td.company_id)  
            AND td.amount_currency_id = TL.currency_id
        )  
        -  
        (  
            SELECT  
                ISNULL(SUM(tm.base_match_amount),0)  
            FROM transdetail td  
            JOIN document d  
                ON d.document_id = td.document_id  
                AND d.document_date <= @end_date
            JOIN transmatch tm  
                ON tm.transdetail_id = td.transdetail_id  
            JOIN matchgroup mg  
                ON mg.match_id = tm.match_id  
                AND mg.match_date <= @end_date  
            WHERE td.account_id = TL.account_id  
            AND td.company_id = ISNULL(@branch_id, td.company_id)  
            AND td.amount_currency_id = TL.currency_id
        )  
    FROM #TransLines TL
END
ELSE
BEGIN
    UPDATE TL
    SET balance = 
        (  
            SELECT  
                ISNULL(SUM(td.currency_amount),0)  
            FROM transdetail td  
            JOIN document d  
                ON d.document_id = td.document_id  
                AND d.document_date <= @end_date 
            WHERE td.account_id = TL.account_id  
            AND td.company_id = ISNULL(@branch_id, td.company_id)  
            AND td.currency_id = TL.currency_id
        )  
        -  
        (  
            SELECT  
                ISNULL(SUM(tm.currency_match_amount),0)  
            FROM transdetail td  
            JOIN document d  
                ON d.document_id = td.document_id  
                AND d.document_date <= @end_date
            JOIN transmatch tm  
                ON tm.transdetail_id = td.transdetail_id  
            JOIN matchgroup mg  
                ON mg.match_id = tm.match_id  
                AND mg.match_date <= @end_date  
            WHERE td.account_id = TL.account_id  
            AND td.company_id = ISNULL(@branch_id, td.company_id)  
            AND td.currency_id = TL.currency_id
        )  
    FROM #TransLines TL
END

UPDATE #TransLines
SET subagent = subagent - client_amount
WHERE subagent <> 0

UPDATE TL
SET comm_adj = 
    (
        SELECT 
            CASE @TypeOfCurrency
                WHEN 'Base' THEN ISNULL(SUM(td2.amount), 0) * -1
                WHEN 'Transaction' THEN ISNULL(SUM(td2.currency_amount), 0) * -1
            END
        FROM transdetail td2
        WHERE td2.spare = 'COMM ADJ'
        AND td2.ref_date > @end_date
        AND td2.document_id = TD.document_id
    )
FROM #TransLines TL
JOIN transdetail TD on TL.document_id = TD.document_id

UPDATE #Translines
SET os_commission = net_commission - comm_adj

UPDATE #TransLines
SET gross_commission = net_commission + agent + subagent

UPDATE #Translines
SET percentage_paid = (total_premium_paid / total_premium)
where total_premium_paid <> 0 and total_premium <> 0

UPDATE #Translines
SET os_commission = round((net_commission - comm_adj) * ( 1 - percentage_paid ),2)
where percentage_paid <> 0 and (net_commission - comm_adj) <> 0

UPDATE tl
SET tl.currency_code = cy.code,
    tl.currency_description=cy.description
FROM #TransLines tl
JOIN currency cy ON tl.currency_id = cy.currency_id
WHERE ISNULL(tl.currency_id, 0) > 0

DECLARE transaction_cursor CURSOR FAST_FORWARD FOR
    SELECT
        document_id
    FROM #TransLines
    WHERE ledger_short_name = 'SA'
    AND policy_share = 'Y'
    GROUP BY document_id
    ORDER BY document_id

OPEN transaction_cursor

FETCH NEXT FROM transaction_cursor INTO @document_id

WHILE @@FETCH_STATUS = 0
BEGIN

    /*Get the total amounts that need to be split across all clients.*/
    SELECT
        @total_gross_commission = gross_commission,
        @total_fees = fees,
        @total_agent = agent,
        @total_subagent = subagent,
        @total_net_commission = net_commission,
        @total_os_commission = ((net_commission - comm_adj) * (1 - percentage_paid))
    FROM #TransLines
    WHERE document_id = @document_id
    AND account_id = 
        (
            SELECT MIN(account_id)
            FROM #TransLines
            WHERE document_id = @document_id
        )

    /*Default rounding amounts to the total amounts.*/
    SELECT
        @rounding_gross_commission = @total_gross_commission ,
        @rounding_fees = @total_fees ,
        @rounding_agent = @total_agent,
        @rounding_subagent = @total_subagent,
        @rounding_net_commission = @total_net_commission,
        @rounding_os_commission = @total_os_commission
    /*Get the total client amount paid.*/    
    SELECT
        @total_client_amount = (
                     CASE @TypeOfCurrency
                         WHEN 'Base' THEN SUM(ROUND(td.amount,2))
                         WHEN 'Transaction' THEN SUM(ROUND(td.currency_amount,2))
                     END)
    FROM transdetail td
    JOIN account a
        ON a.account_id = td.account_id
    JOIN ledger l
        ON l.ledger_id = a.ledger_id
    WHERE td.document_id = @document_id
    AND l.ledger_short_name = 'SA'
    
    
    DECLARE account_cursor CURSOR FAST_FORWARD FOR
        SELECT
            account_id
        FROM #TransLines
        WHERE document_id = @document_id
        ORDER BY account_id

    OPEN account_cursor

    FETCH NEXT FROM account_cursor INTO @account_id
                
    WHILE @@FETCH_STATUS = 0
    BEGIN
    
        /*Get the client amount for this client*/
       SELECT
            @share_client_amount = (
                     CASE @TypeOfCurrency
                         WHEN 'Base' THEN SUM(ROUND(td.amount,2))
                         WHEN 'Transaction' THEN SUM(ROUND(td.currency_amount,2))
                     END)
        FROM transdetail td
        WHERE td.document_id = @document_id
        AND td.account_id = @account_id
        
        /*Calculate share percentage for this client*/
        SELECT
            @share_percentage = 
                CASE @total_client_amount
                    WHEN 0 THEN 0
                    ELSE @share_client_amount / @total_client_amount
                END
        
        /*Calculate the share amounts.*/
        SELECT
            @share_gross_commission = ROUND(@total_gross_commission * @share_percentage,2),
            @share_fees = ROUND(@total_fees * @share_percentage,2),
            @share_agent = ROUND(@total_agent * @share_percentage,2),
            @share_subagent = ROUND(@total_subagent * @share_percentage,2),
            @share_net_commission = ROUND(@total_net_commission * @share_percentage,2),
            @share_os_commission = ROUND(@total_os_commission * @share_percentage,2)
        
        /*Reduce the rounding amount by the share amount.*/
        SELECT
            @rounding_gross_commission = @rounding_gross_commission - @share_gross_commission,
            @rounding_fees = @rounding_fees - @share_fees,
            @rounding_agent = @rounding_agent - @share_agent,
            @rounding_subagent = @rounding_subagent - @share_subagent,
            @rounding_net_commission = @rounding_net_commission - @share_net_commission,
            @rounding_os_commission = @rounding_os_commission - @share_os_commission
            
        /*Save the new shared values into the temporary table.*/
        UPDATE #TransLines
        SET gross_commission = @share_gross_commission,
            fees = @share_fees,
            agent = @share_agent,
            subagent = @share_subagent,
            net_commission = @share_net_commission,
            os_commission = @share_os_commission
        WHERE document_id = @document_id
        AND account_id = @account_id
                
    
        FETCH NEXT FROM account_cursor INTO @account_id

    END
    
    /*Any any left over amounts, due to rounding, to one of the shares.*/
    UPDATE #TransLines
    SET gross_commission = gross_commission + @rounding_gross_commission,
        fees = fees + @rounding_fees,
        agent = agent + @rounding_agent,
        subagent = subagent + @rounding_subagent,
        net_commission = net_commission + @rounding_net_commission,
        os_commission = os_commission + @rounding_os_commission
    WHERE document_id = @document_id
    AND account_id = 
        (
            SELECT MAX(account_id)
            FROM #TransLines
            WHERE document_id = @document_id
        )
    

    CLOSE account_cursor
    DEALLOCATE account_cursor    

    FETCH NEXT FROM transaction_cursor INTO @document_id

END

CLOSE transaction_cursor
DEALLOCATE transaction_cursor

/*Remove accounts and transactions that we don't need to see.*/
DELETE FROM #translines
WHERE balance = 0
AND gross_commission = 0

SELECT  
    account_code,
    account_name,
    balance,
    ledger_short_name,   

    document_ref,
    policy_share,

    subagent,
    agent,
    net_commission,
    fees,
    gross_commission,
    os_commission,

    currency_id,
    currency_code,
    currency_description
    
FROM #TransLines
ORDER BY 
    ledger_short_name, 
    account_code

DROP TABLE #instalments
DROP TABLE #translines

GO



