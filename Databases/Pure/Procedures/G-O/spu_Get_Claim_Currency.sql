SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_Get_Claim_Currency'
GO

CREATE PROCEDURE spu_Get_Claim_Currency  

	@claim_id INT  
AS  
  
BEGIN  

	SELECT cur.currency_id, cur.description  
	FROM claim cl  
		JOIN currency cur  ON 
			cl.currency_id = cur.currency_id  
	WHERE cl.claim_id = @claim_id  
END  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
