EXECUTE DDLDropProcedure 'spu_GET_Insurer_group_rate'
GO

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE spu_GET_Insurer_group_rate
    @party_cnt int,
    @Scheme int,
    @risk_group_id int,
    @effective_date datetime
AS
SELECT
    party_cnt,
    Scheme,
    risk_group_id,
    effective_date,
    rate1,
    value1,
    minimum_total1,
    rate2,
    value2,
    minimum_total2,
    rate3,
    value3,
    minimum_total3
FROM 	Insurer_group_rate
WHERE 	party_cnt = @party_cnt 
AND 	Scheme = @Scheme 
AND 	risk_group_id = @risk_group_id 
AND 	effective_date <= @effective_date
ORDER BY effective_date DESC

GO