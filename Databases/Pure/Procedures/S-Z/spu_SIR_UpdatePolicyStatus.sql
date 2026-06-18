SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

Execute DDLDropProcedure 'spu_SIR_UpdatePolicyStatus'
GO
CREATE PROCEDURE spu_SIR_UpdatePolicyStatus  
	@base_insurance_file_cnt int	
AS BEGIN  
  
	-- THIS STORED PROCEDURE IS USED TO RESET THE POLICY STATUS TO CORRECT STATUS
	-- WHEN SAVE QUOTE BUTTON IS CLICKED AFTER THE BACKDATED MTA / BACKDATED CANCELLATION PROCESS
	-- CALLED FROM THE bSIRAuotMTA.Business.UpdatePolicyStatus Method
	
	update insurance_file
	set insurance_file_status_id = MTAINS.original_insurance_file_status_id
	from insurance_file INS 
	inner join mta_insurance_file_link MTAINS on  INS.insurance_file_cnt = MTAINS.original_linked_insurance_file_cnt
    where MTAINS.insurance_file_cnt = @base_insurance_file_cnt
	  
	UPDATE insurance_file 
	SET insurance_file_type_id = 4,		-- MTAQUOTE   (MTA Quotation Permanent)
		insurance_file_status_id=NULL 	
	WHERE insurance_file_cnt = @base_insurance_file_cnt  
	AND  insurance_file_type_id = 8		--  MTACAN    (MTA Cancelled)
 
END  

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO