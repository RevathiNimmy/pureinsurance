EXECUTE DDLDropProcedure 'spu_get_pro_rata_rate'
GO

CREATE PROCEDURE spu_get_pro_rata_rate  
    @insurance_file_cnt int,  
    @risk_cnt int,  
    @original_risk_cnt int,  
    @pro_rata_rate float output  
AS  
    -- If we cancel a non-prorata product we have problems!  
    -- The original code will assume that the cancellation is an mta and use the  
    -- mta_prorata option whereas peril allocation will assume a cancellation is  
    -- always pro-rata but we don't know the transaction type.  
    --  
    -- To resolve this and other miscalculation issues the prorata rate is now  
    -- stored on the risk at peril allocation.  
    --  
    -- Where not available (old policy versions) we use the previous method of  
    -- calculation as a fallback.  
  
    DECLARE @is_oos_reversal INT
    
    IF EXISTS (select  null from mta_insurance_file_link where cancelled_linked_insurance_file_cnt=@insurance_file_cnt)
		SET @is_oos_reversal =1            
	ELSE
		SET @is_oos_reversal =0            
    DECLARE  
        @previous_rate float,  
        @current_rate float,  
        @pro_rata_flag tinyint,  
        @is_midnight_renewal tinyint,  
        @cover_start_date datetime,  
        @expiry_date datetime,  
        @policy_scaling float,  
        @days int,  
        @year int,
		@annual_premium NUMERIC(19,4),
 		@premium NUMERIC(19,4),  
 		@inception_date_tpi datetime,    
  		@ri_pro_rata_rate float  
    -- As the previous version of the risk may also be from a pro-rataed policy  
    -- or mta we need to apply the old pro_rata rate to the new rate thus scaling  
    -- the premium up to a full year and back down to our new rate.  
  
    -- Get the previous pro-rata rate  
    -- Note: If null is explicitly passed then just get the rate for the current  
    --       version. In the highly unlikely event that the current version has no  
    --       rate then it will fall through to the old code which will return a  
    --       scaled rate which will be ok for MTAs but incorrect for MTAs on MTAs.  
    
    IF (@original_risk_cnt IS NOT NULL) AND @is_oos_reversal=0 
  	select @ri_pro_rata_rate = MAX(pro_rata_rate) from RI_Arrangement where risk_cnt=@original_risk_cnt and original_flag=0  
         and version_id = (select MAX(version_id) from RI_Arrangement where risk_cnt=@original_risk_cnt and original_flag=0)  

     
    IF ISNULL(@ri_pro_rata_rate,0)=0  
  		SELECT @ri_pro_rata_rate=1  
      
    IF (@original_risk_cnt IS NULL)  
        SELECT  @previous_rate = 1  
    ELSE  
        SELECT  @previous_rate = pro_rata_rate  
        FROM    risk  
        WHERE   risk_cnt = @original_risk_cnt  

  	
	IF (select MAX(version_id) from RI_Arrangement where risk_cnt=@original_risk_cnt and original_flag=0) > 1 AND @is_oos_reversal=0 BEGIN
		SELECT @previous_rate  = 1
	END

    -- Get the current pro-rata rate  
    SELECT  @current_rate = pro_rata_rate  /@ri_pro_rata_rate    
    FROM    risk  
    WHERE   risk_cnt = @risk_cnt  
  
    -- Check for divide by zeroes and apply rates  
    IF (ISNULL(@previous_rate, 0) <> 0)  
        SELECT @pro_rata_rate = @current_rate / @previous_rate  
    ELSE  
        SELECT @pro_rata_rate = NULL  
  
    -- If the rate isn't stored, go back to the old method, this should only be  
    -- necessary where policies have been created before the pro-rata rate field.  
    IF @pro_rata_rate IS NULL  
    BEGIN  
        -- Get the pro rata rate...  
        SELECT  @pro_rata_flag = p.mta_prorata,  
                @is_midnight_renewal = p.is_midnight_renewal  
        FROM    product p  
        JOIN    insurance_file ifi ON p.product_id = ifi.product_id  
        WHERE   insurance_file_cnt = @insurance_file_cnt  
  
        IF @pro_rata_flag = 0  
            SELECT  @pro_rata_rate = 1.00  
        ELSE  
        BEGIN  
            -- Get cover start / end date  
            SELECT  @cover_start_date = ifi.cover_start_date,  
                    @expiry_date = ifi.expiry_date ,    
                    @inception_date_tpi = ifi.inception_date_tpi    
            FROM    insurance_file ifi  
            WHERE   ifi.insurance_file_cnt = @insurance_file_cnt  
  
            -- Check number of days  
            SELECT  @days = DATEDIFF(day, @cover_start_date, @expiry_date)  
  
            IF ISNULL(@is_midnight_renewal, 0) = 1  
                SELECT  @days = @days + 1  
  
            IF @days = 0  
                SELECT  @days = 1  
  
            -- Note: These comments apply before those relating to stored pro-rata  
            --       values....  
            --  
            -- We have a problem with the pro_rata rate when the original policy is  
            -- not done over a full year. We need to add another scaling factor to  
            -- allow for this.  
            --  
            -- Notes:  
            --    The this_premium on the ifi is stored net so we need to sum up the  
            --    premiums on the associated ribands instead.  
            --  
            --    The annual_premium can be Zero or Null (Zero premium policy or quote)  
            --    in which case we should treat this factor as 1.  
  --  
            --    This should now work for:  
            --    a) MTAs on MTAs  
            --    b) Cancellations (Deleted Risks) on MTAs  
            --  
            --    This still doesn't take account of:  
            --    c) Short period rates  
            --        Ideally we need a specific pro_rata SP that can include these.  
            --        In that case we would still need to apply the policy_scaling!  
              
			SELECT @annual_premium=annual_premium 
			FROM Insurance_File 
			WHERE Insurance_file_cnt=@insurance_file_cnt

			SELECT @premium = SUM(Premium) FROM RI_Arrangement WHERE Risk_cnt IN(
				SELECT Risk_cnt
				FROM Insurance_file_risk_link
				WHERE Insurance_file_cnt = @insurance_file_cnt
				AND Original_Risk_cnt IS NULL
				UNION
				SELECT Original_Risk_cnt
				FROM Insurance_file_risk_link
				WHERE Insurance_file_cnt = @insurance_file_cnt
				AND Risk_cnt IS NOT NULL) AND original_flag=0

			SELECT @policy_scaling = CASE WHEN (ISNULL(@annual_premium, 0) = 0) or (ISNULL(@premium, 0) = 0)     
						THEN CONVERT(float, 1)  
						ELSE CONVERT(float, @premium) / CONVERT(float, @annual_premium)  
						END  

					-- Get number of days from the policy start date (leap years an all)  
			SELECT  @year = DATEDIFF(day, @cover_start_date, DATEADD(year, 1, @cover_start_date))  
  
					-- Calculate pro_rata_rate  
			 SELECT  @pro_rata_rate = (CONVERT(float, @days) / CONVERT(float, @year)) / @policy_scaling
        END  
    END  
GO
