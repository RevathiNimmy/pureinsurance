SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_Product_RI_Model_Usage_add'
GO
CREATE PROCEDURE spe_Product_RI_Model_Usage_add
    @ri_band int,
    @ri_model_id int,
    @product_id int,
    @description varchar(255),
    @is_deleted int,
    @effective_date datetime
AS
BEGIN
INSERT INTO Product_RI_Model_Usage (
    ri_band,
    ri_model_id,
    product_id,
    description,
    is_deleted,
    effective_date )
VALUES (
    @ri_band,
    @ri_model_id,
    @product_id,
    @description,
    @is_deleted,
    @effective_date)
END

GO

