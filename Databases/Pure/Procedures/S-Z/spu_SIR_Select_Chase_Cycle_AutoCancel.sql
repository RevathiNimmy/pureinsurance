  SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_SIR_Select_Chase_Cycle_AutoCancel'
GO


/*************************************************************************/  
/*Description: Select insured_cnt,insurance_folder_cnt from Chase_Cycle_Item  table   on basis of @Chase_Cycle_item_id            */  
/* DATE:-06/03/2013                 */  
/*************************************************************************/  
  
create PROCEDURE spu_SIR_Select_Chase_Cycle_AutoCancel    
    @Chase_Cycle_item_id INT    
AS    
    SELECT    
        I.insured_cnt,    
        I.insurance_folder_cnt    
    FROM    
        Chase_Cycle_Item CCI    
    JOIN    
        Insurance_File I ON I.insurance_file_cnt=CCI.insurance_file_cnt    
    WHERE    
        CCI.Chase_Cycle_item_id=@Chase_Cycle_item_id    
        go