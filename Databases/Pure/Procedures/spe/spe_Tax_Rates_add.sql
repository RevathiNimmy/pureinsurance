SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Tax_Rates_add'
GO

CREATE PROCEDURE spe_Tax_Rates_add
    @tax_rates_id smallint OUTPUT ,
    @code char(10) ,
    @description varchar(255) ,
    @effective_date datetime ,
    @is_deleted tinyint ,
    @caption_id int ,
    @country_id smallint ,
    @rate numeric(10,4)
AS
BEGIN
IF @Tax_Rates_id = 0
                SELECT @Tax_Rates_id = NULL
IF @Tax_Rates_id = NULL
                SELECT @Tax_Rates_id = MAX(Tax_Rates_id) + 1
    FROM Tax_Rates
IF @Tax_Rates_id = NULL
    SELECT @Tax_Rates_id = 1
INSERT INTO Tax_Rates (
    tax_rates_id ,
    code ,
    description ,
    effective_date ,
    is_deleted ,
    caption_id ,
    country_id ,
    rate )
VALUES (
    @tax_rates_id,
    @code,
    @description,
    @effective_date,
    @is_deleted,
    @caption_id,
    @country_id,
    @rate)
END

GO

