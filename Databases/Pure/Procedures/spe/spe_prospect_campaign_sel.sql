SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_prospect_campaign_sel'
GO

CREATE PROCEDURE spe_prospect_campaign_sel
    @party_cnt int,
    @record_no int
AS

SELECT
    party_cnt,
    record_no,
    campaign_id

 FROM prospect_campaign

WHERE party_cnt = @party_cnt AND record_no = @record_no

GO

