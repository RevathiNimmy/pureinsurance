SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_taxgroup_taxbands_sel'
GO

CREATE PROCEDURE spu_taxgroup_taxbands_sel
    @tax_group_id int
AS

    SELECT  tg.tax_group_id,
        tg.tax_band_id,
        tg.sequence,
        tb.description,
        tg.allocation_sequence,
        tg.allocation_rule    
    FROM    Tax_group_tax_band tg
    JOIN    tax_band tb on tg.tax_band_id = tb.tax_band_id
    WHERE   tg.tax_group_id = @tax_group_id
GO

