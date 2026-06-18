SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_SirRen_Copy_Out_Pb_Items'
GO

CREATE PROCEDURE spu_SirRen_Copy_Out_Pb_Items
    @OldGisPolicyLinkId int,
    @OldQuoteBinderId int,
    @OldGhQuoteOutId int,
    @NewQuoteBinderId int,
    @NewGisPolicyLinkId int,
    @NewGhQuoteOutId int
AS

Declare @NewOutPbItemsId int
Declare @OutPbItemsId int

/* Create cursor for all old Quick_Quote_Result records */
DECLARE ob_cursor CURSOR FAST_FORWARD FOR
    SELECT out_pb_items_id
    FROM out_pb_items
    WHERE gis_policy_link_id = @OldGisPolicyLinkId
    AND GIIHquote_binder_id = @OldQuoteBinderId
    AND qh_quote_out_id = @OldGhQuoteOutId

OPEN ob_cursor

FETCH NEXT FROM ob_cursor
    INTO @OutPbItemsId

WHILE @@FETCH_STATUS = 0
BEGIN

    /* Get next available Quick_Quote_Result id */
    SELECT @NewOutPbItemsId = Max(out_pb_items_id) + 1 FROM out_pb_items

    /* Copy record */
    INSERT INTO out_pb_items (
        qh_quote_out_id,
        gis_policy_link_id,
        out_pb_items_id,
        GIIHquote_binder_id,
        out_pb_bnet,
        out_pb_value,
        out_pb_comp_xs,
        out_pb_spec_yn,
        out_pb_premium,
        out_pb_status_flag
        )
    SELECT @NewGhQuoteOutId,
        @NewGisPolicyLinkId,
        @NewOutPbItemsId,
        @NewQuoteBinderId,
        out_pb_bnet,
        out_pb_value,
        out_pb_comp_xs,
        out_pb_spec_yn,
        out_pb_premium,
        out_pb_status_flag
    FROM out_pb_items
    WHERE gis_policy_link_id = @OldGisPolicyLinkId
    AND GIIHquote_binder_id = @OldQuoteBinderId
    AND qh_quote_out_id = @OldGhQuoteOutId
    AND out_pb_items_id = @OutPbItemsId

    /* Next record */
    FETCH NEXT FROM ob_cursor
        INTO @OutPbItemsId

END

CLOSE ob_cursor
DEALLOCATE ob_cursor
GO

