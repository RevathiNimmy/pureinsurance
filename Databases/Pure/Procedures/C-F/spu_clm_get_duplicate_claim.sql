SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_clm_get_duplicate_claim'
GO

CREATE PROCEDURE spu_clm_get_duplicate_claim  
  
 @policy_number varchar(30),  
 @risk_type_id int,  
 @loss_date datetime  
  
AS  
  
BEGIN  
  
--This stored procedure should accept the claim_id as a parameter  
--and use this to query the Claim table looking for matches :-  
--looking for other claim records  
--  
--where the claim.policy_id and claim.risk_type_id and claim.loss_date  
--match the policy_id, risk_type_id and loss_date of the passed claim_id.  
  
  SELECT  
  claim.claim_id,  
  claim.claim_number,  
  claim.description,  
  progress_status.description as progress_status,  
  claim.reported_date,  
  claim.last_modified_date,  
  primary_cause.description as primary_cause,  
  claim_status_id  
  
 FROM claim  
  
 LEFT JOIN primary_cause ON  
  claim.primary_cause_id = primary_cause.primary_cause_id  
  
 LEFT JOIN progress_status ON  
  claim.progress_status_id =progress_status.progress_status_id  
  
 WHERE  claim.policy_number =  @policy_number  
 AND  claim.risk_type_id = @risk_type_id  
 AND  CONVERT(char(10) ,claim.loss_from_date,103) = CONVERT(char(10), @loss_date, 103) 

 AND claim_id in (Select Max(claim_id) from claim 
		   group by base_claim_id)
 AND is_dirty = 0 
  
END  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
