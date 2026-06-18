SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

ddldropprocedure spu_wp_BrokerlinkHtmlMerge
go


CREATE PROCEDURE spu_wp_BrokerlinkHtmlMerge
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @RiskId INT,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT
AS
    SELECT  Brokerlink_Document_Type_Id,
            text_html
    FROM    BROKERLINK_HTML_MERGE 
    WHERE   BROKERLINK_HTML_MERGE_ID = @Instance2
go