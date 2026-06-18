SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_SelAll_Write_Off_Reason'
GO


CREATE PROCEDURE spu_ACT_SelAll_Write_Off_Reason
AS


SELECT
    write_off_reason_id,
    description,
    is_deleted,
    code,
    caption_id,
    effective_date,
	ISNULL(is_valid_for_instalments,0) As is_valid_for_instalments
FROM Write_Off_Reason
GO


