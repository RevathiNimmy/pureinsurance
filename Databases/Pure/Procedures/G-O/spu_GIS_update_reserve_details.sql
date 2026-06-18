SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_GIS_update_reserve_details'
GO

 CREATE PROCEDURE spu_GIS_update_reserve_details    
    @reserveid int,    
    @initialreserve currency,    
    @this_revision currency,    
    @transaction_type varchar(5)  
AS    
    
BEGIN    
IF RTRIM(@transaction_type)='C_CO'  
 BEGIN  
 UPDATE  Reserve    
 SET    
  initial_reserve=@initialreserve    
   WHERE  reserve_id = @reserveid    
  END  
ELSE  
 UPDATE  Reserve    
 SET    
     this_revision=@this_revision    
   WHERE  reserve_id = @reserveid    
END    



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
