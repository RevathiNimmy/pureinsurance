SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_pmuser_group_user_del'
GO


CREATE PROCEDURE spu_pmuser_group_user_del
    @pmuser_group_id INT
AS

/********************************************************************************************************/
/* Revision Description of Modification Date Who */
/* -------- --------------------------- ---- --- */
/* 1.0 Original 20/08/1997 JW */
/********************************************************************************************************/
DELETE FROM pmuser_group_user
    WHERE pmuser_group_id = @pmuser_group_id
GO


