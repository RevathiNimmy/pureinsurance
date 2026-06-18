SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_CLM_Get_Client_And_Agent_Details'
GO

CREATE PROCEDURE spu_CLM_Get_Client_And_Agent_Details

@claim_id int

AS

BEGIN

	DECLARE @AgentUnderwriter varchar(1)  
	  
	SELECT  @AgentUnderwriter = value  
	FROM    hidden_options  
	WHERE   branch_id = 1 and option_number = 1  
	  
	IF ISNULL(@AgentUnderwriter, ' ') = ' '  
		SELECT @AgentUnderwriter = 'A'  
  
	IF @AgentUnderwriter = 'A'  
	BEGIN
	                                            
		SELECT 
		(SELECT party_cnt FROM party WHERE shortname = insurer_short_name),
		(SELECT party_cnt FROM party WHERE shortname = client_short_name),
		insurer_name,
		client_name,
		(SELECT product_id FROM insurance_file WHERE insurance_file_cnt = policy_id)
		FROM claim 
		WHERE Claim_id = @claim_id
	END 
	ELSE	
	BEGIN
		SELECT IsNull(ifi.lead_agent_cnt,0), ifi.insured_cnt,
		(select name from party where party_cnt = IsNull(ifi.lead_agent_cnt,0)),
		ifi.insured_name,
		ifi.product_id
		FROM Claim c 
			JOIN Insurance_File ifi ON 
				c.policy_id = ifi.insurance_file_cnt
		WHERE c.claim_id = @claim_id
	END

END


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
