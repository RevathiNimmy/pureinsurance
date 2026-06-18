SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_prospect_campaign_saa'
GO

CREATE PROCEDURE spe_prospect_campaign_saa
    @party_cnt int
AS

SELECT
    party_cnt,
    record_no,
    campaign_id
 FROM prospect_campaign

WHERE party_cnt = @party_cnt
ORDER BY record_no ASC

GO

