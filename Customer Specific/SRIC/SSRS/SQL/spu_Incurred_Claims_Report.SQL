SET ANSI_NULLS OFF
GO

SET QUOTED_IDENTIFIER OFF
GO

EXECUTE DDLDropProcedure 'spu_Incurred_Claims_Report'
GO

CREATE PROCEDURE [dbo].[spu_Incurred_Claims_Report]
	@LossDateTo DATE,
	@Amount Money
AS

	
	DECLARE @sSQL				NVARCHAR(MAX)
	DECLARE @enumClosedClaim	INT = 3
	DECLARE @enumReClosedClaim	INT = 5
	DECLARE @ClaimVersionID		INT
	DECLARE @ClaimID			INT
	DECLARE @ClaimNumber		VARCHAR(255)
	DECLARE @GrossIncurred		MONEY
	DECLARE @NetIncurred		MONEY
	DECLARE @ClassOfBusinessID	INT
	DECLARE @COBDescription		VARCHAR(255)
	DECLARE @ReserveReinsurance MONEY
	DECLARE @PaymentReinsurance	MONEY
	DECLARE @SalavgeReinsurance MONEY
	DECLARE @TPReinsurance		MONEY

		CREATE TABLE #IncurredClaim
	(
		ClaimID					INT NOT NULL PRIMARY KEY,
		ClaimNumber				VARCHAR(30),
		ClaimVersionNumber		INT,
		GrossIncurred			MONEY,
		NetIncurred				MONEY,
		ClassOfBusinessID		INT,
		COBDescription			VARCHAR(255)
	)


		CREATE TABLE #LatestVersionOfClaim
	(
		MaxVersion				INT,
		BaseClaimID				INT NOT NULL PRIMARY KEY
		
	)

	SELECT @sSQL =		   'INSERT INTO #LatestVersionOfClaim(MaxVersion,BaseClaimID) '
	SELECT @sSQL = @sSQL + 'SELECT MAX(version_id) AS MaxVersion,base_claim_id '
	SELECT @sSQL = @sSQL + 'FROM Claim C '
	SELECT @sSQL = @sSQL + 'JOIN Insurance_File IFL ON IFL.insurance_file_cnt = C.Policy_id '
	SELECT @sSQL = @sSQL + 'WHERE C.is_dirty <> 1 '
	SELECT @sSQL = @sSQL + 'AND C.Last_modified_date <  EOMONTH(' + '''' + CONVERT(VARCHAR,@LossDateTo) + '''' +') ' 
	SELECT @sSQL = @sSQL + 'GROUP BY base_claim_id '
	
	EXEC SP_EXECUTESQL @sSQL

	DECLARE OpenClaimCursor Cursor FOR
	SELECT * FROM #LatestVersionOfClaim

	OPEN OpenClaimCursor

	FETCH NEXT FROM OpenClaimCursor
	INTO @ClaimVersionID, @ClaimID

	WHILE @@FETCH_STATUS = 0
	BEGIN


		SELECT @ClaimNumber = ''
		SELECT @GrossIncurred = 0
		SELECT @ClassOfBusinessID = 0
		SELECT @COBDescription = ''
		--The following SQl statment is a copy from SSP Core SP spu_CLM_Get_Claim_Version_Details with minor modificaitons
		--Start spu_CLM_Get_Claim_Version_Details--
					DECLARE @RI2007 INT  
					DECLARE @nSalvageAndTPRecoveryReservesExcludeTax INT  
					DECLARE @enumSalvageAndTPRecoveryReservesExcludeTax INT  = 5067  
  
					SELECT @nSalvageAndTPRecoveryReservesExcludeTax  = value  
					FROM System_Options  
					WHERE option_number = @enumSalvageAndTPRecoveryReservesExcludeTax  
  
					SELECT @RI2007 = value  
					FROM hidden_options  
					WHERE option_Number=88  
  
					SELECT 
					
					Top 1
					@ClaimNumber = claim.Claim_Number,
  					@GrossIncurred =  reserves.current_reserve + salvage_recovery.received_to_Date + third_party_recovery.received_to_Date
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
		--End spu_CLM_Get_Claim_Version_Details--


			SELECT @ReserveReinsurance = SUM(reserve) FROM Claim_RI_Arrangement_Line WHERE claim_id = @ClaimID AND TYPE <> 'R' 
			SELECT @PaymentReinsurance = SUM(payment) FROM Claim_RI_Arrangement_Line WHERE claim_id = @ClaimID AND TYPE <> 'R'
			SELECT @SalavgeReinsurance = SUM(ABS(salvage)) FROM Claim_RI_Arrangement_Line WHERE claim_id = @ClaimID AND TYPE <> 'R'
			SELECT @TPReinsurance = SUM(ABS(recovery)) FROM Claim_RI_Arrangement_Line WHERE claim_id = @ClaimID AND TYPE <> 'R'


		IF @GrossIncurred >= @Amount BEGIN
			SELECT @ClassOfBusinessID=COB.class_of_business_id, @COBDescription=COB.description FROM Claim_Peril CP 
			JOIN Peril_Type PT ON PT.peril_type_id = CP.Peril_type_id
			JOIN Class_Of_Business COB ON COB.class_of_business_id =  PT.class_of_business_id
			WHERE CP.Claim_id = @ClaimID

			INSERT INTO #IncurredClaim(	ClaimID,ClaimNumber,ClaimVersionNumber,
										GrossIncurred,NetIncurred,ClassOfBusinessID,COBDescription)
											
			SELECT 
			@ClaimID,
			@ClaimNumber,
			@ClaimVersionID,
			@GrossIncurred,
			@GrossIncurred - (@ReserveReinsurance + @PaymentReinsurance + @SalavgeReinsurance + @TPReinsurance) ,
			@ClassOfBusinessID,
			@COBDescription
		END

		FETCH NEXT FROM OpenClaimCursor
		INTO @ClaimVersionID, @ClaimID
	END
CLOSE OpenClaimCursor
DEALLOCATE OpenClaimCursor

SELECT * FROM #IncurredClaim
DROP TABLE #IncurredClaim



