SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_PFRF_delete'
GO


CREATE PROCEDURE spu_PFRF_delete
    @companyno int
AS


BEGIN
DELETE FROM PFRF
WHERE companyno = @companyno
END
GO


