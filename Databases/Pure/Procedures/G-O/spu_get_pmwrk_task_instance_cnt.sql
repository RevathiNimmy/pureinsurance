SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_get_pmwrk_task_instance_cnt'
GO

CREATE PROCEDURE spu_get_pmwrk_task_instance_cnt  
    @KeyName varchar(50),  
    @KeyValue varchar(50)  
  
AS  


IF @keyName = 'claim_id'
BEGIN
	SELECT     PMWrk_Task_Inst_Key.pmwrk_task_instance_cnt  
	FROM       PMWrk_Task_Inst_Key INNER JOIN  
	                      PMNav_Key ON PMWrk_Task_Inst_Key.pmnav_key_id = PMNav_Key.pmnav_key_id
	-- PN-70856 by Sushil Kumar	  
	WHERE     (PMWrk_Task_Inst_Key.key_value in (Select CAST(Claim_Id AS VARCHAR) from Claim Where Base_Claim_Id in (Select base_claim_id from claim where claim_id = @KeyValue)))
	AND (PMNav_Key.name =@KeyName)  
END
ELSE
BEGIN
	SELECT     PMWrk_Task_Inst_Key.pmwrk_task_instance_cnt  
	FROM         PMWrk_Task_Inst_Key INNER JOIN  
	                      PMNav_Key ON PMWrk_Task_Inst_Key.pmnav_key_id = PMNav_Key.pmnav_key_id  
	WHERE     (PMWrk_Task_Inst_Key.key_value =@KeyValue) AND (PMNav_Key.name =@KeyName)  
END	


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
