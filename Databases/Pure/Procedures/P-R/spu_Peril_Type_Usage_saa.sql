SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_Peril_Type_Usage_saa'
GO


CREATE PROCEDURE spu_Peril_Type_Usage_saa
    @peril_group_id int
AS


SELECT
    ptu.peril_group_id,
    ptu.peril_type_id,
    ptu.allocate_percent,
    pt.code,
    pt.description
FROM    Peril_Type_Usage ptu,
    Peril_Type pt

WHERE   peril_group_id = @peril_group_id
AND ptu.peril_type_id = pt.peril_type_id
ORDER BY code
GO


