SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_SirRen_Copy_Premium_Analysis'
GO

CREATE PROCEDURE spu_SirRen_Copy_Premium_Analysis
    @OldQuoteBinderId int,
    @OldGisPolicyLinkId int,
    @OldQuoteResultId int,
    @NewQuoteBinderId int,
    @NewGisPolicyLinkId int,
    @NewQuoteResultId int
AS

Declare @NewPremiumAnalysisId int
Declare @PremiumAnalysisId int

/* Create cursor for all old Quick_Quote_Result records */
DECLARE pa_cursor CURSOR FAST_FORWARD FOR
    SELECT GIIMPremium_analysis_id
    FROM GIIMPremium_analysis pa
    WHERE pa.gis_policy_link_id = @OldGisPolicyLinkId
    AND pa.quote_binder_id = @OldQuoteBinderId
    AND pa.GIIMQuick_Quote_Result_id = @OldQuoteResultId

OPEN pa_cursor

FETCH NEXT FROM pa_cursor
    INTO @PremiumAnalysisId

WHILE @@FETCH_STATUS = 0
BEGIN

    /* Get next available Premium Analysis id */
    SELECT @NewPremiumAnalysisId = Max(GIIMPremium_analysis_id) + 1 FROM GIIMPremium_Analysis

    /* Copy record */
    INSERT INTO GIIMPremium_Analysis (
        gis_policy_link_id,
        GIIMPremium_analysis_id,
        Quote_Binder_id,
        GIIMQuick_Quote_Result_id,
        code,
        description,
        amount,
        running_total
        )
    SELECT @NewGisPolicyLinkId,
        @NewPremiumAnalysisId,
        @NewQuoteBinderId,
        @NewQuoteResultId,
        code,
        description,
        amount,
        running_total
    FROM GIIMPremium_Analysis
    WHERE gis_policy_link_id = @OldGisPolicyLinkId
    AND quote_binder_id = @OldQuoteBinderId
    AND GIIMQuick_Quote_Result_id = @OldQuoteResultId
    AND GIIMPremium_analysis_id = @PremiumAnalysisId

    /* Next record */
    FETCH NEXT FROM pa_cursor
        INTO @PremiumAnalysisId

END

CLOSE pa_cursor
DEALLOCATE pa_cursor
GO

