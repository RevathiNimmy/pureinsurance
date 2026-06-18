SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_GetShowRiskDetails'
GO


CREATE PROCEDURE spu_GetShowRiskDetails
    @ClaimId integer
AS


select  insured_cnt, Party.ShortName, Party.party_type_id,
        Party.resolved_name, insurance_folder_cnt ,
        event_insurance_file.insurance_file_cnt,
        risk_code.risk_group_id
    from    Claim, event_insurance_file, Party, risk_code, risk_group
    where   Claim.Policy_Id = event_insurance_file.insurance_file_cnt
    and event_insurance_file.insured_cnt = Party.Party_cnt
    and claim.Risk_type_id = risk_code.risk_code_id
    and claim_id = @ClaimId
GO


