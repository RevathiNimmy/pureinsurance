SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_Product_Risk_Type_Group_Sel'
GO


CREATE PROCEDURE spu_Product_Risk_Type_Group_Sel
    @product_id int
AS


SELECT
   prtg.risk_type_group_id

 FROM Product_Risk_Type_Group prtg, Risk_Type_Group rtg

WHERE prtg.product_id = @product_id
 AND prtg.risk_type_group_id = rtg.risk_type_group_id
 AND rtg.is_deleted <> 1

ORDER BY rtg.code
GO


