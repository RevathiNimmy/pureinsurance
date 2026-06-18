EXECUTE DDLDropProcedure 'Report_SSRS_OutstandingClaims'
GO

CREATE PROCEDURE Report_SSRS_OutstandingClaims
	@UserID INT
AS

BEGIN

DECLARE @ClaimID			INT
DECLARE @ClientName			VARCHAR(255)
DECLARE @ClaimNumber		VARCHAR(30)
DECLARE @PolicyNumber		VARCHAR(30)
DECLARE @LossFromDate		DATE
DECLARE @OutStandingAmount	DECIMAL
DECLARE @AgeOfClaim			INT
DECLARE @PremiumClass		VARCHAR(10)
DECLARE @RiskType			VARCHAR(10)
DECLARE @PMUserID			INT
DECLARE @UserName			VARCHAR(255)

CREATE TABLE #LatestVersionOfClaim
(
	ClaimID				INT NOT NULL PRIMARY KEY		
)

CREATE TABLE #OutstandingClaims
(
	ClaimID				INT,
	ClaimNumber			VARCHAR(30),
	PolicyNumber		VARCHAR(30),
	LossFromDate		DATE,
	OutstandingAmount	DECIMAL,
	AgeOfClaim			INT,
	PremiumClass		VARCHAR(10),
	RiskType			VARCHAR(10),
	ClientName			VARCHAR(20),
	UserName			VARCHAR(255),
	PMUserID			INT

)

 INSERT INTO #LatestVersionOfClaim
 SELECT MAX(claim_id) AS 'ClaimID' FROM CLAIM 
 WHERE is_dirty = 0
 AND Claim_Status_id in (1,2,4)
 AND created_by_id = (	CASE WHEN @UserID = 0 THEN Handler_id
						ELSE Convert(NVARCHAR,@UserID) END 
					)
 GROUP BY base_claim_id 

DECLARE OpenClaimCursor Cursor FOR
SELECT * FROM #LatestVersionOfClaim

OPEN OpenClaimCursor

FETCH NEXT FROM OpenClaimCursor
INTO @ClaimID

	WHILE @@FETCH_STATUS = 0
	BEGIN

					DECLARE @nSalvageAndTPRecoveryReservesExcludeTax INT  
					DECLARE @enumSalvageAndTPRecoveryReservesExcludeTax INT  = 5067  
  
					SELECT @nSalvageAndTPRecoveryReservesExcludeTax  = value  
					FROM System_Options  
					WHERE option_number = @enumSalvageAndTPRecoveryReservesExcludeTax  

					DECLARE @RI2007 INT
					SELECT @RI2007 = value  
					FROM hidden_options  
					WHERE option_Number=88

					SELECT 
					Top 1 

					@ClientName = claim.Client_short_name,
					@ClaimNumber = claim.Claim_Number,
					@PolicyNumber = claim.Policy_Number,
					@LossFromDate = claim.Loss_from_date,
					@OutStandingAmount = ISNULL(reserves.current_reserve,0),
					@AgeOfClaim = DATEDIFF(MM,claim.reported_date,GetDATE()),
					@PMUserID = claim.created_by_id
					FROM claim    
					LEFT JOIN transaction_type ON  claim.transaction_type_id = transaction_type.transaction_type_id    
					INNER JOIN insurance_file ON  claim.policy_id = insurance_file.insurance_file_cnt  
  					INNER JOIN currency insurance_file_currency ON  insurance_file.currency_id = insurance_file_currency.currency_id    
					INNER JOIN insurance_folder ON  insurance_file.insurance_folder_cnt = insurance_folder.insurance_folder_cnt  
  					INNER JOIN party insurance_holder ON  insurance_folder.insurance_holder_cnt =insurance_holder.party_cnt    
					INNER JOIN currency claim_currency ON  claim.currency_id = claim_currency.currency_id    
					LEFT JOIN pmuser ON  pmuser.user_id= claim.created_by_id    
					LEFT JOIN (  -- reserve payments  and revisions  
								  SELECT  
									SUM(this_payment) AS this_reserve_payment,  
									SUM(this_revision) AS this_reserve_revision,  
									SUM(initial_reserve) + SUM(Revised_reserve) AS reserve_total_incurred,  
									SUM(paid_to_date) AS reserve_total_paid,  
									SUM(initial_reserve) + SUM(revised_reserve) - SUM(paid_to_date) AS current_reserve,  
									claim.claim_id  
									FROM reserve 
									INNER JOIN claim_peril ON  reserve.claim_peril_id = claim_peril.claim_peril_id  
									INNER JOIN claim ON claim_peril.claim_id = claim.claim_id  
									GROUP BY claim.claim_id
								) reserves ON  claim.claim_id = reserves.claim_id  
  					LEFT JOIN  (	-- salvage recovery  
									SELECT 
									CASE WHEN @nSalvageAndTPRecoveryReservesExcludeTax =  1 THEN SUM(this_receipt_net)  
									WHEN @nSalvageAndTPRecoveryReservesExcludeTax = 0 Then SUM(this_receipt_net) + 
									(	SELECT ISNULL(Tax_Amount,0) FROM Claim_Receipt CR  
										WHERE CR.Claim_Peril_id = Claim_Peril.Claim_Peril_id  
										AND CR.claim_receipt_id = CR.base_claim_receipt_id  
										AND SUM(this_receipt_net) <> 0
									)  END  AS this_salvage_recovery,  
									SUM(received_to_date) as received_to_Date,  
									claim.claim_id  
									FROM recovery  
									INNER JOIN claim_peril ON   recovery.claim_peril_id = claim_peril.claim_peril_id  
									INNER JOIN claim ON  claim_peril.claim_id = claim.claim_id  
									WHERE recovery.recovery_type_id IN (SELECT recovery_type_id FROM recovery_type WHERE is_salvage= 1)  
									GROUP BY claim.claim_id,Claim_Peril.Claim_Peril_id 
							  ) salvage_recovery ON claim.claim_id = salvage_recovery.claim_id    
					LEFT JOIN (    -- third party recovery  
									SELECT CASE WHEN @nSalvageAndTPRecoveryReservesExcludeTax =  1 THEN SUM(this_receipt_net)  
									WHEN @nSalvageAndTPRecoveryReservesExcludeTax = 0 Then SUM(this_receipt_net) + ( SELECT ISNULL(CR.Tax_Amount,0) FROM Claim_Receipt CR  
									WHERE CR.Claim_Peril_id = Claim_Peril.Claim_Peril_id  
									AND CR.claim_receipt_id = CR.base_claim_receipt_id  
									AND SUM(this_receipt_net) <> 0)  
									END AS this_third_party_recovery,  
									SUM(received_to_date) as received_to_Date,  
									claim.claim_id  
									FROM recovery  
									INNER JOIN claim_peril ON recovery.claim_peril_id = claim_peril.claim_peril_id  
									INNER JOIN claim ON claim_peril.claim_id = claim.claim_id  
									WHERE recovery.recovery_type_id IN (SELECT recovery_type_id FROM recovery_type WHERE is_salvage= 0)  
									GROUP BY claim.claim_id,Claim_Peril.Claim_Peril_id 
							 ) third_party_recovery ON  claim.claim_id = third_party_recovery.claim_id  
  
					WHERE claim.base_claim_id IN (SELECT base_claim_id FROM claim WHERE claim_id =@ClaimID)				   
					AND claim.is_dirty=0  
					ORDER BY version_id DESC 

					SELECT @PremiumClass  = cob.code  
					FROM    claim_peril cp,  
					peril_type pt,  
					class_of_business cob  
					WHERE cp.Claim_id = @ClaimID 
					AND cp.peril_type_id = pt.peril_type_id  
					AND pt.class_of_business_id = cob.class_of_business_id 

					SELECT @RiskType = RT.code 
					FROM Risk_Type RT
					JOIN Claim_Risk CR ON CR.Risk_type_id = RT.risk_type_id
					WHERE CR.Claim_id =@ClaimID

					SELECT @UserName = username FROM PMUser WHERE user_id = @PMUserID 

					INSERT INTO #OutstandingClaims(	ClaimID,ClaimNumber,PolicyNumber,
													LossFromDate,OutstandingAmount,AgeOfClaim,
													PremiumClass,RiskType,ClientName,
													UserName,PMUserID)

					SELECT 
					@ClaimID,
					@ClaimNumber,
					@PolicyNumber,
					@LossFromDate,
					@OutStandingAmount,
					@AgeOfClaim,
					@PremiumClass,
					@RiskType,
					@ClientName,
					@UserName,
					@PMUserID

		FETCH NEXT FROM OpenClaimCursor
		INTO  @ClaimID
	END
CLOSE OpenClaimCursor
DEALLOCATE OpenClaimCursor

SELECT * FROM #OutstandingClaims
DROP TABLE #OutstandingClaims








END
	

