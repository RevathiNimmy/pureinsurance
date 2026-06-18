SET QUOTED_IDENTIFIER OFF
Go
SET ANSI_NULLS OFF  
GO

EXECUTE DDLDropProcedure 'spu_SIR_Mandatory_Risk_Sel'
GO

CREATE PROCEDURE spu_SIR_Mandatory_Risk_Sel
    @ProductId INT,
    @insurance_file_cnt INT
AS
BEGIN

SELECT rt.Risk_Type_Id, rt.description, rt.gis_screen_id FROM product p
INNER JOIN risk_type rt ON rt.risk_type_id = p.Mandatory_Risk_Type_Id
WHERE p.product_id = @productId
	AND p.Mandatory_Risk_Type_Id NOT IN
(SELECT r.Risk_Type_Id FROM insurance_file ifi 
INNER JOIN insurance_file_risk_link ifrl ON ifrl.insurance_file_cnt = ifi.insurance_file_cnt
INNER JOIN risk r ON r.risk_cnt = ifrl.risk_cnt AND r.Is_Mandatory_Risk = 1
WHERE ifi.insurance_file_cnt = @insurance_file_cnt)

END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO