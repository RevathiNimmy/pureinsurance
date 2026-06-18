SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_wp_RatingSection_get_keys'
GO


CREATE PROCEDURE spu_wp_RatingSection_get_keys
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @RiskId INT = NULL,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT
AS


SELECT  rating_section_id
    FROM    rating_Section
    WHERE   risk_cnt = @RiskId
--  AND original_flag = 0
GO


