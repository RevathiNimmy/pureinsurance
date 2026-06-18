SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

EXECUTE DDLDropProcedure 'spu_wp_PFPolicies'
GO

CREATE PROCEDURE spu_wp_PFPolicies	@PartyCnt INT,
					@InsuranceFileCnt INT, 
					@ClaimCnt INT, 
					@DocumentRef varchar(25),
					@Instance1 INT,
					@Instance2 INT,
					@Instance3 INT
AS 

 
SELECT	P.resolved_name,
	R.description,
	I.this_premium,
	I.insurance_ref,
	DT.description "TransType",
	I.renewal_date
	
FROM	PFPremiumFinance PF,
	PFTransaction_id    PT,
	Orion_for_Broking..Transdetail  TD,
	Orion_for_Broking..Document	D,	
	Orion_for_Broking..DocumentType DT,
	Insurance_file I, 
	Risk_code R,
	Party P

WHERE	PF.insurance_file_cnt = @InsuranceFileCnt
AND	(PT.pfprem_finance_cnt = PF.pfprem_finance_cnt and PT.pfprem_finance_version = PF.pfprem_finance_version)
AND	PT.pftransaction_id = TD.transdetail_id
AND	TD.insurance_ref = I.insurance_ref
AND 	I.risk_code_id = R.risk_code_Id 
AND	TD.document_id = D.document_ID
AND 	D.DocumentType_Id = DT.documentType_id
AND	I.lead_insurer_cnt = P.party_cnt
AND	PF.pfprem_finance_cnt = @instance2 /1000000
AND	PF.pfprem_finance_version = (@instance2 - ((@instance2 / 1000000) * 1000000))

 	

 


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

