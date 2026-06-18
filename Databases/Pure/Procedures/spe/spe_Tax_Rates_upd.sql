SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Tax_Rates_upd'
GO

CREATE PROCEDURE spe_Tax_Rates_upd
    @tax_rates_id smallint,
    @code char(10),
    @description varchar(255),
    @effective_date datetime,
    @is_deleted tinyint,
    @caption_id int,
    @country_id smallint,
    @rate numeric(10,4)
AS
BEGIN
UPDATE Tax_Rates
    SET
    code=@code,
    description=@description,
    effective_date=@effective_date,
    is_deleted=@is_deleted,
    caption_id=@caption_id,
    country_id=@country_id,
    rate=@rate
WHERE tax_rates_id = @tax_rates_id
END

GO

