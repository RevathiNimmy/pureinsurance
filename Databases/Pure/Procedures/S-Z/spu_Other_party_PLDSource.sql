SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXEC DDLDropProcedure 'spu_Other_Party_PLDSource'
GO

CREATE PROCEDURE spu_Other_Party_PLDSource  --Delete      
    @PartyCnt INT,      
    @Branchid INT,
	@UserId int = NULL,
	@UniqueId VARCHAR(50) = NULL,
	@ScreenHierarchy VARCHAR(500) = NULL      
AS      
    
UPDATE Other_Party_Branch SET UserId = @UserId, UniqueId = @UniqueId, ScreenHierarchy = @ScreenHierarchy
WHERE party_cnt = @PartyCnt

DELETE FROM      
    Other_Party_Branch      
WHERE      
    party_cnt = @PartyCnt      
    
GO
