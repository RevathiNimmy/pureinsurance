SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_SIR_MarkInstalmentStatus'
GO


CREATE PROCEDURE spu_SIR_MarkInstalmentStatus
    @lPFInstalmentId INT,
    @lStatus INT
AS

UPDATE
    PFInstalments
SET
    Status = @lStatus
WHERE
    pfinstalments_id = @lPFInstalmentId

GO
