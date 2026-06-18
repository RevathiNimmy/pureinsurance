SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_get_all_reserve_types'
GO


CREATE PROCEDURE spu_get_all_reserve_types
AS


SELECT Reserve_type_id, Name, Description,Include_in_Total
    FROM reserve_type
GO


