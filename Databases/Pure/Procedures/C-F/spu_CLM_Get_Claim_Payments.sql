SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_CLM_Get_Claim_Payments'
GO

CREATE PROCEDURE spu_CLM_Get_Claim_Payments  

@claim_peril_id int  
  
AS  
  
BEGIN  
--**********************************************  
--  Decide whether we are Underwriter or Broker  
--**********************************************  
  
DECLARE @AgentUnderwriter varchar(1)  
SELECT  @AgentUnderwriter = value  
FROM    hidden_options  
WHERE   branch_id = 1 and option_number = 1  
IF @AgentUnderwriter is null  
    SELECT @AgentUnderwriter = 'A'  
IF @AgentUnderwriter = ''  
    SELECT @AgentUnderwriter = 'A'  
IF @AgentUnderwriter = 'U'  
 BEGIN  
  SELECT  
    reserve_id,  
    recovery_id,  
    SUM(this_payment) AS this_payment,  
    SUM(this_payment_tax) AS this_payment_tax,  
    SUM(this_payment_tax_WHT) AS this_payment_tax_WHT,  
    SUM(paid_to_date) AS paid_to_date,  
    SUM(paid_to_date_tax) AS paid_to_date_tax,  
    SUM(paid_to_date_tax_WHT) AS paid_to_date_tax_WHT  
  
   FROM  (  
  
   --*********************  
   -- not live payments - this payment  
   --*********************  
  
    SELECT  
     wcpi.reserve_id AS reserve_id,  
     wcpi.recovery_id AS recovery_id,  
     (wcpi.this_payment * wcpi.payment_loss_xrate) + (wcpi.tax_amount * wcpi.payment_loss_xrate) + (wcpi.tax_amount_WHT * wcpi.payment_loss_xrate) AS this_payment,  
     wcpi.tax_amount * wcpi.payment_loss_xrate AS this_payment_tax ,  
     wcpi.tax_amount_WHT * wcpi.payment_loss_xrate AS this_payment_tax_WHT ,  
     0 AS paid_to_date ,  
     0 AS paid_to_date_tax,  
     0 AS paid_to_date_tax_WHT  
  
    FROM claim_payment_item wcpi  
  
     INNER JOIN (SELECT claim_payment_id  
          FROM claim_payment  
          WHERE claim_peril_id =@claim_peril_id  AND ISNULL(is_referred,0)<>2
          ) wcp ON  
      wcpi.claim_payment_id = wcp.claim_payment_id  
  
     INNER JOIN Claim_Peril cp ON cp.claim_peril_id = @claim_peril_id  
     WHERE wcpi.base_claim_payment_item_id = claim_payment_item_id AND cp.version_id > 1  
  
   --*********************  
    UNION ALL  
   --*********************  
  
   --*********************  
   -- live payments - paid to date  
   --*********************  
  
    SELECT  
     wcpi.reserve_id,  
     wcpi.recovery_id,  
     0 AS this_payment,  
     0 AS this_payment_tax,  
     0 AS this_payment_tax_WHT,  
     wcpi.this_payment * wcpi.payment_loss_xrate  AS paid_to_date, 
     --wcp.Amount AS paid_to_date,  
     wcpi.tax_amount * wcpi.payment_loss_xrate AS paid_to_date_tax,  
     wcpi.tax_amount_WHT * wcpi.payment_loss_xrate AS paid_to_date_tax_WHT  
  
    FROM claim_payment_item wcpi  
  
    INNER JOIN (SELECT *  
          FROM claim_payment  
           WHERE claim_peril_id = @claim_peril_id  AND ISNULL(is_referred,0)<>2
           ) wcp ON  
      wcpi.claim_payment_id = wcp.claim_payment_id  
  
     INNER JOIN Claim_Peril cp ON cp.claim_peril_id = @claim_peril_id  
     WHERE wcpi.base_claim_payment_item_id <> wcpi.claim_payment_item_id OR cp.version_id = 1  
  
   ) Claim_Payment  
  
   GROUP BY reserve_id, recovery_id  
 END  
ELSE  
 BEGIN  
 SELECT  
   reserve_id,  
   recovery_id,  
   SUM(this_payment) AS this_payment,  
   SUM(this_payment_tax) AS this_payment_tax,  
   SUM(this_payment_tax_WHT) AS this_payment_tax_WHT,  
   SUM(paid_to_date) AS paid_to_date,  
   SUM(paid_to_date_tax) AS paid_to_date_tax,  
   SUM(paid_to_date_tax_WHT) AS paid_to_date_tax_WHT  
  
  FROM  (  
  
  --*********************  
  -- Latest payments - this payment  
  --*********************  
  
/*   SELECT  
    cpi.reserve_id AS reserve_id,  
    cpi.recovery_id AS recovery_id,  
    cpi.this_payment * cpi.payment_loss_xrate AS this_payment,  
    cpi.tax_amount * cpi.payment_loss_xrate AS this_payment_tax ,  
    cpi.tax_amount_WHT * cpi.payment_loss_xrate AS this_payment_tax_WHT ,  
    0 AS paid_to_date ,  
    0 AS paid_to_date_tax,  
    0 AS paid_to_date_tax_WHT  
  
   FROM claim_payment_item cpi  
  
    INNER JOIN (SELECT max(claim_payment_id) claim_payment_id  
         FROM claim_payment  
         WHERE claim_peril_id =@claim_peril_id  
         ) cp ON  
     cpi.claim_payment_id = cp.claim_payment_id  
  
  --*********************  
   UNION ALL  
  --*********************  
  */  
  --*********************  
  -- Previous payments - paid to date  
  --*********************  
  
   SELECT  
    cpi.reserve_id,  
    cpi.recovery_id,  
    0 AS this_payment,  
    0 AS this_payment_tax,  
    0 AS this_payment_tax_WHT,  
--  cpi.this_payment * cpi.payment_loss_xrate AS paid_to_date,  
    cp.Amount AS paid_to_date,  
    cpi.tax_amount * cpi.payment_loss_xrate AS paid_to_date_tax,  
    cpi.tax_amount_WHT * cpi.payment_loss_xrate AS paid_to_date_tax_WHT  
  
   FROM claim_payment_item cpi  
  
   INNER JOIN (SELECT *  
         FROM claim_payment  
          WHERE claim_peril_id = @claim_peril_id  
          ) cp ON  
      cpi.claim_payment_id = cp.claim_payment_id  
  
  ) Claim_Payment  
  
  GROUP BY reserve_id, recovery_id  
 END  
  
  
END  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
