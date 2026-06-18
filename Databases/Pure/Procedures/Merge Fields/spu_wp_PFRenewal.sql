SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

EXECUTE DDLDropProcedure 'spu_wp_PFRenewal'
GO

CREATE PROCEDURE spu_wp_PFRenewal
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @ClaimCnt INT,
    @DocumentRef varchar(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT 
AS
declare @RenewalInd char(1),
	@MTAInd char(1)


	 
SELECT @renewalInd =  'X'
FROM	PFPremiumFinance PF,
	PFTransaction_id    PT,
	Orion_for_Broking..Transdetail  TD,
	Orion_for_Broking..Document	D, 
	Insurance_file I
 

WHERE	PF.insurance_file_cnt = 1916
AND	(PT.pfprem_finance_cnt = PF.pfprem_finance_cnt and PT.pfprem_finance_version = PF.pfprem_finance_version)
AND	PT.pftransaction_id = TD.transdetail_id
AND	TD.insurance_ref = I.insurance_ref
AND	TD.document_id = D.document_ID
AND 	left(D.Document_ref,3) in ('SND','SNC','SRC','SRD')
 
SELECT 	@MTAInd =  'X'
FROM	PFPremiumFinance PF,
	PFTransaction_id    PT,
	Orion_for_Broking..Transdetail  TD,
	Orion_for_Broking..Document	D, 
	Insurance_file I
 

WHERE	PF.insurance_file_cnt = 1916
AND	(PT.pfprem_finance_cnt = PF.pfprem_finance_cnt and PT.pfprem_finance_version = PF.pfprem_finance_version)
AND	PT.pftransaction_id = TD.transdetail_id
AND	TD.insurance_ref = I.insurance_ref
AND	TD.document_id = D.document_ID
AND 	left(D.Document_ref,3) in ('SED','SCC')
SELECT  @RenewalInd,
  	@MTAInd	


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

