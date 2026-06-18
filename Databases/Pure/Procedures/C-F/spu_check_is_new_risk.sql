SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
EXECUTE DDLDropProcedure 'spu_check_is_new_risk'
GO

CREATE PROCEDURE spu_check_is_new_risk  
@nRisk_cnt INT,  
@is_new tinyint OUT  
As  
  
DECLARE @base_risk_cnt INT  
  
select @base_risk_cnt=MIN(risk_cnt) from risk where risk_folder_cnt=  
(select risk_folder_cnt from risk where risk_cnt=@nRisk_cnt)  
  
if @base_risk_cnt=@nRisk_cnt  
SELECT @is_new=1  
else  
SELECT @is_new=0  

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
  