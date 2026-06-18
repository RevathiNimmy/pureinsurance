SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_wp_InfoChkListRequirement'
GO


CREATE PROCEDURE spu_wp_InfoChkListRequirement
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @RiskId INT,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT
AS


SELECT  ces.service Requirement_type,
    ces.description Requirement_type_description,
    ces.reference Requirement_reference,
    ces.contact Requirement_contact,
    ces.date_requested Requirement_requested_date,
    ces.date_critical Requirement_critical_date,
    ces.date_received Requirement_received_date

FROM    claim_expert_service ces

WHERE   ces.claim_id = @ClaimCnt
AND ces.claim_expert_service_id = @Instance2
AND ces.service_type_id = 1
GO


