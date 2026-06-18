SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_SirRen_Copy_Out_Hr_Items'
GO

CREATE PROCEDURE spu_SirRen_Copy_Out_Hr_Items
    @OldGisPolicyLinkId int,
    @OldQuoteBinderId int,
    @OldGhQuoteOutId int,
    @NewQuoteBinderId int,
    @NewGisPolicyLinkId int,
    @NewGhQuoteOutId int
AS

Declare @NewOutHrItemsId int
Declare @OutHrItemsId int

/* Create cursor for all old Quick_Quote_Result records */
DECLARE oh_cursor CURSOR FAST_FORWARD FOR
    SELECT out_hr_items_id
    FROM out_hr_items
    WHERE gis_policy_link_id = @OldGisPolicyLinkId
    AND GIIHquote_binder_id = @OldQuoteBinderId
    AND qh_quote_out_id = @OldGhQuoteOutId

OPEN oh_cursor

FETCH NEXT FROM oh_cursor
    INTO @OutHrItemsId

WHILE @@FETCH_STATUS = 0
BEGIN

    /* Get next available Quick_Quote_Result id */
    SELECT @NewOutHrItemsId = Max(out_hr_items_id) + 1 FROM out_hr_items

    /* Copy record */
    INSERT INTO out_hr_items (
        qh_quote_out_id,
        gis_policy_link_id,
        out_hr_items_id,
        GIIHquote_binder_id,
        out_hr_bnet,
        out_hr_value,
        out_hr_spec_yn,
        out_hr_premium,
        out_hr_status_flag
        )
    SELECT @NewGhQuoteOutId,
        @NewGisPolicyLinkId,
        @NewOutHrItemsId,
        @NewQuoteBinderId,
        out_hr_bnet,
        out_hr_value,
        out_hr_spec_yn,
        out_hr_premium,
        out_hr_status_flag
    FROM out_hr_items
    WHERE gis_policy_link_id = @OldGisPolicyLinkId
    AND GIIHquote_binder_id = @OldQuoteBinderId
    AND qh_quote_out_id = @OldGhQuoteOutId
    AND out_hr_items_id = @OutHrItemsId

    /* Next record */
    FETCH NEXT FROM oh_cursor
        INTO @OutHrItemsId

END

CLOSE oh_cursor
DEALLOCATE oh_cursor
GO

