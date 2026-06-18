SET QUOTED_IDENTIFIER ON    
Go
SET ANSI_NULLS ON  
GO

EXECUTE DDLDropProcedure 'spu_SIR_Get_Id_From_Code'
GO

CREATE PROCEDURE spu_SIR_Get_Id_From_Code 
 @table_name varchar(50),  
 @code varchar(20)  
AS  
BEGIN  
  
If (@table_name = 'PARTY')  
    Select party_cnt From Party  
 Where shortname LIKE @code  
  
If (@table_name = 'SOURCE')  
    Select source_id From Source  
 Where code LIKE @code  
  
If (@table_name = 'PRODUCT')  
    Select Product_Id From Product  
 Where code LIKE @code  
  
If (@table_name = 'Currency')  
    Select currency_id From currency  
 Where code LIKE @code  

If (@table_name = 'Write_Off_Reason')  
    Select write_off_reason_id From Write_Off_Reason
 Where code LIKE @code 
   
END  
  
SET QUOTED_IDENTIFIER OFF  
GO
SET ANSI_NULLS OFF 
GO

