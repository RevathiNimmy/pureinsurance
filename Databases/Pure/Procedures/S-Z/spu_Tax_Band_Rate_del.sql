SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Tax_Band_Rate_del'
GO


CREATE PROCEDURE spu_Tax_Band_Rate_del
    @tax_band_id int,
	@userid INT = NULL,
	@uniqueid VARCHAR(50) = NULL,
	@screenhierarchy VARCHAR(100) = NULL
AS

UPDATE tbr  SET 
        userid=@UserId,
        uniqueid=@uniqueid,
		screenhierarchy=@screenhierarchy + ' / ' + description + ' / ' + CAST(convert(date,effective_date,103) AS VARCHAR)
    FROM tax_band_rate tbr 

    WHERE
	    tax_band_id = @tax_band_id

    DELETE FROM tax_band_rate
        WHERE tax_band_id = @tax_band_id

GO


