SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_Copy_Rating_Section'
GO

CREATE PROCEDURE spu_Copy_Rating_Section  
    @OldRiskCnt int,  
    @NewRiskCnt int  
AS  
  
/*******************************************************************************************/  
/*  Created by RWH(23/11/2000)  to copy Rating_Section records for one     */  
/*                                                    risk to another                      */  
/*  Amended by Tom(16/08/2001)  to take account of original flag     */  
/*  RKS 13/10/2005 - Premium Override work */  
/*******************************************************************************************/  
INSERT INTO Rating_Section (risk_cnt,  
    rating_section_id,  
    rating_section_type_id,  
    policy_section_type_id,  
    sequence_number,  
    description,  
    rate_type_id,  
    annual_rate,  
    sum_insured,  
    annual_premium,  
    this_premium,  
    original_flag,  
    currency_id,  
    is_amended,  
    calculated_premium,  
    override_reason,
    earning_pattern_id,
    state_id,
    country_id )    
SELECT  @NewRiskCnt,  
    rating_section_id,  
    rating_section_type_id,  
    policy_section_type_id,  
    sequence_number,  
    description,  
    rate_type_id,  
    annual_rate,  
    sum_insured,  
    annual_premium,  
    this_premium,  
    original_flag,  
    currency_id,  
    is_amended,  
    calculated_premium,  
    override_reason, earning_pattern_id,
    state_id,
    country_id  
FROM    Rating_Section  
WHERE   risk_cnt = @OldRiskCnt  
AND original_flag = 0  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
