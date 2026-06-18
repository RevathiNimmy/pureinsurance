SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_add_claim_risk'
GO

CREATE PROCEDURE spu_add_claim_risk  
    @ClaimId int,  
    @RiskTypeId int,  
    @Description varchar(255),  
    @Comments text  
AS  
  
--*******************************************************************************************  
-- Version      Author  Date        Desc  
-- 1.00.0001    TN      27/04/2001  process data from work tables for underwriting  
--  
--*******************************************************************************************  
  
BEGIN  
	DECLARE @Count int  
	DECLARE @version_id int 
	DECLARE @claim_risk_id int 
	
	EXEC spu_CLM_get_claim_version 
			@claim_id = @claimid, 
			@version_id = @version_id OUTPUT
  
        SELECT  @Count = count(*)  
	FROM Claim_Risk  
        WHERE   Claim_Id = @ClaimId  
        AND     Risk_type_Id = @RiskTypeId  
  
        IF @count > 0  
        BEGIN  
  		UPDATE  Claim_Risk  
		SET     Description = @Description,  
			Comments = @Comments  
		WHERE   Claim_Id = @ClaimId  
		AND     Risk_type_Id = @RiskTypeId  
	END  
        ELSE  
	BEGIN  
		INSERT INTO Claim_Risk (Claim_Id , Risk_Type_Id, Description,Comments, version_id)  
		VALUES (@ClaimId, @RiskTypeId, @Description, @Comments, @version_id)  

		SELECT @claim_risk_id = @@IDENTITY

		UPDATE claim_risk 
		SET base_claim_risk_id =@claim_risk_id 
		WHERE claim_risk_id = @claim_risk_id
	END  
END  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
