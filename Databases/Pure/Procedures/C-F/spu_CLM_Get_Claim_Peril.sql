SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

EXECUTE DDLDropProcedure 'spu_CLM_Get_Claim_Peril'
GO

/*******************************************************************************************
 Version      Author  Date        Desc
 1.00.0001    RSB     22/08/2003  Get claim perils, recovery and reserve. Change this to
				  left outer joins because claims may not have a peril or reserve.
*******************************************************************************************/
CREATE PROCEDURE spu_CLM_Get_Claim_Peril

	@ClaimId int
AS

SELECT	wcp.claim_peril_id AS ClaimPeril_ClaimPerilId,
		wcp.peril_type_id AS ClaimPeril_PerilTypeId,
		wcp.description AS ClaimPeril_Description,
		wrec.recovery_type_id AS Recovery_RecoveryTypeId,
		wrec.initial_reserve AS Recovery_InitialReserve,
		wrec.received_to_date AS Recovery_ReceivedToDate,
		wrec.revised_reserve AS Recovery_RevisedReserve,
		wres.initial_reserve AS Reserve_InitialReserve,
		wres.paid_to_date AS Reserve_PaidToDate,
		wres.revised_reserve AS Reserve_RevisedReserve
FROM		Work_Claim_Peril wcp
-- CQ 2017 - Start
LEFT OUTER JOIN	Work_Recovery wrec
ON		wcp.claim_peril_id = wrec.claim_peril_id
LEFT OUTER JOIN	Work_Reserve wres
-- CQ 2017 - End
ON		wcp.claim_peril_id = wres.claim_peril_id
WHERE	wcp.claim_id = @ClaimId
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
