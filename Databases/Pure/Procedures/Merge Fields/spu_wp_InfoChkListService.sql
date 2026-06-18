SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_wp_InfoChkListService'
GO


CREATE PROCEDURE spu_wp_InfoChkListService
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @RiskId INT,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT
AS


SELECT  ces.service service_type,
    ces.description service_type_description,
    ces.reference service_reference,
    ces.contact service_contact,
    ces.date_requested service_requested_date,
    ces.date_critical service_critical_date,
    ces.date_received service_received_date

FROM    claim_expert_service ces

WHERE   ces.claim_id = @ClaimCnt
AND ces.claim_expert_service_id = @Instance2
AND ces.service_type_id = 2
GO


