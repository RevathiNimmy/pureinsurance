EXECUTE DDLDropProcedure 'spu_GIS_Insurer_Desc_Get'
GO
CREATE PROCEDURE spu_GIS_Insurer_Desc_Get    
AS

BEGIN
  SELECT RTRIM(LTRIM(gi.description)) + ' (' + RTRIM(LTRIM(gi.code)) +')' AS Description,
            gi.gis_insurer_id,
            p.party_cnt,
            (SELECT count(s.gis_scheme_id) 
                FROM gis_scheme s
                WHERE s.gis_insurer_id = gi.gis_insurer_id
                AND (s.gis_business_type_id = 1)
                AND s.scheme_status > 0 
            ) AS live_motor_scheme_count,
            (SELECT count(s.gis_scheme_id) 
                FROM gis_scheme s
                WHERE s.gis_insurer_id = gi.gis_insurer_id
                AND (s.gis_business_type_id = 1)                 
            ) AS total_motor_scheme_count,
            
            (SELECT count(s.gis_scheme_id) 
		FROM gis_scheme s
		WHERE s.gis_insurer_id = gi.gis_insurer_id
		AND (s.gis_business_type_id = 2)
		AND s.scheme_status > 0 
	     ) AS live_home_scheme_count,
             (SELECT count(s.gis_scheme_id) 
		FROM gis_scheme s
		WHERE s.gis_insurer_id = gi.gis_insurer_id
		AND (s.gis_business_type_id = 2)                 
             ) AS total_home_scheme_count,
            
            (SELECT count(s.gis_scheme_id) 
	        FROM gis_scheme s
	        WHERE s.gis_insurer_id = gi.gis_insurer_id
	        AND (s.gis_business_type_id = 3)
	        AND s.scheme_status > 0 
	    ) AS live_cv_scheme_count,
	    (SELECT count(s.gis_scheme_id) 
	        FROM gis_scheme s
	        WHERE s.gis_insurer_id = gi.gis_insurer_id
	        AND (s.gis_business_type_id = 3)                 
             ) AS total_cv_scheme_count
     FROM GIS_Insurer gi
     LEFT JOIN Party p ON gi.abi_81_insurer = p.abi_code_on_81
         AND gi.party_cnt = p.party_cnt
      
     WHERE gi.is_deleted = 0
 ORDER BY gi.description
END

GO