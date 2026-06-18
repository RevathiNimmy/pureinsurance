
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_PM_Do_FindUser'
GO

--Start (Ravikumar Pasupuleti) - (Tech Spec - UIIC WR28(a) - User Get List - Find Users.doc) 
 CREATE PROCEDURE spu_PM_Do_FindUser    
    @UserName varchar(255) = NULL,    
    @FullName varchar(255) = NULL,
    @MaxRowsToFetch INT = -1,
	@AgentKey INT=0
AS    
   
/* To return the list of matching users*/    
IF @MaxRowsToFetch<>-1
BEGIN
  SET NOCOUNT ON    
  SET ROWCOUNT @MaxRowsToFetch
END
    
SELECT     
       username,    
       full_name,  
       effective_date,  
       user_id  
FROM    
       PMUser     
      
WHERE  (username LIKE @UserName OR @UserName IS NULL)    
AND    (full_name LIKE @FullName OR @FullName IS NULL)    
AND	   (party_cnt=@AgentKey OR @AgentKey=0)  
ORDER BY username, full_name, effective_date  

IF @MaxRowsToFetch<>-1
BEGIN
  SET ROWCOUNT 0
  SET NOCOUNT OFF    
END
--End (Ravikumar Pasupuleti) - (Tech Spec - UIIC WR28(a) - User Get List - Find Users.doc)
GO

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
