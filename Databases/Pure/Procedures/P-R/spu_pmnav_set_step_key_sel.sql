SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_pmnav_set_step_key_sel'
GO


CREATE PROCEDURE spu_pmnav_set_step_key_sel
    @pmnav_map_id integer
AS

/*******************************************************************************************************/
/* Selects all of the Set Step keys for a Map. */
/*******************************************************************************************************/
/*******************************************************************************************************/
/* Revision Description of Modification Date Who */
/* -------- --------------------------- ---- --- */
/* 1.0 Original 02/09/1998 RFC */
/*******************************************************************************************************/
BEGIN

SELECT
    nk.name,
    ssk.initial_key_value,
    ssk.pmnav_step_id

FROM pmnav_set_step_key ssk,
    pmnav_key nk

WHERE ssk.pmnav_map_id = @pmnav_map_id
AND ssk.pmnav_key_id = nk.pmnav_key_id

ORDER BY
    ssk.pmnav_step_id ASC

END
GO


