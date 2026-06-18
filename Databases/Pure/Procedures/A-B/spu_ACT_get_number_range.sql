SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_get_number_range'
GO


CREATE PROCEDURE spu_ACT_get_number_range
    @GroupCode char(10),
    @RangeCode char(10),
    @EffectiveDate datetime,
    @NumberRangeID int OUTPUT
AS

/********************************************************************************************************/
/* Revision Description of Modification Date Who */
/* -------- --------------------------- ---- ---- */
/* 1.0 Original 14/10/1998 CTAF */
/********************************************************************************************************/
BEGIN

DECLARE @actnumber_group_id int

    /* Get the group_id */
    SELECT @actnumber_group_id = actnumber_group_id
    FROM ACTNumber_Group
    WHERE code = @GroupCode
    AND (is_deleted = 0)
    AND (effective_date <= @EffectiveDate)

    /* Get the Range */
    SELECT @NumberRangeID = ACTnumber_range_id FROM ACTNumber_Range
    WHERE actnumber_group_id = @actnumber_group_id
    AND (code = @RangeCode)

END
GO


