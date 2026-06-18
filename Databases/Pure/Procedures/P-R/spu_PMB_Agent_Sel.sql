/* DC290702 created to obtain all agents */

EXECUTE DDLDropProcedure 'spu_PMB_Agent_Sel'
GO

SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


CREATE PROCEDURE spu_PMB_Agent_Sel
AS

	Select Party_cnt, Shortname 
	from Party p 
	join party_type pt
	on p.party_type_id = pt.party_type_id
	where pt.code = 'AG'
	order by shortname

GO

SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

