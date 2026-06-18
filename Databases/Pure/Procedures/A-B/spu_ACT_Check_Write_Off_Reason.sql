SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Check_Write_Off_Reason'
GO


CREATE PROCEDURE spu_ACT_Check_Write_Off_Reason
    @write_off_reason_id int OUTPUT
AS


BEGIN
    SELECT @write_off_reason_id = write_off_reason_id
    FROM Write_Off_Reason
    WHERE write_off_reason_id = @write_off_reason_id
END
BEGIN
IF @write_off_reason_id = NULL
    SELECT @write_off_reason_id = -1
END
GO


