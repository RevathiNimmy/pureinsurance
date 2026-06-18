SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure spu_wp_RatingSection
GO

CREATE PROCEDURE spu_wp_RatingSection
    @PartyCnt INT,  
    @InsuranceFileCnt INT,  
    @RiskId INT = NULL,  
    @ClaimCnt INT,  
    @DocumentRef VARCHAR(25),  
    @Instance1 INT,  
    @Instance2 INT,  
    @Instance3 INT  
AS
BEGIN  
  
    SELECT  
        rst.code AS 'rating_section_type_code',  
        rst.description AS 'rating_section_type',  
        pst.code AS 'policy_section_type_code',  
        pst.description AS 'policy_section_type',  
        rs.description AS 'description',  
        rt.code AS 'rate_type_code',  
        rt.description AS 'rate_type',  
        rs.annual_rate AS 'annual_rate',  
        rs.sum_insured AS 'sum_insured',  
        rs.annual_premium AS 'annual_premium',  
        rs.this_premium AS 'this_premium',  
        rs.original_flag AS 'original_flag',  
        rs.is_amended AS 'is_amended',  
        rs.calculated_premium AS 'calculated_premium',  
        rs.override_reason AS 'override_reason'  
    FROM
        rating_section rs
        LEFT OUTER JOIN rate_type rt
            ON  rs.rate_type_id = rt.rate_type_id
        LEFT OUTER JOIN rating_section_type rst
            ON rs.rating_section_type_id = rst.rating_section_type_id    
        LEFT OUTER JOIN policy_section_type pst
            ON rs.policy_section_type_id = pst.policy_section_type_id    
    WHERE  
        rs.risk_cnt = @RiskId
        AND rs.rating_section_id = @Instance2

END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
