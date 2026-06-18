SET ANSI_NULLS OFF
GO

SET QUOTED_IDENTIFIER OFF
GO

EXECUTE DDLDropProcedure 'spu_Open_Claims_Per_Product'
GO

CREATE PROCEDURE [dbo].[spu_Open_Claims_Per_Product]
	@BranchName VARCHAR(MAX),
	@LossDateTo DATE,
	@LossDateFrom DATE,	
	@Product VARCHAR(MAX)
AS

	
	DECLARE @sSQL				NVARCHAR(MAX)
	DECLARE @enumClosedClaim	INT = 3
	DECLARE @enumReClosedClaim	INT = 5
	DECLARE @ClaimVersionID		INT
	DECLARE @ClaimID			INT
	DECLARE @ClientName			VARCHAR(255)
	DECLARE @ClaimNumber		VARCHAR(255)
	DECLARE @PolicyNumber		VARCHAR(30)
	DECLARE @LossFromDate		DATE
	DECLARE @ProductCode		CHAR(10)
	DECLARE @ProductDescription	VARCHAR(255)
	DECLARE @TotalIncurred		DECIMAL
	DECLARE @TotalPaid			DECIMAL
	DECLARE @CurrentReserve		DECIMAL
	DECLARE @ProductID			INT
	DECLARE @SourceID			INT

		CREATE TABLE #OpenClaimPerProduct
	(
		ClaimID					INT NOT NULL PRIMARY KEY,
		ClientName				VARCHAR(255),
		ClaimNumber				VARCHAR(30),
		PolicyNumber			VARCHAR(30),
		LossFromDate			DATE,
		ProductCode				CHAR(10),
		ProductDescription		VARCHAR(255),
		TotalIncurred			DECIMAL,
		TotalPaid				DECIMAL,
		CurrentReserve			DECIMAL,
		ProductID				INT,
		SourceID				INT
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
	SELECT @sSQL = @sSQL + 'AND C.Claim_Status_id NOT IN (' + Convert(varchar,@enumClosedClaim) + ',' + Convert(varchar,@enumReClosedClaim) + ') '
	SELECT @sSQL = @sSQL + 'AND IFL.product_id IN (' + @Product + ') '
	SELECT @sSQL = @sSQL + 'AND IFL.source_id IN (' + @BranchName + ') '
	SELECT @sSQL = @sSQL + 'AND C.loss_from_date BETWEEN ' + '''' + CONVERT(VARCHAR,@LossDateFrom) + '''' +' AND ' + ''''+ CONVERT(VARCHAR,@LossDateTo) + '''' + ' '
	SELECT @sSQL = @sSQL + 'GROUP BY base_claim_id '
	
	EXEC SP_EXECUTESQL @sSQL
	
	DECLARE OpenClaimCursor Cursor FOR
	SELECT * FROM #LatestVersionOfClaim

	OPEN OpenClaimCursor

	FETCH NEXT FROM OpenClaimCursor
	INTO @ClaimVersionID, @ClaimID

	WHILE @@FETCH_STATUS = 0
	BEGIN

		--The following SQl statment is a copy from SSP Core SP spu_CLM_Get_Claim_Version_Details with minor modificaitons
		--Start spu_CLM_Get_Claim_Version_Details--

					SELECT	@ClaimID = 0,
							@ClientName = '', 
							@ClaimNumber = '', 
							@PolicyNumber = '',		
							@LossFromDate = NULL,		
							@ProductCode = '',	
							@ProductDescription = '',	
							@TotalIncurred = 0,	
							@TotalPaid = 0,		
							@CurrentReserve = 0 ,
							@ProductID = 0,
							@SourceID = 0
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

					@ClientName = claim.Client_short_name,
					@ClaimNumber = claim.Claim_Number,
					@PolicyNumber = claim.Policy_Number,
					@LossFromDate = claim.Loss_from_date,
  					@TotalIncurred = CASE WHEN @RI2007=1 THEN  
					reserve_total_incurred -(ISNULL(salvage_recovery.received_to_Date,0)+ISNULL(third_party_recovery.received_to_date,0))  
					ELSE  
					   reserve_total_incurred  
					END ,  
  					@TotalPaid = reserve_total_paid,  
					@CurrentReserve = current_reserve ,  
					@PolicyNumber = claim.policy_number ,  
					@ClaimNumber = claim.claim_number,  
					@LossFromDate = claim.loss_from_date,
					@SourceID =  insurance_file.source_id
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

		SELECT Top 1 @ProductCode = P.code, @ProductDescription = P.description , @ProductID = P.product_id FROM Product P
		JOIN Insurance_File IFL ON IFL.product_id = P.product_id
		JOIN Claim C ON C.Policy_id = IFL.insurance_file_cnt
		WHERE C.claim_id = @ClaimID

		INSERT INTO #OpenClaimPerProduct(	ClaimID,ClientName,ClaimNumber,
											PolicyNumber,LossFromDate,ProductCode,
											ProductDescription,
											TotalIncurred,TotalPaid,CurrentReserve, ProductID, SourceID)
		SELECT 
		@ClaimID,
		@ClientName, 
		@ClaimNumber, 
		@PolicyNumber,		
		@LossFromDate,		
		@ProductCode,	
		@ProductDescription,	
		@TotalIncurred,	
		@TotalPaid,		
		@CurrentReserve,
		@ProductID,
		@SourceID







		FETCH NEXT FROM OpenClaimCursor
		INTO @ClaimVersionID, @ClaimID
	END
CLOSE OpenClaimCursor
DEALLOCATE OpenClaimCursor

SELECT * FROM #OpenClaimPerProduct
DROP TABLE #OpenClaimPerProduct



