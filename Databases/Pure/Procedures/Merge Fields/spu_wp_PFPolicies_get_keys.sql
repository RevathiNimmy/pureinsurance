SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

EXECUTE DDLDropProcedure 'spu_wp_PFPolicies_get_keys'
GO

CREATE PROCEDURE spu_wp_PFPolicies_get_keys
					@PartyCnt INT,
					@InsuranceFileCnt INT, 
					@RiskID INT,
					@ClaimCnt INT,
					@DocumentRef varchar(25),
					@Instance1 INT,
					@Instance2 INT,
					@Instance3 INT
AS 
 
SELECT	((PF.pfprem_finance_cnt * 1000000) + PF.pfprem_finance_version)
	
FROM	PFPremiumFinance PF,
	PFTransaction_id    PT,
	Orion_for_Broking..Transdetail TD,
	Insurance_file I 
 
WHERE	PF.insurance_file_cnt = @InsuranceFileCnt
AND	(PT.pfprem_finance_cnt = PF.pfprem_finance_cnt and PT.pfprem_finance_version = PF.pfprem_finance_version)
AND	PT.pftransaction_id = TD.transdetail_id
AND	TD.insurance_ref = I.insurance_ref 
AND	I.insurance_file_cnt = PF.Insurance_file_cnt
 


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

