SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

--**********************************************************************************************
-- Removes all corresponding Risk Data for a Policy
--**********************************************************************************************

EXECUTE DDLDropProcedure 'spu_SIR_Delete_Risk_Data'
GO

CREATE PROCEDURE spu_SIR_Delete_Risk_Data      
    @insurance_file_cnt INT      
AS      
BEGIN   
     
    DECLARE @gis_policy_link_id INT  
  
    SELECT @gis_policy_link_id = gis_policy_link_id FROM GIS_Policy_Link  
    WHERE insurance_file_cnt = @insurance_file_cnt  
  
    EXEC spu_SIR_Delete_GIS_Data @gis_policy_link_id  
  
    DELETE FROM insurance_file_risk_link  
    WHERE insurance_file_cnt = @insurance_file_cnt  
  
END

GO

