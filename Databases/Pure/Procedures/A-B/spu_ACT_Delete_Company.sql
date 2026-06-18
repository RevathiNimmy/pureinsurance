SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Delete_Company'
GO


CREATE PROCEDURE spu_ACT_Delete_Company
    @company_id smallint
AS


DELETE FROM Company
WHERE company_id = @company_id
GO


