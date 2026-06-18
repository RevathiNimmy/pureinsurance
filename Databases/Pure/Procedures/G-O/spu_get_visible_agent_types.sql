SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_get_visible_agent_types'
GO

CREATE PROCEDURE spu_get_visible_agent_types
AS
	select  [description], party_agent_type_id
	from	party_agent_type
	where	is_visible = 1
	and	is_deleted = 0
	and	effective_date <= GetDate()
GO

