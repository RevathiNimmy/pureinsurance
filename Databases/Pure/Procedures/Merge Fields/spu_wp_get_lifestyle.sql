SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_wp_get_lifestyle'
GO

CREATE PROCEDURE spu_wp_get_lifestyle
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @RiskID INT,	
    @ClaimCnt INT,
    @category_code CHAR(10),
    @name VARCHAR(255) OUTPUT,
    @date_of_birth DATETIME OUTPUT,
    @gender VARCHAR(70) OUTPUT,
    @occupation VARCHAR(70) OUTPUT,
    @secondary_occupation VARCHAR(70) OUTPUT,
    @is_smoker TINYINT OUTPUT
AS

SELECT @name = NULL,
    @date_of_birth = NULL,
    @gender = NULL,
    @occupation = NULL,
    @secondary_occupation = NULL,
    @is_smoker = NULL

SELECT @name = pl.name,
    @date_of_birth = pl.date_of_birth,
    @gender = pl.gender_code,
    @occupation = pl.occupation_code,
    @secondary_occupation = pl.secondary_occupation_code,
    @is_smoker = pl.is_smoker
    FROM party_lifestyle pl,
    lifestyle_category lc
    WHERE pl.party_cnt = @PartyCnt
    AND pl.category = lc.lifestyle_category_id
    AND lc.code = @category_code

GO

