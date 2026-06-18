Execute DDLDropProcedure 'spu_SAM_CLM_Close_Now'
GO
CREATE PROCEDURE spu_SAM_CLM_Close_Now
@claim_id integer  
  
AS  
BEGIN  
  
Declare @nClosedCnt As integer  
Declare @nClaim_Status_ID As integer  
Set @nClosedCnt=0  
  
Select @nClosedCnt=Count(Claim_id) From Claim where ISnull(is_dirty,0)<>1 And Claim_Status_id=3  
And Claim_id<>@claim_id And base_claim_id in (Select base_claim_id from Claim where Claim_id=@claim_id)  
  
IF @nClosedCnt>=1  
  Begin  
  Set @nClaim_Status_ID=5  
  End  
 Else  
 Begin  
        Set @nClaim_Status_ID=3  
 End  

DECLARE @pStatus INT = NULL
IF NOT EXISTS(SELECT c.progress_status_id FROM claim c
JOIN progress_status p ON c.Progress_Status_id = p.progress_status_id
WHERE Claim_id = @claim_id AND p.is_closed_check_status = 1)
BEGIN
	 SELECT @pStatus = progress_status_id FROM Progress_Status WHERE code = 'CLOSED'
END

UPDATE Claim  
SET Claim_Status_ID = @nClaim_Status_ID  
 , progress_status_id = ISNULL(@pStatus,progress_status_id)  
 , Claims_status_date=GETDATE()  
WHERE Claim_ID = @claim_id 

END  

GO