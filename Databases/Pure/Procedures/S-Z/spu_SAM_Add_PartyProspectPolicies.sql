SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_SAM_Add_PartyProspectPolicies'
GO
--Start (girija) - (UIIC WR27 - MTA Amend Client.doc) - (7.7.3.1.9) 
create procedure spu_SAM_Add_PartyProspectPolicies
	@party_cnt int,
	@risk_group_id int,
	@renewal_date datetime,
	@no_of_times_quoted int,
	@target_premium numeric,
	@prospect_policy_id int output
as 
Begin
	IF @prospect_policy_id = 0  
	                SELECT @prospect_policy_id = NULL  
	IF @prospect_policy_id IS NULL  
	                SELECT @prospect_policy_id = MAX(prospect_policy_id) + 1  
	    FROM Prospect_Policy  
	                WHERE party_cnt = @party_cnt  
	IF @prospect_policy_id IS NULL  
	    SELECT @prospect_policy_id = 1 
	
	Insert into Prospect_Policy(
		prospect_policy_id,
		party_cnt,
		risk_group_id,
		renewal_date,
		no_of_times_quoted,
		target_premium)
	Values (
		@prospect_policy_id, 
		@party_cnt ,
		@risk_group_id ,
		@renewal_date ,
		@no_of_times_quoted ,
		@target_premium)
	
	SET @prospect_policy_id = @@IDENTITY

End 
--End (girija) - (UIIC WR27 - MTA Amend Client.doc) - (7.7.3.1.9) 

GO

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

