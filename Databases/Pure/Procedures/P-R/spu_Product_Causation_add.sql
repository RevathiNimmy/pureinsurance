SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Product_Causation_add'
GO


CREATE PROCEDURE spu_Product_Causation_add
    @product_id int,
    @primary_cause_id int
AS


BEGIN
INSERT INTO Product_Allowed_Causation (
    product_id ,
    primary_cause_id )
VALUES (
    @product_id,
    @primary_cause_id)
END
GO


