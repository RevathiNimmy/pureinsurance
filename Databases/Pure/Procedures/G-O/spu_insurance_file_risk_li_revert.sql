SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_insurance_file_risk_li_revert'
GO


CREATE PROCEDURE spu_insurance_file_risk_li_revert  
 @insurance_file_cnt int,  
 @risk_cnt int  
AS 
UPDATE 
   Insurance_file_risk_link  
SET 
   risk_cnt = original_risk_cnt,
   Status_Flag = 'U',
   original_risk_cnt = NULL  
WHERE 
   insurance_file_cnt = @insurance_file_cnt  
AND 
   risk_cnt = @risk_cnt


--PN31498
delete from insurance_file_persistent_risk_link WHERE
   insurance_file_cnt = @insurance_file_cnt
AND
   status_flag = 'U'    
AND
   risk_cnt=(select original_risk_cnt from insurance_file_persistent_risk_link WHERE insurance_file_cnt = @insurance_file_cnt AND risk_cnt = @risk_cnt)



UPDATE  
    insurance_file_persistent_risk_link  
SET  
    risk_cnt = original_risk_cnt,  
    status_flag = 'U',  
    original_risk_cnt = NULL  
WHERE  
    insurance_file_cnt = @insurance_file_cnt  
AND  
    risk_cnt = @risk_cnt  


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
