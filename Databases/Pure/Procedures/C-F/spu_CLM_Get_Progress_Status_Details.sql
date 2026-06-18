SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_CLM_Get_Progress_Status_Details'
GO

CREATE PROCEDURE spu_CLM_Get_Progress_Status_Details
    @Progress_Status_Id INT
AS
BEGIN
		SELECT is_closed_check_status, is_claim_Payment_valid 
        FROM Progress_Status
        WHERE Progress_status_id = @Progress_Status_Id
END
