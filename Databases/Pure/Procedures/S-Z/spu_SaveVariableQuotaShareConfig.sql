
-- *****************************************************************************
-- * Stored Procedures for Variable Quota Share Configuration
-- *****************************************************************************

-- Procedure to save Variable Quota Share configuration
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
 EXECUTE DDLDropProcedure spu_SaveVariableQuotaShareConfig
GO
CREATE PROCEDURE spu_SaveVariableQuotaShareConfig 
    @RIModelID INT,
	@RIModelLineID INT,
	@TreatyID INT,
	@SALowerLimit DECIMAL(18,2),
	@SAUpperLimit DECIMAL(18,2),
	@SharePercent DECIMAL(5,2),
	@TreatyLimit DECIMAL(18,2),
	@VariableQuotaShareID INT = NULL
AS
BEGIN
    SET NOCOUNT ON
    
    IF @VariableQuotaShareID IS NULL OR @VariableQuotaShareID = 0
    BEGIN
        -- Insert new record
        INSERT INTO Variable_Quota_Share_Config 
        (ri_model_id, ri_model_line_id,treaty_id, sa_lower_limit, sa_upper_limit, share_percent, treaty_limit)
        VALUES 
        (@RIModelID, @RIModelLineID, @TreatyID, @SALowerLimit, @SAUpperLimit, @SharePercent, @TreatyLimit)
        
        SET @VariableQuotaShareID = SCOPE_IDENTITY()
        print @VariableQuotaShareID
    END
    ELSE
    BEGIN
        -- Update existing record
        UPDATE Variable_Quota_Share_Config 
        SET ri_model_id = @RIModelID,
            ri_model_line_id = @RIModelLineID,
			sa_lower_limit = @SALowerLimit,
            sa_upper_limit = @SAUpperLimit,
            share_percent = @SharePercent,
            treaty_limit = @TreatyLimit,
            modified_date = GETDATE()
        WHERE variable_quota_share_id = @VariableQuotaShareID
          AND is_deleted = 0
    END
END
GO