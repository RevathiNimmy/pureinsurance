EXECUTE DDLDropProcedure 'spu_CLM_save_recovery'
GO

CREATE PROCEDURE spu_CLM_save_recovery 
      @lClaimPerilID INT,  
	  @lRecoveryTypeID INT,  
	  @crReserveAmount MONEY,
	  @lRecoveryPartyTypeID INT=NULL,
	  @nRecoveryPartyCnt INT =NUll 
AS  

DECLARE
@lVersionID INT,
@lCurrencyID INT,
@lRecoveryID INT

  
IF NOT Exists(SELECT NULL FROM recovery 
	WHERE claim_peril_id = @lClaimPerilID AND recovery_type_id = @lRecoveryTypeID
 AND (
     recovery_party_cnt = @nRecoveryPartyCnt
     OR (recovery_party_cnt IS NULL AND @nRecoveryPartyCnt IS NULL)
 ))
BEGIN


	EXEC spu_CLM_Get_Claim_Version  
	@claim_peril_id = @lClaimPerilID,  
	@version_id  = @lVersionID OUTPUT  


	SELECT @lCurrencyID = currency_id FROM claim c
	Inner join claim_peril cp ON cp.claim_id = c.claim_id 
	Where cp.claim_peril_id = @lClaimPerilID

	INSERT INTO recovery (  
	claim_peril_id ,  
	recovery_type_id,  
	currency_id,  
	initial_reserve ,  
	revised_reserve,  
	received_to_date,  
	revision_count,  
	tax_amount,  
	version_id ,
	recovery_party_type_id,
	recovery_party_cnt )  
	
	VALUES (
	@lClaimPerilID,  
	@lRecoveryTypeID,  
	@lCurrencyID,  
	@crReserveAmount,  
	0,  
	0,  
	0,  
	0,  
	@lVersionID,
	@lRecoveryPartyTypeID,
	@nRecoveryPartyCnt)  
   
	SELECT @lRecoveryID = @@IDENTITY  
	
	UPDATE recovery  
	SET base_recovery_id = @lRecoveryID  
	WHERE recovery_id = @lRecoveryID  
END

ELSE

BEGIN
	--Update existing
	UPDATE  recovery  
	SET revised_reserve = revised_reserve + @crReserveAmount,
	revision_count = revision_count + 1,			 
	recovery_party_type_id= ISNULL(@lRecoveryPartyTypeID,recovery_party_type_id),
	recovery_party_cnt= ISNULL(@nRecoveryPartyCnt,recovery_party_cnt)
	Where claim_peril_id = @lClaimPerilID 
	AND recovery_type_id = @lRecoveryTypeID
	AND recovery_party_cnt = @nRecoveryPartyCnt
		
END  
GO
