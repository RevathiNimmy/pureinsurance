SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_PFPaymentMethod_Upd'
GO

CREATE PROCEDURE spu_PFPaymentMethod_Upd 
    @insurance_file_cnt INT  
AS  
BEGIN  
    UPDATE insurance_file SET payment_method='Invoice' 
	WHERE insurance_file_cnt=@insurance_file_cnt  
END 

GO
