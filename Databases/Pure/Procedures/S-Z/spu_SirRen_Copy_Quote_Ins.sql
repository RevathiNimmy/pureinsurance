SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON
GO


EXECUTE DDLDropProcedure 'spu_SirRen_Copy_Quote_Ins'
GO


CREATE PROCEDURE spu_SirRen_Copy_Quote_Ins
    @NewQuoteBinderId int OUTPUT,
    @NewGisPolicyLinkId int,
    @OldQuoteBinderId int,
    @OldInsuranceFileCnt int,
    @OldGisSchemeId int,
    @GisBusinessTypeId int
AS


Declare @OldGisPolicyLinkId int
Declare @GisBusinessTypeCode varchar(4)
	
	/* Select the Gis Business Type */
	SELECT @GisBusinessTypeCode = gbt.code FROM GIS_Business_Type gbt WHERE gbt.gis_business_type_id = @GisBusinessTypeId

	
	/* Retrieve the old gis policy link id */
	SELECT @OldGisPolicyLinkId = gis_policy_link_id FROM gis_policy_link WHERE insurance_file_cnt = @OldInsuranceFileCnt

	/* Copy Quote Relationships */
	IF  @GisBusinessTypeCode = 'GIIM'
		BEGIN
		    /* Retrieve the next quote binder id */
	        SELECT @NewQuoteBinderId = MAX(quote_binder_id) + 1 FROM quote_binder
	        /* Insert new quote_binder record */
			INSERT INTO Quote_Binder
				(gis_policy_link_id,
				quote_binder_id,
				gis_scheme_id)
			VALUES
				(@NewGisPolicyLinkId, 
				@NewQuoteBinderId,
				@OldGisSchemeId)

			EXEC spu_SirRen_Copy_Quick_Quote_Res_Ins @OldQuoteBinderId,  @OldGisPolicyLinkId, @NewQuoteBinderId, @NewGisPolicyLinkId
			EXEC spu_SirRen_Copy_Declines_Ins @OldQuoteBinderId,  @OldGisPolicyLinkId, @NewQuoteBinderId, @NewGisPolicyLinkId
		END
	
	IF  @GisBusinessTypeCode = 'GIIH'
		BEGIN
		    /* Retrieve the next quote binder id */
	        SELECT @NewQuoteBinderId = MAX(giihquote_binder_id) + 1 FROM giihquote_binder
			
			INSERT INTO GIIHQuote_Binder
				(gis_policy_link_id,
				GIIHQuote_binder_id,
				gis_scheme_id)
			VALUES
				(@NewGisPolicyLinkId, 
				@NewQuoteBinderId,
				@OldGisSchemeId)

			EXEC spu_SirRen_Copy_Qh_Quote_Out_Ins @OldQuoteBinderId,  @OldGisPolicyLinkId, @NewQuoteBinderId, @NewGisPolicyLinkId
		END

	EXEC spu_SirRen_Copy_Quote_Error @OldQuoteBinderId,  @OldGisPolicyLinkId, @NewQuoteBinderId, @NewGisPolicyLinkId
GO


SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON
GO


