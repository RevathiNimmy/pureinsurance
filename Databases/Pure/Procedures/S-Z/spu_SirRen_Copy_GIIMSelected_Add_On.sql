SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_SirRen_Copy_GIIMSelected_Add_On'
GO

CREATE PROCEDURE spu_SirRen_Copy_GIIMSelected_Add_On
    @OldQuoteBinderId int,
    @OldGisPolicyLinkId int,
    @OldQuoteResultId int,
    @NewQuoteBinderId int,
    @NewGisPolicyLinkId int,
    @NewQuoteResultId int
AS

Declare @NewSelectedAddOnId int
Declare @SelectedAddOnId int

/* Create cursor for all old Quick_Quote_Result records */
DECLARE sa_cursor CURSOR FAST_FORWARD FOR
    SELECT GIIMSelected_Add_On_id
    FROM GIIMSelected_Add_On sa
    WHERE sa.gis_policy_link_id = @OldGisPolicyLinkId
    AND sa.quote_binder_id = @OldQuoteBinderId
    AND sa.GIIMQuick_Quote_Result_id = @OldQuoteResultId

OPEN sa_cursor

FETCH NEXT FROM sa_cursor
    INTO @SelectedAddOnId

WHILE @@FETCH_STATUS = 0
BEGIN

    /* Get next available Premium Analysis id */
    SELECT @NewSelectedAddOnId = Max(GIIMSelected_Add_On_id) + 1 FROM GIIMSelected_Add_On

    /* Copy record */
    INSERT INTO GIIMSelected_Add_On (
        gis_policy_link_id,
        GIIMSelected_Add_On_id,
        Quote_Binder_id,
        GIIMQuick_Quote_Result_id,
        Add_On_Def_ID,
        Description,
        Net_Cost,
        Gross_Cost
        )
    SELECT @NewGisPolicyLinkId,
        @NewSelectedAddOnId,
        @NewQuoteBinderId,
        @NewQuoteResultId,
        Add_On_Def_ID,
        Description,
        Net_Cost,
        Gross_Cost
    FROM GIIMSelected_Add_On
    WHERE gis_policy_link_id = @OldGisPolicyLinkId
    AND quote_binder_id = @OldQuoteBinderId
    AND GIIMQuick_Quote_Result_id = @OldQuoteResultId
    AND GIIMSelected_Add_On_id = @SelectedAddOnId

    /* Next record */
    FETCH NEXT FROM sa_cursor
        INTO @SelectedAddOnId

END

CLOSE sa_cursor
DEALLOCATE sa_cursor
GO

