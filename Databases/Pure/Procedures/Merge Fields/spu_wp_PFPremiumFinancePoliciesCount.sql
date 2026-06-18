SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

EXECUTE DDLDropProcedure 'spu_wp_PFPremiumFinancePoliciesCount'
GO


CREATE PROCEDURE spu_wp_PFPremiumFinancePoliciesCount	@PartyCnt INT,
							@InsuranceFileCnt INT, 
							@RiskId int,
							@ClaimCnt INT,
							@documentRef varchar(25),
							@Instance1 INT,
							@Instance2 INT,
							@Instance3 INT
AS 

    DECLARE 
        @pfprem_finance_cnt int,
    	@pfprem_finance_version int
    
    select 	@pfprem_finance_cnt = max(pfprem_finance_cnt),
        	@pfprem_finance_version = max(pfprem_finance_version)
    from	pftransaction_id 
    where 	insurance_file_cnt=@InsuranceFileCnt
    
    
    SELECT	count(insurance_file_cnt) "how_many"
    FROM	pftransaction_id 
    WHERE	pfprem_finance_cnt=@pfprem_finance_cnt
      AND	pfprem_finance_version=@pfprem_finance_version

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

