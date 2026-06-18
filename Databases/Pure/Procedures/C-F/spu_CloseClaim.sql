SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_CloseClaim'
GO


CREATE PROCEDURE spu_CloseClaim
    @ClaimID integer
AS

    DECLARE @progress_status_id int


    -- Get current progress id if it's already a closed status
    SELECT  @progress_status_id = c.progress_status_id 
    FROM    Claim c
    JOIN    Progress_Status ps ON ps.progress_status_id = c.progress_status_id 
    WHERE   c.claim_id = @ClaimID
    AND     ISNULL(ps.is_closed_check_status, 0) = 1


    -- If we haven't got a closed status get one (fallback to old style)
    IF @progress_status_id IS NULL BEGIN
        SELECT  @progress_status_id = progress_status_id 
        FROM    Progress_Status 
        WHERE   code = 'CLOSED' 
        AND     effective_date <= getdate() 
        AND     is_deleted = 0
    END

    -- Update Claim
    UPDATE  claim
    SET     claim_status_id = 3,
    	    claims_status_date=getdate(), 	
            progress_status_id = @progress_status_id
    WHERE   claim_id = @ClaimID


GO