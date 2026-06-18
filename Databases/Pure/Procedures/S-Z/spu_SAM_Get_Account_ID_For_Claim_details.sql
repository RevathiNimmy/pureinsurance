SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure spu_SAM_Get_Account_ID_For_Claim_details
GO

CREATE  PROCEDURE spu_SAM_Get_Account_ID_For_Claim_details

	@sAccount_Short_Code Varchar(50)
AS
BEGIN
	SELECT
		account_id 
	FROM Account 
	WHERE short_code   = @sAccount_Short_Code
END	
	
