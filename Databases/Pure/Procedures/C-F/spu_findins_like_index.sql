SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure spu_findins_like_index
GO

CREATE PROCEDURE spu_findins_like_index
    @index_value VARCHAR(30)  
AS  
  
/* End of user defined lookup maintenance rotuines */  
BEGIN 
 
    SELECT  ifile.insurance_file_id,
            ifile.source_id ins_file_source_id,  
            ifile.insurance_file_cnt,  
            ifile.insurance_ref,  
            fold.code insurance_folder_code,  
            type.code type_code,  
            part.name insured_name,  
            part.shortname insured_shortname,  
            part.party_id,  
            part.source_id party_source_id,  
            sys.last_modified,  
            fold.insurance_holder_cnt,  
            fold.insurance_folder_cnt,  
            ifile.product_id,  
            prod.code,  
            cap.caption,  
            ifile.lead_agent_cnt,  
            sys.date_created,  
            GIS.property_name,  
            search.value  
    FROM    insurance_file ifile
        INNER JOIN insurance_file_system sys
            ON ifile.insurance_file_cnt = sys.insurance_file_cnt
        INNER JOIN insurance_folder fold
            ON ifile.insurance_folder_cnt = fold.insurance_folder_cnt  
        INNER JOIN insurance_file_type type
            ON ifile.insurance_file_type_id = type.insurance_file_type_id  
        INNER JOIN party part
            ON fold.insurance_holder_cnt = part.party_cnt  
        INNER JOIN product prod
            ON ifile.product_id = prod.product_id  
        INNER JOIN GIS_Policy_Link GPL
            ON ifile.insurance_file_cnt = GPL.insurance_file_cnt
        INNER JOIN GIS_search_property search  
            ON GPL.gis_policy_link_id = search.gis_policy_link_id  
        INNER JOIN GIS_Property GIS
            ON GIS.gis_property_id = search.gis_property_id    
        LEFT OUTER JOIN pmcaption cap
            ON prod.caption_id = cap.caption_id  
    WHERE (ifile.insurance_file_status_id IS NULL  
            OR ifile.insurance_file_status_id = (
                            SELECT  insurance_file_status_id  
                            FROM Insurance_File_Status  
                            WHERE code = 'REN')  
            )  
        AND search.Value LIKE @index_value + "%"  
    ORDER BY sys.date_created DESC  

END 

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO