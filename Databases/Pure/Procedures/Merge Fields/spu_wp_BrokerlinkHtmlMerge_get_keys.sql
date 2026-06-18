SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

ddldropprocedure spu_wp_BrokerlinkHtmlMerge_get_keys
go

CREATE PROCEDURE spu_wp_BrokerlinkHtmlMerge_get_keys
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @RiskId INT = NULL,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT
AS
	SELECT       BHM.BROKERLINK_HTML_MERGE_ID
	FROM         BROKERLINK_HTML_MERGE BHM
	join gis_policy_link gpl on gpl.Gis_policy_link_id = bhm.gis_policy_link_id
	WHERE        gpl.insurance_file_cnt=@InsuranceFileCnt
go