SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_pmuser_group_group_add'
GO


CREATE PROCEDURE spu_pmuser_group_group_add
    @pmuser_group_id INT,
    @pmuser_member_group_id INT,
    @display_sequence_num SMALLINT
AS

/********************************************************************************************************/
/* Revision Description of Modification Date Who */
/* -------- --------------------------- ---- --- */
/* 1.0 Original 20/08/1997 JW */
/********************************************************************************************************/
INSERT INTO pmuser_group_group (pmuser_group_id,
        pmuser_member_group_id,
        display_sequence_num)
    SELECT @pmuser_group_id,
        @pmuser_member_group_id,
        @display_sequence_num
GO


