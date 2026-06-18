SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_SirRen_Copy_Notes_Breakdown'
GO

CREATE PROCEDURE spu_SirRen_Copy_Notes_Breakdown
    @OldQuoteBinderId int,
    @OldGisPolicyLinkId int,
    @OldQuoteResultId int,
    @NewQuoteBinderId int,
    @NewGisPolicyLinkId int,
    @NewQuoteResultId int
AS

Declare @NewNotesBreakdownId int
Declare @NotesBreakdownId int

/* Create cursor for all old Quick_Quote_Result records */
DECLARE nb_cursor CURSOR FAST_FORWARD FOR
    SELECT GIIMNotes_Breakdown_id
    FROM GIIMNotes_Breakdown eb
    WHERE eb.gis_policy_link_id = @OldGisPolicyLinkId
    AND eb.quote_binder_id = @OldQuoteBinderId
    AND eb.GIIMQuick_Quote_Result_id = @OldQuoteResultId

OPEN nb_cursor

FETCH NEXT FROM nb_cursor
    INTO @NotesBreakdownId

WHILE @@FETCH_STATUS = 0
BEGIN

    /* Get next available NotesBreakdown id */
    SELECT @NewNotesBreakdownId = Max(GIIMNotes_Breakdown_id) + 1 FROM GIIMNotes_Breakdown

    /* Copy record */
    INSERT INTO GIIMNotes_Breakdown (
        gis_policy_link_id,
        GIIMNotes_Breakdown_id,
        Quote_Binder_id,
        GIIMQuick_Quote_Result_id,
        code,
        description,
        value1,
        value2
        )
    SELECT @NewGisPolicyLinkId,
        @NewNotesBreakdownId,
        @NewQuoteBinderId,
        @NewQuoteResultId,
        code,
        description,
        value1,
        value2
    FROM GIIMNotes_Breakdown
    WHERE gis_policy_link_id = @OldGisPolicyLinkId
    AND quote_binder_id = @OldQuoteBinderId
    AND GIIMQuick_Quote_Result_id = @OldQuoteResultId
    AND GIIMNotes_Breakdown_id = @NotesBreakdownId

    /* Next record */
    FETCH NEXT FROM nb_cursor
        INTO @NotesBreakdownId

END

CLOSE nb_cursor
DEALLOCATE nb_cursor
GO

