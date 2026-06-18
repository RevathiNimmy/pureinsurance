SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_pmnav_set_map_key_sel'
GO


CREATE PROCEDURE spu_pmnav_set_map_key_sel
    @pmnav_map_id integer
AS

/*******************************************************************************************************/
/* sp_pmnav_set_map_key_sel selects the Set Keys for a Map */
/*******************************************************************************************************/
/*******************************************************************************************************/
/* Revision Description of Modification Date Who */
/* -------- --------------------------- ---- --- */
/* 1.0 Original 02/09/1998 RFC */
/*******************************************************************************************************/
BEGIN

/* Select the map Set Keys */
SELECT
    nk.name,
    smk.initial_key_value

FROM pmnav_set_map_key smk,
    pmnav_key nk

WHERE smk.pmnav_map_id = @pmnav_map_id
AND smk.pmnav_key_id = nk.pmnav_key_id

END
GO


