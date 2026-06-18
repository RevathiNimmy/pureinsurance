SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_PFGetPaidInstalments'
GO


CREATE PROCEDURE spu_PFGetPaidInstalments
	@premium_finance_cnt int,
	@premium_finance_version int

AS

/*Get Paid Instalments */

SELECT  PFinstalments_id, isnull(a.allocation_id,0) 
FROM PFinstalments i 
JOIN PFInstalments_status s 
	ON i.status = s.PFinstalments_status_id
LEFT JOIN AllocationDetail a
	ON i.PFTransaction_id = a.transdetail_id
WHERE pfprem_finance_Cnt = @premium_finance_cnt
AND pfprem_finance_version = @premium_finance_version
AND s.code = 'C'

  
GO
