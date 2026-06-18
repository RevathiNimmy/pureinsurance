SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_SAM_Update_PartyProspectPolicies'
GO
--Start (girija) - (UIIC WR27 - MTA Amend Client.doc) - (7.7.3.1.34) 
create procedure spu_SAM_Update_PartyProspectPolicies
	@party_cnt int,
	@prospect_policy_id int, 
	@risk_group_id int,
	@renewal_date datetime,
	@no_of_times_quoted int,
	@target_premium numeric(19, 4)
	
as 
Begin
	Update Prospect_Policy
	set	
		risk_group_id=@risk_group_id ,
		renewal_date=@renewal_date ,
		no_of_times_quoted=@no_of_times_quoted ,
		target_premium=@target_premium
	where party_cnt=@party_cnt and prospect_policy_id=@prospect_policy_id
End
--End (girija) - (UIIC WR27 - MTA Amend Client.doc) - (7.7.3.1.34) 

GO

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO


  
