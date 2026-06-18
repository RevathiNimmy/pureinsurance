SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_ACT_Get_TransDetail_Type_Id'
GO

CREATE PROCEDURE spu_ACT_Get_TransDetail_Type_Id
    @transdetail_type_code varchar(30)
AS

BEGIN

	select transdetail_type_id
	from transdetail_type
	where code = @transdetail_type_code

END

GO