SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXEC DDLDropProcedure 'spu_Other_party_PLSSource'
GO
CREATE PROCEDURE spu_Other_party_PLSSource  --Add  
    @PartyCnt INT,  
    @Branchid INT,
	@UserId int = NULL,
	@UniqueId VARCHAR(50) = NULL,
	@ScreenHierarchy VARCHAR(500) = NULL   
  
AS  
  
INSERT INTO  
    Other_Party_Branch (Party_cnt,Source_id,UserId,UniqueId,ScreenHierarchy)  
VALUES  
    (@PartyCnt,@Branchid,@UserId,@UniqueId,@ScreenHierarchy)  

GO