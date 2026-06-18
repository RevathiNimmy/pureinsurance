SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_get_source_base_currency'
GO

CREATE PROCEDURE spu_get_source_base_currency
        @SourceID INT
AS
BEGIN

    SELECT base_currency_id 
    FROM source 
    WHERE source_id = @SourceID

END
GO
