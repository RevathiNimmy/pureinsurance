-------------------------------------------------------------------------------
--  Author:  AMB
--  Date:    02 Oct 2003
--  Desc:    SFB 1.8.6 Accident Management development
--           10/02/04 Renamed from 'spu_GIS_Scheme_by_Risk_Group_sel' to 'spu_Extra_Scheme_by_Risk_Group_sel'
--                    Changed link from GIS_Scheme to new table Extra_scheme
-------------------------------------------------------------------------------

SET QUOTED_IDENTIFIER OFF 
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Extra_Scheme_by_Risk_Group_sel'
GO

CREATE PROCEDURE spu_Extra_Scheme_by_Risk_Group_sel
(
    @party_cnt      int,
    @risk_group_id  int
)
AS 

SELECT 
    ex.extra_scheme_id,
    ex.description
FROM 
    Fee_Amounts AS fa
INNER JOIN
    Extra_Scheme AS ex ON fa.extra_scheme_id = ex.extra_scheme_id
WHERE
    fa.party_cnt = @party_cnt
AND
    fa.risk_group_id = @risk_group_id
AND
    ex.description <> ''
ORDER BY
    ex.description ASC

GO


