SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_Get_LoginUserName'
GO

CREATE PROCEDURE spu_SAM_Get_LoginUserName 
 @usernameIN VARCHAR(255),
 @usernameOUT VARCHAR(255) OUTPUT
AS
 BEGIN
 
 	SELECT @usernameOUT = pmuser.Username 
 	FROM pmuser 
 	WHERE alternative_identifier = @usernameIN AND (is_deleted = 0 AND getdate() >= effective_date)
                               
 END
    
GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
