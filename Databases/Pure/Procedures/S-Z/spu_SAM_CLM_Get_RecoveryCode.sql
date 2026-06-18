SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_CLM_Get_RecoveryCode'
GO



  /*******************************************************************************************************/  
/* spu_CLM_Get_RecoveryCode                                                                            */  
/*                                                                                        */  
/* Selects Recovery Code by using recovery_id  */  
/*******************************************************************************************************/  
CREATE   PROCEDURE spu_SAM_CLM_Get_RecoveryCode
    @recovery_id int,  
 @code varchar(10) OUTPUT,
@Rec_Type_id int OUTPUT
     
AS  
BEGIN  
  
SELECT @code=code,@Rec_Type_id=recType.recovery_type_id FROM  recovery_type recType    
JOIN recovery rec ON    
rec.recovery_type_id=rectype.recovery_type_id    
WHERE rec.recovery_id=@recovery_id    
    

END 

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
 