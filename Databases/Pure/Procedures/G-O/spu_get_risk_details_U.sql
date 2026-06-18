SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_get_risk_details_U'
GO


CREATE PROCEDURE spu_get_risk_details_U
    @pol_id int,
    @clm_dt datetime,
    @clm_no int = NULL
AS

DECLARE @clm_risk_cnt INT
--RWH(05/03/2001) We need risk_type_id for checklist stuff.
--RWH(12/04/2001) We need risk_type description in case ther is no description for the risk itself.
IF @clm_no IS NULL
	SELECT  ris.risk_cnt,
	        ris.description,
	        ris.risk_type_id,
	        rt.description,
	        ris.inception_date,  
	  	rt.display_claims_reinsurance_screen
	
	FROM    Risk ris,
	        Risk_Type rt,
	        Insurance_File_Risk_Link ifr
	
	WHERE   ifr.insurance_file_cnt = @pol_id
	AND     ifr.risk_cnt = ris.risk_cnt
	AND     ifr.status_flag <> 'D'
	AND     rt.risk_type_id = ris.risk_type_id
	-- Tracy Richards 29/09/03 - There should only be Quoted risks on 
	-- Live policies, but make sure anyway
	-- Alix 05/02/04 - Added type 9 (quoted deferred RI)
	AND     ris.risk_status_id IN (3,9,4)
ELSE
BEGIN
	SELECT @clm_risk_cnt = risk_type_id
	FROM 	claim
	WHERE	claim_id = @clm_no

	SELECT  ris.risk_cnt,  
	        ris.description,  
	        ris.risk_type_id,  
	        rt.description,  
	        ris.inception_date,  
	   rt.display_claims_reinsurance_screen  
	  
	FROM    Risk ris,  
	        Risk_Type rt,  
	        Insurance_File_Risk_Link ifr  
	  
	WHERE   ris.risk_cnt = @clm_risk_cnt  
	AND     ifr.risk_cnt = ris.risk_cnt  
	AND     ifr.status_flag <> 'D'  
	AND     rt.risk_type_id = ris.risk_type_id  
	-- Tracy Richards 29/09/03 - There should only be Quoted risks on  
	-- Live policies, but make sure anyway  
	-- Alix 05/02/04 - Added type 9 (quoted deferred RI)  
	AND     ris.risk_status_id IN (3,9,4)  		
END
GO


