SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_get_stored_proc'
GO


CREATE PROCEDURE spu_get_stored_proc
	@sp_name varchar(128)
AS
SELECT 	*
FROM 		sysobjects so
WHERE 	so.xtype = 'P'
AND		so.name = @sp_name

GO


