/****** Object:  StoredProcedure [dbo].[spu_XMLGenerator_GetRiskCode]    Script Date: 18-05-2016 5:23:09 PM ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[spu_XMLGenerator_GetRiskCode]

( 
	@Productid integer
)
AS
BEGIN
SELECT rt.code as RiskCode,rt.risk_type_id as risktypeid FROM Risk_Type rt
join Risk_Type_Usage rtg on rtg. risk_type_id =rt.risk_type_id 
join Product_Risk_Type_Group prtg on prtg.risk_type_group_id =rtg.risk_type_group_id 
where prtg.product_id = (select product_id from Product where product_id  =@Productid) AND rt.is_deleted =0
END


