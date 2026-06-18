SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_SirRen_Copy_Selected_Add_On'
GO

CREATE PROCEDURE spu_SirRen_Copy_Selected_Add_On
    @OldGisPolicyLinkId int,
    @OldQuoteBinderId int,
    @OldGhQuoteOutId int,
    @NewQuoteBinderId int,
    @NewGisPolicyLinkId int,
    @NewGhQuoteOutId int
AS

Declare @NewSelectedAddOnId int
Declare @SelectedAddOnId int

/* Create cursor for all old Quick_Quote_Result records */
DECLARE sa_cursor CURSOR FAST_FORWARD FOR
    SELECT selected_add_on_id
    FROM selected_add_on
    WHERE gis_policy_link_id = @OldGisPolicyLinkId
    AND GIIHquote_binder_id = @OldQuoteBinderId
    AND qh_quote_out_id = @OldGhQuoteOutId

OPEN sa_cursor

FETCH NEXT FROM sa_cursor
    INTO @SelectedAddOnId

WHILE @@FETCH_STATUS = 0
BEGIN

    /* Get next available Quick_Quote_Result id */
    SELECT @NewSelectedAddOnId = Max(selected_add_on_id) + 1 FROM selected_add_on

    /* Copy record */
    INSERT INTO selected_add_on (
        qh_quote_out_id,
        gis_policy_link_id,
        selected_add_on_id,
        GIIHquote_binder_id,
        add_on_def_id,
        add_on_desc,
        add_on_net_cost,
        add_on_gross_cost,
        add_on_screen
        )
    SELECT @NewGhQuoteOutId,
        @NewGisPolicyLinkId,
        @NewSelectedAddOnId,
        @NewQuoteBinderId,
        add_on_def_id,
        add_on_desc,
        add_on_net_cost,
        add_on_gross_cost,
        add_on_screen
    FROM selected_add_on
    WHERE gis_policy_link_id = @OldGisPolicyLinkId
    AND GIIHquote_binder_id = @OldQuoteBinderId
    AND qh_quote_out_id = @OldGhQuoteOutId
    AND selected_add_on_id = @SelectedAddOnId

    /* Next record */
    FETCH NEXT FROM sa_cursor
        INTO @SelectedAddOnId

END

CLOSE sa_cursor
DEALLOCATE sa_cursor
GO

