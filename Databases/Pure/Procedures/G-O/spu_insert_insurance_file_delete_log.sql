SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_insert_insurance_file_delete_log'
GO

CREATE PROCEDURE spu_insert_insurance_file_delete_log  
@InsuranceFileCnt INT,  
@Status INT,  
@FailureDescription varchar(255) = ''  
  
As  
BEGIN  
INSERT INTO INSURANCE_FILE_DELETE_LOG  
(  
 insurance_file_cnt,  
 status,  
 deletion_date,  
 failure_description,  
 insurance_ref,  
 insured_cnt,  
 product_id,  
 lead_agent_cnt,
 quote_version,
 quote_status_description
)  
SELECT  
@InsuranceFileCnt,  
@Status,  
GETDATE(),  
@FailureDescription,  
IFL.insurance_ref,  
ISNULL(IFL.insured_cnt,0),  
ISNULL(IFL.product_id,0),  
ISNULL(IFL.lead_agent_cnt,0),
ISNULL(IFL.quote_version,0),
(SELECT description from quote_status QS where QS.quote_status_id = IFL.quote_status_id)
from Insurance_file  IFL
where IFL.insurance_file_cnt = @InsuranceFileCnt  
  
END  