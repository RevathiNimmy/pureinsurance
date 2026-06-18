SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_Get_Account_Base_Country'
GO


CREATE PROCEDURE spu_Get_Account_Base_Country  
@account_id int  
  
AS  
  
BEGIN  
  
	SELECT country_id
	FROM source  
	WHERE source_id =  
		(SELECT company_id  
		 FROM account  
	         WHERE account_id=@account_id)  
  
END  

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
