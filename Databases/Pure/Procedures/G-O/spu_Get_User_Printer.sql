SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

EXECUTE DDLDropProcedure 'spu_Get_User_Printer'
GO

CREATE Procedure spu_Get_User_Printer  
@UserName VARCHAR(50)  

As  
Begin  
  
   	SELECT server_printer FROM PMUser WHERE username = @UserName

End

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO