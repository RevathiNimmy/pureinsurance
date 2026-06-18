SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_ACT_max_min_document_id_sel'
GO

CREATE PROCEDURE spu_ACT_max_min_document_id_sel
    @max_ind TINYINT,
    @effective_date DATETIME
AS
    
    DECLARE @date1 DATETIME
    DECLARE @date2 DATETIME
        
    SELECT @date1 = convert(datetime, convert(char(10), @effective_date,120), 120)
    SELECT @date2 = dateadd(d,1,@date1)
    
    
    IF @max_ind = 1
    BEGIN
        SELECT MAX(document_id) FROM document
            WHERE created_date >= @date1
            AND created_date  < @date2
    END
    ELSE
    BEGIN
        SELECT MIN(document_id) FROM document
            WHERE created_date >= @date1
            AND created_date  < @date2
END

GO

