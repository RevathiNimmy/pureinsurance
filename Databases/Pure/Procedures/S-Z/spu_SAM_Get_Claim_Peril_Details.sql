SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_Get_Claim_Peril_Details'
GO

CREATE PROCEDURE spu_SAM_Get_Claim_Peril_Details

@claim_id integer

AS


	SELECT 
		cp.Claim_Peril_id,
		cp.Claim_id,
		cp.Peril_type_id,
		pt.code as peril_type_code,
		cp.description,
		cp.comments,
		cp.sum_insured,
		cp.ri_band,
		rib.code as ri_band_code,
		cp.gis_screen_id,
		gs.code as gis_screen_code,
		cp.base_claim_peril_id,
		cp.version_id
	
	FROM Claim_Peril cp
		
	
		LEFT JOIN Peril_Type pt ON 
			pt.peril_type_id = cp.peril_type_id
		
		LEFT JOIN Gis_Screen gs ON 
			gs.gis_screen_id = cp.gis_screen_id
	
		LEFT JOIN ri_band rib ON 
			rib.ri_band_id = cp.ri_band
	
	
	WHERE claim_id = @claim_id
      	AND  (pt.is_stamp_duty_insurer<>1 OR pt.is_stamp_duty_insurer IS NULL)
      	AND  (pt.is_stamp_duty_insured<>1 OR pt.is_stamp_duty_insured IS NULL)


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
