SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_SirRen_Copy_Qho_Section_Line'
GO

CREATE PROCEDURE spu_SirRen_Copy_Qho_Section_Line
    @OldGisPolicyLinkId int,
    @OldQuoteBinderId int,
    @OldGhQuoteOutId int,
    @NewQuoteBinderId int,
    @NewGisPolicyLinkId int,
    @NewGhQuoteOutId int
AS

Declare @NewSectionLineId int
Declare @SectionLineId int

/* Create cursor for all old Quick_Quote_Result records */
DECLARE sl_cursor CURSOR FAST_FORWARD FOR
    SELECT qho_section_line_id
    FROM qho_section_line
    WHERE gis_policy_link_id = @OldGisPolicyLinkId
    AND GIIHquote_binder_id = @OldQuoteBinderId
    AND qh_quote_out_id = @OldGhQuoteOutId

OPEN sl_cursor

FETCH NEXT FROM sl_cursor
    INTO @SectionLineId

WHILE @@FETCH_STATUS = 0
BEGIN

    /* Get next available Quick_Quote_Result id */
    SELECT @NewSectionLineId = Max(qho_section_line_id) + 1 FROM qho_section_line

    /* Copy record */
    INSERT INTO qho_section_line (
        qh_quote_out_id,
        gis_policy_link_id,
        qho_section_line_id,
        GIIHquote_binder_id,
        out_sl_sect_code,
        out_sl_comp_xs,
        out_sl_vol_xs,
        out_sl_sum_ins,
        out_sl_premium,
        out_sl_status_flag
        )
    SELECT @NewGhQuoteOutId,
        @NewGisPolicyLinkId,
        @NewSectionLineId,
        @NewQuoteBinderId,
        out_sl_sect_code,
        out_sl_comp_xs,
        out_sl_vol_xs,
        out_sl_sum_ins,
        out_sl_premium,
        out_sl_status_flag
    FROM qho_section_line
    WHERE gis_policy_link_id = @OldGisPolicyLinkId
    AND GIIHquote_binder_id = @OldQuoteBinderId
    AND qh_quote_out_id = @OldGhQuoteOutId
    AND qho_section_line_id = @SectionLineId

    /* Next record */
    FETCH NEXT FROM sl_cursor
        INTO @SectionLineId

END

CLOSE sl_cursor
DEALLOCATE sl_cursor
GO

