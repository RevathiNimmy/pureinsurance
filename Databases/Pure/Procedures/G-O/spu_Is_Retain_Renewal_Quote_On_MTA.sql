SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Is_Retain_Renewal_Quote_On_MTA'
GO


CREATE PROCEDURE spu_Is_Retain_Renewal_Quote_On_MTA  
@insurance_file_cnt int  
AS  
BEGIN  
SELECT p.Delete_And_ReRun_RenQuote FROM insurance_file i 
			INNER JOIN  Product p ON p.product_id=i.product_id 
			WHERE i.insurance_file_cnt=@insurance_file_cnt  
END  

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO