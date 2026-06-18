SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_get_grace_period'
GO

-- Created: PW211002

CREATE PROCEDURE spu_get_grace_period
    @product_id integer,
    @grace_period_days integer OUT
AS

BEGIN

        SELECT @grace_period_days = grace_period
          FROM product
         WHERE product_id = @product_id

END
GO