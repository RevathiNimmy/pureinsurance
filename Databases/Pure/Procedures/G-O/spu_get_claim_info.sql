SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_get_claim_info'
GO

CREATE PROCEDURE spu_get_claim_info
    @insurance_file_cnt INT,
    @claim_year_to_check INT
AS

--************************************************************************************************************
-- Desc : get back number of allowed claims, total incurred of all allowed claims and largest incurred
-- Hist : 16 July 2001 - Tinny
-- Note : claim_year_to_check = 1 (current version)
--        claim_year_to_check = 2 (previous year/version) and so on
--************************************************************************************************************
DECLARE @correct_version INT,
        @claim_id INT,
        @number_of_claim INT,
        @total_incurred money,
        @largest_incurred money,
        @current_incurred money

SELECT  @number_of_claim = 0,
        @total_incurred = 0,
        @largest_incurred = 0

-- GET CORRECT VERSION OF POLICY
SELECT  @correct_version = ifi2.insurance_file_cnt
FROM    insurance_file ifi,
        insurance_file ifi2
WHERE   ifi.insurance_file_cnt = @insurance_file_cnt
AND     ifi.insurance_folder_cnt = ifi2.insurance_folder_cnt
AND     ifi2.cover_start_date >= dateadd(year,1-@claim_year_to_check,ifi.cover_start_date)
AND     ifi2.expiry_date <= dateadd(year,1-@claim_year_to_check,ifi.expiry_date)

-- CURSOR TO GET ALL CLAIMS FOR THIS VERSION  (EXCLUDE ALLOWED ONES)
DECLARE ClaimForPolicy CURSOR FAST_FORWARD FOR
    SELECT  c.claim_id
    FROM    Claim c,
            Insurance_File ifi
    WHERE   ifi.insurance_file_cnt = @correct_version
    AND     ifi.insurance_file_cnt = c.policy_id
    AND     c.primary_cause_id NOT IN
            (SELECT primary_cause_id
            FROM    Product_Allowed_Causation pac
            WHERE   ifi.product_id = pac.product_id)

-- OPEN CURSOR
OPEN ClaimForPolicy

-- GET FIRST RECORD
FETCH NEXT FROM ClaimForPolicy INTO @claim_id

-- LOOP THRO AND PROCESS EACH CLAIM
WHILE @@FETCH_STATUS = 0 BEGIN
    -- INCREMENT NUMBER OF CLAIMS
    SELECT @number_of_claim = @number_of_claim + 1

    -- GET TOTAL INCURRED
    SELECT  @current_incurred = sum(r.initial_reserve + r.revised_reserve)
    FROM    claim_peril cp,
            reserve r
    WHERE   cp.claim_id = @claim_id
    AND     cp.claim_peril_id = r.claim_peril_id

    -- IS THIS THE LARGEST CLAIM INCURRED?
    IF @current_incurred > @largest_incurred
        SELECT @largest_incurred = @current_incurred

    -- SUM UP INCURRED FOR ALL CLAIMS
    SELECT @total_incurred = @total_incurred + @current_incurred

    FETCH NEXT FROM ClaimForPolicy INTO @claim_id
END

CLOSE ClaimForPolicy
DEALLOCATE ClaimForPolicy

SELECT  @number_of_claim,
        @total_incurred,
        @largest_incurred
GO

