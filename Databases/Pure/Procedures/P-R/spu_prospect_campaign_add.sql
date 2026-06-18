SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_prospect_campaign_add'
GO


CREATE PROCEDURE spu_prospect_campaign_add
    @party_cnt int,
    @campaign_id int
AS


BEGIN

DECLARE @record_no int,
    @party_prospect int

SELECT  @party_prospect = party_cnt
FROM    party_prospect
WHERE   party_cnt = @party_cnt

IF @party_prospect IS NULL
    INSERT INTO party_prospect (
        party_cnt,
        agent_reference,
        current_intermediary,
        prospect_status_id)
    VALUES (@party_cnt,
        NULL,
        NULL,
        NULL)

SELECT  @record_no = MAX(record_no)
FROM    prospect_campaign
WHERE   party_cnt = @party_cnt

IF @record_no IS NULL
    SELECT @record_no = 0

SELECT @record_no = @record_no + 1

INSERT INTO prospect_campaign (
    party_cnt,
    record_no,
    campaign_id)
VALUES (
    @party_cnt,
    @record_no,
    @campaign_id)
END
GO


