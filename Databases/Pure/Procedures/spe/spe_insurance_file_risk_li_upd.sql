SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_insurance_file_risk_li_upd'
GO

CREATE PROCEDURE spe_insurance_file_risk_li_upd
    @insurance_file_cnt int,  
    @risk_cnt int,  
    @status_flag varchar(1),  
    @original_risk_cnt int,
    @is_manually_changed int
AS  
BEGIN  
  
UPDATE insurance_file_risk_link  
    SET  
    status_flag=@status_flag,  
    original_risk_cnt=@original_risk_cnt,
    is_manually_changed=@is_manually_changed
    WHERE insurance_file_cnt = @insurance_file_cnt  
    AND risk_cnt = @risk_cnt  
  
UPDATE insurance_file_persistent_risk_link  
    SET  
    status_flag=@status_flag,  
    original_risk_cnt=@original_risk_cnt,
    is_manually_changed=@is_manually_changed  
    WHERE insurance_file_cnt = @insurance_file_cnt  
    AND risk_cnt = @risk_cnt  
  
END  

GO