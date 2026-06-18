SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_wp_policyfeeu'
GO

CREATE PROCEDURE spu_wp_policyfeeu  
    @PartyCnt INT,  
    @InsuranceFileCnt INT,  
    @RiskID INT,  
    @ClaimCnt INT,  
    @DocumentRef VARCHAR(25),  
    @Instance1 INT,  
    @Instance2 INT,  
    @Instance3 INT  
AS  
  
BEGIN
 
SELECT  
 p.resolved_name,  
 pf.fee_rate_percentage 'fee_percentage',  
 pf.currency_amount 'fee_amount'  
FROM policy_fee_u pf  
JOIN party p  
 ON p.party_cnt = pf.party_cnt  
WHERE pf.insurance_file_cnt = @InsuranceFileCnt  
 And policy_fee_u_id = @instance2 
  
END
  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
