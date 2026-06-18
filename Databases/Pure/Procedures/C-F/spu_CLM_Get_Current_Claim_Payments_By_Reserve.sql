SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_CLM_Get_Current_Claim_Payments_By_Reserve'
GO

CREATE PROCEDURE spu_CLM_Get_Current_Claim_Payments_By_Reserve  
  
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
 --******************************************  
 -- Get Claim Peril Payments  
 --******************************************  
  
 CREATE TABLE #Claim_Payments(  
    reserve_id int,  
    recovery_id int,  
    this_payment money,  
    this_payment_tax money,  
    this_payment_tax_WHT money,  
    paid_to_date money,  
    paid_to_date_tax money,  
    paid_to_date_tax_WHT money)  
  
 INSERT INTO #Claim_Payments (  
    reserve_id,  
    recovery_id,  
    this_payment,  
    this_payment_tax,  
    this_payment_tax_WHT,  
    paid_to_date,  
    paid_to_date_tax,  
    paid_to_date_tax_WHT)  
   EXEC spu_CLM_Get_Claim_Payments @claim_peril_id  
  
 --******************************************  
 -- Reserves And Associated Payments  
 --******************************************  
 IF @AgentUnderwriter = 'A'  
   BEGIN 
 
   SELECT  
	1 AS type,  
	reserve.reserve_id,  
	reserve_type.description,  
	ISNULL(initial_reserve,0) + ISNULL(revised_reserve,0),  
	Claim_Payments.paid_to_date,  
	Claim_Payments.paid_to_date_tax,  
	Claim_Payments.paid_to_date_tax_WHT,  
	ISNULL(initial_reserve,0) + ISNULL(revised_reserve,0) - ISNULL(Claim_Payments.paid_to_date,0) AS current_reserve,  
	Claim_Payments.this_payment,  
	Claim_Payments.this_payment_tax,  
	Claim_Payments.this_payment_tax_WHT,  
	ISNULL(Claim_Payments.this_payment,0) - ISNULL(Claim_Payments.this_payment_tax,0) - ISNULL(Claim_Payments.this_payment_tax_WHT,0) AS cost_to_claim,  
	reserve_type.is_excess  as is_excess,  
 	CASE WHEN ISNULL(Claim_Payments.reserve_id,0) = 0 THEN  
  		0  
 	ELSE  
  		1  
 	END AS is_History,
 	reserve.this_revision AS this_revision
   FROM reserve  
  
   LEFT JOIN reserve_type ON  
   	reserve.reserve_type_id = reserve_type.reserve_type_id  
  
   LEFT JOIN #Claim_Payments Claim_Payments ON  
   	reserve.reserve_id = claim_payments.reserve_id  
  
   WHERE reserve.claim_peril_id = @claim_peril_id  
  
   UNION ALL  
  
--******************************************  
-- Recoveries And Associated Payments  
--******************************************  
  
   SELECT MIN(type),  
   is_salvage,  
   MIN(description),  
   -SUM(initial_reserve),  
   -SUM(paid_to_date),  
   -SUM(paid_to_date_tax),  
   -SUM(paid_to_date_WHT),  
   -SUM(current_reserve),  
   0,  
   0,  
   0,  
   0,  
   0,
   0,
   0 AS this_revision 
  
   FROM  
  
   (SELECT  
   2 AS type,  
   is_salvage AS is_salvage,  
   CASE WHEN recovery_type.is_salvage = 0 THEN  
    'Third Party Recovery'  
   ELSE  
    'Salvage Recovery'  
   END AS description,  
   0 AS initial_reserve,  
   0 AS paid_to_date,  
   0 AS paid_to_date_tax,  
   0 AS paid_to_date_WHT,  
   0 AS current_reserve,  
   0 AS this_payment,  
   0 AS this_payment_tax,  
   0 AS this_payment_tax_WHT,  
   0 AS cost_to_claim,  
   0 AS is_excess,  
   0 AS is_History,
   0 AS this_revision    
  
  FROM (select distinct IS_SALVAGE  
         from recovery_type) Recovery_type  
  
  UNION ALL  
  
  SELECT  
	2 AS type,  
	is_salvage AS is_salvage,  
	CASE WHEN recovery_type.is_salvage = 0 THEN  
	'Third Party Recovery'  
	ELSE  
	'Salvage Recovery'  
	END AS description,  
	SUM(initial_reserve) AS initial_reserve,  
	SUM(received_to_date) AS paid_to_date,  
	SUM(tax_amount) AS paid_to_date_tax,  
	0 AS paid_to_date_WHT,  
	SUM(initial_reserve) + SUM(revised_reserve) AS current_reserve,  
	0 AS this_payment,  
	0 AS this_payment_tax,  
	0 AS this_payment_tax_WHT,  
	0  AS cost_to_claim,  
	0  as is_excess,  
 	CASE WHEN ISNULL(Claim_Payments.reserve_id,0) = 0 THEN  
  		0  
 	ELSE  
   		1  
 	END AS is_History,
 	0 AS this_revision  

  FROM recovery  
  
   LEFT JOIN recovery_type ON  
    recovery.recovery_type_id = recovery_type.recovery_type_id  
  
   LEFT JOIN #Claim_Payments Claim_Payments ON  
    recovery.recovery_id = claim_payments.recovery_id  
  
  WHERE recovery.claim_peril_id = @claim_peril_id  
  
  GROUP BY is_salvage,Claim_Payments.reserve_id)   Recovery  
  
  GROUP BY is_salvage  
  
  ORDER by type, 2  
  
  drop table #claim_payments  
 END  
ELSE  


 --UNDERWRITING

 BEGIN  
  SELECT  
   1 AS type,  
   reserve.reserve_id,  
   reserve_type.description,  
   ISNULL(initial_reserve,0) + ISNULL(revised_reserve,0),  
   Claim_Payments.paid_to_date,  
   Claim_Payments.paid_to_date_tax,  
   Claim_Payments.paid_to_date_tax_WHT,  
   ISNULL(initial_reserve,0) + ISNULL(revised_reserve,0) - ISNULL(Claim_Payments.paid_to_date,0) AS current_reserve,  
   Claim_Payments.this_payment,  
   Claim_Payments.this_payment_tax,  
   Claim_Payments.this_payment_tax_WHT,  
   ISNULL(Claim_Payments.this_payment,0) - ISNULL(Claim_Payments.this_payment_tax,0) - ISNULL(Claim_Payments.this_payment_tax_WHT,0) AS cost_to_claim,  
   ISNULL(reserve_type.is_excess,0)  as is_excess, 
   reserve_type.name reserve_type_code,
   CASE WHEN ISNULL(Claim_Payments.reserve_id,0) = 0 THEN  
    0  
   ELSE  
    1  
   END AS is_History,
   reserve.this_revision AS this_revision 
  FROM reserve  
  
   LEFT JOIN reserve_type ON  
    reserve.reserve_type_id = reserve_type.reserve_type_id  
  
   LEFT JOIN #Claim_Payments Claim_Payments ON  
     reserve.reserve_id = claim_payments.reserve_id  
  
  WHERE reserve.claim_peril_id = @claim_peril_id  
  
  UNION ALL  
  
 --******************************************  
 -- Recoveries And Associated Payments  
 --******************************************  
  
   SELECT MIN(type),  
   is_salvage,  
   MIN(description),  
   -SUM(initial_reserve),  
   -SUM(paid_to_date),  
   -SUM(paid_to_date_tax),  
   -SUM(paid_to_date_WHT),  
   -SUM(current_reserve),  
   0,  
   0,  
   0,  
   0,  
   0,  
   'N/A',  
   0,
   0 AS this_revision

   FROM  
  
   (SELECT  
   2 AS type,  
   is_salvage AS is_salvage,  
   CASE WHEN recovery_type.is_salvage = 0 THEN  
    'Third Party Recovery'  
   ELSE  
    'Salvage Recovery'  
   END AS description,  
   0 AS initial_reserve,  
   0 AS paid_to_date,  
   0 AS paid_to_date_tax,  
   0 AS paid_to_date_WHT,  
   0 AS current_reserve,  
   0 AS this_payment,  
   0 AS this_payment_tax,  
   0 AS this_payment_tax_WHT,  
   0 AS cost_to_claim,  
   0 AS is_excess,  
   0 AS is_History,
   0 AS this_revision  
  
  FROM (select distinct IS_SALVAGE  
         from recovery_type) Recovery_type  
  
  UNION ALL  
  
  SELECT  
   2 AS type,  
   is_salvage AS is_salvage,  
   CASE WHEN recovery_type.is_salvage = 0 THEN  
    'Third Party Recovery'  
   ELSE  
    'Salvage Recovery'  
   END AS description,  
   SUM(initial_reserve) + SUM(revised_reserve) AS total_reserve,  
   SUM(received_to_date) AS paid_to_date,  
   SUM(tax_amount) AS paid_to_date_tax,  
   0 AS paid_to_date_WHT,  
   SUM(initial_reserve) + SUM(revised_reserve) - SUM(received_to_date) AS current_reserve,  
   0 AS this_payment,  
   0 AS this_payment_tax,  
   0 AS this_payment_tax_WHT,  
   0  AS cost_to_claim,  
   0  as is_excess,  
   CASE WHEN ISNULL(Claim_Payments.reserve_id,0) = 0 THEN  
    0  
   ELSE  
    1  
   END AS is_History,
   0 AS this_revision  
  
  FROM recovery  
  
   LEFT JOIN recovery_type ON  
    recovery.recovery_type_id = recovery_type.recovery_type_id  
  
   LEFT JOIN #Claim_Payments Claim_Payments ON  
    recovery.recovery_id = claim_payments.recovery_id  
  
  WHERE recovery.claim_peril_id = @claim_peril_id  
  
  GROUP BY is_salvage,Claim_Payments.reserve_id)   Recovery  
  
  GROUP BY is_salvage  
  
  ORDER by type, 2  
  
  drop table #claim_payments  
 END  
END  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

