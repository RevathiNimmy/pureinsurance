SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_ACT_Get_Document_Details_For_Account'
GO

CREATE PROCEDURE spu_ACT_Get_Document_Details_For_Account  
 @account_id int,
 @document_id int  
AS  
  
BEGIN  
  
 SELECT  SUM(Currency_amount) currency_amount,   
	 td.currency_id  
                
 FROM transdetail  td  

 WHERE document_id =@document_id
 AND account_id = @account_id

 GROUP by currency_id

END  




GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
