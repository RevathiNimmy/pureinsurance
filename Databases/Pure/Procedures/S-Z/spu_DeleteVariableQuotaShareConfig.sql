-- Procedure to delete Variable Quota Share configuration
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
  EXECUTE DDLDropProcedure spu_DeleteVariableQuotaShareConfig
GO
CREATE PROCEDURE spu_DeleteVariableQuotaShareConfig 
    @VariableQuotaShareID INT
AS
BEGIN
    SET NOCOUNT ON
    IF @VariableQuotaShareID <> 0
    BEGIN
    UPDATE Variable_Quota_Share_Config 
    SET is_deleted = 1,
        modified_date = GETDATE()
    WHERE variable_quota_share_id = @VariableQuotaShareID
    END
END
GO