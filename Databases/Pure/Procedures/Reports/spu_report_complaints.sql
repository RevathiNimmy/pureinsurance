SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Report_Complaints'
GO

CREATE PROCEDURE spu_Report_Complaints
    @start_date DATETIME,
    @end_date DATETIME,
    @branch_id INT
AS

create table #ComplaintsTemp
(
	Party Varchar (50),
	Turnover_id INT,
	TurnoverDescription varchar (225),
	Branchname varchar(50),
	Complaintcategoryid INT,
	ComplaintCategory VARCHAR(50),
	MotorComplaintsTotal INT,
	PropertyComplaintTotal INT,
	OtherComplaintTotal INT, 
	BoughtForwardMotorTotal INT,
    	CarriedForwardMotorTotal INT,
    	BoughtForwardPropertyTotal INT,
    	CarriedForwardPropertyTotal INT,
    	BoughtForwardOtherTotal INT,
    	CarriedForwardOtherTotal INT,
	 TotalUpheldMotor INT,
    	TotalReferredMotor INT,
    	TotalCompensationMotor MONEY,
    	TotalUpheldProperty INT,
    	TotalReferredProperty INT,
    	TotalCompensationProperty MONEY,
    	TotalUpheldOther INT,
    	TotalReferredOther INT,
    	TotalCompensationOther MONEY,
    	ClosedWithinFourWeeksMotor INT,
    	ClosedWithinFourWeeksProperty INT,
    	ClosedWithinFourWeeksOther INT,
    	ClosedFourToEightWeeksMotor INT,
    	ClosedFourToEightWeeksProperty INT,
    	ClosedFourToEightWeeksOther INT,
    	ClosedAfterEightWeeksMotor INT,
    	ClosedAfterEightWeeksProperty INT,
    	ClosedAfterEightWeeksOther INT,
    	FSADisabled INT
)

DECLARE  @FSADisabled integer,
	      @Turnoverband_id INT,
	      @Description VARCHAR(225)

SELECT @FSAdisabled = 0

IF NOT EXISTS
    (
        SELECT NULL
        FROM hidden_options
        WHERE option_number = 61
        AND value = '1'
    )
BEGIN
    SELECT @FSADisabled = 1
END

IF @branch_id = 0
BEGIN
    SELECT @branch_id = NULL
END

/* Calculate Summaries - Motor */

----- Inserting Personal client Complaint Details
insert into #ComplaintsTemp
 Select
   'Private Individuals',
   Null,
   Null,
    (
        SELECT
            description
        FROM source
        WHERE source_id = 1
    ) 'Branch Name', /*Meant to be company name*/
    fc.fsa_complaint_category_id 'Complaint Category Id',
    fc.description 'Complaint Category',
    (
        SELECT ISNULL(SUM(1),0)
        FROM FSA_complaint_folder fo
        JOIN FSA_class_of_business fb
            ON fb.FSA_class_of_business_id = fo.FSA_class_of_business_id
        JOIN party p
            ON p.party_cnt = fo.party_cnt
        LEFT JOIN insurance_file i
            ON i.insurance_file_cnt = fo.insurance_file_cnt
        WHERE (ISNULL(i.source_id, p.source_id) = @branch_id OR @branch_id IS NULL)
        AND fo.fsa_complaint_category_id = fc.fsa_complaint_category_id
        AND date_opened BETWEEN @start_date AND @end_date
        AND fb.description = 'Motor' AND p.party_type_id IN (SELECT party_type_id FROM party_type WHERE code='PC' )
    ) ,
    (
        SELECT ISNULL(SUM(1),0)
        FROM FSA_complaint_folder fo
        JOIN FSA_class_of_business fb
            ON fb.FSA_class_of_business_id = fo.FSA_class_of_business_id
        JOIN party p
            ON p.party_cnt = fo.party_cnt
        LEFT JOIN insurance_file i
            ON i.insurance_file_cnt = fo.insurance_file_cnt
        WHERE (ISNULL(i.source_id, p.source_id) = @branch_id OR @branch_id IS NULL)
        AND fo.fsa_complaint_category_id = fc.fsa_complaint_category_id
        AND date_opened BETWEEN @start_date AND @end_date
        AND fb.description = 'Property'  AND p.party_type_id IN (SELECT party_type_id FROM party_type WHERE code='PC' )
    ) ,
    (
        SELECT ISNULL(SUM(1),0)
        FROM FSA_complaint_folder fo
        JOIN FSA_class_of_business fb
            ON fb.FSA_class_of_business_id = fo.FSA_class_of_business_id
        JOIN party p
            ON p.party_cnt = fo.party_cnt
        LEFT JOIN insurance_file i
            ON i.insurance_file_cnt = fo.insurance_file_cnt
        WHERE (ISNULL(i.source_id, p.source_id) = @branch_id OR @branch_id IS NULL)
        AND fo.fsa_complaint_category_id = fc.fsa_complaint_category_id
        AND date_opened BETWEEN @start_date AND @end_date
        AND fb.description = 'Other'  AND p.party_type_id IN (SELECT party_type_id FROM party_type WHERE code='PC' )
    ) ,
    (	
	SELECT
	    ISNULL(SUM(1),0)
	FROM FSA_complaint_folder fo
	JOIN FSA_class_of_business fb
	    ON fb.FSA_class_of_business_id = fo.FSA_class_of_business_id
	JOIN party p
	    ON p.party_cnt = fo.party_cnt
	LEFT JOIN insurance_file i
	    ON i.insurance_file_cnt = fo.insurance_file_cnt
	WHERE (ISNULL(i.source_id, p.source_id) = @branch_id OR @branch_id IS NULL)
	AND date_opened < @start_date
	AND
	(
	    date_settled IS NULL
	    OR
	    date_settled >= @start_date
	)
	AND fb.description = 'Motor'    AND p.party_type_id IN (SELECT party_type_id FROM party_type WHERE code='PC' )
     ),
     (	
	SELECT
	     ISNULL(SUM(1),0)
	FROM FSA_complaint_folder fo
	JOIN FSA_class_of_business fb
	    ON fb.FSA_class_of_business_id = fo.FSA_class_of_business_id
	JOIN party p
	    ON p.party_cnt = fo.party_cnt
	LEFT JOIN insurance_file i
	    ON i.insurance_file_cnt = fo.insurance_file_cnt
	WHERE (ISNULL(i.source_id, p.source_id) = @branch_id OR @branch_id IS NULL)
	AND date_opened < @end_date
	AND
	(
	    date_settled IS NULL
	    OR
	    date_settled >= @end_date
	)
	AND fb.description = 'Motor'    AND p.party_type_id IN (SELECT party_type_id FROM party_type WHERE code='PC' )
      ),
/* Calculate Summaries - Property */
(
	SELECT
	     ISNULL(SUM(1),0)
	FROM FSA_complaint_folder fo
	JOIN FSA_class_of_business fb
	    ON fb.FSA_class_of_business_id = fo.FSA_class_of_business_id
	JOIN party p
	    ON p.party_cnt = fo.party_cnt
	LEFT JOIN insurance_file i
	    ON i.insurance_file_cnt = fo.insurance_file_cnt
	WHERE (ISNULL(i.source_id, p.source_id) = @branch_id OR @branch_id IS NULL)
	AND date_opened < @start_date
	AND
	(
	    date_settled IS NULL
	    OR
	    date_settled >= @start_date
	)
	AND fb.description = 'Property'   AND p.party_type_id IN (SELECT party_type_id FROM party_type WHERE code='PC' )
),

(	SELECT
	     ISNULL(SUM(1),0)
	FROM FSA_complaint_folder fo
	JOIN FSA_class_of_business fb
	    ON fb.FSA_class_of_business_id = fo.FSA_class_of_business_id
	JOIN party p
	    ON p.party_cnt = fo.party_cnt
	LEFT JOIN insurance_file i
	    ON i.insurance_file_cnt = fo.insurance_file_cnt
	WHERE (ISNULL(i.source_id, p.source_id) = @branch_id OR @branch_id IS NULL)
	AND date_opened < @end_date
	AND
	(
	    date_settled IS NULL
	    OR
	    date_settled >= @end_date
	)
	AND fb.description = 'Property'   AND p.party_type_id IN (SELECT party_type_id FROM party_type WHERE code='PC' )

/* Calculate Summaries - Other */
),
(
	SELECT
	     ISNULL(SUM(1),0)
	FROM FSA_complaint_folder fo
	JOIN FSA_class_of_business fb
	    ON fb.FSA_class_of_business_id = fo.FSA_class_of_business_id
	JOIN party p
	    ON p.party_cnt = fo.party_cnt
	LEFT JOIN insurance_file i
	    ON i.insurance_file_cnt = fo.insurance_file_cnt
	WHERE (ISNULL(i.source_id, p.source_id) = @branch_id OR @branch_id IS NULL)
	AND date_opened < @start_date
	AND
	(
	    date_settled IS NULL
	    OR
	    date_settled >= @start_date
	)
	AND fb.description = 'Other'   AND p.party_type_id IN (SELECT party_type_id FROM party_type WHERE code='PC' )
),

(	SELECT
	      ISNULL(SUM(1),0)
	FROM FSA_complaint_folder fo
	JOIN FSA_class_of_business fb
	    ON fb.FSA_class_of_business_id = fo.FSA_class_of_business_id
	JOIN party p
	    ON p.party_cnt = fo.party_cnt
	LEFT JOIN insurance_file i
	    ON i.insurance_file_cnt = fo.insurance_file_cnt
	WHERE (ISNULL(i.source_id, p.source_id) = @branch_id OR @branch_id IS NULL)
	AND date_opened < @end_date
	AND
	(
	    date_settled IS NULL
	    OR
	    date_settled >= @end_date
	)
	AND fb.description = 'Other'   AND p.party_type_id IN (SELECT party_type_id FROM party_type WHERE code='PC' )
),

/* Calculate Motor Values */
(

	SELECT
	     ISNULL(SUM(1),0)
	FROM FSA_complaint_folder fo
	JOIN FSA_class_of_business fb
	    ON fb.FSA_class_of_business_id = fo.FSA_class_of_business_id
	JOIN party p
	    ON p.party_cnt = fo.party_cnt
	LEFT JOIN insurance_file i
	    ON i.insurance_file_cnt = fo.insurance_file_cnt
	WHERE (ISNULL(i.source_id, p.source_id) = @branch_id OR @branch_id IS NULL)
	AND date_settled BETWEEN @start_date AND @end_date
	AND complaint_upheld = 1
	AND fb.description = 'Motor'   AND p.party_type_id IN (SELECT party_type_id FROM party_type WHERE code='PC' )
),
(
	SELECT
	     ISNULL(SUM(1),0)
	FROM FSA_complaint_folder fo
	JOIN FSA_class_of_business fb
	    ON fb.FSA_class_of_business_id = fo.FSA_class_of_business_id
	JOIN party p
	    ON p.party_cnt = fo.party_cnt
	LEFT JOIN insurance_file i
	    ON i.insurance_file_cnt = fo.insurance_file_cnt
	WHERE (ISNULL(i.source_id, p.source_id) = @branch_id OR @branch_id IS NULL)
	AND date_settled BETWEEN @start_date AND @end_date
	AND complaint_referred_to_fos = 1
	AND fb.description = 'Motor'   AND p.party_type_id IN (SELECT party_type_id FROM party_type WHERE code='PC' )
),
(
	SELECT
	     ISNULL(SUM(compensation_paid),0)
	FROM FSA_complaint_folder fo
	JOIN FSA_class_of_business fb
	    ON fb.FSA_class_of_business_id = fo.FSA_class_of_business_id
	JOIN party p
	    ON p.party_cnt = fo.party_cnt
	LEFT JOIN insurance_file i
	    ON i.insurance_file_cnt = fo.insurance_file_cnt
	WHERE (ISNULL(i.source_id, p.source_id) = @branch_id OR @branch_id IS NULL)
	AND date_settled BETWEEN @start_date AND @end_date
	AND fb.description = 'Motor'   AND p.party_type_id IN (SELECT party_type_id FROM party_type WHERE code='PC' )
),
/* Calculate Property Values */
(
	SELECT
	     ISNULL(SUM(1),0)
	FROM FSA_complaint_folder fo
	JOIN FSA_class_of_business fb
	    ON fb.FSA_class_of_business_id = fo.FSA_class_of_business_id
	JOIN party p
	    ON p.party_cnt = fo.party_cnt
	LEFT JOIN insurance_file i
	    ON i.insurance_file_cnt = fo.insurance_file_cnt
	WHERE (ISNULL(i.source_id, p.source_id) = @branch_id OR @branch_id IS NULL)
	AND date_settled BETWEEN @start_date AND @end_date
	AND complaint_upheld = 1
	AND fb.description = 'Property'  AND p.party_type_id IN (SELECT party_type_id FROM party_type WHERE code='PC' )
),
(
	SELECT
	     ISNULL(SUM(1),0)
	FROM FSA_complaint_folder fo
	JOIN FSA_class_of_business fb
	    ON fb.FSA_class_of_business_id = fo.FSA_class_of_business_id
	JOIN party p
	    ON p.party_cnt = fo.party_cnt
	LEFT JOIN insurance_file i
	    ON i.insurance_file_cnt = fo.insurance_file_cnt
	WHERE (ISNULL(i.source_id, p.source_id) = @branch_id OR @branch_id IS NULL)
	AND date_settled BETWEEN @start_date AND @end_date
	AND complaint_referred_to_fos = 1
	AND fb.description = 'Property'   AND p.party_type_id IN (SELECT party_type_id FROM party_type WHERE code='PC' )
),
(
	SELECT
	      ISNULL(SUM(compensation_paid),0)
	FROM FSA_complaint_folder fo
	JOIN FSA_class_of_business fb
	    ON fb.FSA_class_of_business_id = fo.FSA_class_of_business_id
	JOIN party p
	    ON p.party_cnt = fo.party_cnt
	LEFT JOIN insurance_file i
	    ON i.insurance_file_cnt = fo.insurance_file_cnt
	WHERE (ISNULL(i.source_id, p.source_id) = @branch_id OR @branch_id IS NULL)
	AND date_settled BETWEEN @start_date AND @end_date
	AND fb.description = 'Property'   AND p.party_type_id IN (SELECT party_type_id FROM party_type WHERE code='PC' )
),
/* Calculate Other Values */
(
	SELECT
	     ISNULL(SUM(1),0)
	FROM FSA_complaint_folder fo
	JOIN FSA_class_of_business fb
	    ON fb.FSA_class_of_business_id = fo.FSA_class_of_business_id
	JOIN party p
	    ON p.party_cnt = fo.party_cnt
	LEFT JOIN insurance_file i
	    ON i.insurance_file_cnt = fo.insurance_file_cnt
	WHERE (ISNULL(i.source_id, p.source_id) = @branch_id OR @branch_id IS NULL)
	AND date_settled BETWEEN @start_date AND @end_date
	AND complaint_upheld = 1
	AND fb.description = 'Other'   AND p.party_type_id IN (SELECT party_type_id FROM party_type WHERE code='PC' )
),
(
	SELECT
	     ISNULL(SUM(1),0)
	FROM FSA_complaint_folder fo
	JOIN FSA_class_of_business fb
	    ON fb.FSA_class_of_business_id = fo.FSA_class_of_business_id
	JOIN party p
	    ON p.party_cnt = fo.party_cnt
	LEFT JOIN insurance_file i
	    ON i.insurance_file_cnt = fo.insurance_file_cnt
	WHERE (ISNULL(i.source_id, p.source_id) = @branch_id OR @branch_id IS NULL)
	AND date_settled BETWEEN @start_date AND @end_date
	AND complaint_referred_to_fos = 1
	AND fb.description = 'Other'   AND p.party_type_id IN (SELECT party_type_id FROM party_type WHERE code='PC' )
),
(
	SELECT
	    ISNULL(SUM(compensation_paid),0)
	FROM FSA_complaint_folder fo
	JOIN FSA_class_of_business fb
	    ON fb.FSA_class_of_business_id = fo.FSA_class_of_business_id
	JOIN party p
	    ON p.party_cnt = fo.party_cnt
	LEFT JOIN insurance_file i
	    ON i.insurance_file_cnt = fo.insurance_file_cnt
	WHERE (ISNULL(i.source_id, p.source_id) = @branch_id OR @branch_id IS NULL)
	AND date_settled BETWEEN @start_date AND @end_date
	AND fb.description = 'Other'   AND p.party_type_id IN (SELECT party_type_id FROM party_type WHERE code='PC' )
),
/* Calculate Summaries by Business Type*/
(
	SELECT
	     ISNULL(SUM(1),0)
	FROM FSA_complaint_folder fo
	JOIN FSA_class_of_business fb
	    ON fb.FSA_class_of_business_id = fo.FSA_class_of_business_id
	JOIN party p
	    ON p.party_cnt = fo.party_cnt
	LEFT JOIN insurance_file i
	    ON i.insurance_file_cnt = fo.insurance_file_cnt
	WHERE (ISNULL(i.source_id, p.source_id) = @branch_id OR @branch_id IS NULL)
	AND date_settled BETWEEN @start_date AND @end_date
	AND DATEDIFF(dd,date_opened,date_settled) <= 28
	AND fb.description = 'Motor'   AND p.party_type_id IN (SELECT party_type_id FROM party_type WHERE code='PC' )
),
(
	SELECT
	     ISNULL(SUM(1),0)
	FROM FSA_complaint_folder fo
	JOIN FSA_class_of_business fb
	    ON fb.FSA_class_of_business_id = fo.FSA_class_of_business_id
	JOIN party p
	    ON p.party_cnt = fo.party_cnt
	LEFT JOIN insurance_file i
	    ON i.insurance_file_cnt = fo.insurance_file_cnt
	WHERE (ISNULL(i.source_id, p.source_id) = @branch_id OR @branch_id IS NULL)
	AND date_settled BETWEEN @start_date AND @end_date
	AND DATEDIFF(dd,date_opened,date_settled) <= 28
	AND fb.description = 'Property'   AND p.party_type_id IN (SELECT party_type_id FROM party_type WHERE code='PC' )
),
(
	SELECT
	     ISNULL(SUM(1),0)
	FROM FSA_complaint_folder fo
	JOIN FSA_class_of_business fb
	    ON fb.FSA_class_of_business_id = fo.FSA_class_of_business_id
	JOIN party p
	    ON p.party_cnt = fo.party_cnt
	LEFT JOIN insurance_file i
	    ON i.insurance_file_cnt = fo.insurance_file_cnt
	WHERE (ISNULL(i.source_id, p.source_id) = @branch_id OR @branch_id IS NULL)
	AND date_settled BETWEEN @start_date AND @end_date
	AND DATEDIFF(dd,date_opened,date_settled) <= 28
	AND fb.description = 'Other'   AND p.party_type_id IN (SELECT party_type_id FROM party_type WHERE code='PC' )
),
(
	SELECT
	     ISNULL(SUM(1),0)
	FROM FSA_complaint_folder fo
	JOIN FSA_class_of_business fb
	    ON fb.FSA_class_of_business_id = fo.FSA_class_of_business_id
	JOIN party p
	    ON p.party_cnt = fo.party_cnt
	LEFT JOIN insurance_file i
	    ON i.insurance_file_cnt = fo.insurance_file_cnt
	WHERE (ISNULL(i.source_id, p.source_id) = @branch_id OR @branch_id IS NULL)
	AND date_settled BETWEEN @start_date AND @end_date
	AND DATEDIFF(dd,date_opened,date_settled) > 28
	AND DATEDIFF(dd,date_opened,date_settled) <= 56
	AND fb.description = 'Motor'   AND p.party_type_id IN (SELECT party_type_id FROM party_type WHERE code='PC' )
),
(
	SELECT
	     ISNULL(SUM(1),0)
	FROM FSA_complaint_folder fo
	JOIN FSA_class_of_business fb
	    ON fb.FSA_class_of_business_id = fo.FSA_class_of_business_id
	JOIN party p
	    ON p.party_cnt = fo.party_cnt
	LEFT JOIN insurance_file i
	    ON i.insurance_file_cnt = fo.insurance_file_cnt
	WHERE (ISNULL(i.source_id, p.source_id) = @branch_id OR @branch_id IS NULL)
	AND date_settled BETWEEN @start_date AND @end_date
	AND DATEDIFF(dd,date_opened,date_settled) > 28
	AND DATEDIFF(dd,date_opened,date_settled) <= 56
	AND fb.description = 'Property'   AND p.party_type_id IN (SELECT party_type_id FROM party_type WHERE code='PC' )
),
(
	SELECT
	      ISNULL(SUM(1),0)
	FROM FSA_complaint_folder fo
	JOIN FSA_class_of_business fb
	    ON fb.FSA_class_of_business_id = fo.FSA_class_of_business_id
	JOIN party p
	    ON p.party_cnt = fo.party_cnt
	LEFT JOIN insurance_file i
	    ON i.insurance_file_cnt = fo.insurance_file_cnt
	WHERE (ISNULL(i.source_id, p.source_id) = @branch_id OR @branch_id IS NULL)
	AND date_settled BETWEEN @start_date AND @end_date
	AND DATEDIFF(dd,date_opened,date_settled) > 28
	AND DATEDIFF(dd,date_opened,date_settled) <= 56
	AND fb.description = 'Other'   AND p.party_type_id IN (SELECT party_type_id FROM party_type WHERE code='PC' )
),
(
	SELECT
	      ISNULL(SUM(1),0)
	FROM FSA_complaint_folder fo
	JOIN FSA_class_of_business fb
	    ON fb.FSA_class_of_business_id = fo.FSA_class_of_business_id
	JOIN party p
	    ON p.party_cnt = fo.party_cnt
	LEFT JOIN insurance_file i
	    ON i.insurance_file_cnt = fo.insurance_file_cnt
	WHERE (ISNULL(i.source_id, p.source_id) = @branch_id OR @branch_id IS NULL)
	AND date_settled BETWEEN @start_date AND @end_date
	AND DATEDIFF(dd,date_opened,date_settled) > 56
	AND fb.description = 'Motor'  AND p.party_type_id IN (SELECT party_type_id FROM party_type WHERE code='PC' )
),
(	
	SELECT
	      ISNULL(SUM(1),0)
	FROM FSA_complaint_folder fo
	JOIN FSA_class_of_business fb
	    ON fb.FSA_class_of_business_id = fo.FSA_class_of_business_id
	JOIN party p
	    ON p.party_cnt = fo.party_cnt
	LEFT JOIN insurance_file i
	    ON i.insurance_file_cnt = fo.insurance_file_cnt
	WHERE (ISNULL(i.source_id, p.source_id) = @branch_id OR @branch_id IS NULL)
	AND date_settled BETWEEN @start_date AND @end_date
	AND DATEDIFF(dd,date_opened,date_settled) > 56
	AND fb.description = 'Property'   AND p.party_type_id IN (SELECT party_type_id FROM party_type WHERE code='PC' )
),
(
	SELECT
	      ISNULL(SUM(1),0)
	FROM FSA_complaint_folder fo
	JOIN FSA_class_of_business fb
	    ON fb.FSA_class_of_business_id = fo.FSA_class_of_business_id
	JOIN party p
	    ON p.party_cnt = fo.party_cnt
	LEFT JOIN insurance_file i
	    ON i.insurance_file_cnt = fo.insurance_file_cnt
	WHERE (ISNULL(i.source_id, p.source_id) = @branch_id OR @branch_id IS NULL)
	AND date_settled BETWEEN @start_date AND @end_date
	AND DATEDIFF(dd,date_opened,date_settled) > 56
	AND fb.description = 'Other'  AND p.party_type_id IN (SELECT party_type_id FROM party_type WHERE code='PC' )
),
  
@FSADisabled 
FROM FSA_complaint_category fc 
GROUP BY
    fc.FSA_complaint_category_id,
    fc.description
--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
--Inserting Corporatie/Group Client Complaints
--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
 DECLARE turn_Over CURSOR FAST_FORWARD FOR
       SELECT
	turnoverband_id,description
	FROM turnoverband where is_deleted =0
OPEN turn_Over

	select @turnoverband_id=0
	select @description='(Not Known)'

 WHILE @@FETCH_STATUS = 0
BEGIN

	insert into #ComplaintsTemp
	 Select
	   'Commercial',
	   @turnoverband_id,
	   @description,
	    (
	        SELECT
	            description
	        FROM source
	        WHERE source_id = 1
	    ) 'Branch Name', /*Meant to be company name*/
	    fc.fsa_complaint_category_id 'Complaint Category Id',
	    fc.description 'Complaint Category',
	    (
	        SELECT ISNULL(SUM(1),0)
	        FROM FSA_complaint_folder fo
	        JOIN FSA_class_of_business fb
	            ON fb.FSA_class_of_business_id = fo.FSA_class_of_business_id
	        JOIN party p
	            ON p.party_cnt = fo.party_cnt
	        LEFT JOIN insurance_file i
	            ON i.insurance_file_cnt = fo.insurance_file_cnt
	        LEFT JOIN party_corporate_client cc
	            ON p.party_cnt = CC.party_cnt
	        LEFT JOIN party_group_client gc
	             ON p.party_cnt = gc.party_cnt
	        WHERE (ISNULL(i.source_id, p.source_id) = @branch_id OR @branch_id IS NULL)
	        AND fo.fsa_complaint_category_id = fc.fsa_complaint_category_id
	        AND date_opened BETWEEN @start_date AND @end_date
	        AND fb.description = 'Motor' AND p.party_type_id IN (SELECT party_type_id FROM party_type WHERE code='CC'  OR code='GC' )
	       AND ( isnull(cc.turnover,0)=@turnoverband_id  OR  isnull(gc.turnover,0)=@turnoverband_id)
	    ) ,
	    (
	        SELECT ISNULL(SUM(1),0)
	        FROM FSA_complaint_folder fo
	        JOIN FSA_class_of_business fb
	            ON fb.FSA_class_of_business_id = fo.FSA_class_of_business_id
	        JOIN party p
	            ON p.party_cnt = fo.party_cnt
	        LEFT JOIN insurance_file i
	            ON i.insurance_file_cnt = fo.insurance_file_cnt
	        LEFT JOIN party_corporate_client cc
	            ON p.party_cnt = CC.party_cnt
	        LEFT JOIN party_group_client gc
	             ON p.party_cnt = gc.party_cnt
	        WHERE (ISNULL(i.source_id, p.source_id) = @branch_id OR @branch_id IS NULL)
	        AND fo.fsa_complaint_category_id = fc.fsa_complaint_category_id
	        AND date_opened BETWEEN @start_date AND @end_date
	        AND fb.description = 'Property'  AND p.party_type_id IN (SELECT party_type_id FROM party_type WHERE code='CC' OR code='GC' )
	        AND ( isnull(cc.turnover,0)=@turnoverband_id  OR  isnull(gc.turnover,0)=@turnoverband_id)
	    ) ,
	    (
	        SELECT ISNULL(SUM(1),0)
	        FROM FSA_complaint_folder fo
	        JOIN FSA_class_of_business fb
	            ON fb.FSA_class_of_business_id = fo.FSA_class_of_business_id
	        JOIN party p
	            ON p.party_cnt = fo.party_cnt
	        LEFT JOIN insurance_file i
	            ON i.insurance_file_cnt = fo.insurance_file_cnt
	        LEFT JOIN party_corporate_client cc
	            ON p.party_cnt = CC.party_cnt
	        LEFT JOIN party_group_client gc
	             ON p.party_cnt = gc.party_cnt
	        WHERE (ISNULL(i.source_id, p.source_id) = @branch_id OR @branch_id IS NULL)
	        AND fo.fsa_complaint_category_id = fc.fsa_complaint_category_id
	        AND date_opened BETWEEN @start_date AND @end_date
	        AND fb.description = 'Other'  AND p.party_type_id IN (SELECT party_type_id FROM party_type WHERE code='CC'  OR code='GC')
	         AND ( isnull(cc.turnover,0)=@turnoverband_id  OR  isnull(gc.turnover,0)=@turnoverband_id)
	    ) ,
	    (	
		SELECT
		    ISNULL(SUM(1),0)
		FROM FSA_complaint_folder fo
		JOIN FSA_class_of_business fb
		    ON fb.FSA_class_of_business_id = fo.FSA_class_of_business_id
		JOIN party p
		    ON p.party_cnt = fo.party_cnt
		LEFT JOIN insurance_file i
		    ON i.insurance_file_cnt = fo.insurance_file_cnt
	       	 LEFT JOIN party_corporate_client cc
	           	    ON p.party_cnt = CC.party_cnt
	        	LEFT JOIN party_group_client gc
	             	   ON p.party_cnt = gc.party_cnt
		WHERE (ISNULL(i.source_id, p.source_id) = @branch_id OR @branch_id IS NULL)
		AND date_opened < @start_date
		AND
		(
		    date_settled IS NULL
		    OR
		    date_settled >= @start_date
		)
		AND fb.description = 'Motor'    AND p.party_type_id IN (SELECT party_type_id FROM party_type WHERE code='CC' OR code='GC' )
		AND ( isnull(cc.turnover,0)=@turnoverband_id  OR  isnull(gc.turnover,0)=@turnoverband_id)
	     ),
	     (	
		SELECT
		ISNULL(SUM(1),0)
		FROM FSA_complaint_folder fo
		JOIN FSA_class_of_business fb
		    ON fb.FSA_class_of_business_id = fo.FSA_class_of_business_id
		JOIN party p
		    ON p.party_cnt = fo.party_cnt
		LEFT JOIN insurance_file i
		    ON i.insurance_file_cnt = fo.insurance_file_cnt
	       	 LEFT JOIN party_corporate_client cc
	                 ON p.party_cnt = CC.party_cnt
	        	LEFT JOIN party_group_client gc
	                ON p.party_cnt = gc.party_cnt
		WHERE (ISNULL(i.source_id, p.source_id) = @branch_id OR @branch_id IS NULL)
		AND date_opened < @end_date
		AND
		(
		    date_settled IS NULL
		    OR
		    date_settled >= @end_date
		)
		AND fb.description = 'Motor'    AND p.party_type_id IN (SELECT party_type_id FROM party_type WHERE code='CC' OR code='GC' )
		 AND ( isnull(cc.turnover,0)=@turnoverband_id  OR  isnull(gc.turnover,0)=@turnoverband_id)
	      ),
	/* Calculate Summaries - Property */
	(
		SELECT
		     ISNULL(SUM(1),0)
		FROM FSA_complaint_folder fo
		JOIN FSA_class_of_business fb
		    ON fb.FSA_class_of_business_id = fo.FSA_class_of_business_id
		JOIN party p
		    ON p.party_cnt = fo.party_cnt
		LEFT JOIN insurance_file i
		    ON i.insurance_file_cnt = fo.insurance_file_cnt
	       	 LEFT JOIN party_corporate_client cc
	                 ON p.party_cnt = CC.party_cnt
 		LEFT JOIN party_group_client gc
	                ON p.party_cnt = gc.party_cnt
		WHERE (ISNULL(i.source_id, p.source_id) = @branch_id OR @branch_id IS NULL)
		AND date_opened < @start_date
		AND
		(
		    date_settled IS NULL
		    OR
		    date_settled >= @start_date
		)
		AND fb.description = 'Property'   AND p.party_type_id IN (SELECT party_type_id FROM party_type WHERE code='CC' OR code='GC' )
		AND ( isnull(cc.turnover,0)=@turnoverband_id  OR  isnull(gc.turnover,0)=@turnoverband_id)
	),
	
	(	SELECT
		  ISNULL(SUM(1),0)
		FROM FSA_complaint_folder fo
		JOIN FSA_class_of_business fb
		    ON fb.FSA_class_of_business_id = fo.FSA_class_of_business_id
		JOIN party p
		    ON p.party_cnt = fo.party_cnt
		LEFT JOIN insurance_file i
		    ON i.insurance_file_cnt = fo.insurance_file_cnt
	       	 LEFT JOIN party_corporate_client cc
	                 ON p.party_cnt = CC.party_cnt
	       	 LEFT JOIN party_group_client gc
	                ON p.party_cnt = gc.party_cnt
		WHERE (ISNULL(i.source_id, p.source_id) = @branch_id OR @branch_id IS NULL)
		AND date_opened < @end_date
		AND
		(
		    date_settled IS NULL
		    OR
		    date_settled >= @end_date
		)
		AND fb.description = 'Property'   AND p.party_type_id IN (SELECT party_type_id FROM party_type WHERE code='CC' OR code='GC' )
		AND ( isnull(cc.turnover,0)=@turnoverband_id  OR  isnull(gc.turnover,0)=@turnoverband_id)
	/* Calculate Summaries - Other */
	),
	(
		SELECT
		     ISNULL(SUM(1),0)
		FROM FSA_complaint_folder fo
		JOIN FSA_class_of_business fb
		    ON fb.FSA_class_of_business_id = fo.FSA_class_of_business_id
		JOIN party p
		    ON p.party_cnt = fo.party_cnt
		LEFT JOIN insurance_file i
		    ON i.insurance_file_cnt = fo.insurance_file_cnt
	       	 LEFT JOIN party_corporate_client cc
	                 ON p.party_cnt = CC.party_cnt
	              LEFT JOIN party_group_client gc
	                ON p.party_cnt = gc.party_cnt
		WHERE (ISNULL(i.source_id, p.source_id) = @branch_id OR @branch_id IS NULL)
		AND date_opened < @start_date
		AND
		(
		    date_settled IS NULL
		    OR
		    date_settled >= @start_date
		)
		AND fb.description = 'Other'   AND p.party_type_id IN (SELECT party_type_id FROM party_type WHERE code='CC'  OR code='GC')
		AND ( isnull(cc.turnover,0)=@turnoverband_id  OR  isnull(gc.turnover,0)=@turnoverband_id)
	),
	
	(	SELECT
		    ISNULL(SUM(1),0)
		FROM FSA_complaint_folder fo
		JOIN FSA_class_of_business fb
		    ON fb.FSA_class_of_business_id = fo.FSA_class_of_business_id
		JOIN party p
		    ON p.party_cnt = fo.party_cnt
		LEFT JOIN insurance_file i
		    ON i.insurance_file_cnt = fo.insurance_file_cnt
	       	 LEFT JOIN party_corporate_client cc
	                 ON p.party_cnt = CC.party_cnt
	              LEFT JOIN party_group_client gc
	                 ON p.party_cnt = gc.party_cnt
		WHERE (ISNULL(i.source_id, p.source_id) = @branch_id OR @branch_id IS NULL)
		AND date_opened < @end_date
		AND
		(
		    date_settled IS NULL
		    OR
		    date_settled >= @end_date
		)
		AND fb.description = 'Other'   AND p.party_type_id IN (SELECT party_type_id FROM party_type WHERE code='CC' OR code='GC' )
		AND ( isnull(cc.turnover,0)=@turnoverband_id  OR  isnull(gc.turnover,0)=@turnoverband_id)
		
	),          
	
	/* Calculate Motor Values */
	(
		SELECT
		     ISNULL(SUM(1),0)
		FROM FSA_complaint_folder fo
		JOIN FSA_class_of_business fb
		    ON fb.FSA_class_of_business_id = fo.FSA_class_of_business_id
		JOIN party p
		    ON p.party_cnt = fo.party_cnt
		LEFT JOIN insurance_file i
		    ON i.insurance_file_cnt = fo.insurance_file_cnt
	       	 LEFT JOIN party_corporate_client cc
	                 ON p.party_cnt = CC.party_cnt
	        	LEFT JOIN party_group_client gc
	                ON p.party_cnt = gc.party_cnt
		WHERE (ISNULL(i.source_id, p.source_id) = @branch_id OR @branch_id IS NULL)
		AND date_settled BETWEEN @start_date AND @end_date
		AND complaint_upheld = 1
		AND fb.description = 'Motor'   AND p.party_type_id IN (SELECT party_type_id FROM party_type WHERE code='CC'  OR code='GC')
		AND ( isnull(cc.turnover,0)=@turnoverband_id  OR  isnull(gc.turnover,0)=@turnoverband_id)
	),
	(
		SELECT
		     ISNULL(SUM(1),0)

		FROM FSA_complaint_folder fo
		JOIN FSA_class_of_business fb
		    ON fb.FSA_class_of_business_id = fo.FSA_class_of_business_id
		JOIN party p
		    ON p.party_cnt = fo.party_cnt
		LEFT JOIN insurance_file i
		    ON i.insurance_file_cnt = fo.insurance_file_cnt
	       	 LEFT JOIN party_corporate_client cc
	                 ON p.party_cnt = CC.party_cnt
		LEFT JOIN party_group_client gc
	             	    ON p.party_cnt = gc.party_cnt
		WHERE (ISNULL(i.source_id, p.source_id) = @branch_id OR @branch_id IS NULL)
		AND date_settled BETWEEN @start_date AND @end_date
		AND complaint_referred_to_fos = 1
		AND fb.description = 'Motor'   AND p.party_type_id IN (SELECT party_type_id FROM party_type WHERE code='CC' OR code='GC' )
		AND ( isnull(cc.turnover,0)=@turnoverband_id  OR  isnull(gc.turnover,0)=@turnoverband_id)
	),
	(
		SELECT
		     ISNULL(SUM(compensation_paid),0)
		FROM FSA_complaint_folder fo
		JOIN FSA_class_of_business fb
		    ON fb.FSA_class_of_business_id = fo.FSA_class_of_business_id
		JOIN party p
		    ON p.party_cnt = fo.party_cnt
		LEFT JOIN insurance_file i
		    ON i.insurance_file_cnt = fo.insurance_file_cnt
	       	 LEFT JOIN party_corporate_client cc
	                 ON p.party_cnt = CC.party_cnt
	        	LEFT JOIN party_group_client gc
	                ON p.party_cnt = gc.party_cnt
		WHERE (ISNULL(i.source_id, p.source_id) = @branch_id OR @branch_id IS NULL)
		AND date_settled BETWEEN @start_date AND @end_date
		AND fb.description = 'Motor'   AND p.party_type_id IN (SELECT party_type_id FROM party_type WHERE code='CC' OR code='GC' )
		AND ( isnull(cc.turnover,0)=@turnoverband_id  OR  isnull(gc.turnover,0)=@turnoverband_id)
	),
	/* Calculate Property Values */
	(
		SELECT
		     ISNULL(SUM(1),0)
		FROM FSA_complaint_folder fo
		JOIN FSA_class_of_business fb
		    ON fb.FSA_class_of_business_id = fo.FSA_class_of_business_id
		JOIN party p
		    ON p.party_cnt = fo.party_cnt
		LEFT JOIN insurance_file i
		    ON i.insurance_file_cnt = fo.insurance_file_cnt
	       	 LEFT JOIN party_corporate_client cc
	                 ON p.party_cnt = CC.party_cnt
	              LEFT JOIN party_group_client gc
	                ON p.party_cnt = gc.party_cnt
		WHERE (ISNULL(i.source_id, p.source_id) = @branch_id OR @branch_id IS NULL)
		AND date_settled BETWEEN @start_date AND @end_date
		AND complaint_upheld = 1
		AND fb.description = 'Property'  AND p.party_type_id IN (SELECT party_type_id FROM party_type WHERE code='CC'  OR code='GC')
		AND ( isnull(cc.turnover,0)=@turnoverband_id  OR  isnull(gc.turnover,0)=@turnoverband_id)
	),
	(
		SELECT
		     ISNULL(SUM(1),0)
		FROM FSA_complaint_folder fo
		JOIN FSA_class_of_business fb
		    ON fb.FSA_class_of_business_id = fo.FSA_class_of_business_id
		JOIN party p
		    ON p.party_cnt = fo.party_cnt
		LEFT JOIN insurance_file i
		    ON i.insurance_file_cnt = fo.insurance_file_cnt
	       	 LEFT JOIN party_corporate_client cc
	                 ON p.party_cnt = CC.party_cnt
	              LEFT JOIN party_group_client gc
	             	   ON p.party_cnt = gc.party_cnt
		WHERE (ISNULL(i.source_id, p.source_id) = @branch_id OR @branch_id IS NULL)
		AND date_settled BETWEEN @start_date AND @end_date
		AND complaint_referred_to_fos = 1
		AND fb.description = 'Property'   AND p.party_type_id IN (SELECT party_type_id FROM party_type WHERE code='CC' OR code='GC' )
		AND ( isnull(cc.turnover,0)=@turnoverband_id  OR  isnull(gc.turnover,0)=@turnoverband_id)
	),
	(
		SELECT
		      ISNULL(SUM(compensation_paid),0)
		FROM FSA_complaint_folder fo
		JOIN FSA_class_of_business fb
		    ON fb.FSA_class_of_business_id = fo.FSA_class_of_business_id
		JOIN party p
		    ON p.party_cnt = fo.party_cnt
		LEFT JOIN insurance_file i
		    ON i.insurance_file_cnt = fo.insurance_file_cnt
	       	 LEFT JOIN party_corporate_client cc
	                 ON p.party_cnt = CC.party_cnt
	              LEFT JOIN party_group_client gc
	                 ON p.party_cnt = gc.party_cnt
		WHERE (ISNULL(i.source_id, p.source_id) = @branch_id OR @branch_id IS NULL)
		AND date_settled BETWEEN @start_date AND @end_date
		AND fb.description = 'Property'   AND p.party_type_id IN (SELECT party_type_id FROM party_type WHERE code='CC' OR code='GC' )
		AND ( isnull(cc.turnover,0)=@turnoverband_id  OR  isnull(gc.turnover,0)=@turnoverband_id)
	),
	/* Calculate Other Values */
	(
		SELECT
		     ISNULL(SUM(1),0)
		FROM FSA_complaint_folder fo
		JOIN FSA_class_of_business fb
		    ON fb.FSA_class_of_business_id = fo.FSA_class_of_business_id
		JOIN party p
		    ON p.party_cnt = fo.party_cnt
		LEFT JOIN insurance_file i
		    ON i.insurance_file_cnt = fo.insurance_file_cnt

	       	 LEFT JOIN party_corporate_client cc
	                 ON p.party_cnt = CC.party_cnt
	              LEFT JOIN party_group_client gc
	                ON p.party_cnt = gc.party_cnt
		WHERE (ISNULL(i.source_id, p.source_id) = @branch_id OR @branch_id IS NULL)
		AND date_settled BETWEEN @start_date AND @end_date
		AND complaint_upheld = 1
		AND fb.description = 'Other'   AND p.party_type_id IN (SELECT party_type_id FROM party_type WHERE code='CC' OR code='GC' )
		AND ( isnull(cc.turnover,0)=@turnoverband_id  OR  isnull(gc.turnover,0)=@turnoverband_id)
	),
	(
		SELECT
		     ISNULL(SUM(1),0)
		FROM FSA_complaint_folder fo
		JOIN FSA_class_of_business fb
		    ON fb.FSA_class_of_business_id = fo.FSA_class_of_business_id
		JOIN party p
		    ON p.party_cnt = fo.party_cnt
		LEFT JOIN insurance_file i
		    ON i.insurance_file_cnt = fo.insurance_file_cnt
	       	 LEFT JOIN party_corporate_client cc
	                 ON p.party_cnt = CC.party_cnt
	       	 LEFT JOIN party_group_client gc
	                 ON p.party_cnt = gc.party_cnt
		WHERE (ISNULL(i.source_id, p.source_id) = @branch_id OR @branch_id IS NULL)
		AND date_settled BETWEEN @start_date AND @end_date
		AND complaint_referred_to_fos = 1
		AND fb.description = 'Other'   AND p.party_type_id IN (SELECT party_type_id FROM party_type WHERE code='CC'  OR code='GC')
		AND ( isnull(cc.turnover,0)=@turnoverband_id  OR  isnull(gc.turnover,0)=@turnoverband_id)
	),
	(
		SELECT
		    ISNULL(SUM(compensation_paid),0)
		FROM FSA_complaint_folder fo
		JOIN FSA_class_of_business fb
		    ON fb.FSA_class_of_business_id = fo.FSA_class_of_business_id
		JOIN party p
		    ON p.party_cnt = fo.party_cnt
		LEFT JOIN insurance_file i
		    ON i.insurance_file_cnt = fo.insurance_file_cnt
	       	 LEFT JOIN party_corporate_client cc
	                 ON p.party_cnt = CC.party_cnt
	       	 LEFT JOIN party_group_client gc
	                 ON p.party_cnt = gc.party_cnt
		WHERE (ISNULL(i.source_id, p.source_id) = @branch_id OR @branch_id IS NULL)
		AND date_settled BETWEEN @start_date AND @end_date
		AND fb.description = 'Other'   AND p.party_type_id IN (SELECT party_type_id FROM party_type WHERE code='CC' OR code='GC' )
		AND ( isnull(cc.turnover,0)=@turnoverband_id  OR  isnull(gc.turnover,0)=@turnoverband_id)
	),
	/* Calculate Summaries by Business Type*/
	(
		SELECT
		     ISNULL(SUM(1),0)
		FROM FSA_complaint_folder fo
		JOIN FSA_class_of_business fb
		    ON fb.FSA_class_of_business_id = fo.FSA_class_of_business_id
		JOIN party p
		    ON p.party_cnt = fo.party_cnt
		LEFT JOIN insurance_file i
		    ON i.insurance_file_cnt = fo.insurance_file_cnt
	       	 LEFT JOIN party_corporate_client cc
	                 ON p.party_cnt = CC.party_cnt
	       	 LEFT JOIN party_group_client gc
	                 ON p.party_cnt = gc.party_cnt
		WHERE (ISNULL(i.source_id, p.source_id) = @branch_id OR @branch_id IS NULL)
		AND date_settled BETWEEN @start_date AND @end_date
		AND DATEDIFF(dd,date_opened,date_settled) <= 28
		AND fb.description = 'Motor'   AND p.party_type_id IN (SELECT party_type_id FROM party_type WHERE code='CC' OR code='GC' )
		AND ( isnull(cc.turnover,0)=@turnoverband_id  OR  isnull(gc.turnover,0)=@turnoverband_id)
	),
	(
		SELECT
		     ISNULL(SUM(1),0)
		FROM FSA_complaint_folder fo
		JOIN FSA_class_of_business fb
		    ON fb.FSA_class_of_business_id = fo.FSA_class_of_business_id
		JOIN party p
		    ON p.party_cnt = fo.party_cnt
		LEFT JOIN insurance_file i
		    ON i.insurance_file_cnt = fo.insurance_file_cnt
	       	 LEFT JOIN party_corporate_client cc
	                 ON p.party_cnt = CC.party_cnt
	       	 LEFT JOIN party_group_client gc
	                 ON p.party_cnt = gc.party_cnt
		WHERE (ISNULL(i.source_id, p.source_id) = @branch_id OR @branch_id IS NULL)
		AND date_settled BETWEEN @start_date AND @end_date
		AND DATEDIFF(dd,date_opened,date_settled) <= 28
		AND fb.description = 'Property'   AND p.party_type_id IN (SELECT party_type_id FROM party_type WHERE code='CC' OR code='GC' )
		AND ( isnull(cc.turnover,0)=@turnoverband_id  OR  isnull(gc.turnover,0)=@turnoverband_id)
	),
	(
		SELECT
		     ISNULL(SUM(1),0)
		FROM FSA_complaint_folder fo
		JOIN FSA_class_of_business fb
		    ON fb.FSA_class_of_business_id = fo.FSA_class_of_business_id
		JOIN party p
		    ON p.party_cnt = fo.party_cnt
		LEFT JOIN insurance_file i
		    ON i.insurance_file_cnt = fo.insurance_file_cnt
	       	 LEFT JOIN party_corporate_client cc
	                 ON p.party_cnt = CC.party_cnt
	       	 LEFT JOIN party_group_client gc
	                 ON p.party_cnt = gc.party_cnt
		WHERE (ISNULL(i.source_id, p.source_id) = @branch_id OR @branch_id IS NULL)
		AND date_settled BETWEEN @start_date AND @end_date
		AND DATEDIFF(dd,date_opened,date_settled) <= 28
		AND fb.description = 'Other'   AND p.party_type_id IN (SELECT party_type_id FROM party_type WHERE code='CC' OR code='GC' )
		AND ( isnull(cc.turnover,0)=@turnoverband_id  OR  isnull(gc.turnover,0)=@turnoverband_id)
	),
	(
		SELECT
		     ISNULL(SUM(1),0)
		FROM FSA_complaint_folder fo
		JOIN FSA_class_of_business fb
		    ON fb.FSA_class_of_business_id = fo.FSA_class_of_business_id
		JOIN party p
		    ON p.party_cnt = fo.party_cnt
		LEFT JOIN insurance_file i
		    ON i.insurance_file_cnt = fo.insurance_file_cnt
	       	 LEFT JOIN party_corporate_client cc
	                 ON p.party_cnt = CC.party_cnt
	       	 LEFT JOIN party_group_client gc
	                 ON p.party_cnt = gc.party_cnt
		WHERE (ISNULL(i.source_id, p.source_id) = @branch_id OR @branch_id IS NULL)
		AND date_settled BETWEEN @start_date AND @end_date
		AND DATEDIFF(dd,date_opened,date_settled) > 28
		AND DATEDIFF(dd,date_opened,date_settled) <= 56
		AND fb.description = 'Motor'   AND p.party_type_id IN (SELECT party_type_id FROM party_type WHERE code='CC' OR code='GC' )
		AND ( isnull(cc.turnover,0)=@turnoverband_id  OR  isnull(gc.turnover,0)=@turnoverband_id)
	),
	(
		SELECT
		     ISNULL(SUM(1),0)
		FROM FSA_complaint_folder fo
		JOIN FSA_class_of_business fb
		    ON fb.FSA_class_of_business_id = fo.FSA_class_of_business_id
		JOIN party p
		    ON p.party_cnt = fo.party_cnt
		LEFT JOIN insurance_file i
		    ON i.insurance_file_cnt = fo.insurance_file_cnt
	       	 LEFT JOIN party_corporate_client cc
	                 ON p.party_cnt = CC.party_cnt
	       	 LEFT JOIN party_group_client gc
	                 ON p.party_cnt = gc.party_cnt
		WHERE (ISNULL(i.source_id, p.source_id) = @branch_id OR @branch_id IS NULL)
		AND date_settled BETWEEN @start_date AND @end_date
		AND DATEDIFF(dd,date_opened,date_settled) > 28
		AND DATEDIFF(dd,date_opened,date_settled) <= 56
		AND fb.description = 'Property'   AND p.party_type_id IN (SELECT party_type_id FROM party_type WHERE code='CC'  OR code='GC')
		AND ( isnull(cc.turnover,0)=@turnoverband_id  OR  isnull(gc.turnover,0)=@turnoverband_id)
	),
	(
		SELECT
		      ISNULL(SUM(1),0)
		FROM FSA_complaint_folder fo
		JOIN FSA_class_of_business fb
		    ON fb.FSA_class_of_business_id = fo.FSA_class_of_business_id
		JOIN party p
		    ON p.party_cnt = fo.party_cnt
		LEFT JOIN insurance_file i
		    ON i.insurance_file_cnt = fo.insurance_file_cnt
	       	 LEFT JOIN party_corporate_client cc
	                 ON p.party_cnt = CC.party_cnt
	       	 LEFT JOIN party_group_client gc
	                 ON p.party_cnt = gc.party_cnt
		WHERE (ISNULL(i.source_id, p.source_id) = @branch_id OR @branch_id IS NULL)
		AND date_settled BETWEEN @start_date AND @end_date
		AND DATEDIFF(dd,date_opened,date_settled) > 28
		AND DATEDIFF(dd,date_opened,date_settled) <= 56
		AND fb.description = 'Other'   AND p.party_type_id IN (SELECT party_type_id FROM party_type WHERE code='CC'  OR code='GC')
		AND ( isnull(cc.turnover,0)=@turnoverband_id  OR  isnull(gc.turnover,0)=@turnoverband_id)
	),
	(
		SELECT
		      ISNULL(SUM(1),0)
		FROM FSA_complaint_folder fo
		JOIN FSA_class_of_business fb
		    ON fb.FSA_class_of_business_id = fo.FSA_class_of_business_id
		JOIN party p
		    ON p.party_cnt = fo.party_cnt
		LEFT JOIN insurance_file i
		    ON i.insurance_file_cnt = fo.insurance_file_cnt
	       	 LEFT JOIN party_corporate_client cc
	                 ON p.party_cnt = CC.party_cnt
	       	 LEFT JOIN party_group_client gc
	                 ON p.party_cnt = gc.party_cnt
		WHERE (ISNULL(i.source_id, p.source_id) = @branch_id OR @branch_id IS NULL)
		AND date_settled BETWEEN @start_date AND @end_date
		AND DATEDIFF(dd,date_opened,date_settled) > 56
		AND fb.description = 'Motor'  AND p.party_type_id IN (SELECT party_type_id FROM party_type WHERE code='CC' OR code='GC' )
		AND ( isnull(cc.turnover,0)=@turnoverband_id  OR  isnull(gc.turnover,0)=@turnoverband_id)
	),
	(	
		SELECT
		      ISNULL(SUM(1),0)
		FROM FSA_complaint_folder fo
		JOIN FSA_class_of_business fb
		    ON fb.FSA_class_of_business_id = fo.FSA_class_of_business_id
		JOIN party p
		    ON p.party_cnt = fo.party_cnt
		LEFT JOIN insurance_file i
		    ON i.insurance_file_cnt = fo.insurance_file_cnt
	       	 LEFT JOIN party_corporate_client cc
	                 ON p.party_cnt = CC.party_cnt
	       	 LEFT JOIN party_group_client gc
	                 ON p.party_cnt = gc.party_cnt
		WHERE (ISNULL(i.source_id, p.source_id) = @branch_id OR @branch_id IS NULL)
		AND date_settled BETWEEN @start_date AND @end_date
		AND DATEDIFF(dd,date_opened,date_settled) > 56
		AND fb.description = 'Property'   AND p.party_type_id IN (SELECT party_type_id FROM party_type WHERE code='CC' OR code='GC' )
		AND ( isnull(cc.turnover,0)=@turnoverband_id  OR  isnull(gc.turnover,0)=@turnoverband_id)
	),
	(
		SELECT
		      ISNULL(SUM(1),0)
		FROM FSA_complaint_folder fo
		JOIN FSA_class_of_business fb
		    ON fb.FSA_class_of_business_id = fo.FSA_class_of_business_id
		JOIN party p
		    ON p.party_cnt = fo.party_cnt
		LEFT JOIN insurance_file i
		    ON i.insurance_file_cnt = fo.insurance_file_cnt
	       	 LEFT JOIN party_corporate_client cc
	                 ON p.party_cnt = CC.party_cnt
	       	 LEFT JOIN party_group_client gc
	                 ON p.party_cnt = gc.party_cnt
		WHERE (ISNULL(i.source_id, p.source_id) = @branch_id OR @branch_id IS NULL)
		AND date_settled BETWEEN @start_date AND @end_date
		AND DATEDIFF(dd,date_opened,date_settled) > 56
		AND fb.description = 'Other'  AND p.party_type_id IN (SELECT party_type_id FROM party_type WHERE code='CC' OR code='GC' )
		AND ( isnull(cc.turnover,0)=@turnoverband_id  OR  isnull(gc.turnover,0)=@turnoverband_id)
	),
	  
	@FSADisabled 
	FROM FSA_complaint_category fc 
	GROUP BY
	    fc.FSA_complaint_category_id,
	    fc.description

FETCH NEXT FROM turn_Over INTO
	@turnoverband_id,
	@description
END

CLOSE  turn_Over 
DEALLOCATE turn_Over 

select * from  #ComplaintsTemp
GO
