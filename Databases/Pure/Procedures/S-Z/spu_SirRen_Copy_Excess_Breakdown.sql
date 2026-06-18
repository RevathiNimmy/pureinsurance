SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_SirRen_Copy_Excess_Breakdown'
GO

CREATE PROCEDURE spu_SirRen_Copy_Excess_Breakdown
    @OldQuoteBinderId int,
    @OldGisPolicyLinkId int,
    @OldQuoteResultId int,
    @NewQuoteBinderId int,
    @NewGisPolicyLinkId int,
    @NewQuoteResultId int
AS

Declare @NewExcessBreakdownId int
Declare @ExcessBreakdownId int

/* Create cursor for all old Quick_Quote_Result records */
DECLARE eb_cursor CURSOR FAST_FORWARD FOR
    SELECT GIIMExcess_Breakdown_id
    FROM GIIMExcess_Breakdown eb
    WHERE eb.gis_policy_link_id = @OldGisPolicyLinkId
    AND eb.quote_binder_id = @OldQuoteBinderId
    AND eb.GIIMQuick_Quote_Result_id = @OldQuoteResultId

OPEN eb_cursor

FETCH NEXT FROM eb_cursor
    INTO @ExcessBreakdownId

WHILE @@FETCH_STATUS = 0
BEGIN

    /* Get next available Quick_Quote_Result id */
    SELECT @NewExcessBreakdownId = Max(GIIMExcess_Breakdown_id) + 1 FROM GIIMExcess_Breakdown

    /* Copy record */
    INSERT INTO GIIMExcess_Breakdown
        (gis_policy_link_id,
        GIIMExcess_Breakdown_id,
        Quote_Binder_id,
        GIIMQuick_Quote_Result_id,
        amt,
        section_code,
        description
        )
    SELECT @NewGisPolicyLinkId,
        @NewExcessBreakdownId,
        @NewQuoteBinderId,
        @NewQuoteResultId,
        amt,
        section_code,
        description
    FROM GIIMExcess_Breakdown
    WHERE gis_policy_link_id = @OldGisPolicyLinkId
    AND quote_binder_id = @OldQuoteBinderId
    AND GIIMQuick_Quote_Result_id = @OldQuoteResultId
    AND GIIMExcess_Breakdown_id = @ExcessBreakdownId

    /* Next record */
    FETCH NEXT FROM eb_cursor
        INTO @ExcessBreakdownId

END

CLOSE eb_cursor
DEALLOCATE eb_cursor
GO

