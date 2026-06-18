SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_SIR_Get_Chase_Cycle_Doc_IDs'
GO


    
        
CREATE PROCEDURE spu_SIR_Get_Chase_Cycle_Doc_IDs         
    @session_id INT            
AS            
            
BEGIN            
            
 SELECT DISTINCT            
  ccs.document_template_id,            
  cci.insurance_file_cnt,            
  cci.Chase_Cycle_item_id,            
  IFL.Insurance_Folder_Cnt,        
  dt.description,        
  p.party_cnt      
            
 FROM TempIDList til            
            
  INNER JOIN Chase_Cycle_Item cci ON            
   til.link_id = cci.Chase_Cycle_item_id            
            
   INNER JOIN Insurance_File IFL ON          
    cci.Insurance_File_Cnt = IFL.Insurance_file_cnt          
              
  INNER JOIN Chase_Cycle_Step ccs ON            
   cci.Chase_Cycle_step_id = ccs.Chase_Cycle_step_id            
            
          
  INNER JOIN Document_Template dt ON          
   ccs.document_template_id=dt.document_template_id       
        
  LEFT JOIN Party p ON IFL.insured_cnt = p.party_cnt        
 WHERE til.session_id = @session_id            
            
END       
GO


