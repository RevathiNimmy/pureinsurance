SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_check_existence_in_clmrsk'
GO

CREATE PROCEDURE spu_check_existence_in_clmrsk  
    @ClaimId int,  
    @RiskTypeId int,  
    @ClaimExists bit  
AS  
  
--*******************************************************************************************  
-- Version      Author  Date        Desc  
-- 1.00.0001    TN      27/04/2001  process data from work tables for underwriting  
--  
--*******************************************************************************************  
BEGIN  

	DECLARE @RecCount int
	DECLARE @bVal bit  
  
	SELECT  @RecCount = count(*)
	FROM Claim_Risk  
	WHERE   claim_id = @ClaimId  
	AND     Risk_Type_Id = @RiskTypeId  
	
	IF @Reccount > 0  
	    SELECT @bVal = 1  
	ELSE  
	    SELECT @bVal = 0  
	    SELECT @ClaimExists = @bVal  
END  
  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
