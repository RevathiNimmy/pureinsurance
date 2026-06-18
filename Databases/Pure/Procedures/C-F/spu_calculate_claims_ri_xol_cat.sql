SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_calculate_claims_ri_xol_cat'
GO

CREATE PROCEDURE spu_calculate_claims_ri_xol_cat
    @claim_id int,
    @ri_arrangement_id int,
    @total_reserve money,
    @catastrophe_code_id int,
    @original_claim_id int,
    @original_reserve money,
    @original_payment money,
    @retained_reserve money output,
    @retained_payment money output
As

    Declare
        -- XOL info for next layer
        @xol_arrangement_id int,
        @xol_layer int,
        @xol_model_id int,
        @xol_limit money,
        @xol_reinstatements int,
        @next_arrangement_id int,
        @effective_limit_payment money,
		@effective_limit_reserve money,
        -- The number of instances of a given catastrophe and it's owning model
        @ri_model_id int,
        @parent_model_id int,
		@cat_reserves money,
        @cat_payments money,
        @cat_count int,
        -- Outstanding amounts to put on XOL
        @xol_os_reserve money,
        @xol_os_payment money,
        -- Amount for this xol
        @xol_this_reserve money,
        @xol_this_payment money,
        -- Total amount allocated to xol
        @xol_total_reserve money,
        @xol_total_payment money,
		@return int

    -- If we have no catastrophe return
    If IsNull(@catastrophe_code_id, 0) = 0
        Return
	
	SET @return=1

    -- Get base ri_model
    Select  @ri_model_id = ri_model_id
    From    claim_ri_arrangement
    Where   claim_id = @claim_id
    And     ri_arrangement_id = @ri_arrangement_id

    -- Get the total amount already allocated to this catastrophe, across all retained, per claim and cat lines!!!
    -- This is our initial cat xol trigger limit
    Select  @cat_payments = IsNull(Sum(ral.payment), 0),
	@cat_reserves = IsNull(Sum(ral.reserve), 0)
    From    claim c
    Join    claim_ri_arrangement ra
            On ra.claim_id = c.claim_id
    Join    claim_ri_arrangement_line ral
            On ral.claim_id = ra.claim_id
            And ral.ri_arrangement_id = ra.ri_arrangement_id
    Where   c.claim_id <> ISNULL(@original_claim_id,0) -- ignore values on original version of this claim
    And     c.catastrophe_code_id = @catastrophe_code_id
    And     ra.ri_model_id = @ri_model_id and is_dirty=0
    And     ral.type = 'R' -- Only retained counts
	AND     c.claim_id in (select max(c1.claim_id) from claim c1 WHERE c.catastrophe_code_id = @catastrophe_code_id group by c1.base_claim_id)

    -- Check if we already have a catastrophe xol layer 1
    Select  @xol_arrangement_id = xol_arrangement_id,
            @xol_layer = layer,
            @xol_model_id = ri_model_id,
            @xol_limit = trigger_limit,
            @xol_reinstatements = Null -- Indicate that this is existing and not subject to reinstatement check
    From    claim_xol_arrangement
    Where   claim_id = @claim_id
    And     ri_arrangement_id = @ri_arrangement_id
    And     layer = 1
    And     catastrophe_code_id = @catastrophe_code_id

    -- No we don't, check if catastrophe xol is configured
    If @xol_arrangement_id Is Null Begin
        -- Next, what is the catastrophe ri config on this model?
        Select  @xol_arrangement_id = null,
                @xol_layer = 1,
                @xol_model_id = rim.xol_cat_ri_model_id,
                @xol_limit = rim.xol_cat_limit,
                @xol_reinstatements = IsNull(rim.xol_cat_reinstatements, 0), -- Unspecified = no reinstatements
                @parent_model_id = rim.ri_model_id
        From    claim_ri_arrangement ra
        Left Join
                ri_model rim On rim.ri_model_id = ra.ri_model_id
        Where   claim_id = @claim_id
        And     ri_arrangement_id = @ri_arrangement_id
    End Else Begin
        Select  @parent_model_id = ri_model_id
        From    claim_ri_arrangement
        Where   claim_id = @claim_id
        And     ri_arrangement_id = @ri_arrangement_id
    End

	If (@xol_model_id Is Not Null) And (@retained_reserve + @cat_reserves > @xol_limit) Begin
        -- have we already exceeded the cat limit?
        If @cat_reserves > @xol_limit Begin
            -- If so we move ALL retained
            Select  @xol_os_reserve = @retained_reserve,
					@cat_reserves = @cat_reserves - @xol_limit
        End Else Begin
            -- If not we move whatever would exceed the limit.
            Select  @xol_os_reserve = (@retained_reserve + @cat_reserves) - @xol_limit,
					@cat_reserves = 0
            Select  @xol_os_reserve = Case When @xol_os_reserve > @retained_reserve Then @retained_reserve Else @xol_os_reserve End
        End
	 -- Zero running totals
        Select  @xol_this_reserve = 0,
                @xol_total_reserve = 0
		SELECT @return=0
    End 

	If (@xol_model_id Is Not Null) And (@retained_payment + @cat_payments > @xol_limit) Begin
        -- have we already exceeded the cat limit?
        If @cat_payments > @xol_limit Begin
            -- If so we move ALL retained
            Select  @xol_os_payment = @retained_payment,
                    -- Reduce existing payments by trigger limit for layer checks
                    @cat_payments = @cat_payments - @xol_limit
        End Else Begin
            -- If not we move whatever would exceed the limit.
            Select  @xol_os_payment = (@retained_payment + @cat_payments) - @xol_limit,
                    -- No excess payments to consider for layer checks
                    @cat_payments = 0

            -- Quick check to ensure we don't move claim xol
            Select @xol_os_payment = Case When @xol_os_payment > @retained_payment Then @retained_payment Else @xol_os_payment End
        End
	 -- Zero running totals
        Select  @xol_this_payment = 0,
                @xol_total_payment = 0
		SELECT @return=0
    End

	If @return=1
	Begin
		return		
	END

    -- If os value ever go negative stop processing them
    Select  @xol_os_reserve = Case When @xol_os_reserve < 0 Then 0 Else @xol_os_reserve End,
            @xol_os_payment = Case When @xol_os_payment < 0 Then 0 Else @xol_os_payment End

    -- Loop while we have excess to allocate
    While (@xol_os_reserve > 0 Or @xol_os_payment > 0) And (@xol_model_id Is Not Null) Begin
        -- If we don't have an xol_arrangement add one, if we can
        If (IsNull(@xol_arrangement_id, 0) = 0) Begin
            -- With cat xol we have a limit of catastrophes the xol will be effective
            -- for. If we have already used this catastrophe continue to do so, else
            -- check if we have any reinstatements available.
            If @xol_layer = 1 Begin
                -- Layer one check against claim
                If Exists (Select * From    claim_xol_arrangement xa
                                    Join    claim_ri_arrangement ra On ra.ri_arrangement_id = xa.ri_arrangement_id
                                    Where   ra.ri_model_id = @parent_model_id
                                    And     xa.catastrophe_code_id = @catastrophe_code_id
                                    And     xa.layer = @xol_layer) Begin
                    -- We already have this catastrophe so continue to use it,
                    -- Set existing count to zero to ensure we allow this one to be created
                    Select  @cat_count = 0
                End Else Begin
                    -- We don't already have this catastrophe, check how many other we have
                    Select  @cat_count = Count(Distinct xa.catastrophe_code_id)
                    From    claim_xol_arrangement xa
                    Join    claim_ri_arrangement ra On ra.ri_arrangement_id = xa.ri_arrangement_id
                    Where   ra.ri_model_id = @parent_model_id
                    And     xa.catastrophe_code_id <> @catastrophe_code_id
                End
            End Else Begin
                -- Above, check against prior layer
                If Exists (Select * From    claim_xol_arrangement xa
                                    Join    claim_xol_arrangement xap On xap.ri_arrangement_id = xa.ri_arrangement_id
                                                                     And xap.layer = xa.layer - 1
                                    Where   xap.ri_model_id = @parent_model_id
                                    And     xa.catastrophe_code_id = @catastrophe_code_id
                                    And     xa.layer = @xol_layer) Begin
                    -- We already have this catastrophe so continue to use it,
                    -- Set existing count to zero to ensure we allow this one to be created
                 Select  @cat_count = 0
                End Else Begin
                    -- We don't already have this catastrophe, check how many other we have
                    Select  @cat_count = Count(Distinct xa.catastrophe_code_id)
                    From    claim_xol_arrangement xa
                    Join    claim_xol_arrangement xap On xap.ri_arrangement_id = xa.ri_arrangement_id
                                                     And xap.layer = xa.layer - 1
                    Where   xap.ri_model_id = @parent_model_id
                    And     xa.catastrophe_code_id <> @catastrophe_code_id
                End
            End

            -- Compare our actual count with the reinstatement count
            If IsNull(@xol_reinstatements, @cat_count) <= @cat_count Begin
                -- Add the arrangement
                Exec spu_Claim_Xol_Arrangement_add
                    @claim_id = @claim_id,
                    @ri_arrangement_id = @ri_arrangement_id,
                    @catastrophe_code_id = @catastrophe_code_id,
                    @layer = @xol_layer,
                    @ri_model_id = @xol_model_id,
                    @trigger_limit = @xol_limit,
                    @xol_arrangement_id = @xol_arrangement_id output
            End
        End

        -- We may not have been allowed to create a new arrangement based on the number
        -- or preexisting catastrophe's so check before we do anymore work
        If (IsNull(@xol_arrangement_id, 0) <> 0) Begin
            -- Get the total amount already allocated to this catastrophe layer
            -- This is used to trigger layer 2, 3...
            Select  @cat_payments = IsNull(Sum(ral.payment), 0),@cat_reserves= IsNull(Sum(ral.reserve), 0)
            From    claim_ri_arrangement ra
            Join    claim_ri_arrangement_line ral
                    On ral.claim_id = ra.claim_id
                    And ral.ri_arrangement_id = ra.ri_arrangement_id
            Join    claim_xol_arrangement xa
                    On xa.claim_id = ral.claim_id
                    And xa.xol_arrangement_id = ral.xol_arrangement_id
			Join	claim c on c.Claim_id=xa.claim_id
            Where   ra.claim_id <> ISNULL(@original_claim_id,0) and is_dirty=0 -- ignore values on original version of this claim
            And     xa.catastrophe_code_id = @catastrophe_code_id -- match catastrophe
            And     xa.ri_model_id = @xol_model_id -- only payment made to this model

            -- Check for next layer
            -- Note:
            --    We are doing this up front because we need to know the limit
            --    for the next layer as the total liability on the current layer
            --    may exceed it.
            Select  @next_arrangement_id = Null,
                    @xol_layer = @xol_layer + 1,
                    @parent_model_id = @xol_model_id

            -- Check for existing next layer
            Select  @next_arrangement_id = xol_arrangement_id,
                    @xol_model_id = ri_model_id,
                    @xol_limit = trigger_limit,
                    @xol_reinstatements = Null -- Indicate that this is existing and not subject to reinstatement check
            From    claim_xol_arrangement
            Where   claim_id = @claim_id
            And     ri_arrangement_id = @ri_arrangement_id
            And     layer = @xol_layer
            And     catastrophe_code_id = @catastrophe_code_id

            -- We don't have one, check if per claim xol is configured on last ri model
            If @next_arrangement_id Is Null Begin
                -- Next, what is the per claim ri config on this model?
                Select  @next_arrangement_id = null,
                        @xol_model_id = xol_cat_ri_model_id,
                        @xol_limit = xol_cat_limit,
                        @xol_reinstatements = IsNull(xol_cat_reinstatements, 0)
                From    ri_model
                Where   ri_model_id = @xol_model_id
            End

            -- Check next xol limit and adjust according to total of existing payments
            If @xol_limit Is Null Begin
                -- No next layer configured, set null to use limit of lines
                Select  @effective_limit_reserve = Null
				Select  @effective_limit_payment = Null
            End Else If @xol_limit > @cat_payments or @xol_limit > @cat_reserves Begin
                -- Limit is greater than previous payments, reduce limit by previous payment total.
				If @xol_limit > @cat_reserves 
					Select  @effective_limit_reserve = @xol_limit - @cat_reserves
				If @xol_limit > @cat_payments
					Select  @effective_limit_payment = @xol_limit - @cat_payments
            End Else Begin
                -- Limit is configured and less than previous payment, no allocated possible
                Select  @effective_limit_reserve = 0
				Select  @effective_limit_payment = 0
            End

            -- Allocate the xol model
            Exec spu_Claim_Xol_Arrangement_calc
                @claim_id = @claim_id,
                @ri_arrangement_id = @ri_arrangement_id,
                @xol_arrangement_id = @xol_arrangement_id,
                -- Pass in total reserve for share calculations
                @total_reserve = @total_reserve,
                -- Pass in our outstanding reserve and payment
                @xol_os_reserve = @xol_os_reserve,
                @xol_os_payment = @xol_os_payment,
                -- Pass in the limit for the next layer
                @xol_limit = @effective_limit_payment,
                -- Retrieve the amount allocated to this xol layer
                @xol_this_reserve = @xol_this_reserve output,
                @xol_this_payment = @xol_this_payment output,
                -- For cat xol also pass in the amount allocated already
                @xol_to_date = @cat_payments,
				@xol_reserve_limit =@effective_limit_reserve,
				@xol_reserve_to_date =@cat_reserves

            -- Update our running totals and next arrangement id
            Select  @xol_os_reserve = @xol_os_reserve - @xol_this_reserve,
                    @xol_os_payment = @xol_os_payment - @xol_this_payment,
                    @xol_total_reserve = @xol_total_reserve + @xol_this_reserve,
                    @xol_total_payment = @xol_total_payment + @xol_this_payment,
                    @xol_arrangement_id = @next_arrangement_id

            -- Now check if next layer is applicable
            -- Note:
            --    We need to check if the xol amount applicable to the last layer exceeds
            --    the xol limit for that layer, do not check the allocated amount or that
            --    the os amount is none zero as our xol limit may not match the liability
			If (@xol_model_id Is Not Null) And ((@xol_os_reserve + @xol_this_reserve) > @effective_limit_reserve) Begin
                -- We have exceeded the limit, reset this totals
                Select  @xol_this_reserve = 0
                        
            End Else Begin
                -- We haven't exceeded this layers limit but we may still have outstanding
                -- amounts, these must go back onto the retained lines. Zero os to exit loop
                Select  @xol_os_reserve = 0
            End
            If (@xol_model_id Is Not Null) And ((@xol_os_payment + @xol_this_payment) > @effective_limit_payment) Begin
                -- We have exceeded the limit, reset this totals
                Select  @xol_this_reserve = 0,
                        @xol_this_payment = 0
            End Else Begin
                -- We haven't exceeded this layers limit but we may still have outstanding
                -- amounts, these must go back onto the retained lines. Zero os to exit loop
                Select  @xol_os_reserve = 0,
                        @xol_os_payment = 0
            End

        End Else Begin
            -- We couldn't create this xol layer because of catastrope reinstatement rules
            -- but we may still have outstanding amounts, these must go back onto the
            -- retained lines. Zero os to exit loop
            Select  @xol_os_reserve = 0,
                    @xol_os_payment = 0
        End

        -- If os value ever go negative stop processing them
        Select  @xol_os_reserve = Case When @xol_os_reserve < 0 Then 0 Else @xol_os_reserve End,
                @xol_os_payment = Case When @xol_os_payment < 0 Then 0 Else @xol_os_payment End
    End

    -- Now we are done reduce the retained reserves by the amount allocated to XOL
    Select  @retained_reserve = @retained_reserve - @xol_total_reserve,
            @retained_payment = @retained_payment - @xol_total_payment





GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
