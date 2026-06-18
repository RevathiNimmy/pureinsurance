

EXECUTE DDLDropProcedure 'spu_Report_Ren_Check_For_Claim_SFU'
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON
GO
/**********************************************************************************************************************************
** Created by Thinh Nguyen
**
** RSA Reports - Renewal_Pre_List.rpt (subreport)
**
-- $Author: Tom.brown $
-- $Revision: 4 $
-- $Modtime: 19/09/02 10:40 $
-- $Workfile: sp_Report_Ren_Check_For_Claim.sql $
-- $Logfile: /Sirius For Underwriting/Crystal Reports/Stored Procedures/sp_Report_Ren_Check_For_Claim.sql $

-- $History: sp_Report_Ren_Check_For_Claim.sql $
-- 
-- *****************  Version 4  *****************
-- User: Tom.brown    Date: 19/09/02   Time: 10:50
-- Updated in $/Sirius For Underwriting/Crystal Reports/Stored Procedures
-- F00050097.  Where claims criteria is NULL in the product, this sp was
-- not picking up the claims at all.  Fixed by using ISNULL on product
-- claims criteria.
**  06/08/2001  JMK     Product.claim_year_to_check can be NULL, so use isnull
**
**  08/08/2001  JMK amend Product.claim_year_to_check - to conform to renewal status SQL
**
***********************************************************************************************************************************/
CREATE PROCEDURE spu_Report_Ren_Check_For_Claim_SFU
        @Insurance_File_Cnt int
AS
BEGIN

DECLARE @number_of_claim int,
        @value_of_single_claim money,
        @value_of_total_claim money,
        @max_single_claim_value money,
        @max_number_of_claim int,
        @max_total_claim_value money,
        @claim_id int,
        @claim_number varchar(30),
        @description varchar(50),
        @claim_date datetime,
        @notified_date datetime,
        @initial_reserve money,
        @current_reserve money,
        @paid_to_date money,
        @claim_status_id int,
        @done int


--initialise variables
SELECT @number_of_claim = 0,
        @value_of_single_claim = 0,
        @value_of_total_claim = 0,
        @max_single_claim_value = 0,
        @max_number_of_claim = 0,
        @max_total_claim_value = 0,
        @done = 0

-- get claim values for product
-- TB 19/09/2002 - Added ISNULL modifiers
SELECT @max_single_claim_value = isnull(prod.max_single_claim_value,0),
        @max_number_of_claim = isnull(prod.max_number_of_claim,0),
        @max_total_claim_value = isnull(prod.max_total_claim_value,0)
FROM    insurance_file inf,
        product prod
WHERE inf.insurance_file_cnt = @insurance_file_cnt
AND     inf.product_id = prod.product_id

-- Create temporary table
CREATE TABLE #tmpRenCheckForClaim
(       claim_number varchar(30),
        description varchar(50),
        claim_date datetime,
        notified_date datetime,
        initial_reserve money,
        current_reserve money,
        paid_to_date money,
        claim_status_id int
)

-- Cursor to get all claims for policy
DECLARE ClaimForPolicy CURSOR LOCAL SCROLL FOR
   SELECT cl.claim_id,
          cl.claim_number,
          cl.description,
          cl.loss_from_date,
          cl.reported_date,
          cl.claim_status_id
   FROM Claim cl,
        Insurance_File inf,
        Product prod
   WHERE inf.insurance_file_cnt = @insurance_file_cnt
   AND  inf.insurance_file_cnt = cl.policy_id
   AND  inf.product_id = prod.product_id
   -- JMK 06/08/2001; 08/08/2001
   AND  cl.loss_from_date >=
        dateadd(year, 1 - isnull(prod.claim_year_to_check, 1), inf.cover_start_date)
   AND  cl.primary_cause_id NOT IN
        (SELECT primary_cause_id
        FROM Product_Allowed_Causation pac
        WHERE   prod.product_id = pac.product_id)

-- open cursor
OPEN ClaimForPolicy

-- get first record
FETCH NEXT FROM ClaimForPolicy INTO @claim_id, @claim_number, @description, @claim_date, @notified_date, @claim_status_id

-- loop thro and process each claim
WHILE @@FETCH_STATUS = 0
BEGIN
        -- increment number of claims
        SELECT @number_of_claim = @number_of_claim + 1

        -- get values from reseve table
        SELECT @value_of_single_claim = sum(re.initial_reserve + re.revised_reserve),
                @initial_reserve = sum(re.initial_reserve),
                @paid_to_date = sum(re.paid_to_date)
        FROM Claim_Peril cp, Reserve re
        WHERE cp.claim_id = @claim_id
        AND     cp.claim_peril_id = re.claim_peril_id

        -- set current reseve = value_of_single_claim
        SELECT @current_reserve = @value_of_single_claim

        -- add to temporary table if single claim value > max_single_claim_value
        IF @value_of_single_claim > @max_single_claim_value
        BEGIN
           -- set flag to say we've added claim to temporary table
           SELECT @done = -1

           INSERT INTO #tmpRenCheckForClaim
              (claim_number,
              description,
              claim_date,
              notified_date,
              initial_reserve,
              current_reserve,
              paid_to_date,
              claim_status_id)
           VALUES
              (@claim_number,
              @description,
              @claim_date,
              @notified_date,
              @initial_reserve,
              @current_reserve,
              @paid_to_date,
              @claim_status_id)
        END

        -- increment value_of_total_claim
        SELECT @value_of_total_claim = @value_of_total_claim + @value_of_single_claim


--get next claim
FETCH NEXT FROM ClaimForPolicy INTO @claim_id, @claim_number, @description, @claim_date, @notified_date, @claim_status_id
END

-- check to see if we've added claims to temporary table
IF @done = -1
   GOTO ALLDONE

-- is number of claims exceeds maximun claims or total claims value exceeds maximum total claims
IF @number_of_claim > @max_number_of_claim OR @value_of_total_claim > @max_total_claim_value
BEGIN
        FETCH FIRST FROM ClaimForPolicy INTO @claim_id, @claim_number, @description, @claim_date, @notified_date, @claim_status_id

        -- loop thro and add each claims to temporary table
        WHILE @@FETCH_STATUS = 0
        BEGIN
                -- get values from reseve table
                SELECT @value_of_single_claim = sum(re.initial_reserve + re.revised_reserve),
                        @initial_reserve = sum(re.initial_reserve),
                        @paid_to_date = sum(re.paid_to_date)
                FROM Claim_Peril cp, Reserve re
                WHERE cp.claim_id = @claim_id
                AND     cp.claim_peril_id = re.claim_peril_id

                -- set current reseve = value_of_single_claim
                SELECT @current_reserve = @value_of_single_claim

                INSERT INTO #tmpRenCheckForClaim
                        (claim_number,
                        description,
                        claim_date,
                        notified_date,
                        initial_reserve,
                        current_reserve,
                        paid_to_date,
                        claim_status_id)
                VALUES
                        (@claim_number,
                        @description,
                        @claim_date,
                        @notified_date,
                        @initial_reserve,
                        @current_reserve,
                        @paid_to_date,
                        @claim_status_id)

        FETCH NEXT FROM ClaimForPolicy INTO @claim_id, @claim_number, @description, @claim_date, @notified_date, @claim_status_id
        END
END

ALLDONE:

-- get data back
SELECT  claim_number,
        description,
        claim_date,
        notified_date,
        initial_reserve,
        current_reserve,
        paid_to_date,
        claim_status_id
FROM    #tmpRenCheckForClaim

-- delete temporary table
DROP TABLE #tmpRenCheckForClaim

-- close cursor
CLOSE ClaimForPolicy
DEALLOCATE ClaimForPolicy


END

GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON
GO

