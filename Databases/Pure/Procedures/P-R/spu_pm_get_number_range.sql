SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_pm_get_number_range'
GO


CREATE PROCEDURE spu_pm_get_number_range
    @ProductCode char(10),
    @GroupCode char(10),
    @RangeCode char(10),
    @EffectiveDate datetime,
    @NumberRangeID int OUTPUT
AS

/********************************************************************************************************/
/* Revision Description of Modification Date Who */
/* -------- --------------------------- ---- ---- */
/* 1.0 Original 14/10/1998 CTAF */
/* 1.1 Modification 08/06/2001 RDC */
/********************************************************************************************************/
BEGIN
    DECLARE @pmnumber_group_id int,
    @pmproduct_id smallint

    /* Get the pmproductid */
    SELECT @pmproduct_id = pmproduct_id
    FROM PMProduct
    WHERE code = @ProductCode
        AND (is_deleted = 0)
        AND (effective_date <= @EffectiveDate)

    /* Get the group_id */
    SELECT @pmnumber_group_id = pmnumber_group_id
    FROM PMNumber_Group
    WHERE code = @GroupCode
        AND (pmproduct_id = @pmproduct_id)
        AND (is_deleted = 0)
        AND (effective_date <= @EffectiveDate)

    /* Get the Range */
    /* RDC08062001 added MIN() to get first pmnumber_range_id */
    SELECT @NumberRangeID = MIN(pmnumber_range_id)
    FROM PMNumber_Range
    WHERE pmnumber_group_id = @pmnumber_group_id
        AND (code = @RangeCode)
END
GO


