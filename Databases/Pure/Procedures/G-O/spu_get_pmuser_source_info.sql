SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_get_pmuser_source_info'
GO

CREATE PROCEDURE spu_get_pmuser_source_info
    @user_id integer
AS

BEGIN
	select s.source_id, s.code, s.description, isnull(ps.source_id, 0)
	from source s
	left outer join pmuser_source ps
	on ps.source_id = s.source_id
	and ps.user_id = @user_id
END
GO
