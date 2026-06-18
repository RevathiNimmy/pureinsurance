SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_SirRen_Copy_Qho_Message_Line'
GO

CREATE PROCEDURE spu_SirRen_Copy_Qho_Message_Line
    @OldGisPolicyLinkId int,
    @OldQuoteBinderId int,
    @OldGhQuoteOutId int,
    @NewQuoteBinderId int,
    @NewGisPolicyLinkId int,
    @NewGhQuoteOutId int
AS

Declare @NewMessageLineId int
Declare @MessageLineId int

/* Create cursor for all old Quick_Quote_Result records */
DECLARE ml_cursor CURSOR FAST_FORWARD FOR
    SELECT qho_message_line_id
    FROM qho_message_line
    WHERE gis_policy_link_id = @OldGisPolicyLinkId
    AND GIIHquote_binder_id = @OldQuoteBinderId
    AND qh_quote_out_id = @OldGhQuoteOutId

OPEN ml_cursor

FETCH NEXT FROM ml_cursor
    INTO @MessageLineId

WHILE @@FETCH_STATUS = 0
BEGIN

    /* Get next available Quick_Quote_Result id */
    SELECT @NewMessageLineId = Max(qho_message_line_id) + 1 FROM qho_message_line

    /* Copy record */
    INSERT INTO qho_message_line (
        qh_quote_out_id,
        gis_policy_link_id,
        qho_message_line_id,
        GIIHquote_binder_id,
        out_ml_sect_code,
        out_ml_mess_code,
        out_ml_mess_amount,
        out_ml_status_flag
        )
    SELECT @NewGhQuoteOutId,
        @NewGisPolicyLinkId,
        @NewMessageLineId,
        @NewQuoteBinderId,
        out_ml_sect_code,
        out_ml_mess_code,
        out_ml_mess_amount,
        out_ml_status_flag
    FROM qho_message_line
    WHERE gis_policy_link_id = @OldGisPolicyLinkId
    AND GIIHquote_binder_id = @OldQuoteBinderId
    AND qh_quote_out_id = @OldGhQuoteOutId
    AND qho_message_line_id = @MessageLineId

    /* Next record */
    FETCH NEXT FROM ml_cursor
        INTO @MessageLineId

END

CLOSE ml_cursor
DEALLOCATE ml_cursor
GO

