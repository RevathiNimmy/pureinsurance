SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_SirRen_Copy_Declines_Reasons'
GO

CREATE PROCEDURE spu_SirRen_Copy_Declines_Reasons
    @OldQuoteBinderId int,
    @OldGisPolicyLinkId int,
    @OldDeclinesId int,
    @NewQuoteBinderId int,
    @NewGisPolicyLinkId int,
    @NewDeclinesId int
AS

Declare @NewDeclineReasonsId int
Declare @DeclineReasonsId int

/* Create cursor for all old Quick_Quote_Result records */
DECLARE dr_cursor CURSOR FAST_FORWARD FOR
    SELECT GIIMDecline_Reasons_id
    FROM GIIMDecline_Reasons
    WHERE gis_policy_link_id = @OldGisPolicyLinkId
    AND quote_binder_id = @OldQuoteBinderId
    AND GIIMDeclines_id = @OldDeclinesId

OPEN dr_cursor

FETCH NEXT FROM dr_cursor
    INTO @DeclineReasonsId

WHILE @@FETCH_STATUS = 0
BEGIN

    /* Get next available Quick_Quote_Result id */
    SELECT @NewDeclineReasonsId = Max(GIIMDecline_Reasons_id) + 1 FROM GIIMDecline_Reasons

    /* Copy record */
    INSERT INTO GIIMDecline_Reasons (
        GIIMDecline_Reasons_id,
        gis_policy_link_id,
        Quote_Binder_id,
        GIIMDeclines_id,
        Reason,
        Code,
        Text,
        Vehicle_PRN,
        Driver_PRN
        )
    SELECT GIIMDecline_Reasons_id,
        gis_policy_link_id,
        Quote_Binder_id,
        GIIMDeclines_id,
        Reason,
        Code,
        Text,
        Vehicle_PRN,
        Driver_PRN
    FROM GIIMDecline_Reasons
    WHERE gis_policy_link_id = @OldGisPolicyLinkId
    AND quote_binder_id = @OldQuoteBinderId
    AND GIIMDeclines_id = @OldDeclinesId
    AND GIIMDecline_Reasons_id = @DeclineReasonsId

    /* Next record */
    FETCH NEXT FROM dr_cursor
        INTO @DeclineReasonsId

END

CLOSE dr_cursor
DEALLOCATE dr_cursor
GO

