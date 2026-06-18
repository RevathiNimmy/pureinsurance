SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

EXECUTE DDLDropProcedure 'spu_wp_PFPoliciesCount'
GO


CREATE PROCEDURE spu_wp_PFPoliciesCount		@PartyCnt INT,
						@InsuranceFileCnt INT, 
						@ClaimCnt INT,
						@documentRef varchar(25),
						@Instance1 INT,
						@Instance2 INT,
						@Instance3 INT
AS 
SELECT	count(I.insurance_file_cnt) "how_many"
	
FROM	PFPremiumFinance PF,
	PFTransaction_Id    PT,
	Orion_for_Broking..Transdetail TD,
	Insurance_file I,	
	Party P
	 
WHERE	PF.insurance_file_cnt = @InsuranceFileCnt
AND	(PT.pfprem_finance_cnt = PF.pfprem_finance_cnt and PT.pfprem_finance_version = PF.pfprem_finance_version)
AND	PT.pftransaction_id = TD.transdetail_id
AND	TD.insurance_ref = I.insurance_ref
AND	I.lead_insurer_cnt = P.Party_Cnt


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

