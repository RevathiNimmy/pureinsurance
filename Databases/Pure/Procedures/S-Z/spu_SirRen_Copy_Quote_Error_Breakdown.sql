SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_SirRen_Copy_Quote_Error_Breakdown'
GO

CREATE PROCEDURE spu_SirRen_Copy_Quote_Error_Breakdown
    @OldQuoteBinderId int,
    @OldGisPolicyLinkId int,
    @OldQuoteErrorId int,
    @NewQuoteBinderId int,
    @NewGisPolicyLinkId int,
    @NewQuoteErrorId int
AS

Declare @NewQuoteErrorBreakdownId int
Declare @QuoteErrorBreakdownId int

/* Create cursor for all old Quick_Quote_Result records */
DECLARE eb_cursor CURSOR FAST_FORWARD FOR
    SELECT GIIMQuote_Error_Breakdown_id
    FROM GIIMQuote_Error_Breakdown
    WHERE GIIMQuote_Error_id = @OldGisPolicyLinkId
    AND quote_binder_id = @OldQuoteBinderId
    AND GIIMQuote_Error_id = @OldQuoteErrorId

OPEN eb_cursor

FETCH NEXT FROM eb_cursor
    INTO @QuoteErrorBreakdownId

WHILE @@FETCH_STATUS = 0
BEGIN

    /* Get next available Quick_Quote_Result id */
    SELECT @NewQuoteErrorBreakdownId = Max(GIIMQuote_Error_Breakdown_id) + 1 FROM GIIMQuote_Error_Breakdown

    /* Copy record */
    INSERT INTO GIIMQuote_Error_Breakdown (
        GIIMQuote_Error_Breakdown_id,
        GIIMQuote_Error_id,
        gis_policy_link_id,
        Quote_Binder_id,
        Error_Level,
        Screen_Name,
        Description
        )
    SELECT @NewQuoteErrorBreakdownId,
        @NewQuoteErrorId,
        @NewGisPolicyLinkId,
        @NewQuoteBinderId,
        Error_Level,
        Screen_Name,
        Description
    FROM GIIMQuote_Error_Breakdown
    WHERE gis_policy_link_id = @OldGisPolicyLinkId
    AND quote_binder_id = @OldQuoteBinderId
    AND GIIMQuote_Error_id = @OldQuoteErrorId
    AND GIIMQuote_Error_Breakdown_id = @QuoteErrorBreakdownId

    /* Next record */
    FETCH NEXT FROM eb_cursor
        INTO @QuoteErrorBreakdownId

END

CLOSE eb_cursor
DEALLOCATE eb_cursor
GO

