SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDROPPROCEDURE spu_get_risk_details_for_claim
GO
CREATE PROCEDURE spu_get_risk_details_for_claim
    @pol_id int
AS  
  
SELECT  ris.risk_cnt,  
        ris.description,  
        ris.risk_type_id,  
        rt.description,  
        ris.inception_date,  
   rt.display_claims_reinsurance_screen  
  
FROM    Risk ris,  
        Risk_Type rt,  
  Insurance_File_Risk_Link ifr  
WHERE  ifr.status_flag <> 'D'  
AND    ifr.risk_cnt = ris.risk_cnt  
AND    rt.risk_type_id = ris.risk_type_id  
AND		ifr.insurance_file_cnt=@pol_id
AND    ris.risk_status_id IN (3,9,4)  

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
