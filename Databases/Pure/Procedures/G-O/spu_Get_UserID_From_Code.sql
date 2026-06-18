SET QUOTED_IDENTIFIER OFF 
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Get_UserID_From_Code'
GO

CREATE PROCEDURE spu_Get_UserID_From_Code      
@UserName VarChar(255)      
AS    
    
BEGIN      
SELECT user_id FROM PMUser WHERE username = @UserName      
END
GO