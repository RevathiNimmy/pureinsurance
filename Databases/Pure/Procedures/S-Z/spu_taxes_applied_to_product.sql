SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_taxes_applied_to_product'
GO


CREATE PROCEDURE spu_taxes_applied_to_product
    @insurance_file_cnt int
AS


SELECT  p.is_tax_suppressed
FROM        Product p,
        Insurance_File ifi
WHERE   ifi.insurance_file_cnt = @insurance_file_cnt
AND     ifi.product_id = p.product_id
GO


