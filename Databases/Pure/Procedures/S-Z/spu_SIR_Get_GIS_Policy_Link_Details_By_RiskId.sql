SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SIR_Get_GIS_Policy_Link_Details_By_RiskId'
GO

CREATE PROCEDURE spu_SIR_Get_GIS_Policy_Link_Details_By_RiskId  
  
@risk_id  int  
  
AS  
  
BEGIN  
  
	SELECT gis_policy_link_id 
	FROM gis_policy_link 
	WHERE risk_id =@risk_id  
	ORDER BY gis_policy_link_id
  
END   


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
