SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_Risk_Type_Usage_sel'
GO


CREATE PROCEDURE spu_Risk_Type_Usage_sel
    @risk_type_id int
AS


SELECT
    RTU.risk_type_group_id

 FROM   Risk_Type_Usage RTU,
    Risk_Type_Group RTG

WHERE RTU.risk_type_id = @risk_type_id
 AND RTU.risk_type_group_id = RTG.risk_type_group_id
 AND RTG.is_deleted = 0

ORDER BY RTG.code ASC
GO


