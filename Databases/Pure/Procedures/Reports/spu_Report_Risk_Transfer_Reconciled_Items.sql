SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

EXECUTE DDLDropProcedure 'spu_Report_Risk_Transfer_Reconciled_Items'
GO

CREATE PROCEDURE spu_Report_Risk_Transfer_Reconciled_Items
    @company_id INT,
    @insurer_code VARCHAR(20),
    @PaymentGroups INT = 0,
    @Reconciliation_Date DATETIME = NULL
AS

SET NOCOUNT ON

DECLARE @PartyTypeCode VARCHAR(10)
DECLARE @ReportIndicator VARCHAR(2)

DECLARE @amt_settled MONEY
DECLARE @transdetail_id INT
DECLARE @document_id INT
DECLARE @document_id_copy INT
DECLARE @currency_amount MONEY
DECLARE @commadj_amount MONEY
DECLARE @commadj_trans VARCHAR(255)

SELECT @PartyTypeCode = RTRIM(PT.code)
FROM Party_Type PT
JOIN Party P
    ON P.party_type_id = PT.party_type_id
JOIN Account A
    ON A.account_key = P.party_cnt
WHERE A.short_code = @insurer_code

IF @PartyTypeCode = 'IN' /*Short Code For Insurer*/
BEGIN 
    SELECT @ReportIndicator = 'I' + CAST(PI.report_indicator AS CHAR(1))
    FROM Party_Insurer PI
    JOIN Party P
    ON P.party_cnt = PI.party_cnt
    JOIN Account A
    ON A.account_key = P.party_cnt
    WHERE A.short_code = @insurer_code
END

CREATE TABLE #Report_Temp
(
    Insurer VARCHAR(255),
    Address1 VARCHAR(60),
    Address2 VARCHAR(60),
    Address3 VARCHAR(60),
    Address4 VARCHAR(60),
    Postal_Code VARCHAR(20),
    Document_Ref VARCHAR(20),
    Document_ID INT,
    Policy_Doc_Date DATETIME ,
    Document_Type VARCHAR(255) ,
    Transaction_Date DATETIME ,
    Effective_Date DATETIME ,
    Client VARCHAR(255),
    Client_Code CHAR(20) ,
    Ledger_ID SMALLINT,
    Policy_Ref VARCHAR(30),
    Premium MONEY,
    Commission MONEY,
    Fee MONEY,
    IPT MONEY,
    This_Payment MONEY,
    Total_Payment MONEY,
    Sort_Column VARCHAR(255),
    gross_amount MONEY,
    commadj_transdetail_id VARCHAR(255),
    commadj_amount MONEY,
    amt_settled MONEY,
    payment MONEY,
)

INSERT INTO #Report_Temp
SELECT DISTINCT
    ISNULL(P.name, '')      Insurer,
    ISNULL(AD.address1, '')     Address1,
    ISNULL(AD.address2, '')     Address2,
    ISNULL(AD.address3, '')     Address3,
    ISNULL(AD.address4, '')     Address4,
    ISNULL(AD.postal_code, '')  Postal_Code,
    DCli.document_ref       Document_Ref,
    DCli.document_id        Document_ID,
    DCli.document_date      Policy_Doc_Date,
    DTCli.description       Document_Type,
    TD.accounting_date      Transaction_Date,
    DCli.created_date       Effective_Date,
    ISNULL(PCli.resolved_name, ISNULL(ACli.account_name, '')) Client,
    ISNULL(ACli.short_code, '') Client_Code,
        (CASE l.ledger_short_name
         WHEN 'IN' THEN 4
         ELSE 0 END) ledger_id,
    ISNULL(TCli.insurance_ref, '')  Policy_Ref,
    ISNULL
    (
        (
        SELECT  SUM(round(currency_amount,2))
        FROM    Transdetail
        WHERE   document_id = DCli.document_id
        AND account_id = A.account_id
        AND (
                (
                    A.ledger_id IN ( SELECT ledger_id FROM ledger WHERE ledger_short_name = 'IN' )

                    AND
                    (
                        (spare = 'GROSS'
                        OR
                        spare = '')
                        OR
                        DCli.documenttype_id IN (33, 34)
                    )
                )
            )
        )
    , 0)                Premium,

    ISNULL
    (
        (
        SELECT  SUM(round(currency_amount,2))
        FROM    Transdetail
        WHERE   document_id = DCli.document_id
        AND account_id = A.account_id
        AND (
                (
                    A.ledger_id IN ( SELECT ledger_id FROM ledger WHERE ledger_short_name = 'IN' )

                    AND
                    spare like 'COMM%'
                )
            )
        )
    , 0)                Commission,
    (
        SELECT ISNULL(SUM(ROUND(tdf.currency_amount,2)),0)
        FROM transdetail tdf
        JOIN transdetail_type ttf
            ON ttf.transdetail_type_id = tdf.transdetail_type_id
        WHERE tdf.document_id = DCli.document_id
        AND tdf.account_id = A.account_id
        AND ttf.code = 'IFEE'
    ) Fee,
    ISNULL
    (
        (
        SELECT SUM(
            CASE amount
                WHEN 0 THEN 0.00
            ELSE
                ROUND((ref_amount * amount/abs(amount)),2)
            END )

        FROM    Transdetail
        WHERE   document_id = DCli.document_id
        AND account_id = A.account_id
        AND (
                (
                    A.ledger_id IN ( SELECT ledger_id FROM ledger WHERE ledger_short_name = 'IN' )

                    AND
                    (
                        (
                        spare = 'GROSS'
                        OR
                        spare = ''
                        )
                        OR
                        DCli.documenttype_id IN (33, 34)
                    )
                )
            )
        )
    , 0)                IPT,
    ISNULL
    (
        (
        SELECT  SUM(ROUND(currency_match_amount,2))
        FROM    TransMatch  A1,
            Transdetail T1
        WHERE   T1.transdetail_id = A1.transdetail_id
        AND T1.document_id = DCli.document_id
        AND T1.account_id = A.account_id
        AND A1.allocationdetail_id IS NULL
        )
    , 0)                This_Payment,
    ISNULL
    (
        (
        SELECT  SUM(ROUND(currency_match_amount,2))
        FROM    TransMatch  A1,
            Transdetail T1
        WHERE   T1.transdetail_id = A1.transdetail_id
        AND T1.document_id = DCli.document_id
        AND T1.account_id = A.account_id
        AND A1.allocationdetail_id IS NOT NULL
        AND T1.spare <> 'COMM'
        )
    , 0)                Total_Payment,
    (
        CASE
        WHEN (@ReportIndicator = 'I0')
            THEN ACli.short_code 
        WHEN (@ReportIndicator = 'I1')
            THEN TCli.insurance_ref
        WHEN (@ReportIndicator = 'I2')
            THEN ACli.short_code
        WHEN (@ReportIndicator = 'I3')
            THEN CONVERT(VARCHAR(20),TransExp.cover_start_date,120)
        ELSE ACli.short_code
        END
    )                   Sort_Column,
    TD.currency_amount  gross_amount,
    ''          commadj_transdetail_id,
    0           commadj_amount,
    (
        SELECT ISNULL(SUM(tm.currency_match_amount),0)
        FROM transdetail t
        JOIN transmatch tm
            ON tm.transdetail_id = t.transdetail_id
        WHERE tm.allocationdetail_id IS NOT NULL
        AND t.account_id = TD.account_id
        AND t.document_id = TD.document_id
    )           amt_settled,
    (
        SELECT ISNULL(SUM(tm.currency_match_amount),0)
        FROM transdetail t
        JOIN transmatch tm
            ON tm.transdetail_id = t.transdetail_id
        WHERE tm.allocationdetail_id IS NULL
        AND t.account_id = TD.account_id
        AND t.document_id = TD.document_id
    )           payment


FROM    Account     A
JOIN    Party               P
ON A.account_key = P.Party_cnt
JOIN TransDetail    TD
ON TD.account_id = A.account_id
JOIN Document       DCli
ON DCli.document_id = TD.document_id
JOIN  DocumentType  DTCli
ON DTCli.documenttype_id = DCli.documenttype_id
JOIN TransDetail    TCli
ON TCli.account_id = A.Account_id
AND TCli.document_id = TD.document_id
JOIN Currency CU
ON TCli.currency_id=CU.currency_id
JOIN Account        ACli
ON  ACli.account_id =
(
    SELECT  T2.account_id
    FROM    TransDetail T2
    WHERE   transdetail_id =
    (   SELECT  MIN(transdetail_id)
        FROM    Transdetail T3,
            Account     A3
        WHERE   T3.document_id = DCli.document_id
        AND A3.account_id = T3.account_id
        AND
        (
            (
            DTCli.description <> 'Journal'

            AND
            A3.ledger_id IN ( SELECT ledger_id FROM ledger WHERE ledger_short_name = 'SA' ) 

            )
            OR
            (
            DTCli.description = 'Journal'
            AND
            TCli.account_id <> A3.account_id
            )
            OR
            (
            DTCli.description = 'Fee'
            AND
            A3.ledger_id IN ( SELECT ledger_id FROM ledger WHERE ledger_short_name = 'FE' )
            )
        )
    )
)
LEFT JOIN Party PCli
    ON PCli.Party_cnt = ACli.account_key
JOIN Ledger L
    ON L.Ledger_Id = A.Ledger_Id
LEFT OUTER JOIN Transaction_Export_Folder           TransExp
    ON DCli.Document_Ref = TransExp.Document_Ref
    AND TransExp.source_id = DCli.company_id
LEFT OUTER JOIN     Insurance_File              InsFile
    ON TransExp.Insurance_file_cnt = InsFile.insurance_file_cnt
LEFT OUTER JOIN Policy_Coinsurers               CoIns
    ON  InsFile.insurance_file_cnt = CoIns.Insurance_file_cnt
    AND CoIns.Party_cnt = P.Party_cnt
LEFT OUTER JOIN Party_Address_Usage     PAU
    JOIN Address_Usage_Type         AUT
        ON AUT.address_usage_type_id = PAU.address_usage_type_id
    JOIN Address                AD
        ON AD.address_cnt = PAU.address_cnt
    ON PAU.party_cnt = P.party_cnt
    AND AUT.code = '3131 XCO'

JOIN    transdetail tda
ON  tda.document_id = DCli.document_id

WHERE A.short_code = @insurer_code
AND TD.risk_transfer = 4 
AND TD.document_sequence =
    (
        SELECT
            MIN(document_sequence)
        FROM transdetail
        WHERE document_id = TD.document_id
        AND account_id = TD.account_id
    )
AND     TD.risk_transfer_reconciliation_date=@Reconciliation_Date
AND
(
    (
        (
            (@company_id > 0 AND DCli.company_id = @company_id)
            OR
            (@company_id = 0)
        )
        AND @PaymentGroups=0
    )
    OR
    (
        @PaymentGroups=1
        AND
        DCli.company_Id in (select company_id from insurerpayment where paymentgroup_id=@company_id and account_id=a.account_id)
    )
)

ORDER BY
    Sort_Column


/*Commission Adjustments*/

DECLARE it_adjtemp CURSOR FAST_FORWARD FOR
SELECT
    td.currency_amount,
    it.document_id,
    td.transdetail_id
FROM #Report_Temp it
JOIN transdetail td
    ON td.document_id = it.document_id
JOIN transdetail_type tt
    ON tt.transdetail_type_id = td.transdetail_type_id
WHERE td.account_id = (SELECT account_id FROM account WHERE short_code= @insurer_code)
AND tt.code = 'COMMADJ'
ORDER BY it.document_id

OPEN it_adjtemp

FETCH NEXT FROM it_adjtemp INTO
@currency_amount,
@document_id,
@transdetail_id

/*Initialise variables*/
SELECT @commadj_amount = 0
SELECT @commadj_trans = ''
SELECT @document_id_copy = @document_id

WHILE @@FETCH_STATUS = 0
BEGIN

/*For the same transaction add up all of the commission adjustments and make a note of their transdetail_ids*/
IF @document_id_copy = @document_id
BEGIN
    SELECT @commadj_amount = @commadj_amount + @currency_amount
    SELECT @commadj_trans = @commadj_trans + CONVERT(VARCHAR,@transdetail_id) + '|'
END

FETCH NEXT FROM it_adjtemp INTO
    @currency_amount,
    @document_id,
    @transdetail_id

IF @document_id_copy <> @document_id OR @@FETCH_STATUS <> 0
BEGIN
    /*Update transaction line with commission adjustments*/
    UPDATE #Report_Temp
    SET commadj_amount = ISNULL(@commadj_amount,0),
        commadj_transdetail_id = ISNULL(@commadj_trans,0)
    WHERE document_id = @document_id_copy

    /*Initialise variables*/
    SELECT @commadj_amount = 0
    SELECT @commadj_trans = ''
    SELECT @document_id_copy = @document_id
END
END

/* Close and Deallocate Cursor */
CLOSE it_adjtemp
DEALLOCATE it_adjtemp

/*If the marked amount is greater than the outstanding amount then set it to the outstanding amount*/
UPDATE it
SET payment = gross_amount + Commission + commadj_amount + fee - amt_settled
FROM #Report_Temp it
WHERE ABS(payment) > ABS(gross_amount + Commission + commadj_amount + fee - amt_settled)

SET NOCOUNT OFF

SELECT
    rt.Insurer,
    rt.Address1,
    rt.Address2,
    rt.Address3,
    rt.Address4,
    rt.Postal_Code,
    rt.Document_Ref,
    rt.Document_ID,
    rt.Policy_Doc_Date,
    rt.Document_Type,
    rt.Transaction_Date,
    rt.Effective_Date,
    rt.Client,
    rt.Client_Code,
    rt.Ledger_ID,
    rt.Policy_Ref,
    rt.Premium,
    rt.commission,
    rt.fee,
    rt.IPT,
    rt.This_Payment,
    rt.Total_Payment,
    rt.Sort_Column,
    rt.gross_amount,
    rt.commadj_transdetail_id,
    rt.commadj_amount,
    rt.amt_settled,
    rt.payment
FROM #Report_Temp rt

ORDER BY
    rt.Sort_Column,
    rt.document_id

    /* Remove the temp table */
   DROP TABLE #Report_Temp

   GO
   SET QUOTED_IDENTIFIER OFF   
   GO
   SET ANSI_NULLS OFF 
   GO

