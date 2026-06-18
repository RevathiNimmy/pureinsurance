SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

ddldropprocedure spu_wp_BrokerlinkHtmlMerge_Get_Parent_Key
go


CREATE PROCEDURE spu_wp_BrokerlinkHtmlMerge_Get_Parent_Key
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @RiskId INT = NULL,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT
AS
	SELECT @InsuranceFileCnt
go