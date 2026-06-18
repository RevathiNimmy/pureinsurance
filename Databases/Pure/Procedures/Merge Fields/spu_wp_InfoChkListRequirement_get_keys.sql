SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_wp_InfoChkListRequirement_get_keys'
GO


CREATE PROCEDURE spu_wp_InfoChkListRequirement_get_keys
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @RiskId INT = NULL,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT
AS


SELECT       ces.claim_expert_service_id

FROM         claim_expert_service ces

WHERE        ces.claim_id = @ClaimCnt
AND      ces.service_type_id = 1
GO


