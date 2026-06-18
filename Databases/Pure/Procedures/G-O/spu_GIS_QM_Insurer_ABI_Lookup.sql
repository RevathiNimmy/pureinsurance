SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_GIS_QM_Insurer_ABI_Lookup'
GO


CREATE PROCEDURE spu_GIS_QM_Insurer_ABI_Lookup
    @qm_insurer_ref varchar(10)
AS

/* SP to return Insurer ABI Code for given QM code, joins Insurer and Scheme tables */
SELECT DISTINCT
    I.gis_insurer_id,
    I.abi_81_insurer,
    I.code,
    I.description

FROM
    GIS_Scheme  AS S,
    GIS_Insurer AS I
WHERE
    I.gis_insurer_id = S.gis_insurer_id
AND S.qm_insurer_ref = @qm_insurer_ref
GO


