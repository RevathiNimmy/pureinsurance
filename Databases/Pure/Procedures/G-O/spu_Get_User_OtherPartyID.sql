SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO

Execute DDLDropProcedure 'spu_Get_User_OtherPartyID'
GO

CREATE PROCEDURE spu_Get_User_OtherPartyID
 -- Add the parameters for the stored procedure here  
     @userid int  
AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
  
    -- Insert statements for procedure here  
 SELECT other_party_id,name,shortname from PMUser u LEFT JOIN Party p  
 on p.party_cnt=u.other_party_id  
 where user_id=@userid  
END  
  
GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
