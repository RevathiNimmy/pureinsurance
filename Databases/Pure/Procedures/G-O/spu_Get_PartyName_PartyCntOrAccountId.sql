SET QUOTED_IDENTIFIER OFF 
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Get_PartyName_PartyCntOrAccountId'
GO

CREATE PROCEDURE spu_Get_PartyName_PartyCntOrAccountId
	@Party_cnt		INT,
	@Account_Id		INT,
	@Party_Name		VARCHAR(255)=NULL OUTPUT

AS

IF (@Account_Id=0 AND @Party_cnt>0)
	BEGIN
		SELECT 	@Party_Name=resolved_name
			FROM Party
		WHERE 	Party_cnt = @Party_cnt
	END	
ELSE IF (@Party_cnt=0 AND @Account_Id>0)
	BEGIN
		SELECT 	@Party_Name=Account_name
			FROM Account
		WHERE 	Account_Id=@Account_Id
	END	

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO