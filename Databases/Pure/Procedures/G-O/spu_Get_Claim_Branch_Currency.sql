SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_Get_Claim_Branch_Currency'
GO

CREATE PROCEDURE spu_Get_Claim_Branch_Currency  
  
@claim_id int  
  
AS  
  
BEGIN  
  
	SELECT currency_id, description  
	FROM currency  
	WHERE currency_id in  
		(SELECT currency_id  
		 FROM companycurrency  
	 	 WHERE company_id in  
			(SELECT source_id  
		 	 FROM insurance_file  
			 WHERE Insurance_file_cnt in  
				(SELECT Policy_id  
				 FROM Claim  
				 WHERE claim_id =@claim_id)))  
  
END  


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
