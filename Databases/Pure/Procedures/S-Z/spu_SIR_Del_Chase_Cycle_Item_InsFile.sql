
  SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_SIR_Del_Chase_Cycle_Item_InsFile'
GO


/*************************************************************************/  
/*Description: Update Chase_Cycle_Item table on the basis of insurance_file_cnt           */  
/* DATE:-06/03/2013                 */  
/*************************************************************************/  
  
create PROCEDURE spu_SIR_Del_Chase_Cycle_Item_InsFile    
 @insurance_file_cnt INT    
AS    
    
BEGIN    
 UPDATE Chase_Cycle_Item    
 SET is_deleted  =1    
 WHERE insurance_file_cnt = @insurance_File_cnt    
END    
go