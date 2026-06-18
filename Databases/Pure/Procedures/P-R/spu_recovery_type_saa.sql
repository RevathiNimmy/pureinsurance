SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_recovery_type_saa'
GO


CREATE PROCEDURE spu_recovery_type_saa
    @is_salvage tinyint,
    @effective_date datetime = null
AS

    SELECT  recovery_type_id, 
            description
    FROM    recovery_type
    WHERE   effective_date <= isnull(@effective_date, getdate())
    AND     is_deleted = 0
    AND     is_salvage = @is_salvage

GO

