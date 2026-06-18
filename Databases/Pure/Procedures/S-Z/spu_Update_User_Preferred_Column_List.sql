SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXEC DDLDropProcedure 'spu_Update_User_Preferred_Column_List'
GO   
CREATE PROCEDURE spu_Update_User_Preferred_Column_List     
 @sUserName VARCHAR(255),      
 @sInterfaceName VARCHAR(100),     
 @sColumnList VARCHAR(1000)
     
AS      
    
BEGIN      
        
 IF EXISTS (SELECT NULL FROM User_Preferred_Column_List WHERE username= @sUserName AND InterfaceName=@sInterfaceName)    
 BEGIN        
	UPDATE User_Preferred_Column_List     
	SET ColumnList=@sColumnList
	WHERE UserName = @sUserName AND InterfaceName=@sInterfaceName         
 END    
 ELSE    
 BEGIN    
   INSERT INTO User_Preferred_Column_List 
   (UserName,
   InterfaceName,
   ColumnList)				
	VALUES(@sUserName,      
	@sInterfaceName,     
	@sColumnList  )    
 END    
    
END    