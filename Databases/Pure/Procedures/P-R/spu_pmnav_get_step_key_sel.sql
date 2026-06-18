SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_pmnav_get_step_key_sel'
GO


CREATE PROCEDURE spu_pmnav_get_step_key_sel
    @pmnav_map_id integer
AS

/*******************************************************************************************************/
/* Selects all of the Get Step keys for a Map. */
/*******************************************************************************************************/
/*******************************************************************************************************/
/* Revision Description of Modification Date Who */
/* -------- --------------------------- ---- --- */
/* 1.0 Original 02/09/1998 RFC */
/*******************************************************************************************************/
BEGIN

SELECT
    nk.name,
    NULL,
    gsk.pmnav_step_id

FROM pmnav_get_step_key gsk,
    pmnav_key nk

WHERE gsk.pmnav_map_id = @pmnav_map_id
AND gsk.pmnav_key_id = nk.pmnav_key_id

ORDER BY
    gsk.pmnav_step_id ASC

END
GO


