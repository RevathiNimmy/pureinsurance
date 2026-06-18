EXECUTE DDLDropProcedure 'spu_update_reserve_details'
GO
CREATE PROCEDURE spu_update_reserve_details  
    @reserveid INT,
    @revisedreserve currency,  
    @initialreserve currency,  
    @paidtodate currency,  
    @average currency,  
    @this_payment currency,  
    @this_revision currency,  
    @revised_entered TINYINT,
    @transaction_type VARCHAR(5)='',  
    @sum_insured currency  = Null

AS  
  
BEGIN  
	
DECLARE @nCount INT
DECLARE @crRevision currency
Declare @PerilSumInsured CURRENCY
	UPDATE Claim
	SET Last_modified_date = Getdate()
	WHERE Claim_id = (SELECT DISTINCT Claim_id From Claim_Peril 
	Where Claim_Peril_id IN
	(SELECT distinct Claim_Peril_id 
	FROM Reserve 
	WHERE  reserve_id = @reserveid ))

	SELECT  @crRevision=revised_reserve,
			@nCount=revision_count
	FROM reserve 
	WHERE reserve_id=@reserveid  
  
		  
	IF @nCount IS NULL
		SELECT @nCount = 0
  
	IF @crRevision IS NULL
		SELECT @crRevision = 0
  
  	IF @revisedreserve!=@crRevision
		SELECT @nCount=@nCount+1
  
	
	IF @transaction_type = 'C_CR'  
  BEGIN  
	UPDATE  Reserve SET    
	initial_reserve=@initialreserve,    
	--revised_reserve=@revisedreserve,    
	revised_reserve = case when revised_reserve = 0 then @this_revision else revised_reserve - this_revision + @this_revision end,    
	this_revision=@this_revision,    
	this_payment=@this_payment,    
	paid_to_date=@paidtodate,    
	average=@average,    
	revision_count=@nCount,    
	revised_reserve_entered=@revised_entered,    
	sum_insured = @sum_insured    
	WHERE  reserve_id = @reserveid     
  END  
  ELSE  
  BEGIN  
	UPDATE  Reserve SET    
	initial_reserve=@initialreserve,    
	revised_reserve=@revisedreserve,    
	this_revision=@this_revision,    
	this_payment=@this_payment,    
	paid_to_date=@paidtodate,    
	average=@average,    
	revision_count=@nCount,    
	revised_reserve_entered=@revised_entered,    
	sum_insured = @sum_insured    
	WHERE  reserve_id = @reserveid    
  END  

	SELECT @PerilSumInsured = SUM(sum_insured) FROM Reserve  
	WHERE Claim_Peril_id =  (SELECT  Claim_Peril_id  
	FROM Reserve  
	WHERE  reserve_id = @reserveid )  
 
    IF @PerilSumInsured <> 0
	BEGIN
		UPDATE  Claim_Peril SET    
		sum_insured =  @PerilSumInsured    
		WHERE  Claim_Peril_id IN    
		(SELECT  Claim_Peril_id    
		FROM Reserve    
		WHERE  reserve_id = @reserveid )  
	END 

END  

SET QUOTED_IDENTIFIER OFF
Go
