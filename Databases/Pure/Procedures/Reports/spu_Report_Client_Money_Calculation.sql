SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Report_Client_Money_Calculation'
GO

CREATE PROCEDURE spu_Report_Client_Money_Calculation
    @branch_id SMALLINT,
    @period_end_date DATETIME,
    @rpt_TypeIN VARCHAR(50) 
AS

DECLARE
    @rpt_type TINYINT,    
    
    @sCMRes VARCHAR(50),
    @sCMResBankBalances VARCHAR(50),
    @sCMResMoneyHeldatTP VARCHAR(50),
    @sCMResInsuranceDebtors VARCHAR(100),
    @sCMResDesignatedInvestments VARCHAR(100),
    @sCMReq VARCHAR(50),
    @sCMReqInsuranceCreditors VARCHAR(50),
    @sCMReqUnearnedCommision VARCHAR(50),
    @sCMReqMoneyHeldatTP VARCHAR(50),    
    
    @sCMResOrder INT,
    @sCMResBankBalancesOrder INT,
    @sCMResBankBalancesBankOrder INT,
    @sCMResBankBalancesSuspenceOrder INT,
    @sCMResMoneyHeldatTPOrder INT,
    @sCMResInsuranceDebtorsOrder INT,
    @sCMResDesignatedInvestmentsOrder INT,
    @sCMReqOrder INT,
    @sCMReqInsuranceCreditorsOrder INT,
    @sCMReqUnearnedCommisionOrder INT,
    @sCMReqMoneyHeldatTPOrder INT,
    
    @valErnComm INT,
    @funded MONEY,
    @returns MONEY,
    
    @insurer_comm_os MONEY,
    @client_comm_os MONEY,
    @subagent_comm_os MONEY,
    @finance_comm_os MONEY,
    @direct_comm_os MONEY

SET NOCOUNT ON

IF UPPER(ISNULL(@rpt_typeIN, 'STATUTORY TRUST ACCOUNT')) = 'STATUTORY TRUST ACCOUNT'
BEGIN
   SELECT @rpt_type = 0
END
ELSE
BEGIN
   SELECT @rpt_type = 1
END

SELECT @period_end_date = ISNULL(@period_end_date, GETDATE())

IF @branch_id = 0
BEGIN
    SELECT @branch_id = NULL
END

CREATE TABLE #tmpReport_Client_Money_Calculation
(
    amount MONEY,
    group_name1 VARCHAR(100),
    group_name2 VARCHAR(100),   
    group_name3 VARCHAR(100),
    group_order1 INT,
    group_order2 INT,
    group_order3 INT,
    fsa_disabled BIT
)

IF NOT EXISTS 
    (
        SELECT NULL
        FROM hidden_options
        WHERE option_number = 61
        AND value = '1'
    )
BEGIN
    INSERT INTO #tmpReport_Client_Money_Calculation
    (
        fsa_disabled
    )
    VALUES
    (
        1
    )
    
    SELECT *
    FROM #tmpReport_Client_Money_Calculation
    
    DROP TABLE #tmpReport_Client_Money_Calculation
    
    RETURN
END

CREATE TABLE #Amount_Due
(
    ledger_short_name VARCHAR(2),
    document_id INT,
    amount MONEY,
    debitcredit VARCHAR(1)  
)

SELECT @sCMRes = 'Client Money Resource'
SELECT @sCMResOrder = 1
SELECT @sCMResBankBalances = 'Bank Balances'
SELECT @sCMResBankBalancesOrder = 1
SELECT @sCMResBankBalancesBankOrder = 1
SELECT @sCMResBankBalancesSuspenceOrder = 2
SELECT @sCMResMoneyHeldatTP = 'Money held at Third Parties'
SELECT @sCMResMoneyHeldatTPOrder = 2
SELECT @sCMResInsuranceDebtors = 'Insurance Debtors (not including pre-funded items)'
SELECT @sCMResInsuranceDebtorsOrder = 3
SELECT @sCMResDesignatedInvestments = 'Value of any Designated Investments'
SELECT @sCMResDesignatedInvestmentsOrder = 4

SELECT @sCMReq = 'Client Money Requirement'
SELECT @sCMReqOrder = 2
SELECT @sCMReqInsuranceCreditors = 'Insurance Creditors'
SELECT @sCMReqInsuranceCreditorsOrder = 1
SELECT @sCMReqUnearnedCommision = 'Unearned Commission'
SELECT @sCMReqUnearnedCommisionOrder = 2
SELECT @sCMReqMoneyHeldatTP = 'Money held at Third Parties'
SELECT @sCMReqMoneyHeldatTPOrder = 3

/*Use temporary table for massive speed boost*/
CREATE TABLE #Future_Allocations
(
    match_id INT
)
CREATE INDEX I__#Future_Allocations__match_id ON #Future_Allocations (match_id)

INSERT INTO #Future_Allocations
SELECT
    tm.match_id
FROM document d
JOIN transdetail td
    ON td.document_id = d.document_id
JOIN transmatch tm
    ON tm.transdetail_id = td.transdetail_id
WHERE d.document_date > @period_end_date
GROUP BY tm.match_id

/*BANK BALANCES*/
INSERT INTO #tmpReport_Client_Money_Calculation
SELECT
    (
        SELECT 
            ISNULL(SUM(td.amount),0)
        FROM transdetail td
        JOIN document d
            ON d.document_id = td.document_id
            AND d.company_id = ISNULL(@branch_id, d.company_id)
            AND d.document_date <= @period_end_date
        WHERE td.account_id = a.account_id
    )
    -
    (
        SELECT 
            ISNULL(SUM(tm.base_match_amount),0)
        FROM transdetail td
        JOIN document d
            ON d.document_id = td.document_id
            AND d.company_id = ISNULL(@branch_id, d.company_id)
            AND d.document_date <= @period_end_date
        JOIN transmatch tm            
            ON tm.transdetail_id = td.transdetail_id
            AND tm.is_reversed IS NULL
            AND tm.allocationdetail_id IS NOT NULL
        JOIN matchgroup mg
            ON mg.match_id = tm.match_id
            AND mg.match_date <= @period_end_date
        WHERE td.account_id = a.account_id
        AND NOT EXISTS /*Don't include matches containing future transactions*/
            (
                SELECT 
                    NULL
                FROM #Future_Allocations
                WHERE match_id = tm.match_id
            )
    ),
    @sCMRes,
    @sCMResBankBalances,
    a.account_name,
    @sCMResOrder,
    @sCMResBankBalancesOrder,
    @sCMResBankBalancesBankOrder,
    0
FROM account a
JOIN bankaccount ba 
    ON ba.account_id = a.account_id
WHERE a.client_money_calc_account_type = 1
AND a.client_bank_account_type = @rpt_type
GROUP BY 
    a.account_id,
    a.account_name


/*SUSPENSE ACCOUNTS*/
INSERT INTO #tmpReport_Client_Money_Calculation
SELECT
    (
        SELECT 
            ISNULL(SUM(td.amount),0)
        FROM transdetail td
        JOIN document d
            ON d.document_id = td.document_id
            AND d.company_id = ISNULL(@branch_id, d.company_id)
            AND d.document_date <= @period_end_date
        WHERE td.account_id = a.account_id
    )
    -
    (
        SELECT 
            ISNULL(SUM(tm.base_match_amount),0)
        FROM transdetail td
        JOIN document d
            ON d.document_id = td.document_id
            AND d.company_id = ISNULL(@branch_id, d.company_id)
            AND d.document_date <= @period_end_date
        JOIN transmatch tm            
            ON tm.transdetail_id = td.transdetail_id
            AND tm.is_reversed IS NULL
            AND tm.allocationdetail_id IS NOT NULL
        JOIN matchgroup mg
            ON mg.match_id = tm.match_id
            AND mg.match_date <= @period_end_date
        WHERE td.account_id = a.account_id
        AND NOT EXISTS /*Don't include matches containing future transactions*/
            (
                SELECT 
                    NULL
                FROM #Future_Allocations
                WHERE match_id = tm.match_id
            )
    ),
    @sCMRes,
    @sCMResBankBalances,
    a.account_name,
    @sCMResOrder,
    @sCMResBankBalancesOrder,
    @sCMResBankBalancesSuspenceOrder,
    0
FROM account a
WHERE a.client_money_calc_account_type = 2
GROUP BY 
    a.account_id,
    a.account_name

IF @rpt_type=1
BEGIN

    /*DESIGNATED INVESTMENT ACCOUNTS*/
    CREATE TABLE #tmpIA
    (
    amount MONEY
    )


    INSERT INTO #tmpIA
    SELECT
       (
        SELECT 
            ISNULL(SUM(td.amount),0)
        FROM transdetail td
        JOIN document d
            ON d.document_id = td.document_id
            AND d.company_id = ISNULL(@branch_id, d.company_id)
            AND d.document_date <= @period_end_date
        WHERE td.account_id = a.account_id
        )
        -
        (
        SELECT 
            ISNULL(SUM(tm.base_match_amount),0)
        FROM transdetail td
        JOIN document d
            ON d.document_id = td.document_id
            AND d.company_id = ISNULL(@branch_id, d.company_id)
            AND d.document_date <= @period_end_date
        JOIN transmatch tm            
            ON tm.transdetail_id = td.transdetail_id
            AND tm.is_reversed IS NULL
            AND tm.allocationdetail_id IS NOT NULL
        JOIN matchgroup mg
            ON mg.match_id = tm.match_id
            AND mg.match_date <= @period_end_date
        WHERE td.account_id = a.account_id
        AND NOT EXISTS /*Don't include matches containing future transactions*/
            (
                SELECT 
                    NULL
                FROM #Future_Allocations
                WHERE match_id = tm.match_id
            )
        )
        FROM account a
    WHERE a.client_money_calc_account_type = 3
    GROUP BY 
        a.account_id,
        a.account_name

    INSERT INTO #tmpReport_Client_Money_Calculation
    SELECT
            (SELECT ISNULL(SUM(amount),0) FROM #tmpIA),
        @sCMRes,
        @sCMResDesignatedInvestments,
        '',
        @sCMResOrder,
        @sCMResDesignatedInvestmentsOrder,
        0,
        0
    
    DROP TABLE #tmpIA

END

/*MONEY HELD AT TP*/
INSERT INTO #tmpReport_Client_Money_Calculation
SELECT 
    (
        SELECT 
            ISNULL(SUM(td.amount),0) * -1
        FROM transdetail td
        JOIN document d
            ON d.document_id = td.document_id
            AND d.company_id = ISNULL(@branch_id, d.company_id)
            AND d.document_date <= @period_end_date
        WHERE td.risk_transfer BETWEEN 2 AND 3
    ),
    @sCMRes,
    @sCMResMoneyHeldatTP,
    '',
    @sCMResOrder,
    @sCMResMoneyHeldatTPOrder,
    0,
    0

/*INSURANCE DEBTORS*/


INSERT INTO #Amount_Due
(
    ledger_short_name,
    document_id,
    amount,
    debitcredit
)
SELECT   
    l.ledger_short_name,
    d.document_id,
    td.amount -
    (
        SELECT 
            ISNULL(SUM(tm.base_match_amount),0)
        FROM transmatch tm            
        JOIN matchgroup mg
            ON mg.match_id = tm.match_id
            AND mg.match_date <= @period_end_date
        WHERE tm.transdetail_id = td.transdetail_id
        AND tm.is_reversed IS NULL
        AND tm.allocationdetail_id IS NOT NULL
        AND NOT EXISTS /*Don't include matches containing future transactions*/
            (
                SELECT 
                    NULL
                FROM #Future_Allocations
                WHERE match_id = tm.match_id
            )
    ),
    'D'
FROM transdetail td
JOIN document d
    ON d.document_id = td.document_id
    AND d.company_id = ISNULL(@branch_id, d.company_id)
JOIN account a
    ON a.account_id = td.account_id
    AND a.short_code <> 'ISUSP'
JOIN ledger l 
    ON l.ledger_id = a.ledger_id
    AND l.ledger_short_name IN ('SA','UB','RF','IN')
WHERE 
    (
        (
            d.document_date <= @period_end_date
            AND 
            td.spare NOT IN ('COMM ADJ', 'AGENT ADJ', 'BROK ADJ')
        )
        OR
        (
            td.ref_date <= @period_end_date
            AND
            td.spare IN ('COMM ADJ', 'AGENT ADJ', 'BROK ADJ')
        )
    )

UPDATE ad
SET ad.debitcredit = 'C'
FROM #Amount_Due ad
WHERE EXISTS
(
    SELECT 
        NULL
    FROM #Amount_Due
    WHERE document_id = ad.document_id
    AND ledger_short_name = ad.ledger_short_name
    HAVING SUM(amount) < 0
)


/*CLIENT*/
INSERT INTO #tmpReport_Client_Money_Calculation
SELECT   
    (
        SELECT 
            ISNULL(SUM(amount),0)
        FROM #Amount_Due
        WHERE ledger_short_name = 'SA'
        AND debitcredit = 'D'
    ),
    @sCMRes,
    @sCMResInsuranceDebtors,
    'Due from Clients',
    @sCMResOrder,
    @sCMResInsuranceDebtorsOrder,
    1,
    0

/*SUB AGENTS*/
INSERT INTO #tmpReport_Client_Money_Calculation
SELECT
    (
        SELECT 
            ISNULL(SUM(amount),0)
        FROM #Amount_Due
        WHERE ledger_short_name = 'UB'
        AND debitcredit = 'D'
    ),
    @sCMRes,
    @sCMResInsuranceDebtors,
    'Due from Sub Agents',
    @sCMResOrder,
    @sCMResInsuranceDebtorsOrder,
    2,
    0

/*PREMIUM FINANCE*/
INSERT INTO #tmpReport_Client_Money_Calculation
SELECT
    (
        SELECT 
            ISNULL(SUM(amount),0)
        FROM #Amount_Due
        WHERE ledger_short_name = 'RF'
        AND debitcredit = 'D'
    ),
    @sCMRes,
    @sCMResInsuranceDebtors,
    'Due from Premium Finance',
    @sCMResOrder,
    @sCMResInsuranceDebtorsOrder,
    3,
    0

/*INSURER*/
INSERT INTO #tmpReport_Client_Money_Calculation
SELECT   
    (
        SELECT 
            ISNULL(SUM(amount),0)
        FROM #Amount_Due
        WHERE ledger_short_name = 'IN'
        AND debitcredit = 'D'
    ),
    @sCMRes,
    @sCMResInsuranceDebtors,
    'Due from Insurers',
    @sCMResOrder,
    @sCMResInsuranceDebtorsOrder,
    4,
    0

/*Get the funded and returns given amounts*/
EXEC spu_Report_Client_Money_Cal_Funded 
    @branch_id, 
    @period_end_date, 
    @funded OUTPUT,
    @returns OUTPUT
    
    
IF @rpt_type=0      
BEGIN      

    /*ITEMS FUNDED*/
    INSERT INTO #tmpReport_Client_Money_Calculation
    SELECT
        @funded,
        @sCMRes,
        @sCMResInsuranceDebtors,
        'Items Funded',
        @sCMResOrder,
        @sCMResInsuranceDebtorsOrder,
        5,
        0

    /*RETURNS GIVEN*/
    INSERT INTO #tmpReport_Client_Money_Calculation
    SELECT
        @returns,
        @sCMRes,
        @sCMResInsuranceDebtors,
        'Returns Given',
        @sCMResOrder,
        @sCMResInsuranceDebtorsOrder,
        6,
        0

END


/*INSURENCE CREDITORS*/
/*CLIENT*/
INSERT INTO #tmpReport_Client_Money_Calculation
SELECT   
    (
        SELECT 
            ISNULL(SUM(amount),0) * -1
        FROM #Amount_Due
        WHERE ledger_short_name = 'SA'
        AND debitcredit = 'C'
    ),
    @sCMReq,
    @sCMReqInsuranceCreditors,
    'Due to Clients',
    @sCMReqOrder,
    @sCMReqInsuranceCreditorsOrder,
    1,
    0

/*SUB AGENTS*/
INSERT INTO #tmpReport_Client_Money_Calculation
SELECT
    (
        SELECT 
            ISNULL(SUM(amount),0) * -1
        FROM #Amount_Due
        WHERE ledger_short_name = 'UB'
        AND debitcredit = 'C'
    ),
    @sCMReq,
    @sCMReqInsuranceCreditors,
    'Due to Sub Agents',
    @sCMReqOrder,
    @sCMReqInsuranceCreditorsOrder,
    2,
    0

/*PREMIUM FINANCE*/
INSERT INTO #tmpReport_Client_Money_Calculation
SELECT
    (
        SELECT 
            ISNULL(SUM(amount),0) * -1
        FROM #Amount_Due
        WHERE ledger_short_name = 'RF'
        AND debitcredit = 'C'
    ),
    @sCMReq,
    @sCMReqInsuranceCreditors,
    'Due to Premium Finance',
    @sCMReqOrder,
    @sCMReqInsuranceCreditorsOrder,
    3,
    0

/*INSURER*/
INSERT INTO #tmpReport_Client_Money_Calculation
SELECT   
    (
        SELECT 
            ISNULL(SUM(amount),0) * -1
        FROM #Amount_Due
        WHERE ledger_short_name = 'IN'
        AND debitcredit = 'C'
    ),
    @sCMReq,
    @sCMReqInsuranceCreditors,
    'Due to Insurers',
    @sCMReqOrder,
    @sCMReqInsuranceCreditorsOrder,
    4,
    0
    
/*UNEARNED COMMISION*/

/*Get commission movement option*/
SELECT 
    @valErnComm = Value 
FROM system_options 
WHERE option_number = 16 

/*If earning when insurer has settled*/
IF @valErnComm = 2 
BEGIN

    /*Calculation for "Due from Insurer" based on spu_Report_Insurer_Binder*/
    EXEC spu_Report_Client_Money_Cal_Insurer_Binder
        @branch_id, 
        @period_end_date,
        @insurer_comm_os OUTPUT

    /*Due from Insurer*/    
    INSERT INTO #tmpReport_Client_Money_Calculation
    SELECT        
        @insurer_comm_os,
        @sCMReq,
        @sCMReqUnearnedCommision,
        'Due from Insurer',
        @sCMReqOrder,
        @sCMReqUnearnedCommisionOrder,
        1,
        0
  
END 
ELSE 
BEGIN

    /*Calculation for "Due from Clients" and "Due from Sub Agents" based on spu_Report_Comm_Outstanding*/
    EXEC spu_Report_Client_Money_Cal_Comm_Outstanding
        @branch_id, 
        @period_end_date,
        @client_comm_os OUTPUT,
        @subagent_comm_os OUTPUT 

    /*Due from Clients*/
    INSERT INTO #tmpReport_Client_Money_Calculation
    SELECT        
        @client_comm_os,
        @sCMReq,
        @sCMReqUnearnedCommision,
        'Due from Clients',
        @sCMReqOrder,
        @sCMReqUnearnedCommisionOrder,
        1,
        0
        

    /*Due from Sub Agents*/
    INSERT INTO #tmpReport_Client_Money_Calculation
    SELECT        
        @subagent_comm_os,
        @sCMReq,
        @sCMReqUnearnedCommision,
        'Due from Sub Agents',
        @sCMReqOrder,
        @sCMReqUnearnedCommisionOrder,
        2,
        0

    
    /*Calculation for "Due from Premium Finance" based on spu_Report_Unsettled_Premium_Finance*/
    EXEC spu_Report_Client_Money_Cal_UnSettled_Premium_Finance
        @branch_id,    
        @period_end_date,
        @finance_comm_os OUTPUT
    
    /*Due from Premium Finance*/
    INSERT INTO #tmpReport_Client_Money_Calculation
    SELECT        
        @finance_comm_os,
        @sCMReq,
        @sCMReqUnearnedCommision,
        'Due from Premium Finance',
        @sCMReqOrder,
        @sCMReqUnearnedCommisionOrder,
        3,
        0
            
    /*Calculation for "Due from Direct to Insurer" based on spu_Report_Direct_Business*/
    EXEC spu_Report_Client_Money_Cal_Unpaid_Direct_Business
        @branch_id,
        @period_end_date,
        @direct_comm_os OUTPUT

    /*Due from Direct to Insurer*/
    INSERT INTO #tmpReport_Client_Money_Calculation
    SELECT        
        @direct_comm_os,
        @sCMReq,
        @sCMReqUnearnedCommision,
        'Due from Direct to Insurer',
        @sCMReqOrder,
        @sCMReqUnearnedCommisionOrder,
        4,
        0

       
END 

/*MONEY HELD AT TP*/
INSERT INTO #tmpReport_Client_Money_Calculation
SELECT 
    (
        SELECT 
            ISNULL(SUM(td.amount),0) * -1
        FROM transdetail td
        JOIN document d
            ON d.document_id = td.document_id
            AND d.company_id = ISNULL(@branch_id, d.company_id)
            AND d.document_date <= @period_end_date
        WHERE td.risk_transfer BETWEEN 2 AND 3
    ),
    @sCMReq,
    @sCMReqMoneyHeldatTP,
    '',
    @sCMReqOrder,
    @sCMReqMoneyHeldatTPOrder,
    0,
    0

SELECT 
    * 
FROM #tmpReport_Client_Money_Calculation
ORDER BY 
   group_order1,
   group_order2,
   group_order3,
   group_name3

DROP TABLE #Amount_Due   
DROP TABLE #tmpReport_Client_Money_Calculation
DROP TABLE #Future_Allocations

GO


