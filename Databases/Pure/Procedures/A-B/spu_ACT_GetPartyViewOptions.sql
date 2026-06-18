SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_ACT_GetPartyViewOptions'
GO

CREATE PROCEDURE spu_ACT_GetPartyViewOptions  
	@user_id smallint
AS
SELECT	
  	is_view_only_client_manager,
	is_view_only_agents_maintenance,
   	is_view_only_account_handler_maintenance,
   	is_view_only_account_executive_maintenance,
   	is_view_only_insurer_maintenance,
   	is_view_only_other_party_maintenance
FROM
    User_Authorities
Where 
    user_id = @user_id

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO