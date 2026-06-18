SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_prospect_campaign_upd'
GO

CREATE PROCEDURE spe_prospect_campaign_upd
    @party_cnt int,
    @record_no int,
    @campaign_id int

AS
BEGIN

UPDATE prospect_campaign

    SET
    campaign_id=@campaign_id

WHERE party_cnt = @party_cnt AND record_no = @record_no

END

GO

