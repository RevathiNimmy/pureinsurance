SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_wp_RatingSectioncount'
GO


CREATE PROCEDURE spu_wp_RatingSectioncount
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @RiskId INT = NULL,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT
AS


SELECT  count(rating_section_id) 'how_many'
    FROM    rating_Section
    WHERE   risk_cnt = @RiskId
--  AND original_flag = 0
GO


