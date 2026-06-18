SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_GetShowRiskDetails_U'
GO


CREATE PROCEDURE spu_GetShowRiskDetails_U
    @ClaimId integer
AS


select TOP 1 insurance_file.insured_cnt, Party.ShortName, Party.party_type_id,
        Party.resolved_name, insurance_file.insurance_folder_cnt ,
        insurance_file.insurance_file_cnt,
        risk_code.risk_group_id
    from    Claim, Party, risk_code, risk_group,risk,insurance_file  
    where risk.risk_cnt=claim.risk_type_id
    and risk_code.Risk_code_id = risk.risk_type_id  
    and insurance_file.insurance_file_cnt=claim.policy_id
    and party.party_cnt=insurance_file.insured_cnt
    and claim_id = @ClaimId
GO