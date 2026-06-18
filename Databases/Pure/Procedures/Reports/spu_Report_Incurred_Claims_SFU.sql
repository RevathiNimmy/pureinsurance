EXECUTE DDLDropProcedure 'spu_Report_Incurred_Claims_SFU'
GO

/*****************************************
**  Incurred Claims Report.
**
**  Created: P. Haynes 3/9/2001
**
**  Incurred_Claims.rpt
**
***********************************************************************************************************************************
** VER      DATE        WHO     WHAT
** 1.01     25/01/2002  JMK     Add date parameters
**                              Add Include Closed parameter
**
** 1.02     15/02/2002  JMK     Increase description fields to accept db maximum
** 1.03		01/09/2002	JT		Multicurrency Changes
***********************************************************************************************************************************/

CREATE PROCEDURE spu_Report_Incurred_Claims_SFU
(
    @IncludeRecoveries varchar(3),
    @IncludeClosed varchar (5),
    @Start_Date 		nvarchar(50),    
    @End_Date 			nvarchar(50),
    @DateRange varchar(20),
    @TypeOfCurrency	Varchar(30),
    @GroupByCode	Varchar(30),
    @TPACode Varchar(30) = NULL
)
AS
SET NOCOUNT ON
SELECT @Start_Date= CONVERT(DATETIME, @Start_Date, 103),    
    @End_Date = CONVERT(DATETIME, @End_Date, 103) 
--for testing
/*
DateRange:
    Specify Dates
    Today
    Yesterday
    This Week
    Last Full Week
    This Month
    Last Full Month
*/
/*
DECLARE @IncludeRecoveries varchar (3),
        @IncludeClosed varchar (5),
        @Start_Date datetime,
        @End_Date datetime,
        @DateRange  varchar(20)
SELECT @IncludeRecoveries = 'yes', @IncludeClosed = 'no',
        @start_date = dateadd(day,-55,getdate()),
        @end_date = getdate(),
        @DateRange = 'This Month'
*/
CREATE TABLE #tempClaim 
(
    Initial_Reserve     money null,
    Revised_Reserve     money null,
    Sum_insured         money null,
    Paid_to_Date        money null,
    claim_Number        varchar(60),
    Policy_number       varchar(60),
    Client_Name         varchar(255),
    Insurer_Name        varchar(60),
    loss_from_date      datetime null,
    Client_Short_name   varchar(60),
    ClaimDescription    varchar(255),
    ProductDescription  varchar(255),
    RiskTypeDescription varchar(255),
    IncurredAmount      money null,
    CurrentReserve      money null,
    IsRecovery          varchar(3),
    Currencyid			INT,
    SourceID			Int	Null
)
/*Get System Currency Details--jitendra*/
	declare @SystemCurrencyCode varchar(10)
	declare @SystemCurrencyDesc varchar(255)
	declare @SystemCurrencyId Int
    SELECT
    	@SystemCurrencyCode = c.iso_code,
    	@SystemCurrencyDesc = c.description,
        @SystemCurrencyId = c.currency_id
    FROM PMSystem pms
    JOIN currency c
    	ON c.currency_id = pms.currency_id
    WHERE pms.system_id = 1
/*end  Get System Currency*/
Declare @Branch Int
Declare @TypeOfRates Int
EXEC spu_ACT_GetTypeOfRates @TypeOfRates OUTPUT 
IF @TypeOfRates =1 
	SELECT @branch=1 
ELSE 
	BEGIN
		SELECT @branch=NULL 
	END
	
DECLARE @TPAId INT

SELECT @TPAId=party_cnt from party where shortname= @TPACode


IF @IncludeRecoveries <> 'Yes'
BEGIN

    --print 'insert1: @IncludeRecoveries = ' + @IncludeRecoveries
    --print '@IncludeClosed = ' + @IncludeClosed
    INSERT INTO #tempClaim

    SELECT   
    	Case @TypeOfCurrency 
			WHEN 'Base' THEN (isnull(r.Initial_reserve,0)*ISNULL(C.currency_base_xrate,CR.rate_against_base))
			--WHEN 'System' THEN (isnull(r.Initial_reserve,0)*ISNULL(C.currency_base_xrate,CR.rate_against_base)/ISNULL(C.system_base_xrate,CR.rate_against_base))
			WHEN 'System' THEN (isnull(r.Initial_reserve ,0)*ISNULL(CR.rate_against_base,0) / ISNULL(CS.rate_against_base,0)) 
			WHEN 'Transaction' THEN isnull(r.Initial_reserve,0)
		END Initial_reserve,
        Case @TypeOfCurrency 
			WHEN 'Base' THEN (isnull(r.Revised_reserve,0)*ISNULL(c.currency_base_xrate,CR.rate_against_base)) -(isnull(cpp.unauthorise_reserve,0)*ISNULL(c.currency_base_xrate,CR.rate_against_base))
			WHEN 'System' THEN  (isnull(r.Revised_reserve,0)*ISNULL(c.currency_base_xrate,CR.rate_against_base) / ISNULL(c.system_base_xrate,CR.rate_against_base))
			      - (isnull(cpp.unauthorise_reserve,0)*ISNULL(c.currency_base_xrate,CR.rate_against_base) / ISNULL(c.system_base_xrate,CR.rate_against_base))
			WHEN 'Transaction' THEN isnull(r.Revised_reserve,0) -isnull(cpp.unauthorise_reserve,0)
		END Revised_reserve,
        Case @TypeOfCurrency 
			WHEN 'Base' THEN (isnull(r.Sum_insured,0)*ISNULL(C.currency_base_xrate,CR.rate_against_base))
			WHEN 'System' THEN (isnull(r.Sum_insured,0)*ISNULL(C.currency_base_xrate,CR.rate_against_base)/ISNULL(C.system_base_xrate,CR.rate_against_base))
			WHEN 'Transaction' THEN isnull(r.Sum_insured,0)
		END Sum_insured,
		 Case @TypeOfCurrency 
			WHEN 'Base' THEN (isnull(r.Paid_to_date,0)*ISNULL(c.currency_base_xrate,CR.rate_against_base)) -  ISNULL(cpp.revised_reserve,0)
			WHEN 'System' THEN (isnull(r.Paid_to_date,0)*ISNULL(CR.rate_against_base,0) / ISNULL(CS.rate_against_base,0))  -  ISNULL(cpp.revised_reserve,0)
			WHEN 'Transaction' THEN isnull(r.Paid_to_date,0)-  ISNULL(cpp.revised_reserve,0)
		END Paid_to_date,
         c.claim_Number,
        c.Policy_number,
        c.Client_Name,
        c.Insurer_Name,
        c.loss_from_date,
        c.Client_Short_name,
        LEFT(c.description,255) ClaimDescription,
        p.description ProductDescription,
        rt.description RiskTypeDescription,
		 (Case @TypeOfCurrency 
					WHEN 'Base' THEN (isnull(r.Initial_reserve,0)*ISNULL(C.currency_base_xrate,CR.rate_against_base))
					WHEN 'System' THEN (isnull(r.Initial_reserve,0)*ISNULL(CR.rate_against_base,0) / ISNULL(CS.rate_against_base,0))  
					WHEN 'Transaction' THEN isnull(r.Initial_reserve,0)
		END)
			+
		(Case @TypeOfCurrency 
				WHEN 'Base' THEN (isnull(r.Revised_reserve,0)*ISNULL(c.currency_base_xrate,CR.rate_against_base))
				WHEN 'System' THEN  (isnull(r.Revised_reserve,0)*ISNULL(CR.rate_against_base,0) / ISNULL(CS.rate_against_base,0))  
				WHEN 'Transaction' THEN isnull(r.Revised_reserve,0)
		END) as IncurredAmount,

        
		(Case @TypeOfCurrency 
			WHEN 'Base' THEN (isnull(r.Initial_reserve,0)*ISNULL(C.currency_base_xrate,CR.rate_against_base))
			WHEN 'System' THEN (isnull(r.Initial_reserve,0)*ISNULL(CR.rate_against_base,0) / ISNULL(CS.rate_against_base,0))  
			WHEN 'Transaction' THEN isnull(r.Initial_reserve,0)
		END)
		+
		(Case @TypeOfCurrency 
			WHEN 'Base' THEN (isnull(r.Revised_reserve,0)*ISNULL(c.currency_base_xrate,CR.rate_against_base))
			WHEN 'System' THEN  (isnull(r.Revised_reserve,0)*ISNULL(CR.rate_against_base,0) / ISNULL(CS.rate_against_base,0))  
			WHEN 'Transaction' THEN isnull(r.Revised_reserve,0)
		END
		)
		-
		(Case @TypeOfCurrency 
			WHEN 'Base' THEN (isnull(r.Paid_to_date,0)*ISNULL(c.currency_base_xrate,CR.rate_against_base))
			WHEN 'System' THEN (isnull(r.Paid_to_date,0)*ISNULL(CR.rate_against_base,0) / ISNULL(CS.rate_against_base,0))  
			WHEN 'Transaction' THEN isnull(r.Paid_to_date,0)
		END) as CurrentReserve,
 
        'no',c.currency_id,i.source_id

    FROM
        Reserve r
        join claim_peril cp on r.claim_peril_id = cp.claim_peril_id
               LEFT JOIN (
			SELECT cpi.reserve_id,SUM(cpi.this_payment*currency_base_xrate) this_payment_base,
					SUM(cpi.this_payment*currency_base_xrate/system_base_xrate) this_payment_system, 
					SUM(cpi.this_payment*payment_loss_xrate) this_payment_trans,
					SUM(r1.this_revision) unauthorise_reserve,
					SUM(ISNULL(RC.revised_reserve,0)) revised_reserve	
			FROM Claim_Payment cp2 
			INNER JOIN Claim_Payment_Item cpi ON cp2.claim_payment_id=cpi.claim_payment_id 
			INNER JOIN Claim_Payment cp1 ON cp1.claim_payment_id=cp2.base_claim_payment_id
			INNER JOIN reserve r1 on r1.reserve_id= cpi.reserve_id
			left join Recovery RC on RC.claim_peril_id = cp1.claim_peril_id
			WHERE (cp1.is_referred=1 OR rc.claim_Peril_id is not null)
			GROUP BY cpi.reserve_id ) cpp ON r.Reserve_id=cpp.reserve_id 
        join claim c on cp.claim_id = c.claim_id
        join Insurance_file I on c.Policy_id = I.insurance_file_cnt
        INNER JOIN currencyrate CR ON CR.currency_id = C.currency_id
        AND CR.company_id = ISNULL(@branch,I.source_id)
		INNER JOIN currencyrate CS ON CS.currency_id = @SystemCurrencyId
        AND CS.company_id = ISNULL(@branch,I.source_id) 
        left join Product p on i.product_id = P.product_id
        left join claim_risk crk on c.claim_id = crk. claim_id
        left join Risk_type rt on crk.risk_type_id = rt.risk_type_Id

    WHERE (
        (c.Claim_Status_id in (2, 4) AND @IncludeClosed = 'no')
        OR
        (c.Claim_Status_id <> 1 AND @IncludeClosed = 'yes')
        )
    And c.is_dirty<>1
    AND (
        isnull(r.initial_reserve,0) <> 0 OR
        isnull(r.revised_reserve,0) <> 0 OR
        isnull(r.paid_to_date,0) <> 0  OR
        isnull(r.Sum_insured,0) <> 0
        )
    AND (
        @DateRange = 'specify dates' AND
            (
            datediff(day, @start_date, c.loss_from_date) >=0
            AND
            datediff(day, c.loss_from_date, @end_date) >=0
            )
        OR
        (@DateRange = 'yesterday' AND
        datediff (day, c.loss_from_date, getdate())= 1)
        OR
        (@DateRange = 'today' AND
        datediff (day, c.loss_from_date, getdate())= 0)
        OR
        (@DateRange = 'last full week' AND
        datediff (week, c.loss_from_date, getdate())= 1)
        OR
        (@DateRange = 'this week' AND
        datediff (week, c.loss_from_date, getdate())= 0)
        OR
        (@DateRange = 'last full month' AND
        datediff (month, c.loss_from_date, getdate())= 1)
        OR
        (@DateRange = 'this month' AND
        datediff (month, c.loss_from_date, getdate())= 0)
        )
		AND CR.effective_from IN
		(
			SELECT MAX(effective_from)
			FROM CurrencyRate 
			WHERE effective_from <= C.reported_date
			AND   currency_id = CR.currency_id
			AND company_id = CR.company_id
		)
		AND CS.effective_from IN
		(
			SELECT MAX(effective_from)
			FROM CurrencyRate 
			WHERE effective_from <= C.reported_date
			AND   currency_id = CS.currency_id
			AND company_id = CS.company_id
		) 
	AND r.version_id=(SELECT MAX(version_id) FROM  claim WHERE claim_Number= c.claim_Number AND ISNULL(is_dirty,0) = 0)
    AND (c.other_party_id= @TPAId or @TPAId IS NULL)
END
ELSE
BEGIN



    --print 'insert2: @IncludeRecoveries = ' + @IncludeRecoveries
    --print '@IncludeClosed = ' + @IncludeClosed
    
	INSERT INTO #tempClaim

    SELECT  distinct
Case @TypeOfCurrency 
			WHEN 'Base' THEN (isnull(r.Initial_reserve,0)*ISNULL(C.currency_base_xrate,CR.rate_against_base))
			--WHEN 'System' THEN (isnull(r.Initial_reserve,0)*ISNULL(C.currency_base_xrate,CR.rate_against_base)/ISNULL(C.system_base_xrate,CR.rate_against_base))
			WHEN 'System' THEN (isnull(r.Initial_reserve ,0)*ISNULL(CR.rate_against_base,0) / ISNULL(CS.rate_against_base,1)) 
			WHEN 'Transaction' THEN isnull(r.Initial_reserve,0)
		END Initial_reserve,
        Case @TypeOfCurrency 
			WHEN 'Base' THEN (isnull(r.Revised_reserve,0)*ISNULL(c.currency_base_xrate,CR.rate_against_base)) 
					-(isnull(cpp.unauthorise_reserve,0)*ISNULL(c.currency_base_xrate,CR.rate_against_base))
			WHEN 'System' THEN  (isnull(r.Revised_reserve,0)*ISNULL(c.currency_base_xrate,CR.rate_against_base) / ISNULL(c.system_base_xrate,CR.rate_against_base))
					- (isnull(cpp.unauthorise_reserve,0)*ISNULL(c.currency_base_xrate,CR.rate_against_base) / ISNULL(c.system_base_xrate,CR.rate_against_base))
			WHEN 'Transaction' THEN isnull(r.Revised_reserve,0) - isnull(cpp.unauthorise_reserve,0)
		END Revised_reserve,
        Case @TypeOfCurrency 
			WHEN 'Base' THEN (isnull(r.Sum_insured,0)*ISNULL(C.currency_base_xrate,CR.rate_against_base))
			WHEN 'System' THEN (isnull(r.Sum_insured,0)*ISNULL(C.currency_base_xrate,CR.rate_against_base)/ISNULL(C.system_base_xrate,CR.rate_against_base))
			WHEN 'Transaction' THEN isnull(r.Sum_insured,0)
		END Sum_insured,
		Case @TypeOfCurrency 
			WHEN 'Base' THEN (isnull(r.Paid_to_date,0)*ISNULL(c.currency_base_xrate,CR.rate_against_base))-  ISNULL(cpp.revised_reserve,0)
			WHEN 'System' THEN (isnull(r.Paid_to_date,0)*ISNULL(CR.rate_against_base,0) / ISNULL(CS.rate_against_base,1)) -  ISNULL(cpp.revised_reserve,0) 
			WHEN 'Transaction' THEN isnull(r.Paid_to_date,0)-  ISNULL(cpp.revised_reserve,0)
		END Paid_to_date,
		
        c.claim_Number,
        c.Policy_number,
        c.Client_Name,
        c.Insurer_Name,
        c.loss_from_date,
        c.Client_Short_name,
        LEFT(c.description,255),
        p.description,
        rt.description RiskTypeDescription,
		 (Case @TypeOfCurrency 
					WHEN 'Base' THEN (isnull(r.Initial_reserve,0)*ISNULL(C.currency_base_xrate,CR.rate_against_base))
					WHEN 'System' THEN (isnull(r.Initial_reserve,0)*ISNULL(CR.rate_against_base,0) / ISNULL(CS.rate_against_base,1))  
					WHEN 'Transaction' THEN isnull(r.Initial_reserve,0)
		END)
			+
		(Case @TypeOfCurrency 
				WHEN 'Base' THEN (isnull(r.Revised_reserve,0)*ISNULL(c.currency_base_xrate,CR.rate_against_base))
				WHEN 'System' THEN  (isnull(r.Revised_reserve,0)*ISNULL(CR.rate_against_base,0) / ISNULL(CS.rate_against_base,1))  
				WHEN 'Transaction' THEN isnull(r.Revised_reserve,0)
		END) as IncurredAmount,

        
		(Case @TypeOfCurrency 
			WHEN 'Base' THEN (isnull(r.Initial_reserve,0)*ISNULL(C.currency_base_xrate,CR.rate_against_base))
			WHEN 'System' THEN (isnull(r.Initial_reserve,0)*ISNULL(CR.rate_against_base,0) / ISNULL(CS.rate_against_base,1))  
			WHEN 'Transaction' THEN isnull(r.Initial_reserve,0)
		END)
		+
		(Case @TypeOfCurrency 
			WHEN 'Base' THEN (isnull(r.Revised_reserve,0)*ISNULL(c.currency_base_xrate,CR.rate_against_base))
			WHEN 'System' THEN  (isnull(r.Revised_reserve,0)*ISNULL(CR.rate_against_base,0) / ISNULL(CS.rate_against_base,1))  
			WHEN 'Transaction' THEN isnull(r.Revised_reserve,0)
		END
		)
		-
		(Case @TypeOfCurrency 
			WHEN 'Base' THEN (isnull(r.Paid_to_date,0)*ISNULL(c.currency_base_xrate,CR.rate_against_base))
			WHEN 'System' THEN (isnull(r.Paid_to_date,0)*ISNULL(CR.rate_against_base,0) / ISNULL(CS.rate_against_base,1))  
			WHEN 'Transaction' THEN isnull(r.Paid_to_date,0)
		END) as CurrentReserve,
          'no',c.currency_id,i.source_id

    FROM
        Reserve r
        join claim_peril cp on r.claim_peril_id = cp.claim_peril_id
        LEFT JOIN (
			SELECT cpi.reserve_id,SUM(cpi.this_payment*currency_base_xrate) this_payment_base,
					SUM(cpi.this_payment*currency_base_xrate/system_base_xrate) this_payment_system, 
					SUM(cpi.this_payment*payment_loss_xrate) this_payment_trans,
					SUM(r1.this_revision) unauthorise_reserve,
					SUM(ISNULL(RC.revised_reserve,0)) revised_reserve
			FROM Claim_Payment cp2 
			INNER JOIN Claim_Payment_Item cpi ON cp2.claim_payment_id=cpi.claim_payment_id 
			INNER JOIN Claim_Payment cp1 ON cp1.claim_payment_id=cp2.base_claim_payment_id
			INNER JOIN reserve r1 on r1.reserve_id= cpi.reserve_id
			left join Recovery RC on RC.claim_peril_id = cp1.claim_peril_id
			WHERE (cp1.is_referred=1 OR rc.claim_Peril_id is not null)
			GROUP BY cpi.reserve_id ) cpp ON r.Reserve_id=cpp.reserve_id 
        join claim c on cp.claim_id = c.claim_id
        join Insurance_file I on c.Policy_id = I.insurance_file_cnt
        INNER JOIN currencyrate CR ON CR.company_id = ISNULL(@branch,I.source_id)
        AND CR.currency_id = C.currency_id
		INNER JOIN currencyrate CS ON CS.company_id = ISNULL(@branch,I.source_id)
        AND CS.currency_id = @SystemCurrencyId 
        left join Product p on i.product_id = P.product_id
        left join claim_risk crk on c.claim_id = crk. claim_id
        left join Risk_type rt on crk.risk_type_id = rt.risk_type_Id
		
    WHERE (
        (c.Claim_Status_id in (2, 4) AND @IncludeClosed = 'no')
        OR
        (c.Claim_Status_id <> 1 AND @IncludeClosed = 'yes')
        )
	 And c.is_dirty<>1
    AND (
        isnull(r.initial_reserve,0) <> 0 OR
        isnull(r.revised_reserve,0) <> 0 OR
        isnull(r.paid_to_date,0) <> 0 OR
        isnull(r.Sum_insured,0) <> 0
        )
    AND (
        @DateRange = 'specify dates' AND
            (
            datediff(day, @start_date, c.loss_from_date) >=0
            AND
            datediff(day, c.loss_from_date, @end_date) >=0
            )
        OR
        (@DateRange = 'yesterday' AND
        datediff (day, c.loss_from_date, getdate())= 1)
        OR
        (@DateRange = 'today' AND
        datediff (day, c.loss_from_date, getdate())= 0)
        OR
        (@DateRange = 'last full week' AND
        datediff (week, c.loss_from_date, getdate())= 1)
        OR
        (@DateRange = 'this week' AND
        datediff (week, c.loss_from_date, getdate())= 0)
        OR
        (@DateRange = 'last full month' AND
        datediff (month, c.loss_from_date, getdate())= 1)
        OR
        (@DateRange = 'this month' AND
        datediff (month, c.loss_from_date, getdate())= 0)
        )
		AND CR.effective_from IN
		(
			SELECT MAX(effective_from)
			FROM CurrencyRate 
			WHERE effective_from <= C.reported_date
			AND   currency_id = CR.currency_id
			AND company_id = CR.company_id
		)
		AND CS.effective_from IN
		(
			SELECT MAX(effective_from)
			FROM CurrencyRate 
			WHERE effective_from <= C.reported_date
			AND   currency_id = CS.currency_id
			AND company_id = CS.company_id
		) 
	AND r.version_id=(SELECT MAX(version_id) FROM  claim WHERE claim_Number= c.claim_Number AND ISNULL(is_dirty,0) = 0)
	AND (c.other_party_id= @TPAId or @TPAId IS NULL)
	
    --
    --print 'insert3: @IncludeRecoveries = ' + @IncludeRecoveries
    --print '@IncludeClosed = ' + @IncludeClosed
    INSERT INTO #tempClaim
 
    SELECT  0 as Initial_reserve,  
         Case @TypeOfCurrency  
            WHEN 'Base' THEN (isnull(r.Revised_reserve,0)*ISNULL(c.currency_base_xrate,CR.rate_against_base) + isnull(r.Initial_reserve,0)*ISNULL(c.currency_base_xrate,CR.rate_against_base))  
    WHEN 'System' THEN  (isnull(r.Revised_reserve,0)*ISNULL(CR.rate_against_base,0) / ISNULL(c.system_base_xrate,CR.rate_against_base) + isnull(r.Initial_reserve,0)*ISNULL(CR.rate_against_base,0) / ISNULL(c.system_base_xrate,CR.rate_against_base))  
    WHEN 'Transaction' THEN isnull(r.Revised_reserve,0) + isnull(r.Initial_reserve,0)  
   END * (-1) Revised_reserve,   
        null,  
            Case @TypeOfCurrency  
    WHEN 'Base' THEN (isnull(r.Received_to_date,0)*ISNULL(c.currency_base_xrate,CR.rate_against_base))  *-1
    WHEN 'System' THEN  (isnull(r.Received_to_date,0)*ISNULL(c.currency_base_xrate,CR.rate_against_base) / ISNULL(c.system_base_xrate,CR.rate_against_base))  *-1
    WHEN 'Transaction' THEN isnull(r.Received_to_date,0)  *-1
   END Received_to_date,  
        c.claim_Number,  
        c.Policy_number,  
        c.Client_Name,  
        c.Insurer_Name,  
        c.loss_from_date,  
        c.Client_Short_name,  
        LEFT(c.description,255),  
        p.description,  
        rt.description RiskTypeDescription,  
		 Case @TypeOfCurrency  
WHEN 'Base' THEN (isnull(r.Revised_reserve,0)*ISNULL(c.currency_base_xrate,CR.rate_against_base) + isnull(r.Initial_reserve,0)*ISNULL(c.currency_base_xrate,CR.rate_against_base))  
WHEN 'System' THEN  (isnull(r.Revised_reserve,0)*ISNULL(CR.rate_against_base,0) / ISNULL(c.system_base_xrate,CR.rate_against_base) + isnull(r.Initial_reserve,0)*ISNULL(CR.rate_against_base,0) / ISNULL(c.system_base_xrate,CR.rate_against_base))  
WHEN 'Transaction' THEN isnull(r.Revised_reserve,0) + isnull(r.Initial_reserve,0)  
END * (-1) as IncurredAmount,   
         Case @TypeOfCurrency  
            WHEN 'Base' THEN (isnull(r.Revised_reserve,0)*ISNULL(c.currency_base_xrate,CR.rate_against_base) + isnull(r.Initial_reserve,0)*ISNULL(c.currency_base_xrate,CR.rate_against_base))  
    WHEN 'System' THEN  (isnull(r.Revised_reserve,0)*ISNULL(CR.rate_against_base,0) / ISNULL(c.system_base_xrate,CR.rate_against_base) + isnull(r.Initial_reserve,0)*ISNULL(CR.rate_against_base,0) / ISNULL(c.system_base_xrate,CR.rate_against_base))  
    WHEN 'Transaction' THEN isnull(r.Revised_reserve,0) + isnull(r.Initial_reserve,0) 
   END * (-1) as CurrentReserve, 
         'yes',c.currency_id,i.source_id  




    FROM 
        [Recovery] r
        join claim_peril cp on r.claim_peril_id = cp.claim_peril_id
        join Claim c on cp.claim_id = c.claim_id
        join Insurance_file I on c.Policy_id = I.insurance_file_cnt
        INNER JOIN currencyrate CR ON CR.currency_id = C.currency_id
        AND CR.company_id = ISNULL(@branch,I.source_id)
        left join Product p on i.product_id = P.product_id
        left join claim_risk crk on c.claim_id = crk. claim_id
        left join Risk_type rt on crk.risk_type_id = rt.risk_type_Id

    WHERE (
        (c.Claim_Status_id in (2, 4) AND @IncludeClosed = 'no')
        OR
        (c.Claim_Status_id <> 1 AND @IncludeClosed = 'yes')
        )
        And c.is_dirty<>1
    AND (
        isnull(r.initial_reserve,0) <> 0 OR
        isnull(r.revised_reserve,0) <> 0 OR
        isnull(r.Received_to_date,0) <> 0          
        )
    AND (
        @DateRange = 'specify dates' AND
            (
            datediff(day, @start_date, c.loss_from_date) >=0
            AND
            datediff(day, c.loss_from_date, @end_date) >=0
            )
        OR
        (@DateRange = 'yesterday' AND
        datediff (day, c.loss_from_date, getdate())= 1)
        OR
        (@DateRange = 'today' AND
        datediff (day, c.loss_from_date, getdate())= 0)
        OR
        (@DateRange = 'last full week' AND
        datediff (week, c.loss_from_date, getdate())= 1)
        OR
        (@DateRange = 'this week' AND
        datediff (week, c.loss_from_date, getdate())= 0)
        OR
        (@DateRange = 'last full month' AND
        datediff (month, c.loss_from_date, getdate())= 1)
        OR
        (@DateRange = 'this month' AND
        datediff (month, c.loss_from_date, getdate())= 0)
        )
        AND CR.effective_from IN
		(
			SELECT MAX(effective_from)
			FROM CurrencyRate 
			WHERE effective_from <= C.reported_date
			AND   currency_id = CR.currency_id
			AND company_id = CR.company_id
		)
	AND r.version_id=(SELECT MAX(version_id) FROM  claim WHERE claim_Number= c.claim_Number AND ISNULL(is_dirty,0) = 0)
	AND (c.other_party_id= @TPAId or @TPAId IS NULL)
END
--
SET NOCOUNT OFF
SELECT * ,
S.Code CompanyCode,S.Description CompanyDesc,
        Case @TypeOfCurrency 
        	WHEN 'Base' THEN CB.Code
        	WHEN 'System' THEN @SystemcurrencyCode
        	WHEN 'Transaction' THEN CT.Code
        END CurrencyCode,
        Case @TypeOfCurrency 
        	WHEN 'Base' THEN CB.Description
        	WHEN 'System' THEN @SystemcurrencyDesc
        	WHEN 'Transaction' THEN CT.Description
        END CurencyDesc,
        Case @GroupByCode 
        	WHEN 'Branch' THEN S.Code
        	WHEN 'Branch And Currency' THEN S.Code
        	WHEN 'Currency' THEN CT.Code
        	ELSE ' '
        END GroupByCode
FROM #tempClaim TC
INNER JOIN Source S ON s.source_id = TC.SourceID
INNER JOIN Currency CB ON CB.Currency_id = S.Base_currency_id
JOIN Currency ct /*Transaction Currency*/
ON ct.currency_id = tc.currencyid
--Where Policy_Number  ='BGPMOT00045767'

Drop Table #tempClaim
GO
