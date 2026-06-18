SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_CALCULATE_CLAIMS_RI_XOL_CLAIM'
GO


CREATE PROCEDURE spu_CALCULATE_CLAIMS_RI_XOL_CLAIM
                @claim_id          INT,
                @ri_arrangement_id INT,
                @total_reserve     MONEY,
                @retained_reserve  MONEY  OUTPUT,
                @retained_payment  MONEY  OUTPUT
                                         
AS
  DECLARE  -- XOL info for next layer  
    @xol_arrangement_id  INT,  
    @xol_layer           INT,  
    @xol_model_id        INT,  
    @xol_limit           MONEY,  
    @next_arrangement_id INT,  
    -- Outstanding amounts to put on XOL  
    @xol_os_reserve MONEY,  
    @xol_os_payment      MONEY,  
    -- Amount for this xol  
    @xol_this_reserve MONEY,  
    @xol_this_payment    MONEY,  
    -- Total amount allocated to xol  
    @xol_total_reserve MONEY,  
    @xol_total_payment   MONEY,  
  
    @model_currency_id INT,
    @claim_currency_id INT,
    @source_id INT, @model_currency_rate numeric(22,18),@effective_date datetime, @claim_currency_rate numeric(22,10)

      SELECT @claim_currency_id=c.currency_id, @source_id=source_id, @effective_date= create_date, @claim_currency_rate=c.currency_base_xrate 
      FROM claim c join insurance_file i on insurance_file_cnt=policy_id
      WHERE claim_id=@claim_id  

      -- Check if we already have a per claim xol layer 1  
      SELECT @xol_arrangement_id = XOL_ARRANGEMENT_ID,  
             @xol_layer = LAYER,  
             @xol_model_id = RI_MODEL_ID,  
             @xol_limit = TRIGGER_LIMIT  
      FROM   CLAIM_XOL_ARRANGEMENT  
      WHERE  CLAIM_ID = @claim_id  
             AND RI_ARRANGEMENT_ID = @ri_arrangement_id  
             AND LAYER = 1  
             AND CATASTROPHE_CODE_ID IS NULL  
  
  -- No we don't, check if per claim xol is configured  
  IF @xol_arrangement_id IS NULL  
    BEGIN  
      -- Next, what is the per claim ri config on this model?  
        SELECT @xol_arrangement_id = NULL,  
               @xol_layer = 1,  
               @xol_model_id = RIM.XOL_CLM_RI_MODEL_ID,  
               @xol_limit = RIM.XOL_CLM_LIMIT,
	       @model_currency_id = RIM.currency_id
        FROM   CLAIM_RI_ARRANGEMENT RA  
               LEFT JOIN RI_MODEL RIM  
                 ON RIM.RI_MODEL_ID = RA.RI_MODEL_ID  
        WHERE  CLAIM_ID = @claim_id  
               AND RI_ARRANGEMENT_ID = @ri_arrangement_id  
 
    If @model_currency_id <> @claim_currency_id Begin
        Execute spu_ACT_Get_Currency_Rate
            @model_currency_id,
            @source_id,
            @effective_date,
            @model_currency_rate Output
       
       If IsNull(@claim_currency_rate, 0) = 0
	Execute spu_ACT_Get_Currency_Rate
            @claim_currency_id,
            @source_id,
            @effective_date,
            @claim_currency_rate Output

        Select @model_currency_rate = @model_currency_rate / @claim_currency_rate
    End Else
        Select @model_currency_rate = 1

   SET @xol_limit= @xol_limit * @model_currency_rate
  END 
  -- We now know, if we have xol and what the base limit is so check if we need it  
  SELECT @xol_os_payment = 0,  
         @xol_this_payment = 0,  
         @xol_total_payment = 0,  
         @xol_os_reserve = 0,  
         @xol_this_reserve = 0,  
         @xol_total_reserve = 0  
  
  -- We have exceeded the limit, work out by how much  
  IF (@xol_model_id IS NOT NULL)  
     AND (@retained_payment > @xol_limit)  
    BEGIN  
      -- We have exceeded the limit, work out by how much  
      SELECT @xol_os_payment = @retained_payment - @xol_limit,  
             @xol_this_payment = 0,  
             @xol_total_payment = 0  
    END  
  
  IF (@xol_model_id IS NOT NULL)  
     AND (ABS(@retained_reserve) > @xol_limit)  
    BEGIN  
      -- We have exceeded the limit, work out by how much  
	  SELECT @xol_os_reserve = CASE WHEN @retained_reserve<0 THEN -(ABS(@retained_reserve) - @xol_limit) ELSE @retained_reserve-@xol_limit END,
			 @xol_this_reserve = 0,  
             @xol_total_reserve = 0  
  
    END  
  
 IF (@xol_model_id IS NOT NULL)  AND (@retained_payment <= @xol_limit)  
   AND (@xol_model_id IS NOT NULL) AND (ABS(@retained_reserve) <= @xol_limit)  
   -- We have not exceeded the limit so exit  
   RETURN  
  
  WHILE (ABS(@xol_os_reserve) > 0  
          OR @xol_os_payment > 0)  
        AND (@xol_model_id IS NOT NULL)  
    BEGIN  
      -- If we don't have an xol_arrangement add one  
      IF (ISNULL(@xol_arrangement_id,0) = 0)  
        BEGIN  
          -- Add the arrangement  
          EXEC SPU_CLAIM_XOL_ARRANGEMENT_ADD  
             @claim_id = @claim_id ,  
             @ri_arrangement_id = @ri_arrangement_id ,  
             @catastrophe_code_id = NULL ,  
             @layer = @xol_layer ,  
             @ri_model_id = @xol_model_id ,  
             @trigger_limit = @xol_limit ,
             @xol_arrangement_id = @xol_arrangement_id OUTPUT  
        END  

      -- Check for next layer  
      -- Note:  
      --    We are doing this up front because we need to know the limit  
      --    for the next layer as the total liability on the current layer  
      --    may exceed it.  
      SELECT @next_arrangement_id = NULL,  
             @xol_layer = @xol_layer + 1  
  
      -- Check for existing next layer  
      SELECT @next_arrangement_id = XOL_ARRANGEMENT_ID,  
        @xol_model_id = RI_MODEL_ID,  
             @xol_limit = TRIGGER_LIMIT  
      FROM   CLAIM_XOL_ARRANGEMENT  
      WHERE  CLAIM_ID = @claim_id  
             AND RI_ARRANGEMENT_ID = @ri_arrangement_id  
             AND LAYER = @xol_layer  
             AND CATASTROPHE_CODE_ID IS NULL  
  
      -- We don't have one, check if per claim xol is configured on last ri model  
      IF @next_arrangement_id IS NULL  
        BEGIN  
          -- Next, what is the per claim ri config on this model?  
          SELECT @next_arrangement_id = NULL,  
                 @xol_model_id = XOL_CLM_RI_MODEL_ID,  
                 @xol_limit = XOL_CLM_LIMIT  
          FROM   RI_MODEL  
          WHERE  RI_MODEL_ID = @xol_model_id  

		  IF  @model_currency_rate IS NULL
		  		    BEGIN
					 SELECT @model_currency_id = RIM.currency_id
					 FROM   CLAIM_RI_ARRANGEMENT RA
					 LEFT JOIN RI_MODEL RIM
					 ON RIM.RI_MODEL_ID = RA.RI_MODEL_ID
     				 WHERE  CLAIM_ID = @claim_id
					 AND RI_ARRANGEMENT_ID = @ri_arrangement_id
		 
					If @model_currency_id <> @claim_currency_id 
					  Begin
					  		Execute spu_ACT_Get_Currency_Rate
								@model_currency_id,
								@source_id,
								@effective_date,
								@model_currency_rate Output

							If IsNull(@claim_currency_rate, 0) = 0
     							Execute spu_ACT_Get_Currency_Rate
								@claim_currency_id,
								@source_id,
								@effective_date,
								@claim_currency_rate Output
								
							Select @model_currency_rate = @model_currency_rate / @claim_currency_rate
						End 
						Else
						Select @model_currency_rate = 1
						SET @xol_limit= @xol_limit * @model_currency_rate
				END 
			ELSE
			    SET @xol_limit= @xol_limit * @model_currency_rate
		END 
        -- Allocate the xol model  
      EXEC SPU_CLAIM_XOL_ARRANGEMENT_CALC  
         @claim_id = @claim_id ,  
         @ri_arrangement_id = @ri_arrangement_id ,  
         @xol_arrangement_id = @xol_arrangement_id ,  
         -- Pass in total reserve for share calculations  
        @total_reserve = @total_reserve ,  
         -- Pass in our outstanding reserve and payment  
        @xol_os_reserve = @xol_os_reserve ,  
         @xol_os_payment = @xol_os_payment ,  
         -- Pass in the limit for the next layer  
        @xol_limit = @xol_limit ,  
         -- Retrieve the amount allocated to this xol layer  
        @xol_this_reserve = @xol_this_reserve OUTPUT ,  
         @xol_this_payment = @xol_this_payment OUTPUT  
  
      -- Update our running totals and next arrangement id  
      SELECT @xol_os_reserve = @xol_os_reserve - ABS(@xol_this_reserve),  
             @xol_os_payment = @xol_os_payment - @xol_this_payment,  
             @xol_total_reserve = @xol_total_reserve + @xol_this_reserve,  
             @xol_total_payment = @xol_total_payment + @xol_this_payment,  
             @xol_arrangement_id = @next_arrangement_id  
  
      -- Now check if next layer is applicable  
      -- Note:  
      --    We need to check if the xol amount applicable to the last layer exceeds  
      --    the xol limit for that layer, do not check the allocated amount or that  
      --    the os amount is none zero as our xol limit may not match the liability  
      IF (@xol_model_id IS NOT NULL)  
         AND ((@xol_os_payment + @xol_total_payment) <= @xol_limit)  
         AND (@xol_model_id IS NOT NULL)  
         AND ((@xol_os_reserve + @xol_total_reserve) <= @xol_limit)  
        -- We haven't exceeded this layers limit but we may still have outstanding  
        -- amounts, these must go back onto the retained lines. Zero os to exit loop  
        SELECT @xol_os_reserve = 0,  
               @xol_os_payment = 0  
  

      IF (@xol_model_id IS NOT NULL)  
         AND ((@xol_os_reserve + @xol_total_reserve) > @xol_limit)  
        -- We have exceeded the limit, reset this total  
        SELECT @xol_this_reserve = 0  
  
      IF (@xol_model_id IS NOT NULL)  
         AND ((@xol_os_payment + @xol_total_payment) > @xol_limit)  
        -- We have exceeded the limit, reset this total  
        SELECT @xol_this_payment = 0  
  END  
  
  -- Now we are done reduce the retained reserves by the amount allocated to XOL  
  SELECT @retained_reserve = @retained_reserve - @xol_total_reserve,  
         @retained_payment = @retained_payment - @xol_total_payment 
GO
