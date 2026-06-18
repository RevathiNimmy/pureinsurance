SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_Add_General_Details'
GO

CREATE PROCEDURE spu_Add_General_Details  
    @ClaimId integer,  
    @RiskDataDefn integer,  
    @Value varchar(30)  
AS  
  
--*******************************************************************************************  
-- Version      Author  Date        Desc  
-- 1.00.0001    TN      27/04/2001  process data from work tables for underwriting  
--*******************************************************************************************  
BEGIN  
 DECLARE @count int  
 DECLARE @version_id int 
 DECLARE @claim_user_def_risk_data_id int 
	
  
 SELECT @count = count(*) FROM claim_user_defined_risk_data  
 WHERE risk_data_defn_id = @RiskDataDefn  
 AND Claim_Id = @ClaimId  
  
 IF @count > 0  
 BEGIN  
     UPDATE claim_user_defined_risk_data  
     SET value = @Value  
     WHERE risk_data_defn_id = @RiskDataDefn  
     AND Claim_Id = @ClaimId  
 END  
 ELSE  
 BEGIN  

	EXEC spu_CLM_Get_Claim_Version 
		@claim_id = @claimid,
		@version_id = @version_id OUTPUT		

	INSERT into claim_user_defined_risk_data (claim_id, risk_data_defn_id, Value, version_id)  
	values (@ClaimId, @RiskDataDefn ,@Value, @version_id)  

	SET @claim_user_def_risk_data_id = @@IDENTITY



	UPDATE claim_user_defined_risk_data 
	SET base_claim_user_defined_risk_data_id =@claim_user_def_risk_data_id
	WHERE claim_user_def_risk_data_id = @claim_user_def_risk_data_id

 END  
END  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
