SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure [spu_ACT_Get_Allocation_Period]
GO

CREATE PROCEDURE spu_ACT_Get_Allocation_Period  

AS  
  
BEGIN  
  
  
SELECT DISTINCT rtrim(CONVERT(VARCHAR,P.period_name))+ ' | ' + CONVERT(VARCHAR,P.Year_Name),c.base_currency ,p.period_end_date  
FROM Period P  
JOIN Company C ON c.company_id = P.company_id order by p.period_end_date  desc  
  
END  