SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'Spu_Sir_insurance_file_UpdateRenewalProduct'
GO

CREATE Procedure spu_Sir_insurance_file_UpdateRenewalProduct  
@ifileCnt int,  
@product_code varchar(50) = NULL,  
@Productid int = NULL
As  
Begin  
  
 IF @product_code is not Null  
 Begin   
	
  Update Insurance_file Set Renewal_product_id =  
    (Select product_id FROM product  
         where code = @product_code)  
  Where Insurance_file_cnt = @ifileCnt  
 End  
 IF @Productid is Not Null
 Begin   
  Update Insurance_file Set Renewal_product_id = @Productid
  Where Insurance_file_cnt = @ifileCnt  
 End 

End  

GO
