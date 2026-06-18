SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

EXECUTE DDLDropProcedure 'spu_policy_coinsurers_section_del'
GO

CREATE PROCEDURE spu_policy_coinsurers_section_del
(
	@policy_coinsurers_section_id int
)
	
AS

DELETE FROM policy_coinsurers_section WHERE policy_coinsurers_section_id=@policy_coinsurers_section_id

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

