Execute DDLDropProcedure 'spu_SAM_CLM_Reopen_Now'
GO
CREATE PROCEDURE spu_SAM_CLM_Reopen_Now
@claim_id integer
AS
BEGIN
	Declare @nClaim_Status_ID As integer
	Declare @nClosedCnt As integer

	Set @nClosedCnt=0
	Select @nClosedCnt=Count(Claim_id) From Claim where ISnull(is_dirty,0)<>1 And Claim_Status_id=3 And
	Claim_id<>@claim_id And base_claim_id in (Select base_claim_id from Claim where Claim_id=@claim_id)  
	
IF @nClosedCnt>=1
	 Begin
		   Set @nClaim_Status_ID=4
	 End
     Else
  
   Begin
       Set @nClaim_Status_ID=2
	End
		UPDATE Claim
					SET Claim_Status_ID =@nClaim_Status_ID
					WHERE Claim_ID = @claim_id

						EXECUTE spu_CLM_Update_Claim_status @claim_id,@nClaim_Status_ID

    End

GO