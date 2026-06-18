SET QUOTED_IDENTIFIER OFF 
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Get_Agent_Detail'
GO

CREATE PROCEDURE spu_Get_Agent_Detail
    @party_cnt INT
AS
SELECT pa.common_renewal_date,
       pc.code,
       pa.is_single_instalment_plan 
FROM party_agent pa
LEFT JOIN party p ON p.party_cnt=pa.party_cnt
LEFT JOIN party_category pc ON pc.party_category_id=p.party_category_id
WHERE p.party_cnt=@party_cnt
GO
