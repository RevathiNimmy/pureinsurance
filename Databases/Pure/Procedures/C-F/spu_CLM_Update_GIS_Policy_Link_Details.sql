SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_CLM_Update_GIS_Policy_Link_Details'
GO

CREATE PROCEDURE spu_CLM_Update_GIS_Policy_Link_Details  
 @gis_policy_link_id int,  
 @claim_id   int = -1
AS  
BEGIN  
 IF (@claim_id <> -1)  
 BEGIN  
  UPDATE  gis_policy_link WITH (ROWLOCK)
         SET  claim_id = @claim_id  
      WHERE  gis_policy_link_id = @gis_policy_link_id  

 END  
END



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
