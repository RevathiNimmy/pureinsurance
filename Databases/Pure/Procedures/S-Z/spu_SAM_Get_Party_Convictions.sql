SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_Get_Party_Convictions'
GO

CREATE PROCEDURE spu_SAM_Get_Party_Convictions

@party_cnt integer

AS

SELECT 

party_conviction_id, 
code, 
conviction_date, 
description, 
fine_amt, 
sentence_code, 
sentence_description, 
sentence_duration, 
sentence_duration_qualifier, 
sentence_effective_date, 
status_code, 
alcohol_level, 
alcohol_measurement_method, 
driving_licence_penalty_pts

FROM party_conviction

WHERE party_cnt = @party_cnt



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
