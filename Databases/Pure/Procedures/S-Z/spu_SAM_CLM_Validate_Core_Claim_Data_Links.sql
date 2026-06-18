SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_CLM_Validate_Core_Claim_Data_Links'
GO


CREATE PROCEDURE spu_SAM_CLM_Validate_Core_Claim_Data_Links

@insurance_file_cnt int ,
@risk_cnt int ,
@primary_cause_id int, 
@secondary_cause_id int = null, 
@status int OUTPUT

AS

SET @status = 0

-- Internal Status  = @intStatus  
-- 1 = Insurance File / Risk Cnt Link Valid
-- 2 = Primary_cause / Risk Cnt Link Valid
-- 4 = Primary_cause / Secondary_Cause Link Valid
-- 7 = ALL LINKS VALID

-- if the insurance file / risk cnt link is valid
IF EXISTS(select NULL from insurance_file_risk_link 
	  where insurance_file_cnt = @insurance_file_cnt 
	  and risk_cnt = @risk_cnt)
SET @status = @status + 1


-- if the primary_cause / risk_cnt link is valid
IF EXISTS(select NULL 
	  from primary_cause_risk_type_group
	  where primary_cause_id = @primary_cause_id
	  and risk_type_group_id in (Select risk_type_group_id 
			  	     from risk 
 	    			     where risk_cnt = @risk_cnt))
SET @status = @status + 2


--if no secondary cause selected assume valid
IF @secondary_cause_id IS NULL
SET @status = @status + 4
ELSE
	-- if the secondary cause selected is valid
	IF EXISTS(select NULL 
		  from secondary_cause 
		  where primary_cause_id = @primary_cause_id
		  and secondary_cause_id = @secondary_cause_id)
		SET @status = @status + 4








GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
