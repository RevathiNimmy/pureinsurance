SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_getdata_for_rskdatadefn'
GO

CREATE PROCEDURE spu_getdata_for_rskdatadefn  
    @RiskDataDefn int,  
    @ClaimId int  
AS  
  
BEGIN
    SELECT claim_user_def_risk_data_id, Value  
    FROM claim_user_defined_risk_data  
    WHERE Risk_Data_Defn_Id = @RiskDataDefn  
    AND Claim_ID = @ClaimID  
END


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
