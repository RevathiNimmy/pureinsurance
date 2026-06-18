SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_non_deleted_treaties_saa'
GO


CREATE PROCEDURE spu_non_deleted_treaties_saa
    @is_deleted int
AS


SELECT treaty_id

FROM treaty

WHERE is_deleted = @is_deleted
GO


