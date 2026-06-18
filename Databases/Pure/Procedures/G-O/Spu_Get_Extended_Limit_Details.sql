SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'Spu_Get_Extended_Limit_Details'
GO


CREATE PROCEDURE Spu_Get_Extended_Limit_Details
@ri_arrangement_id INT,
@FilterType INT
AS
BEGIN
IF ISNULL(@FilterType,0)=1
BEGIN
 SELECT Extended_limit_amount ,
		Is_extended_limit_applied  
 FROM RI_Arrangement 
 WHERE ri_arrangement_id =@ri_arrangement_id 
END
ELSE IF  ISNULL(@FilterType,0)=2
BEGIN
SELECT RAL.Extended_limit_amount ,
		RAL.Is_extended_limit_applied  
 FROM RI_Arrangement RAL
 JOIN Claim_RI_Arrangement CRA ON CRA.risk_cnt  =ral.risk_cnt AND CRA.ri_model_id =RAL.ri_model_id  
 WHERE CRA.ri_arrangement_id  =@ri_arrangement_id 
END


END

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
