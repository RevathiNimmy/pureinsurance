SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_pmuser_group_user_add'
GO


CREATE PROCEDURE spu_pmuser_group_user_add
    @pmuser_group_id INT,
    @user_id INT,
    @display_sequence_num SMALLINT,
    @is_supervisor INT
AS

/********************************************************************************************************//* Revision Description of Modification Date Who */
/* Revision Description of Modification Date Who */
/* -------- --------------------------- ---- --- */
/* 1.0 Original 20/08/1997 JW */
/* 1.1 Added Is Supervisor flag 20/11/1997 TO */
/********************************************************************************************************/
INSERT INTO pmuser_group_user (pmuser_group_id,
        user_id,
        display_sequence_num,
        is_supervisor)
    SELECT @pmuser_group_id,
        @user_id,
        @display_sequence_num,
        @is_supervisor
GO


