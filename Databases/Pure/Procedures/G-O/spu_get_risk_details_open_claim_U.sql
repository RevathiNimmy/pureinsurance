SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_get_risk_details_open_claim_U'
GO

CREATE PROCEDURE spu_get_risk_details_open_claim_U
    @pol_id int,
    @clm_dt datetime
AS

SELECT  DISTINCT ris.risk_cnt,
	        ris.description,
	        ris.risk_type_id,
	        rt.description,
	        ris.inception_date,  
	  	rt.display_claims_reinsurance_screen , 
	    ris.risk_number 
	FROM    Risk ris,
	        Risk_Type rt,
	        Insurance_File_Risk_Link ifr
	
	WHERE   ifr.insurance_file_cnt = @pol_id
	AND     ifr.risk_cnt = ris.risk_cnt
	AND     ifr.status_flag <> 'D'
	AND     rt.risk_type_id = ris.risk_type_id
	AND     ris.risk_status_id IN (3,9,4)
    ORDER BY ris.risk_number 
