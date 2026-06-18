EXECUTE DDLDropProcedure 'spu_GetShowBrokingRiskDetails'
GO

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE proc spu_GetShowBrokingRiskDetails
	@ClaimId integer
as
	select 	insured_cnt, Party.ShortName, Party.party_type_id,
		Party.resolved_name, insurance_folder_cnt ,
		insurance_file.insurance_file_cnt,
		risk_code.risk_group_id,
		risk_group.gis_screen_id,
		insurance_file.insurance_ref,
		claim.claim_number
	from 	Claim, insurance_file, Party, risk_code, risk_group 
	where 	Claim.Policy_Id = insurance_file.insurance_file_cnt
	and	insurance_file.insured_cnt = Party.Party_cnt
	and 	risk_group.risk_group_id = risk_code.risk_group_id
	and 	claim.Risk_type_id = risk_code.risk_code_id
	and 	claim_id = @ClaimId

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO