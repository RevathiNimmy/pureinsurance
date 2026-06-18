SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_clm_transaction_suppression_ind_sel'
GO

CREATE PROCEDURE spu_clm_transaction_suppression_ind_sel  
  
@insurance_file_cnt int,  
@claim_id int  
  
AS  
  
BEGIN  
  
 SELECT  
  ISNULL(p.suppress_reserves,0) as product_suppress_reserves,  
  ISNULL(p.suppress_payments,0)  as product_suppress_payments,  
  ISNULL(p.suppress_recoveries,0)  as product_suppress_recoveries,  
  ISNULL(c.suppress_reserves,0)  as claim_suppress_reserves,  
  ISNULL(c.suppress_payments,0)  as claim_suppress_payments,  
  ISNULL(c.suppress_recoveries,0)  as claim_suppress_recoveries  
  
 FROM product p  WITH (NOLOCK)
  
  INNER JOIN insurance_file ifile WITH (NOLOCK) ON  
   ifile.product_id = p.product_id  
  
   LEFT JOIN (  
    SELECT Top 1  
     suppress_reserves,  
     suppress_payments,  
     suppress_recoveries,  
     claim_id,  
     policy_id  
    FROM claim  WITH (NOLOCK)
    WHERE ((@claim_id = 0) OR (claim_id = @claim_id))  
    ) c ON  
    c.policy_id = ifile.insurance_file_cnt  
  
 WHERE insurance_file_cnt = @insurance_file_cnt  
  
END  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
