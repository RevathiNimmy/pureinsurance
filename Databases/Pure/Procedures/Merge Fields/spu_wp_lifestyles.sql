SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_wp_lifestyles'
GO


CREATE PROCEDURE spu_wp_lifestyles
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @RiskID INT,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT
AS


DECLARE @insured_name VARCHAR(255),
    @insured_date_of_birth DATETIME,
    @insured_gender VARCHAR(70),
    @insured_occupation VARCHAR(70),
    @insured_secondary_occupation VARCHAR(70),
    @is_insured_smoker TINYINT,
    @spouse_name VARCHAR(255),
    @spouse_date_of_birth DATETIME,
    @spouse_gender VARCHAR(70),
    @spouse_occupation VARCHAR(70),

    @spouse_secondary_occupation VARCHAR(70),
    @is_spouse_smoker TINYINT,
    @child1_name VARCHAR(255),
    @child1_date_of_birth DATETIME,
    @child1_gender VARCHAR(70),
    @child1_occupation VARCHAR(70),
    @child1_secondary_occupation VARCHAR(70),
    @is_child1_smoker TINYINT,
    @child2_name VARCHAR(255),
    @child2_date_of_birth DATETIME,
    @child2_gender VARCHAR(70),
    @child2_occupation VARCHAR(70),
    @child2_secondary_occupation VARCHAR(70),
    @is_child2_smoker TINYINT,
    @child3_name VARCHAR(255),
    @child3_date_of_birth DATETIME,
    @child3_gender VARCHAR(70),
    @child3_occupation VARCHAR(70),
    @child3_secondary_occupation VARCHAR(70),
    @is_child3_smoker TINYINT,
    @child4_name VARCHAR(255),
    @child4_date_of_birth DATETIME,
    @child4_gender VARCHAR(70),
    @child4_occupation VARCHAR(70),
    @child4_secondary_occupation VARCHAR(70),
    @is_child4_smoker TINYINT,
    @child5_name VARCHAR(255),
    @child5_date_of_birth DATETIME,
    @child5_gender VARCHAR(70),
    @child5_occupation VARCHAR(70),
    @child5_secondary_occupation VARCHAR(70),
    @is_child5_smoker TINYINT,
    @child6_name VARCHAR(255),
    @child6_date_of_birth DATETIME,
    @child6_gender VARCHAR(70),
    @child6_occupation VARCHAR(70),
    @child6_secondary_occupation VARCHAR(70),
    @is_child6_smoker TINYINT,
    @childx_name VARCHAR(255),
    @childx_date_of_birth DATETIME,
    @childx_gender VARCHAR(70),
    @childx_occupation VARCHAR(70),
    @childx_secondary_occupation VARCHAR(70),
    @is_childx_smoker TINYINT,
    @partner_name VARCHAR(255),
    @partner_date_of_birth DATETIME,
    @partner_gender VARCHAR(70),
    @partner_occupation VARCHAR(70),
    @partner_secondary_occupation VARCHAR(70),
    @is_partner_smoker TINYINT,
    @undef_name VARCHAR(255),
    @undef_date_of_birth DATETIME,
    @undef_gender VARCHAR(70),
    @undef_occupation VARCHAR(70),
    @undef_secondary_occupation VARCHAR(70),
    @is_undef_smoker TINYINT,
    @category_code CHAR(10),
    @name VARCHAR(255),
    @date_of_birth DATETIME,
    @gender VARCHAR(70),
    @occupation VARCHAR(70),
    @secondary_occupation VARCHAR(70),
    @is_smoker TINYINT

    SELECT @category_code = "INSURED"

    EXEC spu_wp_get_lifestyle    @PartyCnt,
                    @InsuranceFileCnt, @RiskID,
                    @ClaimCnt,
                    @category_code,
                    @name OUTPUT,
                    @date_of_birth OUTPUT,
                    @gender OUTPUT,
                    @occupation OUTPUT,
                    @secondary_occupation OUTPUT,
                    @is_smoker OUTPUT

    SELECT  @insured_name = @name,
        @insured_date_of_birth = @date_of_birth,
        @insured_gender = @gender,
        @insured_occupation = @occupation,
        @insured_secondary_occupation = @secondary_occupation,
        @is_insured_smoker = @is_smoker

    SELECT @category_code = "SPOUSE"

    EXEC spu_wp_get_lifestyle    @PartyCnt,
                    @InsuranceFileCnt, @RiskID,
                    @ClaimCnt,
                    @category_code,
                    @name OUTPUT,
                    @date_of_birth OUTPUT,
                    @gender OUTPUT,
                    @occupation OUTPUT,
                    @secondary_occupation OUTPUT,
                    @is_smoker OUTPUT

    SELECT  @spouse_name = @name,
        @spouse_date_of_birth = @date_of_birth,
        @spouse_gender = @gender,
        @spouse_occupation = @occupation,
        @spouse_secondary_occupation = @secondary_occupation,
        @is_spouse_smoker = @is_smoker

    SELECT @category_code = "CHILD1"

    EXEC spu_wp_get_lifestyle    @PartyCnt,
                    @InsuranceFileCnt, @RiskID,
                    @ClaimCnt,
                    @category_code,
                    @name OUTPUT,
                    @date_of_birth OUTPUT,
                    @gender OUTPUT,
                    @occupation OUTPUT,
                    @secondary_occupation OUTPUT,
                    @is_smoker OUTPUT

    SELECT  @child1_name = @name,
        @child1_date_of_birth = @date_of_birth,
        @child1_gender = @gender,
        @child1_occupation = @occupation,
        @child1_secondary_occupation = @secondary_occupation,
        @is_child1_smoker = @is_smoker

    SELECT @category_code = "CHILD2"

    EXEC spu_wp_get_lifestyle    @PartyCnt,
                    @InsuranceFileCnt, @RiskID,
                    @ClaimCnt,
                    @category_code,
                    @name OUTPUT,
                    @date_of_birth OUTPUT,
                    @gender OUTPUT,
                    @occupation OUTPUT,
                    @secondary_occupation OUTPUT,
                    @is_smoker OUTPUT

    SELECT  @child2_name = @name,
        @child2_date_of_birth = @date_of_birth,
        @child2_gender = @gender,
        @child2_occupation = @occupation,
        @child2_secondary_occupation = @secondary_occupation,
        @is_child2_smoker = @is_smoker

    SELECT @category_code = "CHILD3"

    EXEC spu_wp_get_lifestyle    @PartyCnt,
                    @InsuranceFileCnt, @RiskID,
                    @ClaimCnt,
                    @category_code,
                    @name OUTPUT,
                    @date_of_birth OUTPUT,
                    @gender OUTPUT,
                    @occupation OUTPUT,
                    @secondary_occupation OUTPUT,
                    @is_smoker OUTPUT

    SELECT  @child3_name = @name,
        @child3_date_of_birth = @date_of_birth,
        @child3_gender = @gender,
        @child3_occupation = @occupation,
        @child3_secondary_occupation = @secondary_occupation,
        @is_child3_smoker = @is_smoker

    SELECT @category_code = "CHILD4"

    EXEC spu_wp_get_lifestyle    @PartyCnt,
                    @InsuranceFileCnt, @RiskID,
                    @ClaimCnt,
                    @category_code,
                    @name OUTPUT,
                    @date_of_birth OUTPUT,
                    @gender OUTPUT,
                    @occupation OUTPUT,
                    @secondary_occupation OUTPUT,
                    @is_smoker OUTPUT

    SELECT  @child4_name = @name,
        @child4_date_of_birth = @date_of_birth,
        @child4_gender = @gender,
        @child4_occupation = @occupation,
        @child4_secondary_occupation = @secondary_occupation,
        @is_child4_smoker = @is_smoker

    SELECT @category_code = "CHILD5"

    EXEC spu_wp_get_lifestyle    @PartyCnt,
                    @InsuranceFileCnt, @RiskID,
                    @ClaimCnt,
                    @category_code,
                    @name OUTPUT,
                    @date_of_birth OUTPUT,
                    @gender OUTPUT,
                    @occupation OUTPUT,
                    @secondary_occupation OUTPUT,
                    @is_smoker OUTPUT

    SELECT  @child5_name = @name,
        @child5_date_of_birth = @date_of_birth,
        @child5_gender = @gender,
        @child5_occupation = @occupation,
        @child5_secondary_occupation = @secondary_occupation,
        @is_child5_smoker = @is_smoker

    SELECT @category_code = "CHILD6"

    EXEC spu_wp_get_lifestyle    @PartyCnt,
                    @InsuranceFileCnt, @RiskID,
                    @ClaimCnt,
                    @category_code,
                    @name OUTPUT,

                    @date_of_birth OUTPUT,
                    @gender OUTPUT,
                    @occupation OUTPUT,
                    @secondary_occupation OUTPUT,
                    @is_smoker OUTPUT

    SELECT  @child6_name = @name,
        @child6_date_of_birth = @date_of_birth,
        @child6_gender = @gender,
        @child6_occupation = @occupation,
        @child6_secondary_occupation = @secondary_occupation,
        @is_child6_smoker = @is_smoker

    SELECT @category_code = "CHILDX"

    EXEC spu_wp_get_lifestyle    @PartyCnt,
                    @InsuranceFileCnt, @RiskID,
                    @ClaimCnt,
                    @category_code,
                    @name OUTPUT,
                    @date_of_birth OUTPUT,
                    @gender OUTPUT,
                    @occupation OUTPUT,
                    @secondary_occupation OUTPUT,
                    @is_smoker OUTPUT

    SELECT  @childx_name = @name,
        @childx_date_of_birth = @date_of_birth,
        @childx_gender = @gender,
        @childx_occupation = @occupation,
        @childx_secondary_occupation = @secondary_occupation,
        @is_childx_smoker = @is_smoker

    SELECT @category_code = "PARTNER"

    EXEC spu_wp_get_lifestyle    @PartyCnt,
                    @InsuranceFileCnt, @RiskID,
                    @ClaimCnt,
                    @category_code,
                    @name OUTPUT,
                    @date_of_birth OUTPUT,
                    @gender OUTPUT,
                    @occupation OUTPUT,
                    @secondary_occupation OUTPUT,
                    @is_smoker OUTPUT

    SELECT  @partner_name = @name,
        @partner_date_of_birth = @date_of_birth,
        @partner_gender = @gender,
        @partner_occupation = @occupation,
        @partner_secondary_occupation = @secondary_occupation,
        @is_partner_smoker = @is_smoker

    SELECT @category_code = "UNDEFINED"

    EXEC spu_wp_get_lifestyle    @PartyCnt,
                    @InsuranceFileCnt, @RiskID,
                    @ClaimCnt,
                    @category_code,
                    @name OUTPUT,
                    @date_of_birth OUTPUT,
                    @gender OUTPUT,
                    @occupation OUTPUT,
                    @secondary_occupation OUTPUT,
                    @is_smoker OUTPUT

    SELECT  @undef_name = @name,
        @undef_date_of_birth = @date_of_birth,
        @undef_gender = @gender,
        @undef_occupation = @occupation,
        @undef_secondary_occupation = @secondary_occupation,
        @is_undef_smoker = @is_smoker

    SELECT  'insured_name' = @insured_name,
        'insured_date_of_birth' = @insured_date_of_birth,
        'insured_gender' = @insured_gender,
        'insured_occupation' = @insured_occupation,
        'insured_secondary_occupation' = @insured_secondary_occupation,
        'is_insured_smoker' = @is_insured_smoker,
        'spouse_name' = @spouse_name,
        'spouse_date_of_birth' = @spouse_date_of_birth,
        'spouse_gender' = @spouse_gender,
        'spouse_occupation' = @spouse_occupation,
        'spouse_secondary_occupation' = @spouse_secondary_occupation,
        'is_spouse_smoker' = @is_spouse_smoker,
        'child1_name' = @child1_name,
        'child1_date_of_birth' = @child1_date_of_birth,
        'child1_gender' = @child1_gender,
        'child1_occupation' = @child1_occupation,
        'child1_secondary_occupation' = @child1_secondary_occupation,
        'is_child1_smoker' = @is_child1_smoker,

        'child2_name' = @child2_name,
        'child2_date_of_birth' = @child2_date_of_birth,
        'child2_gender' = @child2_gender,
        'child2_occupation' = @child2_occupation,
        'child2_secondary_occupation' = @child2_secondary_occupation,
        'is_child2_smoker' = @is_child2_smoker,
        'child3_name' = @child3_name,
        'child3_date_of_birth' = @child3_date_of_birth,
        'child3_gender' = @child3_gender,
        'child3_occupation' = @child3_occupation,
        'child3_secondary_occupation' = @child3_secondary_occupation,
        'is_child3_smoker' = @is_child3_smoker,
        'child4_name' = @child4_name,
        'child4_date_of_birth' = @child4_date_of_birth,
        'child4_gender' = @child4_gender,
        'child4_occupation' = @child4_occupation,
        'child4_secondary_occupation' = @child4_secondary_occupation,
        'is_child4_smoker' = @is_child4_smoker,
        'child5_name' = @child5_name,
        'child5_date_of_birth' = @child5_date_of_birth,
        'child5_gender' = @child5_gender,
        'child5_occupation' = @child5_occupation,
        'child5_secondary_occupation' = @child5_secondary_occupation,
        'is_child5_smoker' = @is_child5_smoker,
        'child6_name' = @child6_name,
        'child6_date_of_birth' = @child6_date_of_birth,
        'child6_gender' = @child6_gender,
        'child6_occupation' = @child6_occupation,
        'child6_secondary_occupation' = @child6_secondary_occupation,
        'is_child6_smoker' = @is_child6_smoker,
        'childx_name' = @childx_name,
        'childx_date_of_birth' = @childx_date_of_birth,
        'childx_gender' = @childx_gender,
        'childx_occupation' = @childx_occupation,
        'childx_secondary_occupation' = @childx_secondary_occupation,
        'is_childx_smoker' = @is_childx_smoker,
        'partner_name' = @partner_name,
        'partner_date_of_birth' = @partner_date_of_birth,
        'partner_gender' = @partner_gender,
        'partner_occupation' = @partner_occupation,
        'partner_secondary_occupation' = @partner_secondary_occupation,
        'is_partner_smoker' = @is_partner_smoker,
        'undef_name' = @undef_name,
        'undef_date_of_birth' = @undef_date_of_birth,
        'undef_gender' = @undef_gender,
        'undef_occupation' = @undef_occupation,
        'undef_secondary_occupation' = @undef_secondary_occupation,
        'is_undef_smoker' = @is_undef_smoker
GO


