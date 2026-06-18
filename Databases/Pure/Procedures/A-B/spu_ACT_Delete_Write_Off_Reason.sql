SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Delete_Write_Off_Reason'
GO


CREATE PROCEDURE spu_ACT_Delete_Write_Off_Reason
    @write_off_reason_id int
AS


DELETE FROM Write_Off_Reason
WHERE write_off_reason_id = @write_off_reason_id
GO


