SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_chk_resv_type_name_exists'
GO


CREATE PROCEDURE spu_chk_resv_type_name_exists
    @Name varchar(255)
AS


SELECT Name
FROM Reserve_type
WHERE (Name =@Name)
GO


