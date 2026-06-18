SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Report_Weekly_Income'
GO

--ECK291002 Fix to include Short Term and Transferred 
--ECK160802 Fix to include reversals !!!
--ECK120802 Fix to date comparisons which could cause duplicates
CREATE PROCEDURE spu_Report_Weekly_Income
    @branch_id int,
    @start_date datetime,
    @end_date1 datetime,
    @end_date2 datetime,
    @end_date3 datetime,
    @end_date4 datetime,
    @date_type varchar(12)
AS

DECLARE
    @iBranchID int,
    @transdetail_id int,
    @documenttype_id int,
    @accounting_date datetime,
    @amount numeric(19, 4)
    

SELECT @iBranchID = ISNULL(@branch_id, 0)

CREATE TABLE #Report_Weekly_Income
(
    branch_code VARCHAR(20),
    branch varchar(255) NULL,
    Document_type varchar(15) NULL,
    business_type varchar(255) NULL,
    commission1 numeric(19, 4) NULL,
    commission2 numeric(19, 4) NULL,
    commission3 numeric(19, 4) NULL,
    commission4 numeric(19, 4) NULL,
    commissiontotal numeric(19, 4) NULL, 
    policy1 varchar(30),            -- 1.6.9
    policy2 varchar(30),            -- 1.6.9
    policy3 varchar(30),            -- 1.6.9
    policy4 varchar(30),            -- 1.6.9
    policycount1 int,               -- 1.6.9 
    policycount2 int,               -- 1.6.9
    policycount3 int,               -- 1.6.9
    policycount4 int,               -- 1.6.9
    budget numeric(19, 4) NULL,
    variance numeric(19, 4) NULL
)
/*Select all Documents which have commission */
DECLARE c_cursor CURSOR FAST_FORWARD FOR
    SELECT T.transdetail_id,
        D.documenttype_id,
        T.ref_date,                     -- 1.6.9
        T.amount
    FROM Transdetail T,
        Document D
    WHERE   D.documenttype_id IN (4,5,15,16,17,18,31,32,35,36)  -- eck291002    
    AND D.document_id = T.document_id
    AND
    (
        T.spare IN ('BROK', 'BROK ADJ')
        OR
        T.account_id IN
            (
                SELECT account_id
                FROM account
                WHERE account_id = T.account_id
                AND ledger_id in (7, 8)
            )
    )

        AND (
        (
        D.document_date >= @start_date
        AND D.document_date <= @end_date4
        AND @date_type = 'Transaction'
        )
        OR
        (
    T.spare not like '%REVER%'      -- 1.6.9 Remove reversals for effective range
    AND T.ref_date >= @start_date       -- 1.6.9
    AND T.ref_date <= @end_date4        -- 1.6.9
    AND @date_type = 'Effective'
        )
    )
    AND (
            @iBranchID = 0
            OR
            (
                @iBranchID <> 0
                AND
                D.company_id = @iBranchID
            )
        )


OPEN c_cursor

FETCH NEXT FROM c_cursor INTO
    @transdetail_id,
    @documenttype_id,
    @accounting_date,
    @amount
WHILE @@FETCH_STATUS = 0
BEGIN
/* Set up report data in temporary table */

    INSERT INTO #Report_Weekly_Income

SELECT

/* Branch Code*/
    ISNULL(
            (
            SELECT C.code
            FROM Transdetail T,
                Document D,
                    Company C
            WHERE
                @transdetail_Id = T.transdetail_id
                AND T.document_id = D.document_id
                AND D.company_id = C.company_id
            )
    , 'No Branch'),


/* Branch */

            ISNULL(
            (
            SELECT C.description
            FROM Transdetail T,
                Document D,
                    Company C
            WHERE
                @transdetail_Id = T.transdetail_id
                AND T.document_id = D.document_id
                AND D.company_id = C.company_id
            )
, 'No Branch Specified'),
/* Document Type */
    (select document_type =
             ISNULL (
                CASE @Documenttype_id
                    WHEN 4 THEN 'New Business'
                    WHEN 5 THEN 'New Business'
                    WHEN 15 THEN 'Renewals'
                    WHEN 16 THEN 'Renewals'
                    WHEN 17 THEN 'Adjustments'
                    WHEN 18 THEN 'Adjustments'
            WHEN 31  THEN 'Short Term'      --eck291002
            WHEN 32  THEN 'Short Term'      --eck291002             
            WHEN 35  THEN 'Transferred'     --eck291002
            WHEN 36  THEN 'Transferred'     --eck291002                 
        END
, '')
            )
,

/* Business Type */
        ISNULL(
            (
            SELECT BT.description
            FROM Transdetail T,
                    Insurance_File I,
                    Business_Type BT
            WHERE
                @transdetail_id = T.transdetail_id
                AND T.insurance_ref = I.insurance_ref
                AND I.insurance_file_cnt =
                (SELECT MAX(insurance_file_cnt)
                    FROM insurance_file
                    WHERE insurance_ref = I.insurance_ref
                    )
            AND BT.business_type_id = I.Business_type_id
            )
, 'No Business Specified'),

/* Weeks 1 Commission */

        ISNULL(
            (
            SELECT T.amount
            FROM Transdetail T,
                 Document D
            WHERE
                @transdetail_id = T.transdetail_id
--eck160702 extra selection based on date type
                AND T.document_id = D.document_id
                AND (
                        (T.ref_date >=  @start_date
                        AND T.ref_date <= @end_date1
                        AND @date_type = 'Effective'
                        )
                        OR
                        (D.document_date >=  @start_date
                        AND D.document_date <= @end_date1
                        AND @date_type = 'Transaction'
                        )
                   )

            )
, 0),
/* Weeks 2 Commission */

        ISNULL(
            (
            SELECT T.amount
            FROM Transdetail T,
                 Document D
            WHERE
                @transdetail_id = T.transdetail_id
--eck160702 extra selection based on date type
                AND T.document_id = D.document_id
                AND (
                        (T.ref_date > @end_date1
                        AND T.ref_date <= @end_date2
                        AND @date_type = 'Effective'
                        )
                     OR
                        (D.document_date > @end_date1
                        AND D.document_date <= @end_date2
                        AND @date_type = 'Transaction'
                        )
                     )
            )
, 0),
/* Weeks 3 Commission */

        ISNULL(
            (
            SELECT T.amount
            FROM Transdetail T,
                 Document D
            WHERE
                @transdetail_id = T.transdetail_id
--eck160702 extra selection based on date type
                AND T.document_id = D.document_id
                AND (
                        (T.ref_date > @end_date2
                        AND T.ref_date <= @end_date3
                        AND @date_type = 'Effective'
                        )
                     OR
                        (D.document_date > @end_date2
                        AND D.document_date <= @end_date3
                        AND @date_type = 'Transaction'
                        )
                     )
            )
, 0),

/* Weeks 4 Commission */

        ISNULL(
            (
            SELECT T.amount
            FROM Transdetail T,
                 Document D
            WHERE
                @transdetail_id = T.transdetail_id
--eck160702 extra selection based on date type
                AND T.document_id = D.document_id
                AND (
                        (T.ref_date > @end_date3
                        AND T.ref_date <= @end_date4
                        AND @date_type = 'Effective'
                        )
                     OR
                        (D.document_date > @end_date3
                        AND D.document_date <= @end_date4
                    AND @date_type = 'Transaction'
                        )
                     )
            )
, 0),
/* Total Commission */
    0,

--1.6.9Start

/* Weeks 1 Policy Ref */
 
        ISNULL(
            (
            SELECT T.insurance_ref 
            FROM Transdetail T,
                 Document D 
            WHERE
                @transdetail_id = T.transdetail_id
                AND T.document_id = D.document_id
                AND ( 
                        (T.ref_date >= @start_date       
                        AND T.ref_date <= @end_date1         
                        AND @date_type = 'Effective'
                        )
                        OR
                        (D.document_date >= @start_date
                        AND D.document_date <= @end_date1
                        AND @date_type = 'Transaction'
                        )
                   )
                      
            ) 
        ,''),
/* Weeks 2 Policy Ref*/
 
        ISNULL(
            (
            SELECT T.insurance_ref 
            FROM Transdetail T,
                 Document D  
            WHERE
                @transdetail_id = T.transdetail_id
                AND T.document_id = D.document_id
                AND  (
                        (T.ref_date > @end_date1         
                        AND T.ref_date <= @end_date2         
                        AND @date_type = 'Effective'
                        )
                     OR
                        (D.document_date > @end_date1 
                        AND D.document_date <= @end_date2
                        AND @date_type = 'Transaction'
                        )
                     )
            ) 
        ,''),
/* Weeks 3 Policy Ref*/
 
        ISNULL(
            (
            SELECT T.insurance_ref 
            FROM Transdetail T,
                 Document D  
            WHERE
                @transdetail_id = T.transdetail_id
                AND T.document_id = D.document_id
                AND  (
                        (T.ref_date > @end_date2         
                        AND T.ref_date <= @end_date3         
                        AND @date_type = 'Effective'
                        )
                     OR
                        (D.document_date >  @end_date2  
                        AND D.document_date <= @end_date3
                        AND @date_type = 'Transaction'
                        )
                     )
            )
        ,''),        
  
/* Weeks 4 Policy Ref*/
 
        ISNULL(
            (
            SELECT T.insurance_ref 
            FROM Transdetail T,
                 Document D  
            WHERE
                @transdetail_id = T.transdetail_id
                AND T.document_id = D.document_id
                AND  (
                        (T.ref_date > @end_date3         
                        AND T.ref_date <= @end_date4         
                        AND @date_type = 'Effective'
                        )
                     OR
                        (D.document_date > @end_date3 
                        AND D.document_date <= @end_date4
                    AND @date_type = 'Transaction'
                        )
                     )
            )
        ,''),
/* weeks1 policy count */
        0,
/* weeks2 policy count */
        0,
/* weeks3 policy count */
        0,
/* weeks4 policy count */
        0,
--1.6.9end




/* Budget */
    0,
/* Variance */
    0

    FETCH NEXT FROM c_cursor INTO
    @transdetail_id,
    @documenttype_id,
    @accounting_date,
    @amount

END

CLOSE c_cursor
DEALLOCATE c_cursor
--1.6.9 Update policy counts
UPDATE RWI 
SET RWI.policycount1 = (select count(distinct RWI2.policy1) 
               from #Report_Weekly_Income RWI2
               where RWI.branch = RWI2.branch
               and RWI.document_type = RWI2.document_type
               and RWI.business_type = RWI2.business_type
               and RWI.policy1 <> ''
               and RWI2.policy1 <> '')
FROM  #Report_Weekly_Income RWI
           
UPDATE RWI 
SET RWI.policycount2 = (select count(distinct RWI2.policy2) 
               from #Report_Weekly_Income RWI2
               where RWI.branch = RWI2.branch
               and RWI.document_type = RWI2.document_type
               and RWI.business_type = RWI2.business_type
               and RWI.policy2 <> ''
               and RWI2.policy2 <> '')
FROM  #Report_Weekly_Income RWI

UPDATE RWI 
SET RWI.policycount3 = (select count(distinct RWI2.policy3) 
               from #Report_Weekly_Income RWI2
               where RWI.branch = RWI2.branch
               and RWI.document_type = RWI2.document_type
               and RWI.business_type = RWI2.business_type
               and RWI.policy3 <> ''
               and RWI2.policy3 <> '')
FROM  #Report_Weekly_Income RWI

UPDATE RWI 
SET RWI.policycount4 = (select count(distinct RWI2.policy4) 
               from #Report_Weekly_Income RWI2
               where RWI.branch = RWI2.branch
               and RWI.document_type = RWI2.document_type
               and RWI.business_type = RWI2.business_type
               and RWI.policy4 <> ''
               and RWI2.policy4 <> '')
FROM  #Report_Weekly_Income RWI

/* Return required data */

SELECT
    branch_code,
    branch,
    document_type,
    business_type,
    sum(commission1) 'commission1',
    sum(commission2) 'commission2',
    sum(commission3) 'commission3',
    sum(commission4) 'commission4',
    sum(commission1)+ sum(commission2) + sum(commission3) + sum(commission4) 'commissiontotal',
    max(policycount1)   'policies1',    --1.6.9 
    max(policycount2)   'policies2',    --1.6.9 
    max(policycount3)   'policies3',    --1.6.9 
    max(policycount4)   'policies4',    --1.6.9 
    0 'budget',
    0 'variance'

INTO #Report_Weekly_Income_Total
FROM #Report_Weekly_Income
GROUP BY branch_code, branch, document_type, business_type

SELECT
    branch_code,
    branch,
    document_type,
    business_type,
    commission1 * -1'commission1',
    commission2 * -1'commission2',
    commission3 * -1'commission3',
    commission4 * -1'commission4',
    commissiontotal * -1 'commissiontotal', -- eck120802 reverse sign   
    policies1,          --1.6.9
    policies2,          --1.6.9
    policies3,          --1.6.9
    policies4,          --1.6.9    
    budget,
    variance
FROM #Report_Weekly_Income_Total

DROP TABLE #Report_Weekly_Income
DROP TABLE #Report_Weekly_Income_Total

SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

