SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure spu_wp_risk
GO

CREATE PROCEDURE spu_wp_risk
    @PartyCnt INT,  
    @InsuranceFileCnt INT,  
    @RiskId INT,  
    @ClaimCnt INT,  
    @DocumentRef VARCHAR(25),  
    @Instance1 INT,  
    @Instance2 INT,  
    @Instance3 INT  
AS 
BEGIN 
  
    SELECT  
        risk_status = rs.description,  
        accumulation = a.description,  
        risk_type = rt.description,  
        description = r.description,  
        sum_insured_requested = r.sum_insured_requested,  
        inception_date = r.inception_date,  
        expiry_date = r.expiry_date,  
        is_not_index_linked = r.is_not_index_linked,  
        is_accumulated = r.is_accumulated,  
        lapsed_reason = lr.description,  
        lapsed_date = r.lapsed_date,  
        lapsed_description = r.lapsed_description,  
        total_sum_insured = r.total_sum_insured,  
        total_annual_premium = r.total_annual_premium,  
        total_this_premium = r.total_this_premium,  
        is_risk_selected = r.is_risk_selected,
        Risk_Cover_Note_Link_Id = cn.Risk_Cover_Note_Link_Id,
        Cover_Note_Ref = cn.Cover_Note_Ref,
        Cover_Note_From = cn.Cover_Note_From,
        Cover_Note_To = cn.Cover_Note_To
    FROM    
        risk r
        LEFT OUTER JOIN risk_type rt
            ON r.risk_type_id = rt.risk_type_id
        LEFT OUTER JOIN risk_status rs
            ON r.risk_status_id = rs.risk_status_id    
        LEFT OUTER JOIN accumulation a
            ON r.accumulation_id = a.accumulation_id    
        LEFT OUTER JOIN lapsed_reason lr  
            ON r.lapsed_reason_id = lr.lapsed_reason_id  
        LEFT OUTER JOIN Risk_Cover_Note_Link cn  
            ON r.risk_cnt = cn.risk_id  
  WHERE
        r.risk_cnt = @riskid


END

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO


