SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_SIR_renewal_suspension_upd'
GO


CREATE PROCEDURE spu_SIR_renewal_suspension_upd
    @suspension_level int,
    @insurance_folder_cnt int
AS

/*
    CTAF: Reason why this might fail : The PMCaption tables are out of sync.
*/
BEGIN
    UPDATE Renewal_Control
            SET suspension_level = @suspension_level
            WHERE insurance_folder_cnt = @insurance_folder_cnt
END
GO


