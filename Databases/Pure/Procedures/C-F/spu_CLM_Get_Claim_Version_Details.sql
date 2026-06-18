SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_CLM_Get_Claim_Version_Details'
GO
 
CREATE PROCEDURE spu_CLM_Get_Claim_Version_Details   
@claim_id int  
AS  
BEGIN  
  DECLARE @RI2007 INT, @base_claim_id INT

  SELECT @RI2007 = value 
  FROM hidden_options WHERE option_Number=88  

  SELECT @base_claim_id = base_claim_id 
  FROM claim WHERE claim_id = @claim_id
 
  CREATE TABLE #Claims(claim_id INT, transaction_type_id INT)

  INSERT INTO #Claims
  SELECT claim_id,
  transaction_type_id
  FROM claim 
  WHERE base_claim_id = @base_claim_id
  AND is_dirty = 0
  
  CREATE TABLE #Reserve(this_reserve_payment NUMERIC(19,6), this_reserve_revision NUMERIC(19,6), reserve_total_incurred NUMERIC(19,6), reserve_total_paid NUMERIC(19,6), current_reserve NUMERIC(19,6), claim_id INT)
  
  INSERT INTO #Reserve
  SELECT SUM(ISNULL(r.this_payment,0)), 
  SUM(ISNULL(r.this_revision,0)), 
  SUM(ISNULL(r.initial_reserve,0)) + SUM(ISNULL(r.Revised_reserve,0)),
  SUM(ISNULL(r.paid_to_date,0)), 
  SUM(ISNULL(r.initial_reserve,0)) + SUM(ISNULL(r.revised_reserve,0)) - SUM(ISNULL(r.paid_to_date,0)),
  c.claim_id
  FROM reserve r
  INNER JOIN claim_peril cp ON r.claim_peril_id = cp.claim_peril_id
  INNER JOIN #Claims c ON cp.claim_id = c.claim_id
  GROUP BY c.claim_id


  CREATE TABLE #Recoveries(recovery_amount NUMERIC(19,6), received_to_Date NUMERIC(19,6), claim_id INT, transaction_type_code VARCHAR(30))
  
  INSERT INTO #Recoveries
  SELECT SUM(ISNULL(rec.this_receipt_net,0)), 
  SUM(ISNULL(rec.received_to_date,0)), c.claim_id, tt.code
  FROM recovery rec
  INNER JOIN claim_peril cp ON rec.claim_peril_id = cp.claim_peril_id
  INNER JOIN #Claims c ON cp.claim_id = c.claim_id
  INNER JOIN transaction_type tt ON c.transaction_type_id = tt.transaction_type_id
  GROUP BY c.claim_id, tt.code
 
SELECT c.claim_id ClaimKey, c.version_id Version,
  CASE WHEN
  ISNULL(c.create_date,0)<> 0 
  THEN c.create_date 
  ELSE c.last_modified_date END 
  TransactionDate,
  tt.description TransactionType,
  ISNULL(c.claim_version_description,'Pending Authorisation') VersionDescription,
  CASE WHEN @RI2007=1 THEN r.reserve_total_incurred - ISNULL(rec.received_to_Date,0) 
  ELSE r.reserve_total_incurred
  END TotalIncurred,
  r.reserve_total_paid TotalPaid, r.this_reserve_revision ThisRevision, r.this_reserve_payment ThisPayment,
  CASE WHEN rec.transaction_type_code='C_SA' 
  THEN rec.recovery_amount
  ELSE 0 
  END ThisSalvageRecovery,
  CASE WHEN rec.transaction_type_code='C_RV'
  THEN rec.recovery_amount
  ELSE 0 
  END ThisThirdPartyRecovery,
  r.current_reserve CurrentReserve,
  ifc.description PolicyCurrency, 
  cc.description LossCurrency,
  u.username [User], 
  c.description ClaimDescription, 
  c.policy_number InsuranceRef, 
  c.policy_id InsuranceFileKey,
  c.claim_number, 
  c.risk_type_id RiskKey, 
  c.client_short_name, 
  c.loss_from_date,
  ih.shortname InsuranceHolderShortName, inf.insurance_folder_cnt InsuranceFolderKey, tt.code TransactionTypeCode
FROM claim c
LEFT JOIN transaction_type tt ON c.transaction_type_id = tt.transaction_type_id
INNER JOIN insurance_file inf ON c.policy_id = inf.insurance_file_cnt
INNER JOIN currency ifc ON inf.currency_id = ifc.currency_id
INNER JOIN insurance_folder infolder ON inf.insurance_folder_cnt = infolder.insurance_folder_cnt
INNER JOIN party ih ON infolder.insurance_holder_cnt = ih.party_cnt
INNER JOIN currency cc ON c.currency_id = cc.currency_id
LEFT JOIN pmuser u ON u.user_id = c.created_by_id
LEFT JOIN #Reserve r ON c.claim_id = r.claim_id
LEFT JOIN #Recoveries rec ON c.claim_id = rec.claim_id
WHERE c.base_claim_id = @base_claim_id AND c.is_dirty = 0
ORDER BY c.version_id DESC
 
DROP TABLE #Claims
DROP TABLE #Reserve
DROP TABLE #Recoveries  
END 
GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO