SET QUOTED_IDENTIFIER ON
GO

EXECUTE DDLDropProcedure 'spu_SAM_Get_User_Group_id'
GO

CREATE  PROCEDURE spu_SAM_Get_User_Group_id
    @nUser_id INT,
    @nPmuser_group_id INT OUTPUT
AS
BEGIN

   
 if @nUser_id <>0 
 BEGIN  
     SELECT  @nPmuser_group_id = pmuser_group_id      
     FROM    pmuser_group      
     WHERE   code ='CLMSUPER'      
 END  
 
 if @nPmuser_group_id = 0 OR ISNULL(@nPmuser_group_id,'')=''
 BEGIN            
      SELECT  @nPmuser_group_id = MIN(pmuser_group_id) FROM PMUser_Group_User WHERE user_id =  CONVERT(VARCHAR,@nUser_id)       
 END  
   
END
 
SET QUOTED_IDENTIFIER OFF
GO
