SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_Get_Bureau_Details'
GO
CREATE PROCEDURE spu_Get_Bureau_Details  
    @party_cnt int
AS  
BEGIN  
SELECT 	PAI.bureauaccountparty,
		ISNULL(P.resolved_name,P.name) 
FROM 
		party_insurer PAI 
JOIN 
		party P ON PAI.bureauaccountparty = P.party_cnt 
WHERE 
		PAI.party_cnt = @party_cnt
    
END  

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
