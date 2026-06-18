SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_SirRen_Copy_Out_Cycles'
GO

CREATE PROCEDURE spu_SirRen_Copy_Out_Cycles
    @OldGisPolicyLinkId int,
    @OldQuoteBinderId int,
    @OldGhQuoteOutId int,
    @NewQuoteBinderId int,
    @NewGisPolicyLinkId int,
    @NewGhQuoteOutId int
AS

Declare @NewOutCyclesId int
Declare @OutCyclesId int

/* Create cursor for all old Quick_Quote_Result records */
DECLARE oc_cursor CURSOR FAST_FORWARD FOR
    SELECT out_cycles_id
    FROM out_cycles
    WHERE gis_policy_link_id = @OldGisPolicyLinkId
    AND GIIHquote_binder_id = @OldQuoteBinderId
    AND qh_quote_out_id = @OldGhQuoteOutId

OPEN oc_cursor

FETCH NEXT FROM oc_cursor
    INTO @OutCyclesId

WHILE @@FETCH_STATUS = 0
BEGIN

    /* Get next available Quick_Quote_Result id */
    SELECT @NewOutCyclesId = Max(out_cycles_id) + 1 FROM out_cycles

    /* Copy record */
    INSERT INTO out_cycles (
        qh_quote_out_id,
        gis_policy_link_id,
        out_cycles_id,
        GIIHquote_binder_id,
        out_cycle_value,
        out_cycle_premium,
        out_cycle_spec_yn,
        out_cycle_status_flag
        )
    SELECT @NewGhQuoteOutId,
        @NewGisPolicyLinkId,
        @NewOutCyclesId,
        @NewQuoteBinderId,
        out_cycle_value,
        out_cycle_premium,
        out_cycle_spec_yn,
        out_cycle_status_flag
    FROM out_cycles
    WHERE gis_policy_link_id = @OldGisPolicyLinkId
    AND GIIHquote_binder_id = @OldQuoteBinderId
    AND qh_quote_out_id = @OldGhQuoteOutId
    AND out_cycles_id = @OutCyclesId

    /* Next record */
    FETCH NEXT FROM oc_cursor
        INTO @OutCyclesId

END

CLOSE oc_cursor
DEALLOCATE oc_cursor
GO

