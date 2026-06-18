SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_Get_Party_Accidents'
GO

CREATE PROCEDURE spu_SAM_Get_Party_Accidents

@party_cnt integer

AS

SELECT 
	previous_accidents_id, 
	date, 
	description, 
	is_at_fault

FROM previous_accidents

WHERE party_cnt = @party_cnt


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
