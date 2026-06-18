SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_update_risk_folder_for_risk'  
GO

CREATE PROCEDURE [spu_update_risk_folder_for_risk]  
    @risk_cnt INT,  
    @risk_folder_cnt INT  
AS  
  
UPDATE  risk  
SET     risk_folder_cnt = @risk_folder_cnt,
		inception_date = ifi.cover_start_date
FROM	Risk r
INNER JOIN insurance_file_risk_link ifrl ON ifrl.risk_cnt = r.risk_cnt
INNER JOIN Insurance_File ifi ON ifi.insurance_file_cnt = ifrl.insurance_file_cnt
WHERE   r.risk_cnt = @risk_cnt