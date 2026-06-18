SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_short_period_rates_add'
GO

CREATE PROCEDURE spe_short_period_rates_add
    @product_id int,
    @type char(1),
    @period char(1),
    @value int,
    @effective_date datetime,
    @percentage numeric(7,4),
    @is_deleted tinyint

AS

BEGIN
INSERT INTO short_period_rates (
    product_id ,
    type ,
    period ,
    value ,
    effective_date ,
    percentage ,
    is_deleted )
VALUES (
    @product_id,
    @type,
    @period,
    @value,
    @effective_date,
    @percentage,
    @is_deleted)
END

GO

