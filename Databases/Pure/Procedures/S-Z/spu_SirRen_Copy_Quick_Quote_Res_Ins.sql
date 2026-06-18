SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_SirRen_Copy_Quick_Quote_Res_Ins'
GO

CREATE PROCEDURE spu_SirRen_Copy_Quick_Quote_Res_Ins
    @OldQuoteBinderId int,
    @OldGisPolicyLinkId int,
    @NewQuoteBinderId int,
    @NewGisPolicyLinkId int
AS

Declare @NewQuickResultId int
Declare @QuoteResultId int

/* Create cursor for all old Quick_Quote_Result records */
DECLARE gqr_cursor CURSOR FAST_FORWARD FOR
    SELECT GIIMQuick_Quote_Result_id
    FROM GIIMQuick_Quote_Result gqr
    WHERE gqr.gis_policy_link_id = @OldGisPolicyLinkId
    AND gqr.quote_binder_id = @OldQuoteBinderId

OPEN gqr_cursor

FETCH NEXT FROM gqr_cursor
    INTO @QuoteResultId
WHILE @@FETCH_STATUS = 0
BEGIN

    /* Get next available Quick_Quote_Result id */
    SELECT @NewQuickResultId = Max(GIIMQuick_Quote_Result_id) + 1 FROM GIIMQuick_Quote_Result

    /* Copy record */
    INSERT INTO GIIMQuick_Quote_Result
        (gis_policy_link_id,
        GIIMQuick_Quote_Result_id,
        Quote_Binder_id,
        scheme_id,
        scheme_desc,
        commission_rate,
        commission_amount,
        scheme_type,
        premium,
        quote_type,
        ncb_protected,
        cover_type,
        total_excess,
        ipt,
        compulsory_excess,
        voluntary_excess,
        rates_date,
        vehicle_group,
        vehicle_area,
        ncd_years,
        ncd_discount,
        young_driver_excess,
        compulsory_non_ad_excess,
        compulsory_non_ad_desc,
        windscreen_excess,
        windscreen_limit,
        override_percent,
        override_amount,
        override_gross_premium,
        override_premium_from_pot,
        override_authorisation_code,
        commission_override_pct,
        commission_override_amt,
        Insurer_Mnemonic,
        Original_Premium,
        Recalculated_Old_Risk_Premium,
        New_Risk_Calculated_Premium,
        Adjustment_Premium_incl_IPT,
        Adjustment_Premium_excl_IPT,
        True_Net_Premium
        )
    SELECT @NewGisPolicyLinkId,
        @NewQuickResultId,
        @NewQuoteBinderId,
        scheme_id,
        scheme_desc,
        commission_rate,
        commission_amount,
        scheme_type,
        premium,
        quote_type,
        ncb_protected,
        cover_type,
        total_excess,
        ipt,
        compulsory_excess,
        voluntary_excess,
        rates_date,
        vehicle_group,
        vehicle_area,
        ncd_years,
        ncd_discount,
        young_driver_excess,
        compulsory_non_ad_excess,
        compulsory_non_ad_desc,
        windscreen_excess,
        windscreen_limit,
        override_percent,
        override_amount,
        override_gross_premium,
        override_premium_from_pot,
        override_authorisation_code,
        commission_override_pct,
        commission_override_amt,
        Insurer_Mnemonic,
        Original_Premium,
        Recalculated_Old_Risk_Premium,
        New_Risk_Calculated_Premium,
        Adjustment_Premium_incl_IPT,
        Adjustment_Premium_excl_IPT,
        True_Net_Premium
    FROM GIIMQuick_Quote_Result gqr
    WHERE gqr.gis_policy_link_id = @OldGisPolicyLinkId
    AND gqr.quote_binder_id = @OldQuoteBinderId
    AND gqr.GIIMQuick_Quote_Result_id = @QuoteResultId

    /* Copy child-relationship records */
    EXECUTE spu_SirRen_Copy_Excess_Breakdown @OldQuoteBinderId, @OldGisPolicyLinkId, @QuoteResultId, @NewQuoteBinderId, @NewGisPolicyLinkId, @NewQuickResultId
    EXECUTE spu_SirRen_Copy_Notes_Breakdown @OldQuoteBinderId, @OldGisPolicyLinkId, @QuoteResultId, @NewQuoteBinderId, @NewGisPolicyLinkId, @NewQuickResultId
    EXECUTE spu_SirRen_Copy_Premium_Analysis @OldQuoteBinderId, @OldGisPolicyLinkId, @QuoteResultId, @NewQuoteBinderId, @NewGisPolicyLinkId, @NewQuickResultId
    EXECUTE spu_SirRen_Copy_GIIMSelected_Add_On @OldQuoteBinderId, @OldGisPolicyLinkId, @QuoteResultId, @NewQuoteBinderId, @NewGisPolicyLinkId, @NewQuickResultId

    /* Next record */
    FETCH NEXT FROM gqr_cursor
        INTO @QuoteResultId
END

CLOSE gqr_cursor
DEALLOCATE gqr_cursor
GO

