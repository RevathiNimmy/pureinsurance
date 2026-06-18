SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_Update_Risk_Edited_Indicators'
GO

CREATE PROCEDURE spu_SAM_Update_Risk_Edited_Indicators
    
@insurance_file_cnt int,    
@risk_cnt int ,  
@manually_edited int  
    
AS    
    
UPDATE insurance_file_risk_link    
SET is_manually_changed = @manually_edited, is_risk_edited =@manually_edited    
WHERE risk_cnt = @risk_cnt    
AND insurance_file_cnt = @insurance_file_cnt    
    
UPDATE insurance_file_persistent_risk_link    
SET is_manually_changed = @manually_edited    
WHERE risk_cnt = @risk_cnt    
AND insurance_file_cnt = @insurance_file_cnt    
    
UPDATE Risk SET inception_date = (SELECT cover_start_date FROM insurance_file WHERE insurance_file_cnt = @insurance_file_cnt)    
WHERE risk_cnt = (SELECT ifrl.risk_cnt FROM Risk r    
JOIN insurance_file_risk_link ifrl ON r.risk_cnt = ifrl.risk_cnt    
WHERE ifrl.insurance_file_cnt = @insurance_file_cnt AND r.risk_cnt = @risk_cnt AND ifrl.original_risk_cnt IS NULL)    
  
GO