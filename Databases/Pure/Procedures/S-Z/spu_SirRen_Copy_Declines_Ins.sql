SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_SirRen_Copy_Declines_Ins'
GO

CREATE PROCEDURE spu_SirRen_Copy_Declines_Ins
    @OldQuoteBinderId int,
    @OldGisPolicyLinkId int,
    @NewQuoteBinderId int,
    @NewGisPolicyLinkId int
AS

Declare @NewDeclinesId int
Declare @DeclinesId int

/* Create cursor for all old Quick_Quote_Result records */
DECLARE dec_cursor CURSOR FAST_FORWARD FOR
    SELECT GIIMDeclines_id
    FROM GIIMDeclines
    WHERE gis_policy_link_id = @OldGisPolicyLinkId
    AND quote_binder_id = @OldQuoteBinderId

OPEN dec_cursor

FETCH NEXT FROM dec_cursor
    INTO @DeclinesId
WHILE @@FETCH_STATUS = 0
BEGIN

    /* Get next available Quick_Quote_Result id */
    SELECT @NewDeclinesId = Max(GIIMDeclines_id) + 1 FROM GIIMDeclines

    /* Copy record */
    INSERT INTO GIIMDeclines (
        gis_policy_link_id,
        Quote_Binder_id,
        GIIMDeclines_id,
        Scheme_ID,
        Scheme_Desc,
        Commission_rate,
        Commission_amount,
        Scheme_Type,
        Cover_type
        )
    SELECT @NewGisPolicyLinkId,
        @NewQuoteBinderId,
        @NewDeclinesId,
        Scheme_ID,
        Scheme_Desc,
        Commission_rate,
        Commission_amount,
        Scheme_Type,
        Cover_type
    FROM GIIMDeclines
    WHERE gis_policy_link_id = @OldGisPolicyLinkId
    AND quote_binder_id = @OldQuoteBinderId
    AND GIIMDeclines_id = @DeclinesId

    /* Copy child-relationship records */
    EXECUTE spu_SirRen_Copy_Declines_Reasons @OldQuoteBinderId, @OldGisPolicyLinkId, @DeclinesId, @NewQuoteBinderId, @NewGisPolicyLinkId, @NewDeclinesId

    /* Next record */
    FETCH NEXT FROM dec_cursor
        INTO @DeclinesId
END

CLOSE dec_cursor
DEALLOCATE dec_cursor
GO

