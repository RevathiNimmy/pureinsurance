SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_SirRen_Copy_Qho_Analysis_Line'
GO

CREATE PROCEDURE spu_SirRen_Copy_Qho_Analysis_Line
    @OldGisPolicyLinkId int,
    @OldQuoteBinderId int,
    @OldGhQuoteOutId int,
    @NewQuoteBinderId int,
    @NewGisPolicyLinkId int,
    @NewGhQuoteOutId int
AS

Declare @NewAnalysisLineId int
Declare @AnalysisLineId int

/* Create cursor for all old Quick_Quote_Result records */
DECLARE al_cursor CURSOR FAST_FORWARD FOR
    SELECT qho_analysis_line_id
    FROM qho_analysis_line
    WHERE gis_policy_link_id = @OldGisPolicyLinkId
    AND GIIHquote_binder_id = @OldQuoteBinderId
    AND qh_quote_out_id = @OldGhQuoteOutId

OPEN al_cursor

FETCH NEXT FROM al_cursor
    INTO @AnalysisLineId

WHILE @@FETCH_STATUS = 0
BEGIN

    /* Get next available Quick_Quote_Result id */
    SELECT @NewAnalysisLineId = Max(qho_analysis_line_id) + 1 FROM qho_analysis_line

    /* Copy record */
    INSERT INTO qho_analysis_line (
        qh_quote_out_id,
        gis_policy_link_id,
        qho_analysis_line_id,
        GIIHquote_binder_id,
        out_al_sect_code,
        out_al_desc_code,
        out_a1_amount,
        out_a1_total
        )
    SELECT @NewGhQuoteOutId,
        @NewGisPolicyLinkId,
        @NewAnalysisLineId,
        @NewQuoteBinderId,
        out_al_sect_code,
        out_al_desc_code,
        out_a1_amount,
        out_a1_total
    FROM qho_analysis_line
    WHERE gis_policy_link_id = @OldGisPolicyLinkId
    AND GIIHquote_binder_id = @OldQuoteBinderId
    AND qh_quote_out_id = @OldGhQuoteOutId
    AND qho_analysis_line_id = @AnalysisLineId

    /* Next record */
    FETCH NEXT FROM al_cursor
        INTO @AnalysisLineId

END

CLOSE al_cursor
DEALLOCATE al_cursor
GO

