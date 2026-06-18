SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_short_period_rates_del'
GO


CREATE PROCEDURE spu_short_period_rates_del
    @product_id int
AS


DELETE FROM short_period_rates

WHERE product_id = @product_id
GO


