SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Select_Instalment_Post_Details'
GO
CREATE PROCEDURE spu_ACT_Select_Instalment_Post_Details
@instalmentid int
AS
SELECT
PFI.Amount,
PFP.PlanTransaction_id,
PFIS.code
FROM
PFInstalments PFI, PFPremiumFinance PFP, PFInstalments_status PFIS, PFScheme PFS
where PFP.pfprem_finance_cnt = PFI.pfprem_finance_cnt
and PFP.pfprem_finance_version = PFI.pfprem_finance_version
and PFI.status = PFIS.pfinstalments_status_id
and PFP.CompanyNo = PFS.CompanyNo
AND PFP.SchemeNo = PFS.SchemeNo
AND PFP.SchemeVersion = PFS.SchemeVersion
and pfi.pfinstalments_id = @instalmentid
GO
