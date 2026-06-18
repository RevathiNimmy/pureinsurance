SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_SIR_Update_GIS_Policy_Link'
GO

CREATE PROCEDURE spu_SIR_Update_GIS_Policy_Link  
  
 @insurance_file_cnt int,  
 @gis_policy_link_id int  

 AS  
    BEGIN  
  
		UPDATE gis_policy_link 
		SET insurance_file_cnt = @insurance_file_cnt			
		WHERE gis_policy_link_id = @gis_policy_link_id  
	  
    END
      
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO