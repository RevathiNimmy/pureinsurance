SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXEC DDLDropProcedure 'spu_Get_User_Preferred_Column_List'
GO   
CREATE PROCEDURE spu_Get_User_Preferred_Column_List  
  @sUserName VARCHAR(255),
  @sInterfaceName VARCHAR(100)
  AS    
    
  BEGIN        
   SELECT UserName, 
		  InterfaceName, 
		  ColumnList
 FROM User_Preferred_Column_List WHERE username = @sUserName AND InterfaceName=@sInterfaceName
  END 


  