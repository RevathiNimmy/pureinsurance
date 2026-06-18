SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure Spu_GetBasePolicyCntForBackDateMTA
GO

CREATE PROCEDURE Spu_GetBasePolicyCntForBackDateMTA
  @InsFolderCnt INT,  
  @MTAEffectiveDate DATETIME  
AS  
BEGIN  

    SELECT
        ifi.insurance_file_cnt,  
        ifi.policy_version,  
        ifs.last_trans_date,  
        ift.code,  
        ifi.cover_start_date,  
        ifi.expiry_date,  
        ifst.code,  
        s.description,  
        s.is_deleted,  
        s.closed_allow_temp_mta,  
        s.closed_allow_perm_mta  
    FROM    
        insurance_file ifi
        INNER JOIN insurance_file_system ifs
            ON ifi.insurance_file_cnt = ifs.insurance_file_cnt  
            AND  ifi.policy_ignore IS NULL
            AND ifi.insurance_folder_cnt = @InsFolderCnt  
            AND ifi.cover_start_date <= @MTAEffectiveDate  
        INNER JOIN insurance_file_type ift
            ON ifi.insurance_file_type_id = ift.insurance_file_type_id
             AND RTRIM(ift.code) IN ('MTA PERM','POLICY','MTAREINS', 'MTACAN')    
        INNER JOIN source s  
            ON ifi.source_id = s.source_id  
        LEFT OUTER JOIN insurance_file_status ifst
            ON ifi.insurance_file_status_id = ifst.insurance_file_status_id            	 
    ORDER BY
        ifi.cover_start_date DESC,
        ifi.insurance_file_cnt DESC ,
        ifs.last_trans_date DESC  

END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO