if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spu_ACT_Get_PFPremiumFinance]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[spu_ACT_Get_PFPremiumFinance]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO


CREATE PROCEDURE spu_ACT_Get_PFPremiumFinance
		@allocationId INT

AS

SELECT	 
 	F.pfprem_finance_cnt,
 	F.pfprem_finance_version, 
 	SUM(D2.alloc_ccy_amount),
 	SUM(D2.alloc_base_amount),
 	(select SUM(D2.write_off_amount)
	from Allocation A,
 		AllocationDetail D,
		AllocationDEtail D2,
 		PFPremiumFinance F
	where	A.allocation_Id = @allocationid
	AND  	A.allocation_Id = D.Allocation_Id
	AND	D.transdetail_id = F.PlanTransaction_id
	AND 	D.transdetail_id = D2.transdetail_id
	AND 	D2.write_off_amount is not NULL)
 
FROM	
	Allocation A,
 	AllocationDetail D,
	AllocationDEtail D2,
 	PFPremiumFinance F
WHERE	A.allocation_Id = @allocationid
AND  	A.allocation_Id = D.Allocation_Id
AND	D.transdetail_id = F.PlanTransaction_id
AND 	D.transdetail_id = D2.transdetail_id
 	GROUP BY F.pfprem_finance_cnt,
	F.pfprem_finance_version
 

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

