SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_prospect_campaign_add'
GO

CREATE PROCEDURE spe_prospect_campaign_add
    @party_cnt int,
    @record_no int OUTPUT,
    @campaign_id int

AS

BEGIN

IF @record_no = 0
    SELECT @record_no = NULL

IF @record_no IS NULL
                SELECT @record_no= MAX(record_no) + 1
    FROM prospect_campaign
                WHERE party_cnt = @party_cnt

IF @record_no IS NULL
    SELECT @record_no = 1

INSERT INTO prospect_campaign (
    party_cnt ,
    record_no ,
    campaign_id )
VALUES (
    @party_cnt,
    @record_no,

    @campaign_id)
END

GO

