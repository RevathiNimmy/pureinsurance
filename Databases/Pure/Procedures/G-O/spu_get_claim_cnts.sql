SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure spu_get_claim_cnts
GO

Create PROCEDURE spu_get_claim_cnts  
    @claim_id int  
AS  
  
BEGIN  
  
select  ifo.insurance_holder_cnt,  
    ifo.insurance_folder_cnt,  
    ifi.insurance_file_cnt,  
    ifi.insured_name,
        pmuser.username  
  
from    insurance_folder ifo,  
    insurance_file ifi,  
    claim c,PMuser  
where   c.claim_id = @claim_id  
and c.policy_id = ifi.insurance_file_cnt  
and ifi.insurance_folder_cnt = ifo.insurance_folder_cnt  
 AND c.created_by_id=pmuser.user_id 
  
END  