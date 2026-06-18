SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_SirRen_Copy_Quote_Error'
GO

CREATE PROCEDURE spu_SirRen_Copy_Quote_Error
    @OldQuoteBinderId int,
    @OldGisPolicyLinkId int,
    @NewQuoteBinderId int,
    @NewGisPolicyLinkId int
AS

Declare @NewQuoteErrorId int
Declare @QuoteErrorId int

/* Create cursor for all old Quick_Quote_Result records */
DECLARE qe_cursor CURSOR FAST_FORWARD FOR
    SELECT GIIMQuote_Error_id
    FROM GIIMQuote_Error
    WHERE gis_policy_link_id = @OldGisPolicyLinkId
    AND quote_binder_id = @OldQuoteBinderId

OPEN qe_cursor

FETCH NEXT FROM qe_cursor
    INTO @QuoteErrorId
WHILE @@FETCH_STATUS = 0
BEGIN

    /* Get next available Quick_Quote_Result id */
    SELECT @NewQuoteErrorId = Max(GIIMQuote_Error_id) + 1 FROM GIIMQuote_Error

    /* Copy record */
    INSERT INTO GIIMQuote_Error (
        GIIMQuote_Error_id,
        gis_policy_link_id,
        Quote_Binder_id,
        Scheme_Id,
        Scheme_Description
        )
    SELECT @NewQuoteErrorId,
        @NewGisPolicyLinkId,
        @NewQuoteBinderId,
        Scheme_Id,
        Scheme_Description
    FROM GIIMQuote_Error
    WHERE gis_policy_link_id = @OldGisPolicyLinkId
    AND quote_binder_id = @OldQuoteBinderId
    AND GIIMQuote_Error_id = @QuoteErrorId

    /* Copy child-relationship records */
    EXECUTE spu_SirRen_Copy_Quote_Error_Breakdown @OldQuoteBinderId, @OldGisPolicyLinkId, @QuoteErrorId, @NewQuoteBinderId, @NewGisPolicyLinkId, @NewQuoteErrorId

    /* Next record */
    FETCH NEXT FROM qe_cursor
        INTO @QuoteErrorId
END

CLOSE qe_cursor
DEALLOCATE qe_cursor
GO

