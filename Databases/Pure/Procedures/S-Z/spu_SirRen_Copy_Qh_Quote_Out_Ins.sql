SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_SirRen_Copy_Qh_Quote_Out_Ins'
GO

CREATE PROCEDURE spu_SirRen_Copy_Qh_Quote_Out_Ins
    @OldQuoteBinderId int,
    @OldGisPolicyLinkId int,
    @NewQuoteBinderId int,
    @NewGisPolicyLinkId int
AS

Declare @NewQhQuoteOutId int
Declare @QhQuoteOutId int

/* Create cursor for all old Quick_Quote_Result records */
DECLARE qh_cursor CURSOR FAST_FORWARD FOR
    SELECT qh_quote_out_id
    FROM qh_quote_out
    WHERE gis_policy_link_id = @OldGisPolicyLinkId
    AND GIIHquote_binder_id = @OldQuoteBinderId

OPEN qh_cursor

FETCH NEXT FROM qh_cursor
    INTO @QhQuoteOutId
WHILE @@FETCH_STATUS = 0
BEGIN

    /* Get next available Quick_Quote_Result id */
    SELECT @NewQhQuoteOutId = Max(qh_quote_out_id) + 1 FROM qh_quote_out

    /* Copy record */
    INSERT INTO qh_quote_out (
            gis_policy_link_id,
            qh_quote_out_id,
            out_insurer_name,
            GIIHquote_binder_id,
            out_scheme_name,
            out_num_sections,
            out_num_analysis,
            out_num_messages,
            out_total_ann_gross,
            out_contents_ann_gross,
            out_buildings_ann_gross,
            out_total_ann_ipt,
            out_contents_ann_ipt,
            out_buildings_ann_ipt,
            out_total_ann_premium,
            out_contents_ann_prem,
            out_buildings_ann_prem,
            out_contents_only_ann_prem,
            out_buildings_only_ann_prem,
            out_buildings_excess,
            out_contents_excess,
            out_subsidence_excess,
            out_build_vol_excess,
            out_cont_vol_excess,
            out_buildings_flag,
            out_contents_flag,
            out_build_area,
            out_cont_area,
            out_build_rate_date,
            out_cont_rate_date,
            out_build_subs_excess,
            out_build_quote_status,
            out_cont_secu_class,
            out_cont_quote_status,
            out_cont_min_secu_flag,
            original_premium,
            recalculated_old_risk_premium,
            new_risk_calculated_premium,
            adjustment_premium_incl_ipt,
            adjustment_premium_excl_ipt,
            gis_scheme_id,
            out_buildings_si,
            out_contents_si,
            out_cycles_si,
            out_money_si,
            out_credit_si,
            out_freezer_si,
            out_caravan_si,
            out_legal_si,
            out_pb_spec_si,
            out_pb_unspec_si,
            out_hr_si,
            out_freezer_ann_premium,
            out_caravan_ann_premium,
            out_money_ann_premium,
            out_credit_ann_premium,
            out_cycles_ann_premium,
            out_gis_scheme_id,
            out_insurer_code,
            qho_vbs_status,
            qho_g_status,
            out_scheme_num
            )
    SELECT @NewGisPolicyLinkId,
        @NewQhQuoteOutId,
        out_insurer_name,
        @NewQuoteBinderId,
        out_scheme_name,
        out_num_sections,
        out_num_analysis,
        out_num_messages,
        out_total_ann_gross,
        out_contents_ann_gross,
        out_buildings_ann_gross,
        out_total_ann_ipt,
        out_contents_ann_ipt,
        out_buildings_ann_ipt,
        out_total_ann_premium,
        out_contents_ann_prem,
        out_buildings_ann_prem,
        out_contents_only_ann_prem,
        out_buildings_only_ann_prem,
        out_buildings_excess,
        out_contents_excess,
        out_subsidence_excess,
        out_build_vol_excess,
        out_cont_vol_excess,
        out_buildings_flag,
        out_contents_flag,
        out_build_area,
        out_cont_area,
        out_build_rate_date,
        out_cont_rate_date,
        out_build_subs_excess,
        out_build_quote_status,
        out_cont_secu_class,
        out_cont_quote_status,
        out_cont_min_secu_flag,
        original_premium,
        recalculated_old_risk_premium,
        new_risk_calculated_premium,
        adjustment_premium_incl_ipt,
        adjustment_premium_excl_ipt,
        gis_scheme_id,
        out_buildings_si,
        out_contents_si,
        out_cycles_si,
        out_money_si,
        out_credit_si,
        out_freezer_si,
        out_caravan_si,
        out_legal_si,
        out_pb_spec_si,
        out_pb_unspec_si,
        out_hr_si,
        out_freezer_ann_premium,
        out_caravan_ann_premium,
        out_money_ann_premium,
        out_credit_ann_premium,
        out_cycles_ann_premium,
        out_gis_scheme_id,
        out_insurer_code,
        qho_vbs_status,
        qho_g_status,
        out_scheme_num
    FROM qh_quote_out
    WHERE gis_policy_link_id = @OldGisPolicyLinkId
    AND GIIHquote_binder_id = @OldQuoteBinderId
    AND qh_quote_out_id = @QhQuoteOutId

    /* Copy child-relationship records */
    EXECUTE spu_SirRen_Copy_Qho_Section_Line @OldQuoteBinderId, @OldGisPolicyLinkId, @QhQuoteOutId, @NewQuoteBinderId, @NewGisPolicyLinkId, @NewQhQuoteOutId
    EXECUTE spu_SirRen_Copy_Qho_Analysis_Line @OldQuoteBinderId, @OldGisPolicyLinkId, @QhQuoteOutId, @NewQuoteBinderId, @NewGisPolicyLinkId, @NewQhQuoteOutId
    EXECUTE spu_SirRen_Copy_Qho_Message_Line @OldQuoteBinderId, @OldGisPolicyLinkId, @QhQuoteOutId, @NewQuoteBinderId, @NewGisPolicyLinkId, @NewQhQuoteOutId
    EXECUTE spu_SirRen_Copy_Out_Cycles @OldQuoteBinderId, @OldGisPolicyLinkId, @QhQuoteOutId, @NewQuoteBinderId, @NewGisPolicyLinkId, @NewQhQuoteOutId
    EXECUTE spu_SirRen_Copy_Out_Hr_Items @OldQuoteBinderId, @OldGisPolicyLinkId, @QhQuoteOutId, @NewQuoteBinderId, @NewGisPolicyLinkId, @NewQhQuoteOutId
    EXECUTE spu_SirRen_Copy_Out_Pb_Items @OldQuoteBinderId, @OldGisPolicyLinkId, @QhQuoteOutId, @NewQuoteBinderId, @NewGisPolicyLinkId, @NewQhQuoteOutId

    /* Next record */
    FETCH NEXT FROM qh_cursor
        INTO @QhQuoteOutId
END

CLOSE qh_cursor
DEALLOCATE qh_cursor
GO

