
-- Procedure to get Variable Quota Share configuration by Treaty ID
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
    EXECUTE DDLDropProcedure spu_GetVariableQuotaShareConfig
GO
CREATE PROCEDURE spu_GetVariableQuotaShareConfig
    @TreatyID INT,
    @RIModelLineID INT
AS
BEGIN
    SET NOCOUNT ON
    
    SELECT 
        variable_quota_share_id,
        sa_lower_limit,
        sa_upper_limit,
        share_percent,
        treaty_limit,
        created_date,
        modified_date,
        treaty_id,
		ri_model_id,
		ri_model_line_id
    FROM Variable_Quota_Share_Config
    WHERE treaty_id = @TreatyID
    AND ri_model_line_id=@RIModelLineID
      AND is_deleted = 0
    ORDER BY sa_lower_limit
END
GO
