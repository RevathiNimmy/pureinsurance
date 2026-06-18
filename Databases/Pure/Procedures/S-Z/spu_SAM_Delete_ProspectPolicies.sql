SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_SAM_Delete_ProspectPolicies'
GO
--Start (girija) - (UIIC WR27 - MTA Amend Client.doc) - (7.7.3.1.16) 
create procedure spu_SAM_Delete_ProspectPolicies
@party_cnt int,
@prospect_policy_id int
as
Begin
Delete from Prospect_Policy
where party_cnt=@party_cnt and prospect_policy_id=@prospect_policy_id
End
--End (girija) - (UIIC WR27 - MTA Amend Client.doc) - (7.7.3.1.16) 

GO

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

