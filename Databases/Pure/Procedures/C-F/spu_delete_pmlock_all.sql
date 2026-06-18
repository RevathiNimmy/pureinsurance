SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_delete_pmlock_all'
GO


CREATE PROCEDURE spu_delete_pmlock_all
AS

/******************************************************************************************/
/* sp_delete_pmlock_all deletes all lock records in the PMLock table. */
/* no parameters are passed */
/******************************************************************************************/
/* Revision Description of Modification Date Who */
/* -------- --------------------------- ---- --- */
/* 1.0 Original 30/06/1997 TF */
/* 1.1 Removed the user_id parameter. 22/12/1998 RFC */
/******************************************************************************************/
BEGIN
    /* Delete records from PMLock */
    DELETE FROM PMLock
    WHERE Is_system_lock = 0
END
GO


