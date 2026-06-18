SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_PM_Check_Source'
GO


CREATE PROCEDURE spu_PM_Check_Source
    @source_id integer OUTPUT
AS

/********************************************************************************************************/
/* Revision Description of Modification Date Who */
/* -------- --------------------------- ---- --- */
/* 1.0 New procedure 09/05/2000 DAK */
/********************************************************************************************************/
SELECT @source_id = source_id
    FROM Source
    WHERE source_id = @source_id

    IF @source_id = NULL
        SELECT @source_id = -1
GO


