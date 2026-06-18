SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_CLM_Set_Payment_for_Recommendation'
GO

CREATE PROCEDURE spu_CLM_Set_Payment_for_Recommendation  
    @claimid int,  
    @status smallint,
    @user_id INT = null
AS  
    UPDATE Claim_Payment  
    SET Is_Referred_for_recommendation = @status,
    recommended_by = @user_id   
    WHERE Claim_Id = @claimid  