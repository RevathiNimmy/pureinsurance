SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_ACT_GetPFTransactionID'
GO
CREATE PROCEDURE spu_ACT_GetPFTransactionID
@instalmentid int
AS
SELECT
ISNULL(pfi.PFTransaction_id ,0)
FROM
PFInstalments PFI 
where  
  pfi.pfinstalments_id = @instalmentid
  and pfi.Status=3
GO
