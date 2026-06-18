SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_pmnav_get_process_key_sel'
GO


CREATE PROCEDURE spu_pmnav_get_process_key_sel
    @pmnav_process_id integer
AS

/*******************************************************************************************************/
/* sp_pmnav_get_process_key_sel selects the Set Keys for a Process */
/*******************************************************************************************************/
/*******************************************************************************************************/
/* Revision Description of Modification Date Who */
/* -------- --------------------------- ---- --- */
/* 1.0 Original 02/09/1998 RFC */
/*******************************************************************************************************/
BEGIN

/* Select the Process Set Keys */
SELECT
    nk.name,
    NULL

FROM pmnav_get_process_key gpk,
    pmnav_key nk

WHERE gpk.pmnav_process_id = @pmnav_process_id
AND gpk.pmnav_key_id = nk.pmnav_key_id

END
GO


