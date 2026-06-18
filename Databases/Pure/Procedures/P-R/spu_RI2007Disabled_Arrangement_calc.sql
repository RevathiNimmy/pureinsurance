SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_RI2007Disabled_Arrangement_calc'
GO


CREATE PROCEDURE spu_RI2007Disabled_Arrangement_calc
    @ri_arrangement_id integer,
    @band_si money, 
    @band_premium money,
    @transtype varchar(10) = ''     
AS

    Declare
        @line_id int,
        @ret_line_id int,
        @default_percent float,
        @priority int,
        @number_of_lines float, 
        @line_limit money,
        @last_priority int,
        @running_si money,
        @running_premium money,
        @priority_limit money,
        @priority_share float,
        @priority_si money,
        @priority_premium money,
        @this_si money,
        @this_premium money,
        @first_priority int

    -- Set default values   
    Select  @running_si = @band_si,
            @running_premium = @band_premium,
            @last_priority = -666,
            @priority_share = 0,
            @priority_si = 0,
            @priority_premium = 0


    -- Get first retained line to allocate rounding amounts to
    -- Note: Otherwise use first treaty
    Select  @ret_line_id = ri_arrangement_line_id
    From    ri_arrangement_line
    Where   ri_arrangement_id = @ri_arrangement_id
    And     type In ('R', 'T')
    Order By
            type, ri_arrangement_line_id


    -- Get each line in the arrangement
    Declare Line_Cursor Cursor Fast_Forward For
        Select  ri_arrangement_line_id,
                default_share_percent / 100,
                priority,
                number_of_lines,
                line_limit
        From    ri_arrangement_line
        Where   ri_arrangement_id = @ri_arrangement_id
        And     type In ('R', 'T')
        Order By
                priority, default_share_percent

    Open Line_Cursor
    Fetch Next From Line_Cursor
        Into @line_id, @default_percent, @priority, @number_of_lines, @line_limit

    -- Store first priority, we need this for 0 si bands
    Select  @first_priority = @priority

    -- Walk the lines
    While (@@Fetch_Status = 0)
    Begin
        -- Check for change in priority
        If IsNull(@last_priority, -666) <> @priority
        Begin
            -- Update priority and get new priorities total limit
            Select  @last_priority = @priority,
                    @priority_limit = @line_limit * @number_of_lines

            -- Check to see if we allocated what we expected
            Select  @this_si = Round(@priority_share * @band_si, 2) - @priority_si,
                    @this_premium = Round(@priority_share * @band_premium, 2) - @priority_premium

            -- If we have any discrepency allocate them to our ret_line
            If (@this_si <> 0) Or (@this_premium <> 0)
            Begin
                -- Update allocated amounts
                Select  @priority_si = @priority_si + @this_si,
                        @priority_premium = @priority_premium + @this_premium

                -- Get current ret line amounts so we can recalc
                Select  @this_si = sum_insured + @this_si,
                        @this_premium = premium_value + @this_premium
                From    ri_arrangement_line
                Where   ri_arrangement_line_id = @ret_line_id

                -- Update ret line
                Update  ri_arrangement_line
                Set     this_share_percent = Case When @band_si = 0 Then 0 
                            Else (Convert(float, @this_si) / @band_si) * 100 End,
                        premium_percent = Case When @band_premium = 0 Then 0 
                            Else (Convert(float, @this_premium) / @band_premium) * 100 End,
                        sum_insured = @this_si,
                        premium_value = @this_premium,
                        commission_value = @this_premium * (commission_percent / 100)
                Where   ri_arrangement_line_id = @ret_line_id
            End


            -- reduce running totals by those allocated to the last priority
            Select  @running_si = @running_si - @priority_si,
                    @running_premium = @running_premium - @priority_premium

            -- Check what we have left to allocate
            If @priority_limit > @running_si 
                Select  @priority_limit = @running_si

            -- We have a nice effect here, if we have run out of si then all future 
            -- lines will be allocated on a 0 share so no cleanup is required

            -- Get priority share
            -- Note must convert first money to float else calculation will use 
            -- monetary precision of 4dp!!
            Select  @priority_share = Case When @band_si = 0 Then 
                        Case When @first_priority = @priority Then 1 Else 0 End
                        Else convert(float, @priority_limit) / @band_si End
        End

        -- Get this lines values            
        Select  @this_si = @priority_share * @band_si * @default_percent,
                @this_premium = @priority_share * @band_premium * @default_percent

        -- Keep track of what has been allocated to this priority
        Select  @priority_si = @priority_si + @this_si,
                @priority_premium = @priority_premium + @this_premium

        -- Update line
        Update  ri_arrangement_line
        Set     this_share_percent = Case When @band_si = 0 Then 0 
                    Else (Convert(float, @this_si) / @band_si) * 100 End,
                premium_percent = Case When @band_premium = 0 Then 0 
                    Else (Convert(float, @this_premium) / @band_premium) * 100 End,
                sum_insured = @this_si,
                premium_value = @this_premium,
                commission_value = @this_premium * (commission_percent / 100)
        Where   ri_arrangement_line_id = @line_id

        -- Next line
        Fetch Next From Line_Cursor
            Into @line_id, @default_percent, @priority, @number_of_lines, @line_limit
    End

    Close Line_Cursor
    Deallocate Line_Cursor


GO


