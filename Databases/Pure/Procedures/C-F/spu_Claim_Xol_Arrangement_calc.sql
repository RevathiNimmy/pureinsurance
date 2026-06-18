SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_Claim_Xol_Arrangement_calc'
GO

CREATE PROCEDURE spu_Claim_Xol_Arrangement_calc  
    @claim_id int,  
    @ri_arrangement_id int,  
    @xol_arrangement_id int,  
    -- Pass in total reserve for share calculations  
    @total_reserve money,  
    -- Pass in our outstanding reserve and payment  
    @xol_os_reserve money,  
    @xol_os_payment money,  
    -- Pass in the limit for the next layer  
    @xol_limit money,  
    -- Retrieve the amount allocated to this xol layer  
    @xol_this_reserve money output,  
    @xol_this_payment money output,  
    -- Amount allocated to this xol already (catastrophe)  
    @xol_to_date money = null,
	@xol_reserve_limit money= null,
	@xol_reserve_to_date money = null  
AS  
  
    Declare  
        @line_id int,  
        @round_line_id int,  
        @default_percent float,  
        @priority int,  
        @number_of_lines float,  
        @line_limit money,  
        @last_priority int,  
        @running_reserve money,  
        @running_payment money,  
        @priority_limit money,  
        @priority_reserve_share float,  
        @priority_payment_share float,  
        @priority_reserve money,  
        @priority_payment money,  
        @this_reserve money,  
        @this_payment money  
  
    -- Cat xol may not be calculable if a layer has already exceeded it's limit  
    If @xol_limit < 0  
        -- Set limit to zero to avoid any allocations  
        Select  @xol_limit = 0  
    If @xol_reserve_limit < 0
        -- Set limit to zero to avoid any allocations
        Select  @xol_reserve_limit = 0

    -- If xol to date is null set to 0  
    Select  @xol_to_date = IsNull(@xol_to_date, 0)  
    Select  @xol_reserve_to_date = IsNull(@xol_reserve_to_date, 0)

    -- Set default values (ensuring we don't exceed our limit)  
    Select  @running_reserve = Case When IsNull(@xol_reserve_limit, @xol_os_reserve) >= @xol_os_reserve Then @xol_os_reserve Else @xol_reserve_limit End,  
            @running_payment = Case When IsNull(@xol_limit, @xol_os_payment) >= @xol_os_payment Then @xol_os_payment Else @xol_limit End,  
            @last_priority = -666,  
            @priority_reserve_share = 0,  
            @priority_payment_share = 0,  
            @priority_reserve = 0,  
            @priority_payment = 0  
  
    -- Get first retained line to allocate rounding amounts to  
    -- Note: Otherwise use first treaty  
    Select  @round_line_id = ri_arrangement_line_id  
    From    claim_ri_arrangement_line  
    Where   claim_id = @claim_id  
    And     ri_arrangement_id = @ri_arrangement_id  
    And     xol_arrangement_id = @xol_arrangement_id  
    Order By  
            ri_arrangement_line_id  
  
    -- Get each line in the arrangement  
    Declare Line_Cursor Cursor Fast_Forward For  
        Select  ri_arrangement_line_id,  
                default_share_percent / 100,  
                priority,  
                number_of_lines,  
                line_limit  
        From    claim_ri_arrangement_line  
        Where   claim_id = @claim_id  
        And     ri_arrangement_id = @ri_arrangement_id  
        And     xol_arrangement_id = @xol_arrangement_id  
        Order By  
                priority, default_share_percent  
  
    Open Line_Cursor  
    Fetch Next From Line_Cursor  
        Into @line_id, @default_percent, @priority, @number_of_lines, @line_limit  
  
    -- Walk the lines  
    While (@@Fetch_Status = 0) Begin  
        -- Check for change in priority  
        If @last_priority <> @priority  
        Begin  
            -- Check to see if we allocated what we expected taking into account  
            -- any payments already allocated to this catastrophe xol
			If IsNull(@priority_limit, 0) > @xol_reserve_to_date Begin
                Select  @this_reserve = Round(@priority_reserve_share * @xol_os_reserve, 2) - @priority_reserve - @xol_reserve_to_date
            End Else Begin
                Select  @this_reserve = 0
            End

            If IsNull(@priority_limit, 0) > @xol_to_date Begin  
                SELECT @this_payment = Round(@priority_payment_share * @xol_os_payment, 2) - @priority_payment - @xol_to_date  
            End Else Begin  
                Select  @this_payment = 0    
            End  
  
            -- If we have any discrepency allocate them to our ret_line  
            If (@this_reserve <> 0) Or (@this_payment <> 0)  
            Begin  
                -- Update allocated amounts  
                Select  @priority_reserve = @priority_reserve + @this_reserve,  
                        @priority_payment = @priority_payment + @this_payment  
  
                -- Get current rounding line amounts so we can recalc  
                Select  @this_reserve = this_reserve + @this_reserve,  
                        @this_payment = this_payment + @this_payment  
                From    claim_ri_arrangement_line  
                Where   claim_id = @claim_id  
                And     ri_arrangement_id = @ri_arrangement_id  
                And     ri_arrangement_line_id = @round_line_id  
                And     xol_arrangement_id = @xol_arrangement_id  
  
                -- Update ret line  
                Update  claim_ri_arrangement_line  
                Set     this_share_percent = Case When @total_reserve = 0 Then 0  
                            Else (Convert(float, @this_reserve) / @total_reserve) * 100 End,  
                        this_reserve = @this_reserve,  
                        this_payment = @this_payment  
                Where   claim_id = @claim_id  
                And     ri_arrangement_id = @ri_arrangement_id  
                And     ri_arrangement_line_id = @round_line_id  
                And     xol_arrangement_id = @xol_arrangement_id  
            End  
  
            -- Reduce running totals by those allocated to the last priority  
            Select  @running_reserve = @running_reserve - @priority_reserve,  
                    @running_payment = @running_payment - @priority_payment  
  
            -- Update priority and get new priorities total limit  
            Select  @last_priority = @priority,  
                    @priority_limit = @line_limit * @number_of_lines  
  
            -- Check allocated to date  
            If @xol_to_date > @priority_limit Begin  
                Select  @xol_to_date = @xol_to_date - @priority_limit,  
                        @priority_limit = 0  
            End Else Begin  
                Select  @priority_limit = @priority_limit - @xol_to_date,  
                        @xol_to_date = 0  
            End  
  
            -- Check what we have left to allocate, if we've triggered xol assign  
            -- all payment and as much reserve as we can  
          If ABS(@running_reserve) > @running_payment Begin  
                If @priority_limit > ABS(@running_reserve) Begin  
                    Select  @priority_limit = @running_reserve  
                End  
            End Else Begin  
                If @priority_limit > @running_payment Begin  
                    Select  @priority_limit = @running_payment  
                End  
            End  
  
            -- We have a nice effect here, if we have run out of si then all future  
            -- lines will be allocated on a 0 share so no cleanup is required  
  
            -- Get priority share  
            -- Note must convert first money to float else calculation will use  
            -- monetary precision of 4dp!!  
            Select  @priority_reserve_share = Case When @xol_os_reserve = 0 Then 0 Else convert(float, @priority_limit) / @xol_os_reserve End,  
                    @priority_payment_share = Case When @xol_os_payment = 0 Then 0 Else convert(float, @priority_limit) / @xol_os_payment End  
            Select  @priority_reserve_share = Case When @priority_reserve_share > 1 Then 1 Else @priority_reserve_share End,  
                    @priority_payment_share = Case When @priority_payment_share > 1 Then 1 Else @priority_payment_share End  
        End  
  
        -- Get this lines values  
        Select  @this_reserve = Round(@priority_reserve_share * @xol_os_reserve * @default_percent, 2),  
                @this_payment = Round(@priority_payment_share * @xol_os_payment * @default_percent, 2)  
  
        -- Keep track of what has been allocated to this priority  
        Select  @priority_reserve = @priority_reserve + @this_reserve,  
                @priority_payment = @priority_payment + @this_payment  
  
        -- Update line  
        Update  claim_ri_arrangement_line  
        Set     this_share_percent = Case When @total_reserve = 0 Then 0  
                    Else (Convert(float, @this_reserve) / @total_reserve) * 100 End,  
                this_reserve = ISNULL(@this_reserve,0),  
                this_payment = ISNULL(@this_payment,0)  
        Where   claim_id = @claim_id  
        And     ri_arrangement_id = @ri_arrangement_id  
        And     ri_arrangement_line_id = @line_id  
        And     xol_arrangement_id = @xol_arrangement_id  
  
        -- Next line  
        Fetch Next From Line_Cursor  
            Into @line_id, @default_percent, @priority, @number_of_lines, @line_limit  
    End  
  
    Close Line_Cursor  
    Deallocate Line_Cursor  
  
    -- Get our total allocation to return  
    Select  @xol_this_reserve = IsNull(Sum(this_reserve), 0),  
            @xol_this_payment = IsNull(Sum(this_payment), 0)  
    From    claim_ri_arrangement_line  
    Where   claim_id = @claim_id  
    And     ri_arrangement_id = @ri_arrangement_id  
    And     xol_arrangement_id = @xol_arrangement_id  





GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
