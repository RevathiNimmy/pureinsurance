SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spe_Agent_PLLSource'
GO


CREATE PROCEDURE spe_Agent_PLLSource  --List or select  
    @Party_Cnt INT,  
    @Branchid INT = Null,
    @TransType VARCHAR(20)= NULL,
	@user_id INT=NULL,
	@unique_id VARCHAR(50)=NULL,
	@screen_hierarchy VARCHAR(500)=NULL   
  
AS    
    
   
BEGIN    
 DECLARE @SQL VARCHAR(1000)    
    
 SELECT @SQL = 'SELECT B.Source_id, B.description,'    
 SELECT @SQL = @SQL + 'CASE WHEN P.Source_id IS Null THEN 0 ELSE 1 END As Chosen '    
 SELECT @SQL = @SQL + 'FROM Source B LEFT JOIN Party_Agent_branch P ON '    
 SELECT @SQL = @SQL + 'B.Source_id = P.Source_id '    
 SELECT @SQL = @SQL + 'AND P.Party_cnt = ' + CONVERT(Varchar(1000),@Party_Cnt) + ' '    
 SELECT @SQL = @SQL + 'WHERE B.is_deleted <> 1 '    
 IF ((ISNULL(@Branchid,0)<>0 )  AND (@TransType IS NOT NULL))
  BEGIN  
   SELECT @SQL = @SQL + 'AND P.Source_id = ' + CONVERT(Varchar(1000),@Branchid) + ' '    
  END    
    
 EXECUTE (@SQL)    
END   
