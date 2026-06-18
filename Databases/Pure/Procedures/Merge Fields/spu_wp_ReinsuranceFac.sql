SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO



EXECUTE DDLDropProcedure 'spu_wp_ReinsuranceFac'
GO


CREATE PROCEDURE spu_wp_ReinsuranceFac  
    @PartyCnt INT,  
    @InsuranceFileCnt INT,  
    @RiskId INT,  
    @ClaimCnt INT,  
    @DocumentRef VARCHAR(25),  
    @Instance1 INT,  
    @Instance2 INT,  
    @Instance3 INT  
AS  
  
         DECLARE reinsurance_result SCROLL CURSOR FOR  
        SELECT  p.resolved_name AS Reinsurer_Name1,  
                sum(case when original_flag=1 then 0 else ral.this_share_percent end) AS Reinsurer_Share,  
          sum(ral.premium_value) AS Reinsurer_Share_Value,  
          sum(case when original_flag=1 then 0 else ral.sum_insured end) AS Reinsurer_Sum_Insured,  
          sum(case when original_flag=1 then 0 else ral.commission_percent end) AS Reinsurer_Commission_Percent,  
                sum(ral.commission_value) AS Reinsurer_Commission_Value,  
                sum(ral.premium_tax) AS Reinsurer_Premium_Tax,  
                sum(ral.commission_tax) AS Reinsurer_Commission_Tax,  
    a.address1  as fac_address1,  
                a.address2  as fac_address2,  
                a.address3  as fac_address3,  
                a.address4  as fac_address4,  
                a.postal_code   as fac_postal_code,  
    sum(ral.premium_value-ral.commission_value) AS Net_Premium,
	sum(case when original_flag=1 then 0 else r.total_sum_insured end) TotalRiskSumInsured, sum(case when original_flag=1 then 0 else r.total_annual_premium end) TotalRiskAnnualPremium,sum(r.total_this_premium) TotalRiskThisPremium
  
        FROM  ri_arrangement ra  
        JOIN    ri_arrangement_line ral  
                ON ral.ri_arrangement_id = ra.ri_arrangement_id  
        JOIN party P  
                ON p.party_cnt = ral.party_cnt  
  LEFT JOIN Party_Address_Usage AS PAU on PAU.party_cnt = p.party_cnt  
        LEFT JOIN Address_Usage_Type AS AUT on AUT.address_usage_type_id = PAU.address_usage_type_id  
        LEFT JOIN Address as A on A.address_cnt = PAU.address_cnt  
		JOIN Risk r on r.risk_cnt=ra.risk_cnt
        WHERE  ra.risk_cnt = @RiskId  
        AND     ral.type = 'F'  
  AND  PAU.address_usage_type_id=4  
  GROUP BY  
  p.resolved_name ,  
a.address1,  
a.address2,  
a.address3,  
a.address4,  
a.postal_code
  
    OPEN reinsurance_result  
  
    FETCH ABSOLUTE @Instance1   FROM reinsurance_result  
  
    CLOSE reinsurance_result  
    DEALLOCATE reinsurance_result  
GO