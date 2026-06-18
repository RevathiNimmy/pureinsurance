SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_Renewal_Status_Invite_Printed_upd'
GO

CREATE PROCEDURE spu_Renewal_Status_Invite_Printed_upd    
 @renewal_insurance_file_cnt INT,    
 @renewal_exception_reason_ID INT = NULL,  
 @renewal_exception_note varchar(255)= ''  
AS    
  
IF ISNULL(@renewal_exception_reason_ID,0) > 0     
BEGIN  
	UPDATE  Renewal_Status    
	SET  renewal_status_type_id = 2,    
	is_invite_Printed = 0,    
	date_invite_printed = GETDATE(),    
	renewal_exception_reason_id = @renewal_exception_reason_ID,  
	renewal_exception_notes = @renewal_exception_note  
	WHERE  renewal_insurance_file_cnt = @renewal_insurance_file_cnt    
END ELSE  
  
BEGIN    
	UPDATE  Renewal_Status    
	SET  renewal_status_type_id = 5,    
	is_invite_Printed = 1,    
	date_invite_printed = GETDATE(),
	renewal_exception_notes = ''      
	WHERE  renewal_insurance_file_cnt = @renewal_insurance_file_cnt    
END  
GO 

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
