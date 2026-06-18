SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_wp_CombinedLiability'
GO


CREATE PROCEDURE spu_wp_CombinedLiability
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT,
    @is_employers_liability TINYINT OUTPUT,
    @e_indemnity_limit NUMERIC,
    @e_rating_basis VARCHAR(255),
    @is_p_a_p_liability TINYINT OUTPUT,
    @p_a_p_indemnity_limit NUMERIC,
    @p_a_p_rating_basis VARCHAR(255),
    @excess NUMERIC
AS


SELECT
    @is_employers_liability = Combined_Liability.is_employers_liability,
    @e_indemnity_limit = Combined_Liability.e_indemnity_limit,
    @e_rating_basis = Combined_Liability.e_rating_basis,
    @is_p_a_p_liability = Combined_Liability.is_p_a_p_liability,
    @p_a_p_indemnity_limit = Combined_Liability.p_a_p_indemnity_limit,
    @p_a_p_rating_basis = Combined_Liability.p_a_p_rating_basis,
    @excess = Combined_Liability.excess
FROM Combined_Liability
WHERE Combined_Liability.insurance_file_cnt = @insurancefilecnt
GO


