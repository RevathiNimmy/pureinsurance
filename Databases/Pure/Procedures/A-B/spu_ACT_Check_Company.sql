SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Check_Company'
GO


CREATE PROCEDURE spu_ACT_Check_Company
    @company_id smallint OUTPUT
AS


BEGIN
    SELECT @company_id = company_id
    FROM Company
    WHERE company_id = @company_id
END
BEGIN
IF @company_id = NULL
    SELECT @company_id = -1
END
GO


