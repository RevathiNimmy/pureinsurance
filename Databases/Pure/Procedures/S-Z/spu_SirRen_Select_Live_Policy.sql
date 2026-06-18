SET QUOTED_IDENTIFIER ON SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_SirRen_Select_Live_Policy'
GO

CREATE PROCEDURE spu_SirRen_Select_Live_Policy
    @insurance_folder_cnt int
AS

/* SJD 16102006 - Restructure to pick most recent of MTA PERM and POLICY */
DECLARE @iPolicy_Insurance_File_Type_ID INT
DECLARE @iMTAPerm_Insurance_File_Type_ID INT
DECLARE @iINS_FILE_CNT INT

SELECT @iPolicy_Insurance_File_Type_ID = insurance_file_type_id
                           FROM Insurance_File_Type
                           WHERE code = 'POLICY'

SELECT @iMTAPerm_Insurance_File_Type_ID = insurance_file_type_id
                           FROM Insurance_File_Type
                           WHERE code = 'MTA PERM'

SELECT @iINS_FILE_CNT = (SELECT MAX(insurance_file_cnt)
			FROM insurance_file
			WHERE insurance_folder_cnt = @insurance_folder_cnt
			AND (insurance_file_type_id = @iPolicy_Insurance_File_Type_ID or
			     insurance_file_type_id = @iMTAPerm_Insurance_File_Type_ID)
			AND insurance_file_Status_id is NULL)

SELECT i.insurance_file_cnt,                             
       i.product_id,                                     
       l.gis_scheme_id,                                  
       s.is_insurer_lead,                                
       p.party_cnt,                                      
       i.renewal_date,                                   
       l.gis_data_model_id,                              
       g.code,                                           
       l.gis_policy_link_id                              
FROM insurance_file i,                                   
      gis_policy_link l,                                 
      gis_scheme s,                                      
      party p,                                           
      gis_data_model g                                   
WHERE i.insurance_file_cnt = @iINS_FILE_CNT      
AND i.insurance_file_cnt = l.insurance_file_cnt          
AND l.gis_scheme_id = s.gis_scheme_id                    
AND p.party_cnt = i.insured_cnt                          
AND g.gis_data_model_id = l.gis_data_model_id            

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

