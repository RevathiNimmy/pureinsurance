SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
EXECUTE DDLDropProcedure 'spu_get_claiminsfilecnt_productid'
GO

CREATE  PROCEDURE spu_get_claiminsfilecnt_productid
	@InsuranceFileCnt int,
        @BaseClaimID int=NULL
AS
BEGIN
    --Start (Prakash C Varghese) - (Gap Fixing As told by Gaurav)
    -- Added a new optional parameter to get Product ID from claimID
    IF (@BaseClaimID is not null)
    BEGIN
        SELECT @insuranceFileCnt=Policy_ID
        FROM Claim
        WHERE Claim_ID=@BaseClaimID
    END
    --End (Prakash C Varghese) - (Gap Fixing As told by Gaurav)
	SELECT 	I.Product_Id
	FROM	Insurance_File I
	WHERE	I.Insurance_File_Cnt = @InsuranceFileCnt

END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

