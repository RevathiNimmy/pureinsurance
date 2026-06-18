SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXEC DDLDropProcedure 'spu_other_party_PLLSource'
GO

CREATE PROCEDURE spu_other_party_PLLSource  --List or select  
    @PartyCnt INT,  
    @Branchid INT = Null,
	@UserId int = NULL,
	@UniqueId VARCHAR(50) = NULL,
	@ScreenHierarchy VARCHAR(500) = NULL 
	
  
AS  
  
SELECT B.Source_id, B.description,
 CASE WHEN OP.Source_id IS Null THEN 0 ELSE 1 END As Chosen
 FROM Source B LEFT JOIN other_party_branch OP ON 
 B.Source_id = OP.Source_id  
 AND OP.Party_cnt = @PartyCnt
 WHERE B.is_deleted <> 1 

Go



