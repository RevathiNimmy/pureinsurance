SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON 
GO
EXECUTE DDLDropProcedure spu_GetClaimRIVariableQuotaSharePercent_RI2007
GO
CREATE PROCEDURE spu_GetClaimRIVariableQuotaSharePercent_RI2007 
    @ri_model_line_id INT,
    @sum_insured MONEY,
    @ri_arrangement_line_id INT,
    @claim_id   INT
AS
BEGIN
    SET NOCOUNT ON
    
    DECLARE @share_percent DECIMAL(5,2),@treaty_id INT
    
    SELECT   @treaty_id = cral.treaty_id
    FROM    claim_ri_arrangement_line cral 
    WHERE  cral.ri_arrangement_line_id = @ri_arrangement_line_id    and cral.claim_id = @claim_id
    AND     cral.type In ('T')

    IF @treaty_id IS NOT NULL
    BEGIN
        -- Get the share percentage where sum insured falls between lower and upper limit
        SELECT TOP 1 @share_percent = share_percent
        FROM Variable_Quota_Share_Config
        WHERE treaty_id = @treaty_id
            AND ri_model_line_id = @ri_model_line_id
            AND @sum_insured >= sa_lower_limit
            AND @sum_insured <= sa_upper_limit
            AND is_deleted = 0
			ORDER BY sa_lower_limit DESC
        -- Update ri_arrangement_line if percentage found
        IF @share_percent IS NOT NULL
        BEGIN
            UPDATE Claim_RI_Arrangement_Line
            SET default_share_percent = @share_percent
            WHERE claim_id = @claim_id  AND ri_arrangement_line_id = @ri_arrangement_line_id
        END
        -- Return the percentage
        --SELECT @share_percent AS share_percent
    END
    END
GO
