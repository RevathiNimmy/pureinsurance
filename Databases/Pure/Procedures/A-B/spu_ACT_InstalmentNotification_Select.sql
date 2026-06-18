SET QUOTED_IDENTIFIER OFF
EXECUTE DDLDropProcedure 'spu_ACT_InstalmentNotification_Select'
GO

CREATE PROCEDURE  spu_ACT_InstalmentNotification_Select
AS
Begin
SELECT 
	PFI.pfinstalments_id,
	SCH.ColNotDocID,
	PLN.Insurance_File_Cnt,
	PTY.party_cnt,
	PTY.shortname,
	dt.code
FROM
	PFInstalments PFI
INNER JOIN 
	PFInstalments_Status PFIS
ON
    PFIS.PFInstalments_Status_id = PFI.Status
INNER JOIN
	PFPremiumFinance PLN
ON
	PLN.pfprem_finance_cnt = PFI.pfprem_finance_cnt
AND 
	PLN.pfprem_finance_version = PFI.pfprem_finance_version
INNER JOIN 
	PFScheme SCH 
ON
	SCH.schemeversion = PLN.schemeversion
AND 
	SCH.schemeno = PLN.schemeno
AND 
	SCH.companyno = PLN.companyno
INNER JOIN 
	PFRF 
ON
   PFRF.pfrf_id = PLN.pfrf_id
INNER JOIN 
	Party PTY 
ON
   PTY.party_cnt = PLN.clientid
INNER JOIN 
	Document_template dt
ON 
	dt.document_template_id = SCH.ColNotDocID
WHERE 
	SCH.ColNotDocID IS NOT NULL
AND
	PFI.DueDate < DATEADD(d,SCH.ColNotNumDays , GETDATE())
AND
	PFI.DueDate > GETDATE()
AND
	PFI.notification_sent = 0
AND
	PFI.Status = 1
AND
	PFI.InstalmentNumber > 0
AND
	PLN.statusind = '040'
End
