SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_PFScheme_delete'
GO


CREATE PROCEDURE spu_PFScheme_delete
    @companyno int
AS


BEGIN
DELETE FROM PFScheme
WHERE companyno = @companyno
END
GO


