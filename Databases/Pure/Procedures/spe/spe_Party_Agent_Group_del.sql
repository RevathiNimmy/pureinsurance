SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Party_Agent_Group_del'
GO

CREATE PROCEDURE spe_Party_Agent_Group_del
    @party_cnt int,
	@UserId int = NULL,
	@UniqueId varchar(50) = NULL,
	@ScreenHierarchy varchar(500) = NULL
AS

UPDATE Party_Agent_Group SET UserId = @UserId, UniqueId = @UniqueId, ScreenHierarchy = @ScreenHierarchy
WHERE party_cnt = @party_cnt

DELETE FROM Party_Agent_Group
WHERE party_cnt = @party_cnt

GO

